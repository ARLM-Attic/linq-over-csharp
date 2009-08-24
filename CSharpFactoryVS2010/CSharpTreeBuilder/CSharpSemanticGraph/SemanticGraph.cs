using System;
using System.Collections.Generic;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class holds all the semantic information about a project 
  /// (a project is a group of source files compiled together and a group of referenced assemblies).
  /// </summary>
  // ================================================================================================
  public sealed class SemanticGraph
  {
    /// <summary>The name of the global namespace.</summary>
    private const string GLOBAL_NAMESPACE_NAME = "global";

    /// <summary>A dictionary of root namespace entities. The key is the name of the root namespace.</summary>
    private Dictionary<string, RootNamespaceEntity> _RootNamespaces;

    /// <summary>A dictionary of built-in types. The key is the name of the built-in type.</summary>
    private Dictionary<string, BuiltInTypeEntity> _BuiltInTypes;

    /// <summary>A cache that maps metadata objects to semantic entities.</summary>
    private Dictionary<System.Reflection.MemberInfo, SemanticEntity> _MetadataToEntityMap;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="SemanticGraph"/> class.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public SemanticGraph()
    {
      _RootNamespaces = new Dictionary<string, RootNamespaceEntity>();
      AddRootNamespace(new RootNamespaceEntity(GLOBAL_NAMESPACE_NAME));
      
      _BuiltInTypes = new Dictionary<string, BuiltInTypeEntity>();
      InitializeBuiltInTypeDictionary();

      PointerToUnknownType = new PointerToUnknownTypeEntity();

      _MetadataToEntityMap = new Dictionary<System.Reflection.MemberInfo, SemanticEntity>();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an iterate-only collection of root namespace entities. The key is the namespace name.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public IEnumerable<RootNamespaceEntity> RootNamespaces
    {
      get { return _RootNamespaces.Values; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds an root namespace entity to the semantic graph.
    /// </summary>
    /// <param name="rootNamespaceEntity">A root namespace entity.</param>
    // ----------------------------------------------------------------------------------------------
    public void AddRootNamespace(RootNamespaceEntity rootNamespaceEntity)
    {
      _RootNamespaces.Add(rootNamespaceEntity.Name, rootNamespaceEntity);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a root namespace by name.
    /// </summary>
    /// <param name="rootNamespaceName">A root namespace name.</param>
    /// <returns>A root namespace entity with the given name, or null if not found.</returns>
    // ----------------------------------------------------------------------------------------------
    public RootNamespaceEntity GetRootNamespaceByName(string rootNamespaceName)
    {
      if (_RootNamespaces.ContainsKey(rootNamespaceName))
      {
        return _RootNamespaces[rootNamespaceName];
      }
      return null;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the global namespace entity.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public RootNamespaceEntity GlobalNamespace
    {
      get { return _RootNamespaces[GLOBAL_NAMESPACE_NAME]; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an iterate-only collection of root namespace entities. The key is the namespace name.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public IEnumerable<BuiltInTypeEntity> BuiltInTypes
    {
      get { return _BuiltInTypes.Values; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the semantic entity for a built-in type name.
    /// </summary>
    /// <param name="name">A built-in type name.</param>
    /// <returns>The semantic entity representing a built-in type. Null if not found.</returns>
    // ----------------------------------------------------------------------------------------------
    public BuiltInTypeEntity GetBuiltInTypeByName(string name)
    {
      return _BuiltInTypes.ContainsKey(name) ? _BuiltInTypes[name] : null;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the pointer-to-unknown-type singleton entity.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public PointerToUnknownTypeEntity PointerToUnknownType { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the System.Nullable&lt;&gt; type definition.
    /// </summary>
    /// <remarks>If mscorlib is not yet imported, then returns null.</remarks>
    // ----------------------------------------------------------------------------------------------
    public StructEntity NullableGenericTypeDefinition
    {
      get
      {
        return GetEntityByMetadataObject(typeof (System.Nullable<>)) as StructEntity;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the System.Array type.
    /// </summary>
    /// <remarks>If mscorlib is not yet imported, then returns null.</remarks>
    // ----------------------------------------------------------------------------------------------
    public ClassEntity SystemArray
    {
      get
      {
        return GetEntityByMetadataObject(typeof(System.Array)) as ClassEntity;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a metadataObject+semanticEntity pair to the mapping cache.
    /// </summary>
    /// <param name="metadataObject">A metadata object.</param>
    /// <param name="semanticEntity">A semantic entity.</param>
    // ----------------------------------------------------------------------------------------------
    public void AddMetadataToEntityMapping(System.Reflection.MemberInfo metadataObject, SemanticEntity semanticEntity)
    {
      if (!_MetadataToEntityMap.ContainsKey(metadataObject))
      {
        _MetadataToEntityMap.Add(metadataObject, semanticEntity);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the semantic entity that is mapped to the given metadata object.
    /// </summary>
    /// <param name="metadataObject">A metadata object.</param>
    /// <returns>The semantic entity mapped to the metadata object. Null if not found.</returns>
    // ----------------------------------------------------------------------------------------------
    public SemanticEntity GetEntityByMetadataObject(System.Reflection.MemberInfo metadataObject)
    {
        return _MetadataToEntityMap.ContainsKey(metadataObject) ? _MetadataToEntityMap[metadataObject] : null;
    }

    #region Private methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Fills up the dictionary that maps built-in type names to semantic entities.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    private void InitializeBuiltInTypeDictionary()
    {
      _BuiltInTypes.Add("sbyte", new BuiltInTypeEntity(BuiltInType.Sbyte));
      _BuiltInTypes.Add("byte", new BuiltInTypeEntity(BuiltInType.Byte));
      _BuiltInTypes.Add("short", new BuiltInTypeEntity(BuiltInType.Short));
      _BuiltInTypes.Add("ushort",new BuiltInTypeEntity(BuiltInType.Ushort));
      _BuiltInTypes.Add("int",new BuiltInTypeEntity(BuiltInType.Int));
      _BuiltInTypes.Add("uint",new BuiltInTypeEntity(BuiltInType.Uint));
      _BuiltInTypes.Add("long",new BuiltInTypeEntity(BuiltInType.Long));
      _BuiltInTypes.Add("ulong",new BuiltInTypeEntity(BuiltInType.Ulong));
      _BuiltInTypes.Add("char",new BuiltInTypeEntity(BuiltInType.Char));
      _BuiltInTypes.Add("float",new BuiltInTypeEntity(BuiltInType.Float));
      _BuiltInTypes.Add("double",new BuiltInTypeEntity(BuiltInType.Double));
      _BuiltInTypes.Add("bool",new BuiltInTypeEntity(BuiltInType.Bool));
      _BuiltInTypes.Add("decimal",new BuiltInTypeEntity(BuiltInType.Decimal));
      _BuiltInTypes.Add("object",new BuiltInTypeEntity(BuiltInType.Object));
      _BuiltInTypes.Add("string",new BuiltInTypeEntity(BuiltInType.String));
      _BuiltInTypes.Add("void", new BuiltInTypeEntity(BuiltInType.Void));
    }

    #endregion

    #region Visitor methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Accepts a visitor object, according to the Visitor pattern.
    /// </summary>
    /// <param name="visitor">A visitor object</param>
    // ----------------------------------------------------------------------------------------------
    public void AcceptVisitor(SemanticGraphVisitor visitor)
    {
      foreach (var builtInType in BuiltInTypes)
      {
        builtInType.AcceptVisitor(visitor);
      }

      foreach (var rootNamespace in RootNamespaces)
      {
        rootNamespace.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}
