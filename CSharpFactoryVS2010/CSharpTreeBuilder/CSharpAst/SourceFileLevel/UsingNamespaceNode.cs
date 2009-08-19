// ================================================================================================
// UsingNamespaceNode.cs
//
// Created: 2009.03.13, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>This class represents a using namespace directive.</summary>
  /// <remarks>
  /// 	<para>Syntax:</para>
  /// 	<blockquote style="MARGIN-RIGHT: 0px" dir="ltr">
  /// 		<para>"<strong>using</strong>" <em>NamespaceOrTypeName</em>
  ///         "<strong>;</strong>"</para>
  /// 	</blockquote>
  /// 	<para>Representation:</para>
  /// 	<blockquote style="MARGIN-RIGHT: 0px" dir="ltr">
  /// 		<para>
  ///             "<strong>using</strong>": <see cref="ISyntaxNode.StartToken"/><br/>
  /// 			<em>NamespaceOrTypeName</em>: <see cref="NamespaceOrTypeNameNode"/><br/>
  ///             "<strong>;</strong>": <see cref="ISyntaxNode.TerminatingToken"/>
  /// 		</para>
  /// 	</blockquote>
  /// </remarks>
  // ================================================================================================
  public class UsingNamespaceNode : SyntaxNode<NamespaceScopeNode>
  {
    // --- Backing fields
    private NamespaceOrTypeNameNode _NamespaceOrTypeName;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="UsingNamespaceNode"/> class.
    /// </summary>
    /// <param name="parent">The parent node.</param>
    /// <param name="start">Token providing information about the element.</param>
    /// <param name="namespaceOrTypeNode">The namespace-or-type-name node.</param>
    /// <param name="terminating">The terminating token.</param>
    // ----------------------------------------------------------------------------------------------
    public UsingNamespaceNode(NamespaceScopeNode parent, Token start, NamespaceOrTypeNameNode namespaceOrTypeNode, 
      Token terminating)
      : base(start)
    {
      ParentNode = parent;

      if (namespaceOrTypeNode!=null)
      {
        NamespaceOrTypeName = namespaceOrTypeNode;
        NamespaceOrTypeName.ParentNode = parent;
      }

      Terminate(terminating);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new blank <see cref="UsingNamespaceNode"/> object 
    /// with no parent and empty namespace name.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public UsingNamespaceNode()
      : this(null, Token.Using, null, Token.Semicolon)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the namespace-or-type-name belonging to this using directive.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public NamespaceOrTypeNameNode NamespaceOrTypeName
    {
      get
      {
        return _NamespaceOrTypeName;
      }

      private set
      {
        _NamespaceOrTypeName = value;
        if (_NamespaceOrTypeName != null) 
        {
          _NamespaceOrTypeName.ParentNode = this;
        }
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the output segment representing this syntax node.
    /// </summary>
    /// <returns>
    /// The OutputSegment instance describing this syntax node, or null; if the node has no output.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    public override OutputSegment GetOutputSegment()
    {
      return new OutputSegment(
        StartToken,
        MandatoryWhiteSpaceSegment.Default,
        NamespaceOrTypeName,
        TerminatingToken,
        ForceNewLineSegment.Default
        );
    }

    #region Visitor methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Accepts a visitor object, according to the Visitor pattern.
    /// </summary>
    /// <param name="visitor">A visitor object</param>
    // ----------------------------------------------------------------------------------------------
    public override void AcceptVisitor(ISyntaxNodeVisitor visitor)
    {
      visitor.Visit(this);

      if (NamespaceOrTypeName != null)
      {
        NamespaceOrTypeName.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}