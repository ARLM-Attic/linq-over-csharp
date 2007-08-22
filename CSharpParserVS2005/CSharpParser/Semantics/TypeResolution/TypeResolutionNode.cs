using System;

namespace CSharpParser.Semantics
{
  // ==================================================================================
  /// <summary>
  /// This class describes a type name node in the resolution tree.
  /// </summary>
  /// <remarks>
  /// One simple type name can be used by several types due to generics. This node
  /// is a container node for non-generic and generic types having the same name.
  /// </remarks>
  // ==================================================================================
  public sealed class TypeNameResolutionNode : ResolutionNodeBase
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new node with the specified parent and name.
    /// </summary>
    /// <param name="parentNode">Parent node.</param>
    /// <param name="name">The name of this node.</param>
    /// <remarks>
    /// The parent cannot be null.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public TypeNameResolutionNode(ResolutionNodeBase parentNode, string name)
      : base(parentNode, name)
    {
      if (parentNode == null) throw new ArgumentNullException("parentNode");
    }
  }

  // ==================================================================================
  /// <summary>
  /// This class describes a type node in the resolution tree.
  /// </summary>
  /// <remarks>
  /// This node is a child of TypeNameResolutionNode and its name is always the number
  /// of generic type parameters (0 in case of non-generic types).
  /// </remarks>
  // ==================================================================================
  public sealed class TypeResolutionNode : ResolutionNodeBase
  {
    #region Private fields

    private ITypeCharacteristics _Resolver;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new node with the specified parent and name.
    /// </summary>
    /// <param name="parentNode">Parent node.</param>
    /// <param name="type">Type behind this resolution node.</param>
    /// <remarks>
    /// The parent can be null (the node has no parent), name cannot be null or empty.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public TypeResolutionNode(ResolutionNodeBase parentNode, ITypeCharacteristics type)
      : base(parentNode, type.TypeParameterCount.ToString())
    {
      _Resolver = type;
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the first resolvers for this type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public ITypeCharacteristics Resolver
    {
      get { return _Resolver; }
      set { _Resolver = value; }
    }

    #endregion
  }
}
