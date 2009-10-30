using System.Collections.Generic;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a root namespace node in the semantic graph (eg. "global").
  /// </summary>
  // ================================================================================================
  public sealed class RootNamespaceEntity : NamespaceEntity
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="RootNamespaceEntity"/> class.
    /// </summary>
    /// <param name="name">The name of the root namespace.</param>
    // ----------------------------------------------------------------------------------------------
    public RootNamespaceEntity(string name)
      : base(name)
    {
    }

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
