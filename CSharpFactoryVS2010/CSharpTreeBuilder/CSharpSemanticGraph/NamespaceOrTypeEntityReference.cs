using CSharpTreeBuilder.Ast;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a reference to a type entity.
  /// </summary>
  // ================================================================================================
  public sealed class NamespaceOrTypeEntityReference
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="NamespaceOrTypeEntityReference"/> class.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public NamespaceOrTypeEntityReference(TypeOrNamespaceNode typeOrNamespaceNode)
    {
      SyntaxNode = typeOrNamespaceNode;
      ResolutionState = ResolutionState.NotYetResolved;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the syntax node that the reference is based upon.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeOrNamespaceNode SyntaxNode { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the state of the resolution of the reference.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ResolutionState ResolutionState { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the resolved entity.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public NamespaceOrTypeEntity ResolvedEntity { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Sets the reference to resolved state, and sets the resolved entity.
    /// </summary>
    /// <param name="resolvedEntity">The result of the resolution.</param>
    // ----------------------------------------------------------------------------------------------
    public void Resolve(NamespaceOrTypeEntity resolvedEntity)
    {
      ResolvedEntity = resolvedEntity;
      ResolutionState = ResolutionState.Resolved;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Sets the reference to unresolvable state.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public void Unresolvable()
    {
      ResolvedEntity = null;
      ResolutionState = ResolutionState.Unresolvable;
    }
  }
}
