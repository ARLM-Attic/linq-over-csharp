using CSharpTreeBuilder.Ast;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a reference to a namespace or type entity.
  /// </summary>
  /// <remarks>Type-specific features are in TypeEntityReference class.</remarks>
  // ================================================================================================
  public class NamespaceOrTypeEntityReference
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
    /// Gets the resolved namespace or type entity.
    /// </summary>
    /// <remarks>
    /// In case of a constructed type, ResolvedEntity property contains the underlying type.
    /// For the whole constructed type see the TypeReference.TypeEntity property.
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    public NamespaceOrTypeEntity ResolvedEntity { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Sets the reference to resolved state, and sets the resolved entity.
    /// </summary>
    /// <param name="resolvedEntity">The result of the resolution.</param>
    // ----------------------------------------------------------------------------------------------
    public virtual void SetResolved(NamespaceOrTypeEntity resolvedEntity)
    {
      ResolvedEntity = resolvedEntity;
      ResolutionState = ResolutionState.Resolved;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Sets the reference to unresolvable state.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public void SetUnresolvable()
    {
      ResolvedEntity = null;
      ResolutionState = ResolutionState.Unresolvable;
    }
  }
}
