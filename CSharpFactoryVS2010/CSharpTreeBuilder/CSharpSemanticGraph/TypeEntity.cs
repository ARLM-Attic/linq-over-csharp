using System.Collections.Generic;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This abstract class represents a type entity in the semantic graph.
  /// </summary>
  // ================================================================================================
  public abstract class TypeEntity : NamespaceOrTypeEntity
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeEntity"/> class.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    protected TypeEntity()
    {
      BaseTypes = new List<NamespaceOrTypeEntityReference>();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the collection of base types.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public List<NamespaceOrTypeEntityReference> BaseTypes { get; private set; }

    #region Visitor methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Accepts a visitor object, according to the Visitor pattern.
    /// </summary>
    /// <param name="visitor">A visitor object</param>
    // ----------------------------------------------------------------------------------------------
    public override void AcceptVisitor(SemanticGraphVisitor visitor)
    {
      visitor.Visit(this);

      base.AcceptVisitor(visitor);
    }

    #endregion
  }
}