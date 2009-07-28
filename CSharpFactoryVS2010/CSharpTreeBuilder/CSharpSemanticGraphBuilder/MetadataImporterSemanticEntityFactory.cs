using System;
using System.Reflection;
using System.Collections.Generic;
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
    /// <summary>The project used for reporting compilation messages.</summary>
    private CSharpProject _Project;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="MetadataImporterSemanticEntityFactory"/> class.
    /// </summary>
    /// <param name="project">The project used for reporting compilation messages.</param>
    // ----------------------------------------------------------------------------------------------
    public MetadataImporterSemanticEntityFactory(CSharpProject project)
    {
      _Project = project;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates semantic entities from all the types defined in an assembly and adds them to a specified semantic graph.
    /// </summary>
    /// <param name="filename">The name of the assembly file.</param>
    /// <param name="alias">A valid C# identifier that will represent a root namespace 
    /// that will contain all namespaces in the assembly. If null, then "global" is assumed.</param>
    /// <param name="semanticGraph">The new entities will be added to this semantic graph.</param>
    // ----------------------------------------------------------------------------------------------
    public void CreateEntitiesFromAssembly(string filename, string alias, SemanticGraph semanticGraph)
    {
      if (filename == null) { throw new ArgumentNullException("filename"); }
      if (semanticGraph == null) { throw new ArgumentNullException("semanticGraph"); }
      if (alias == null) { alias = semanticGraph.GlobalNamespace.Name; }

      Assembly assembly;

      try
      {
        assembly = Assembly.ReflectionOnlyLoadFrom(filename);
      }
      catch (System.IO.FileNotFoundException)
      {
        ((ICompilationErrorHandler) _Project).Error("CS0006", null, "Metadata file '{0}' could not be found.", filename);
        return;
      }
      catch (System.BadImageFormatException)
      {
        ((ICompilationErrorHandler) _Project).Error("CS0009", null, 
          "Metadata file '{0}' could not be opened. -- An attempt was made to load a program with an incorrect format.", filename);
        return;
      }

      foreach (var type in assembly.GetExportedTypes())
      {
        CreateEntitiesFromReflectedType(type, alias, semanticGraph);
      }

      return;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates semantic entities (namespace and type) from a reflected type and adds them to a specified semantic graph.
    /// </summary>
    /// <param name="type">The type to be added to the semantic graph.</param>
    /// <param name="alias">A valid C# identifier that will represent a root namespace that will contain the type.</param>
    /// <param name="semanticGraph">The created entities will be added to this graph.</param>
    // ----------------------------------------------------------------------------------------------
    private void CreateEntitiesFromReflectedType(Type type, string alias, SemanticGraph semanticGraph)
    {
      if (type == null)
      {
        throw new ArgumentNullException("type");
      }
      if (alias == null)
      {
        throw new ArgumentNullException("alias");
      }
      if (semanticGraph == null)
      {
        throw new ArgumentNullException("semanticGraph");
      }

      // The context entity is our current position in the semantic graph.
      NamespaceOrTypeEntity contextEntity = semanticGraph.GetRootNamespaceByName(alias);

      // If the type has a namespace name, then create the corresponding namespace entity hierarchy.
      if (type.Namespace != null)
      {
        contextEntity = CreateNamespaceHierarchyFromNamespaceName((NamespaceEntity)contextEntity, type.Namespace);
        if (contextEntity==null) { return; }
      }

      // If it's a nested type, then we have to extract the type name hierarchy from the name, 
      // and traverse the entity graph till we reach the parent of the to-be-created-entity 
      var typeName = type.Namespace == null ? type.FullName : type.FullName.Remove(0, type.Namespace.Length + 1);
      var typeNameArray = typeName.Split('+');

      for (int i = 0; i < typeNameArray.Length-1; i++)
      {
        var nameTableEntry = contextEntity.DeclarationSpace[typeNameArray[i]];
        if (nameTableEntry == null)
        {
          throw new ApplicationException(string.Format("Type name '{0}' not found in the declaration space of '{1}'.",
                                                       typeNameArray[i], contextEntity.FullyQualifiedName));
        }

        if (nameTableEntry.State == NameTableEntryState.Definite && nameTableEntry.Entity is TypeEntity)
        {
          contextEntity = nameTableEntry.Entity as TypeEntity;
        }
        else
        {
          throw new ApplicationException(
            string.Format("Error traversing type hierarchy. NameTableEntry name='{0}', state='{1}', type='{2}'.",
                          nameTableEntry.Name, 
                          Enum.GetName(typeof (NameTableEntryState), nameTableEntry.State),
                          nameTableEntry.Entity.GetType()));
        }
      }

      // Create entity for the type
      CreateTypeEntityFromReflectedType(contextEntity, type);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Takes a multipart namespace name (eg. A.B.C) and creates a hierarchy of namespace entities,
    /// starting from contextEntity.
    /// </summary>
    /// <param name="contextEntity">The starting context entity of the namespace entity creation.</param>
    /// <param name="namespaceName">A multipart namespace name.</param>
    /// <returns>The resulting context entity. Null if there was an ambigous name in the hierarchy.</returns>
    // ----------------------------------------------------------------------------------------------
    private NamespaceEntity CreateNamespaceHierarchyFromNamespaceName(NamespaceEntity contextEntity, string namespaceName)
    {
      // Loop through all parts of the namespace name
      foreach (var namespaceTag in namespaceName.Split('.'))
      {
        // Find out whether this namespace already exists
        var nameTableEntry = contextEntity.DeclarationSpace[namespaceTag];

        // If the namespace is not found, then we have to create it.
        if (nameTableEntry == null)
        {
          var namespaceEntity = new NamespaceEntity() {Name = namespaceTag};
          contextEntity.AddChildNamespace(namespaceEntity);
          
          // The newly created namespace will be the context for the next part of the namespace name.
          contextEntity = namespaceEntity;
        }
        else
        {
          // If the namespace name is found, but is not definite then we stop creating new entities.
          if (nameTableEntry.State != NameTableEntryState.Definite)
          {
            ((ICompilationErrorHandler) _Project).Warning("TBD", null,
                                                          "Ambigous name '{0}' found in declaration space '{1}'.",
                                                          namespaceTag, contextEntity.FullyQualifiedName);
            return null;
          }

          // If the namespace name is found and is definite, but is not a namespace then throw an exception
          if (!(nameTableEntry.Entity is NamespaceEntity))
          {
            throw new ApplicationException(
              string.Format("Expected to find a namespace with name '{0}', but found a '{1}'.", namespaceTag,
                            nameTableEntry.Entity.GetType()));
          }

          // The namespace name is found, and definite, and denotes a namespace, so it will be our next contextEntity.
          contextEntity = nameTableEntry.Entity as NamespaceEntity;
        }
      }

      return contextEntity;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a type entity from a reflected type.
    /// </summary>
    /// <param name="contextEntity">The context where the new type will be created.</param>
    /// <param name="type">A reflected type.</param>
    // ----------------------------------------------------------------------------------------------
    private static void CreateTypeEntityFromReflectedType(NamespaceOrTypeEntity contextEntity, Type type)
    {
      // Find out whether this name already exists in this declaration space.
      var nameTableEntry = contextEntity.DeclarationSpace[type.Name 
                            + (type.ContainsGenericParameters ? "`" + type.GetGenericArguments().Length : "")];

      // If the name already exists, that's an error
      if (nameTableEntry!=null)
      {
        throw new ApplicationException(string.Format("Name '{0}' is already defined in declaration space '{1}'.", type.Name,
                                                     contextEntity.FullyQualifiedName));
      }

      if (!(contextEntity is IHasChildTypes))
      {
        throw new ApplicationException(string.Format("Expected IHasChildTypes entity but received '{0}'.", contextEntity.GetType()));
      }

      var parentContextEntity = contextEntity as IHasChildTypes;

      // Remove generic marker tick and number from type name
      var nameArray = type.Name.Split('`');
      if (nameArray.Length > 3)
      {
        throw new ApplicationException(
          string.Format("Expected zero or one generic marker, but found more in name: '{0}'", type.Name));
      }
      var typeName = nameArray[0];

      // Create the appropriate kind of type entity
      TypeEntity typeEntity = null;

      if (type.IsClass && type.BaseType != null && type.BaseType.FullName == "System.MulticastDelegate")
      {
        typeEntity = new DelegateEntity() { Name = typeName };
      }
      else if (type.IsClass || type.FullName == "System.Enum")
      // type.FullName == "System.Enum" is a hack needed because reflection thinks that System.Enum is 
      // not a class, and not an enum, and not a value type, and not an interface. So what is it? We assume a class.
      {
        typeEntity = new ClassEntity() {Name = typeName};
      }
      else if (type.IsEnum)
      {
        typeEntity = new EnumEntity() { Name = typeName };
      }
      else if (type.IsValueType)
      {
        typeEntity = new StructEntity() { Name = typeName };
      }
      else if (type.IsInterface)
      {
        typeEntity = new InterfaceEntity() { Name = typeName };
      }
      else
      {
        throw new ApplicationException(string.Format("Type '{0}' not handled by CreatSemanticEntityFromType", type));
      }

      // If it's a generic type, then add type parameters
      if (typeEntity is GenericCapableTypeEntity && type.IsGenericTypeDefinition)
      {
        var genericEntity = typeEntity as GenericCapableTypeEntity;
        foreach (var typeParamter in type.GetGenericArguments())
        {
          // For nested types, reflection return the parent types' type parameters as well, so we have to filter them out
          if (!ArgumentBelongsToParentType(typeParamter.Name, type))
          {
            var typeParameterEntity = new TypeParameterEntity(typeParamter.Name);
            genericEntity.AddTypeParameter(typeParameterEntity);
          }
        }
      }

      if (typeEntity != null)
      {
        parentContextEntity.AddChildType(typeEntity);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Returns a value indicating whether the given type parameter name belongs to one of the parent types.
    /// </summary>
    /// <param name="typeParameterName">A type parameter name.</param>
    /// <param name="type">The type whose parents will be searched for the type parameter.</param>
    /// <returns>True, if the type parameter name belongs to a parent type, not directly to the supplied type entity.</returns>
    // ----------------------------------------------------------------------------------------------
    private static bool ArgumentBelongsToParentType(string typeParameterName, Type type)
    {
      if (type.DeclaringType == null)
      {
        return false;
      }

      if (type.DeclaringType.IsGenericTypeDefinition)
      {
        foreach (var typeParameter in type.DeclaringType.GetGenericArguments())
        {
          if (typeParameter.Name == typeParameterName)
          {
            return true;
          }
        }
      }

      return ArgumentBelongsToParentType(typeParameterName, type.DeclaringType);
    }
  }
}
