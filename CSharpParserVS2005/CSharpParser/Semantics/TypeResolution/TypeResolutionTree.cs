using System;
using System.Collections.Generic;

namespace CSharpParser.Semantics
{
  // ==================================================================================
  /// <summary>
  /// This class represents an abstract tree that can be used for resolving types
  /// within an assembly or within the source code.
  /// </summary>
  // ==================================================================================
  public abstract class TypeResolutionTree : ResolutionNodeBase
  {
    #region Private fields

    // --- Contains a shortcut cache to accelerate nested namespace node access
    private readonly Dictionary<string, NamespaceResolutionNode> _Cache =
      new Dictionary<string, NamespaceResolutionNode>();

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new instance of the resolution tree.
    /// </summary>
    /// <param name="parentNode">Parent node</param>
    /// <param name="name">Resolution tree name</param>
    // --------------------------------------------------------------------------------
    protected TypeResolutionTree(ResolutionNodeBase parentNode, string name)
      : 
      base(parentNode, name)
    {
    }

    #endregion

    #region public methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Clear all nodes of this hierarchy.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override void Clear()
    {
      base.Clear();
      _Cache.Clear();
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the resolver node belonging to the specified namespace key.
    /// </summary>
    /// <param name="nsKey">Namespace key</param>
    /// <returns>
    /// The resolver node belonging to the namespace if found; otherwise, null.
    /// </returns>
    // --------------------------------------------------------------------------------
    public NamespaceResolutionNode this[string nsKey]
    {
      get
      {
        NamespaceResolutionNode result;
        if (_Cache.TryGetValue(nsKey, out result)) return result;
        return null;
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Imports the type to the specified resolver node and creates the hierarchy
    /// according to type nesting.
    /// </summary>
    /// <param name="type">Type to import.</param>
    /// <returns>Type resolution node of the specified type.</returns>
    /// <remarks>
    /// If the type is a nested type, first imports its declaring type.
    /// </remarks>
    // --------------------------------------------------------------------------------
    //public TypeResolutionNode ImportTypeToHierarchy(ITypeAbstraction type)
    //{
    //  if (type.DeclaringType == null)
    //  {
    //    // --- This is a simple type
    //    return ImportType(this, type);
    //  }
    //  else
    //  {
    //    // --- This is a type nested in an other type. We must provide that the declaring 
    //    // --- type is imported.
    //    TypeResolutionNode typeResolver =
    //      ImportTypeToHierarchy(type.DeclaringType);
    //    return ImportType(typeResolver, type);
    //  }
    //}

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Imports the type to the specified resolver node.
    /// </summary>
    /// <param name="resolverRoot">Resolver node</param>
    /// <param name="type">Type to import.</param>
    /// <returns>Type resolution node of the specified type.</returns>
    // --------------------------------------------------------------------------------
    //public TypeResolutionNode ImportType(ResolutionNodeBase resolverRoot, 
    //  ITypeCharacteristics type)
    //{
    //  // --- Wrap the type
    //  TypeResolutionNode typeResolver;
    //  // --- This is a type not nested in any type. Register it for the root resolver node.
    //  ResolutionNodeBase resolver;
    //  if ((resolver = resolverRoot.FindChild(type.SimpleName)) != null)
    //  {
    //    // --- There is a node with this type name
    //    typeResolver = resolver as TypeResolutionNode;
    //    if (typeResolver == null)
    //    {
    //      throw new InvalidOperationException(
    //        String.Format("Type ({0}) and namespace conflict within the assembly: {1}",
    //        type.FullName, type.DeclaringUnit.Name));
    //    }
    //  }
    //  else
    //  {
    //    // --- There is no node for this type
    //    typeResolver = new TypeResolutionNode(resolverRoot, type);
    //  }
    //  return typeResolver;
    //}

    #endregion

    #region Type resolution methods

    #endregion
  }
}