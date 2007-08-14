using System;
using System.Collections.Generic;

namespace CSharpParser.Semantics
{
  // ==================================================================================
  /// <summary>
  /// This class describes a type node in the resolution tree.
  /// </summary>
  /// <remarks>
  /// One simple type name can be used by several types due to generics. This node
  /// keeps a list of types having the same name.
  /// </remarks>
  // ==================================================================================
  public sealed class TypeResolutionNode : ResolutionNodeBase
  {
    #region Private fields

    private readonly List<ITypeCharacteristics> _Resolvers = 
      new List<ITypeCharacteristics>();

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new node with the specified parent and name.
    /// </summary>
    /// <param name="parentNode">Parent node.</param>
    /// <param name="name">Node name.</param>
    /// <remarks>
    /// The parent can be null (the node has no parent), name cannot be null or empty.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public TypeResolutionNode(ResolutionNodeBase parentNode, string name)
      : base(parentNode, name)
    {
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the list of resolvers for this type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public List<ITypeCharacteristics> Resolvers
    {
      get { return _Resolvers; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this type has only and exactly one resolver.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsUnambigous
    {
      get { return _Resolvers.Count == 1; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the resolver for an unambigously resolved type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public ITypeCharacteristics Resolver
    {
      get
      {
        if (!IsUnambigous) 
          throw new InvalidOperationException("This type is not unambigously resolved!");
        return _Resolvers[0];
      }
    }

    #endregion

    #region Public methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Adds a new resolver for this type.
    /// </summary>
    /// <param name="type">Type to add to the resolver list.</param>
    // --------------------------------------------------------------------------------
    public void AddTypeResolver(ITypeCharacteristics type)
    {
      _Resolvers.Add(type);
    }

    #endregion
  }
}
