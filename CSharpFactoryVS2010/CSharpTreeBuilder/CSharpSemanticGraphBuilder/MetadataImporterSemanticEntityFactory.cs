using System;
using System.Linq;
using System.Reflection;
using CSharpTreeBuilder.CSharpSemanticGraph;
using CSharpTreeBuilder.ProjectContent;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class creates semantic entities from metadata read from referenced assemblies.
  /// </summary>
  // ================================================================================================
  public class MetadataImporterSemanticEntityFactory
  {
    /// <summary>
    /// Error handler object used for reporting compilation messages.
    /// </summary>
    private readonly ICompilationErrorHandler _ErrorHandler;

    /// <summary>
    /// The semantic graph that this factory is working on.
    /// </summary>
    private readonly SemanticGraph _SemanticGraph;

    /// <summary>
    /// The imported assembly represented as a Program object.
    /// </summary>
    private Program _Program;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="MetadataImporterSemanticEntityFactory"/> class.
    /// </summary>
    /// <param name="errorHandler">Error handler object used for reporting compilation messages.</param>
    /// <param name="semanticGraph">The semantic graph that this factory is working on.</param>
    // ----------------------------------------------------------------------------------------------
    public MetadataImporterSemanticEntityFactory(ICompilationErrorHandler errorHandler, SemanticGraph semanticGraph)
    {
      _ErrorHandler = errorHandler;
      _SemanticGraph = semanticGraph;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates semantic entities from all the types defined in an assembly 
    /// and adds them to the semantic graph specified in the constructor.
    /// </summary>
    /// <param name="filename">The name of the assembly file.</param>
    /// <param name="alias">A valid C# identifier that will be the root namespace for the imported types.
    /// If null, then "global" is assumed.</param>
    // ----------------------------------------------------------------------------------------------
    public void ImportTypesIntoSemanticGraph(string filename, string alias)
    {
      if (filename == null) { throw new ArgumentNullException("filename"); }
      if (alias == null) { alias = _SemanticGraph.GlobalNamespace.Name; }

      Assembly assembly;

      try
      {
        assembly = Assembly.ReflectionOnlyLoadFrom(filename);
        _Program = new Program(null, assembly.GetName());
      }
      catch (System.IO.FileNotFoundException)
      {
        _ErrorHandler.Error("CS0006", null, "Metadata file '{0}' could not be found.", filename);
        return;
      }
      catch (System.BadImageFormatException)
      {
        _ErrorHandler.Error("CS0009", null, 
          "Metadata file '{0}' could not be opened. -- An attempt was made to load a program with an incorrect format.", filename);
        return;
      }

      foreach (var type in assembly.GetTypes())
      {
        ImportReflectedType(type, alias);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates semantic entities from all members of a type, and adds them to the semantic graph
    /// specified in the constructor.
    /// </summary>
    /// <param name="type">A reflected type whose members will be imported.</param>
    /// <param name="typeEntity">The semantic entity that will receive the imported members.</param>
    // ----------------------------------------------------------------------------------------------
    public void ImportMembersIntoSemanticGraph(Type type, TypeEntity typeEntity)
    {
      var reflectedMembers = type.GetMembers(BindingFlags.DeclaredOnly |
        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

      foreach (var reflectedMember in reflectedMembers)
      {
        MemberEntity memberEntity = CreateMemberEntity(reflectedMember);

        if (memberEntity != null)
        {
          typeEntity.AddMember(memberEntity);
          MapEntityToReflectedMetadata(memberEntity, reflectedMember);
        }
      }
    }

    #region Private methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates semantic entities (namespace and type) from a reflected type and adds them 
    /// to the semantic graph.
    /// </summary>
    /// <param name="type">The type to be added to the semantic graph.</param>
    /// <param name="alias">A valid C# identifier that will represent a root namespace 
    /// that will contain the type.</param>
    // ----------------------------------------------------------------------------------------------
    private void ImportReflectedType(Type type, string alias)
    {
      if (type == null)
      {
        throw new ArgumentNullException("type");
      }
      if (alias == null)
      {
        throw new ArgumentNullException("alias");
      }

      // The context entity is our current position in the semantic graph.
      NamespaceOrTypeEntity contextEntity = _SemanticGraph.GetRootNamespaceByName(alias);
      
      // If no root namespace exists with the given alias, then create it.
      if (contextEntity == null)
      {
        var rootNamespaceEntity = new RootNamespaceEntity(alias);
        _SemanticGraph.AddRootNamespace(rootNamespaceEntity);
        contextEntity = rootNamespaceEntity;
      }

      // If the type has a namespace name, then create the corresponding namespace entity hierarchy.
      if (type.Namespace != null)
      {
        contextEntity = ImportNamespaceHierarchy((NamespaceEntity)contextEntity, type.Namespace);
        if (contextEntity == null) { return; }
      }

      // If it's a nested type, then we have to extract the type name hierarchy from the name, 
      // and traverse the entity graph till we reach the parent of the to-be-created-entity 
      var typeName = type.Namespace == null ? type.FullName : type.FullName.Remove(0, type.Namespace.Length + 1);
      var typeNameArray = typeName.Split('+');

      for (int i = 0; i < typeNameArray.Length-1; i++)
      {
        var nameAndTypeParameterCountArray = typeNameArray[i].Split('`');
        
        var typeParameterCount = nameAndTypeParameterCountArray.Length > 1
                                   ? int.Parse(nameAndTypeParameterCountArray[1])
                                   : 0;
        
        var foundEntity = (contextEntity is IHasChildTypes)
          ? (contextEntity as IHasChildTypes).GetSingleChildType<TypeEntity>(nameAndTypeParameterCountArray[0], typeParameterCount)
          : null;

        if (foundEntity == null)
        {
          throw new ApplicationException(string.Format("Type name '{0}' not found in the declaration space of '{1}'.",
                                                       typeNameArray[i], contextEntity.FullyQualifiedName));
        }
        contextEntity = foundEntity;
      }

      // If the candidate parent entity cannot have a child type, that's an error.
      if (!(contextEntity is IHasChildTypes))
      {
        throw new ApplicationException(string.Format("Expected IHasChildTypes entity but received '{0}'.",
                                                     contextEntity.GetType()));
      }

      // We have the parent entity for the type to be imported.
      var parentContextEntity = contextEntity as IHasChildTypes;

      // If a type with this name and number of type parameters already exists, that's an error.
      if (parentContextEntity.GetSingleChildType<TypeEntity>(type.Name, type.GetGenericArguments().Length) != null)
      {
        throw new ApplicationException(string.Format("Name '{0}' is already defined in declaration space '{1}'.",
                                                     type.Name,
                                                     contextEntity.FullyQualifiedName));
      }

      // Create entity for the type.
      var typeEntity = CreateTypeEntity(type);

      if (typeEntity != null)
      {
        // If it's a generic type, then add type parameters
        if (typeEntity is GenericCapableTypeEntity && type.IsGenericTypeDefinition)
        {
          var genericEntity = typeEntity as GenericCapableTypeEntity;
          foreach (var typeParameter in type.GetGenericArguments())
          {
            var typeParameterEntity = new TypeParameterEntity(typeParameter.Name);
            genericEntity.AddTypeParameter(typeParameterEntity);
            MapEntityToReflectedMetadata(typeParameterEntity, typeParameter);
          }
        }

        // Set the program that this type belongs to.
        typeEntity.Program = _Program;

        // Add the new type entity to the semantic graph.
        parentContextEntity.AddChildType(typeEntity);
        MapEntityToReflectedMetadata(typeEntity, type);

        // Assign this factory to the type entity object to enable lazy importing the members.
        typeEntity.MetadataImporterFactory = this;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Takes a multipart namespace name (eg. A.B.C) and creates a hierarchy of namespace entities
    /// in the semantic graph.
    /// </summary>
    /// <param name="contextEntity">The parent entity of the first created namespace.</param>
    /// <param name="namespaceName">A multipart namespace name.</param>
    /// <returns>The namespace entity created last.</returns>
    // ----------------------------------------------------------------------------------------------
    private NamespaceEntity ImportNamespaceHierarchy(NamespaceEntity contextEntity, string namespaceName)
    {
      // Loop through all parts of the namespace name
      foreach (var namespaceTag in namespaceName.Split('.'))
      {
        // Find out whether this namespace already exists
        var namespaceEntity = contextEntity.GetChildNamespace(namespaceTag);

        // If the namespace is not found, then we have to create it.
        if (namespaceEntity == null)
        {
          namespaceEntity = new NamespaceEntity(namespaceTag);
          contextEntity.AddChildNamespace(namespaceEntity);
        }

        contextEntity = namespaceEntity;
      }

      return contextEntity;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Maps a semantic entity to a reflected metadata object.
    /// </summary>
    /// <param name="entity">A semantic entity.</param>
    /// <param name="metadata">A reflected metadata object.</param>
    // ----------------------------------------------------------------------------------------------
    private void MapEntityToReflectedMetadata(SemanticEntity entity, MemberInfo metadata)
    {
      entity.ReflectedMetadata = metadata;
      _SemanticGraph.AddMetadataToEntityMapping(metadata, entity);
    }

    #endregion

    #region Private static methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a type entity from a reflected type.
    /// </summary>
    /// <param name="type">A reflected type.</param>
    /// <returns>The created type entity, or null if could not create.</returns>
    // ----------------------------------------------------------------------------------------------
    private static TypeEntity CreateTypeEntity(Type type)
    {
      // Remove generic marker tick and number from type name
      var nameArray = type.Name.Split('`');
      if (nameArray.Length > 3)
      {
        throw new ApplicationException(
          string.Format("Expected zero or one generic marker, but found more in name: '{0}'", type.Name));
      }
      var typeName = nameArray[0];

      var accessibility = GetAccessibility(type);

      // Create the appropriate kind of type entity
      TypeEntity typeEntity = null;

      if (type.IsClass && type.BaseType != null && type.BaseType.FullName == "System.MulticastDelegate")
      {
        typeEntity = new DelegateEntity(accessibility, typeName);
      }
      else
      {
        // type.FullName == "System.Enum" is a hack needed because reflection thinks that System.Enum is 
        // not a class, and not an enum, and not a value type, and not an interface. So what is it? We assume a class.
        if (type.IsClass || type.FullName == "System.Enum")
        {
          var classEntity = new ClassEntity(accessibility, typeName);
          classEntity.IsStatic = type.IsSealed && type.IsAbstract;
          classEntity.IsAbstract = type.IsAbstract;
          classEntity.IsSealed = type.IsSealed;
          typeEntity = classEntity;
        }
        else if (type.IsEnum)
        {
          typeEntity = new EnumEntity(accessibility, typeName);
        }
        else if (type.IsValueType)
        {
          typeEntity = new StructEntity(accessibility, typeName);
        }
        else if (type.IsInterface)
        {
          typeEntity = new InterfaceEntity(accessibility, typeName);
        }
        else
        {
          throw new ApplicationException(string.Format("Type '{0}' not handled by CreatSemanticEntityFromType", type));
        }
      }

      // Populate base type and base interfaces
      if (type.BaseType != null)
      {
        typeEntity.AddBaseTypeReference(new ReflectedTypeBasedTypeEntityReference(type.BaseType));
      }
      foreach (var interfaceItem in type.GetInterfaces())
      {
        typeEntity.AddBaseTypeReference(new ReflectedTypeBasedTypeEntityReference(interfaceItem));
      }

      return typeEntity;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the accessibility of a reflected type.
    /// </summary>
    /// <param name="type">A reflected type.</param>
    /// <returns>An AccessibilityKind value.</returns>
    // ----------------------------------------------------------------------------------------------
    private static AccessibilityKind GetAccessibility(System.Type type)
    {
      AccessibilityKind accessibility;

      if (type.IsNested)
      {
        if (type.IsNestedAssembly)
        {
          accessibility = AccessibilityKind.Assembly;
        }
        else if (type.IsNestedFamANDAssem)
        {
          accessibility = AccessibilityKind.FamilyAndAssembly;
        }
        else if (type.IsNestedFamily)
        {
          accessibility = AccessibilityKind.Family;
        }
        else if (type.IsNestedFamORAssem)
        {
          accessibility = AccessibilityKind.FamilyOrAssembly;
        }
        else if (type.IsNestedPrivate)
        {
          accessibility = AccessibilityKind.Private;
        }
        else if (type.IsNestedPublic)
        {
          accessibility = AccessibilityKind.Public;
        }
        else
        {
          throw new ApplicationException("Unable to determine accessibility.");
        }
      }
      else
      {
        if (type.IsPublic)
        {
          accessibility = AccessibilityKind.Public;
        }
        else
        {
          accessibility = AccessibilityKind.Assembly;
        }
      }

      return accessibility;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the accessibility of a reflected field.
    /// </summary>
    /// <param name="fieldInfo">A reflected field.</param>
    /// <returns>An AccessibilityKind value.</returns>
    // ----------------------------------------------------------------------------------------------
    private static AccessibilityKind GetAccessibility(FieldInfo fieldInfo)
    {
      AccessibilityKind accessibility;

      if (fieldInfo.IsAssembly)
      {
        accessibility = AccessibilityKind.Assembly;
      }
      else if (fieldInfo.IsFamilyAndAssembly)
      {
        accessibility = AccessibilityKind.FamilyAndAssembly;
      }
      else if (fieldInfo.IsFamily)
      {
        accessibility = AccessibilityKind.Family;
      }
      else if (fieldInfo.IsFamilyOrAssembly)
      {
        accessibility = AccessibilityKind.FamilyOrAssembly;
      }
      else if (fieldInfo.IsPrivate)
      {
        accessibility = AccessibilityKind.Private;
      }
      else if (fieldInfo.IsPublic)
      {
        accessibility = AccessibilityKind.Public;
      }
      else
      {
        throw new ApplicationException("Unable to determine accessibility.");
      }

      return accessibility;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the accessibility of a reflected method or accessor.
    /// </summary>
    /// <param name="methodBase">A reflected method or accessor.</param>
    /// <returns>An AccessibilityKind value.</returns>
    // ----------------------------------------------------------------------------------------------
    private static AccessibilityKind GetAccessibility(MethodBase methodBase)
    {
      AccessibilityKind accessibility;

      if (methodBase.IsAssembly)
      {
        accessibility = AccessibilityKind.Assembly;
      }
      else if (methodBase.IsFamilyAndAssembly)
      {
        accessibility = AccessibilityKind.FamilyAndAssembly;
      }
      else if (methodBase.IsFamily)
      {
        accessibility = AccessibilityKind.Family;
      }
      else if (methodBase.IsFamilyOrAssembly)
      {
        accessibility = AccessibilityKind.FamilyOrAssembly;
      }
      else if (methodBase.IsPrivate)
      {
        accessibility = AccessibilityKind.Private;
      }
      else if (methodBase.IsPublic)
      {
        accessibility = AccessibilityKind.Public;
      }
      else
      {
        throw new ApplicationException("Unable to determine accessibility.");
      }

      return accessibility;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a member entity from a reflected member.
    /// </summary>
    /// <param name="reflectedMember">A reflected member.</param>
    /// <returns>A MemberEntity, or null if could not create.</returns>
    // ----------------------------------------------------------------------------------------------
    private static MemberEntity CreateMemberEntity(MemberInfo reflectedMember)
    {
      MemberEntity memberEntity = null;

      switch (reflectedMember.MemberType)
      {
        case (MemberTypes.Field):
          memberEntity = CreateFieldEntity(reflectedMember as FieldInfo);
          break;

        case (MemberTypes.Property):
          memberEntity = CreatePropertyEntity(reflectedMember as PropertyInfo);
          break;

        case (MemberTypes.Method):
          //memberEntity = CreateMethodEntity(reflectedMember as MethodInfo);
          break;

        case (MemberTypes.Event):
          // TODO: implement
          break;

        case (MemberTypes.Constructor):
          // TODO: implement
          break;

        default:
          // Intentionally left blank. We don't care about other member types.
          break;
      }

      return memberEntity;
    }
    
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a field, a constant member, or an enum member from a reflected FieldInfo object.
    /// </summary>
    /// <param name="fieldInfo">A reflected FieldInfo object.</param>
    /// <returns>The created member entity, or null.</returns>
    // ----------------------------------------------------------------------------------------------
    private static MemberEntity CreateFieldEntity(FieldInfo fieldInfo)
    {
      MemberEntity memberEntity = null;

      bool isDeclaredInSource = false;
      AccessibilityKind? accessibility = GetAccessibility(fieldInfo);
      SemanticEntityReference<TypeEntity> typeReference = new ReflectedTypeBasedTypeEntityReference(fieldInfo.FieldType);
      string name = fieldInfo.Name;

      // Decide whether an enum member, a field or a constant member should be created.
      if (fieldInfo.DeclaringType.IsEnum && fieldInfo.IsStatic)
      {
        memberEntity = new EnumMemberEntity(name, typeReference);
      }
      else
      {
        if (fieldInfo.IsLiteral)
        {
          memberEntity = new ConstantMemberEntity(isDeclaredInSource, accessibility, typeReference, name);
        }
        else
        {
          memberEntity = new FieldEntity(isDeclaredInSource, accessibility, fieldInfo.IsStatic, typeReference, name, null);
        }
      }

      return memberEntity;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a property member from a reflected PropertyInfo object.
    /// </summary>
    /// <param name="propertyInfo">A reflected PropertyInfo object.</param>
    /// <returns>The created member entity, or null.</returns>
    // ----------------------------------------------------------------------------------------------
    private static MemberEntity CreatePropertyEntity(PropertyInfo propertyInfo)
    {
      MemberEntity memberEntity = null;

      AccessibilityKind? accessibility = null; // GetAccessibility(propertyInfo);
      bool isStatic = false; // (propertyInfo.GetGetMethod() ?? propertyInfo.GetSetMethod()).IsStatic;
      SemanticEntityReference<TypeEntity> typeReference = new ReflectedTypeBasedTypeEntityReference(propertyInfo.PropertyType);
      SemanticEntityReference<TypeEntity> interfaceReference = null; // propertyInfo.
      string name = propertyInfo.Name;

      memberEntity = new PropertyEntity(false, accessibility, isStatic, typeReference, interfaceReference, name, false);
      
      return memberEntity;
    }

    #endregion
  }
}
