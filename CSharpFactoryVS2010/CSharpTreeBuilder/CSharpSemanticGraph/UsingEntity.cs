using System;
namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This abstract class represents a using directive in the semantic graph.
  /// </summary>
  // ================================================================================================
  public abstract class UsingEntity : SemanticEntity, IHasLexicalScope
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="UsingEntity"/> class.
    /// </summary>
    /// <param name="lexicalScope">The region of program text where the using entity has effect.</param>
    // ----------------------------------------------------------------------------------------------
    protected UsingEntity(SourceRegion lexicalScope)
    {
      if (lexicalScope==null)
      {
        throw new ArgumentNullException("lexicalScope");
      }

      LexicalScope = lexicalScope;
    }

    // Note that this type of entity cannot be affected by type arguments, so no generic clone support here.

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the region of program text where this object has effect on.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public SourceRegion LexicalScope { get; private set; }

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
