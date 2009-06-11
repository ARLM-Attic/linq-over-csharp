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
  /// 		<para>"<strong>using</strong>" <em>TypeOrNamespaceNode</em>
  ///         "<strong>;</strong>"</para>
  /// 	</blockquote>
  /// 	<para>Representation:</para>
  /// 	<blockquote style="MARGIN-RIGHT: 0px" dir="ltr">
  /// 		<para>
  ///             "<strong>using</strong>": <see cref="ISyntaxNode.StartToken"/><br/>
  /// 			<em>TypeOrNamespaceNode</em>: <see cref="TypeName"/><br/>
  ///             "<strong>;</strong>": <see cref="ISyntaxNode.TerminatingToken"/>
  /// 		</para>
  /// 	</blockquote>
  /// </remarks>
  // ================================================================================================
  public class UsingNamespaceNode : SyntaxNode<NamespaceScopeNode>
  {
    // --- Backing fields
    private TypeOrNamespaceNode _TypeName;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="UsingNamespaceNode"/> class.
    /// </summary>
    /// <param name="parent">The parent node.</param>
    /// <param name="start">Token providing information about the element.</param>
    /// <param name="namespaceNode">The namespace node.</param>
    /// <param name="terminating">The terminating token.</param>
    // ----------------------------------------------------------------------------------------------
    public UsingNamespaceNode(NamespaceScopeNode parent, Token start, TypeOrNamespaceNode namespaceNode, 
      Token terminating)
      : base(start)
    {
      ParentNode = parent;

      if (namespaceNode==null)
      {
        TypeName = new TypeOrNamespaceNode();
      }
      else
      {
        TypeName = namespaceNode;
        TypeName.ParentNode = parent;
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
    /// Gets the namespace belonging to this using directive.
    /// </summary>
    /// <value>The namespace.</value>
    // ----------------------------------------------------------------------------------------------
    public TypeOrNamespaceNode TypeName
    {
      get { return _TypeName; }
      private set
      {
        _TypeName = value;
        if (_TypeName != null) _TypeName.ParentNode = this;
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
        TypeName,
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
    }

    #endregion
  }
}