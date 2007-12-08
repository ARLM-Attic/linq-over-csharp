using System;

namespace CSharpParser.Semantics
{
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

    private ITypeAbstraction _Resolver;

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
    public TypeResolutionNode(ResolutionNodeBase parentNode, ITypeAbstraction type)
      : base(parentNode, type.Name)
    {
      _Resolver = type;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the full name of the type or namespace represented by this node.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override string FullName
    {
      get
      {
        int pos = Name.IndexOf('`');
        int argCount = pos < 0 ? 0 : Int32.Parse(Name.Substring(pos+1));
        return _Resolver.SimpleName +  "<" + "".PadRight(argCount, ',') + ">";
      }
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the first resolvers for this type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public ITypeAbstraction Resolver
    {
      get { return _Resolver; }
      set { _Resolver = value; }
    }

    #endregion
  }
}
