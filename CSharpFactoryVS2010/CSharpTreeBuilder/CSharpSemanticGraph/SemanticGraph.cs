using System;
using System.Collections.Generic;
using CSharpTreeBuilder.CSharpSemanticGraphBuilder;

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
    private readonly Dictionary<string, RootNamespaceEntity> _RootNamespaces;

    /// <summary>A cache that maps metadata objects to semantic entities.</summary>
    private readonly Dictionary<System.Reflection.MemberInfo, SemanticEntity> _MetadataToEntityMap;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="SemanticGraph"/> class.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public SemanticGraph()
    {
      _RootNamespaces = new Dictionary<string, RootNamespaceEntity>();
      AddRootNamespace(new RootNamespaceEntity(GLOBAL_NAMESPACE_NAME));

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
    /// Gets the semantic entity for a built-in type.
    /// </summary>
    /// <param name="builtInType">A built-in type.</param>
    /// <returns>The semantic entity representing a built-in type. Null if not found.</returns>
    // ----------------------------------------------------------------------------------------------
    public TypeEntity GetTypeEntityByBuiltInType(BuiltInType builtInType)
    {
      System.Type type = null;

      switch (builtInType)
      {
        case (BuiltInType.Sbyte):
          type = typeof(sbyte);
          break;
        case (BuiltInType.Byte):
          type = typeof(byte);
          break;
        case (BuiltInType.Short):
          type = typeof(short);
          break;
        case (BuiltInType.Ushort):
          type = typeof(ushort);
          break;
        case (BuiltInType.Int):
          type = typeof(int);
          break;
        case (BuiltInType.Uint):
          type = typeof(uint);
          break;
        case (BuiltInType.Long):
          type = typeof(long);
          break;
        case (BuiltInType.Ulong):
          type = typeof(ulong);
          break;
        case (BuiltInType.Char):
          type = typeof(char);
          break;
        case (BuiltInType.Float):
          type = typeof(float);
          break;
        case (BuiltInType.Double):
          type = typeof(double);
          break;
        case (BuiltInType.Bool):
          type = typeof(bool);
          break;
        case (BuiltInType.Decimal):
          type = typeof(decimal);
          break;
        case (BuiltInType.Object):
          type = typeof(object);
          break;
        case (BuiltInType.String):
          type = typeof(string);
          break;
        case (BuiltInType.Void):
          type = typeof(void);
          break;
        default:
          throw new ApplicationException(string.Format("Unexpected BuiltInType: '{0}'", builtInType));
      }

      return _MetadataToEntityMap.ContainsKey(type) ? _MetadataToEntityMap[type] as TypeEntity : null;
    }

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

    #region Visitor methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Accepts a visitor object, according to the Visitor pattern.
    /// </summary>
    /// <param name="visitor">A visitor object</param>
    // ----------------------------------------------------------------------------------------------
    public void AcceptVisitor(SemanticGraphVisitor visitor)
    {
      foreach (var rootNamespace in RootNamespaces)
      {
        rootNamespace.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}
