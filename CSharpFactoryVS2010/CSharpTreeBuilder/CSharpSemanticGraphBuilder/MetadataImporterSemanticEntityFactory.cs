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
    /// <summary>Error handler object used for reporting compilation messages.</summary>
    private ICompilationErrorHandler _ErrorHandler;

    /// <summary>The semantic graph that this factory is working on.</summary>
    private SemanticGraph _SemanticGraph;

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
    /// Creates semantic entities from all the types defined in an assembly and adds them to a specified semantic graph.
    /// </summary>
    /// <param name="filename">The name of the assembly file.</param>
    /// <param name="alias">A valid C# identifier that will represent a root namespace 
    /// that will contain all namespaces in the assembly. If null, then "global" is assumed.</param>
    // ----------------------------------------------------------------------------------------------
    public void CreateEntitiesFromAssembly(string filename, string alias)
    {
      if (filename == null) { throw new ArgumentNullException("filename"); }
      if (alias == null) { alias = _SemanticGraph.GlobalNamespace.Name; }

      Assembly assembly;

      try
      {
        assembly = Assembly.ReflectionOnlyLoadFrom(filename);
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

      foreach (var type in assembly.GetExportedTypes())
      {
        CreateEntitiesFromReflectedType(type, alias);
      }

      return;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates semantic entities (namespace and type) from a reflected type and adds them to a specified semantic graph.
    /// </summary>
    /// <param name="type">The type to be added to the semantic graph.</param>
    /// <param name="alias">A valid C# identifier that will represent a root namespace that will contain the type.</param>
    // ----------------------------------------------------------------------------------------------
    private void CreateEntitiesFromReflectedType(Type type, string alias)
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
      if (contextEntity==null)
      {
        var rootNamespaceEntity = new RootNamespaceEntity(alias);
        _SemanticGraph.AddRootNamespace(rootNamespaceEntity);
        contextEntity = rootNamespaceEntity;
      }

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
          var namespaceEntity = new NamespaceEntity(namespaceTag);
          contextEntity.AddChildNamespace(namespaceEntity);
          
          // The newly created namespace will be the context for the next part of the namespace name.
          contextEntity = namespaceEntity;
        }
        else
        {
          // If the namespace name is found, but is not definite then we stop creating new entities.
          if (nameTableEntry.State != NameTableEntryState.Definite)
          {
            _ErrorHandler.Warning("TBD", null, "Ambigous name '{0}' found in declaration space '{1}'.",
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
    private void CreateTypeEntityFromReflectedType(NamespaceOrTypeEntity contextEntity, Type type)
    {
      // Find out whether this name already exists in this declaration space.
      var nameTableEntry = contextEntity.DeclarationSpace[type.Name
                                                          +
                                                          (type.ContainsGenericParameters
                                                             ? "`" + type.GetGenericArguments().Length
                                                             : "")];

      // If the name already exists, that's an error
      if (nameTableEntry != null)
      {
        throw new ApplicationException(string.Format("Name '{0}' is already defined in declaration space '{1}'.",
                                                     type.Name,
                                                     contextEntity.FullyQualifiedName));
      }

      if (!(contextEntity is IHasChildTypes))
      {
        throw new ApplicationException(string.Format("Expected IHasChildTypes entity but received '{0}'.",
                                                     contextEntity.GetType()));
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
        typeEntity = new DelegateEntity(typeName);
      }
      else
        // type.FullName == "System.Enum" is a hack needed because reflection thinks that System.Enum is 
        // not a class, and not an enum, and not a value type, and not an interface. So what is it? We assume a class.
        if (type.IsClass || type.FullName == "System.Enum")
        {
          typeEntity = new ClassEntity(typeName);
        }
        else if (type.IsEnum)
        {
          typeEntity = new EnumEntity(typeName);
        }
        else if (type.IsValueType)
        {
          typeEntity = new StructEntity(typeName);
        }
        else if (type.IsInterface)
        {
          typeEntity = new InterfaceEntity(typeName);
        }
        else
        {
          throw new ApplicationException(string.Format("Type '{0}' not handled by CreatSemanticEntityFromType", type));
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

      if (typeEntity != null)
      {
        parentContextEntity.AddChildType(typeEntity);
        MapEntityToReflectedMetadata(typeEntity, type);
      }
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
  }
}
