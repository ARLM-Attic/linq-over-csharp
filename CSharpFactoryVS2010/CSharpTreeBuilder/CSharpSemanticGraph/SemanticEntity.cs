using System.Collections.Generic;
using CSharpTreeBuilder.Ast;
using System;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This is the abstract base class of all kinds of nodes in the semantic graph (namespace, type, etc.).
  /// </summary>
  // ================================================================================================
  public abstract class SemanticEntity
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="SemanticEntity"/> class.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    protected SemanticEntity()
    {
      SyntaxNodes = new List<ISyntaxNode>();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the parent of this entity.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public SemanticEntity Parent { get; set;}
 
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the list of syntax nodes that generated this semantic entity. Can be empty.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public List<ISyntaxNode> SyntaxNodes { get; protected set; }

    #region Visitor methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Accepts a visitor object, according to the Visitor pattern.
    /// </summary>
    /// <param name="visitor">A visitor object</param>
    // ----------------------------------------------------------------------------------------------
    public virtual void AcceptVisitor(SemanticGraphVisitor visitor)
    {
      throw new ApplicationException(string.Format("SemanticEntity.AcceptVisitor called on type: {0}", GetType()));
    }

    #endregion
  }
}
