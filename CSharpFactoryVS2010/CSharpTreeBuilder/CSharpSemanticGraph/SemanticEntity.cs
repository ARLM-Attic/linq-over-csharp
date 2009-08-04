using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using CSharpTreeBuilder.Ast;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This is the abstract base class of all kinds of nodes in the semantic graph (namespace, type, etc.).
  /// </summary>
  // ================================================================================================
  public abstract class SemanticEntity
  {
    /// <summary>Backing field for SyntaxNodes property.</summary>
    private List<ISyntaxNode> _SyntaxNodes;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="SemanticEntity"/> class.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    protected SemanticEntity()
    {
      _SyntaxNodes = new List<ISyntaxNode>();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the parent of this entity.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public SemanticEntity Parent { get; set;}

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the reflected metadata (eg. type) that this entity was created from.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public System.Reflection.MemberInfo ReflectedMetadata { get; set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a read-only list of syntax nodes that generated this semantic entity. Can be empty.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ReadOnlyCollection<ISyntaxNode> SyntaxNodes
    {
      get { return _SyntaxNodes.AsReadOnly(); }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a syntax node to the SyntaxNodes collection.
    /// </summary>
    /// <param name="syntaxNode">A syntax node.</param>
    // ----------------------------------------------------------------------------------------------
    public void AddSyntaxNode(ISyntaxNode syntaxNode)
    {
      _SyntaxNodes.Add(syntaxNode);
    }

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
