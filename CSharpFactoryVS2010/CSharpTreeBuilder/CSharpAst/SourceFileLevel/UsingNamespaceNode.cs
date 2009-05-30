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
  /// 	<para>"<strong>using</strong>" <em>TypeOrNamespaceNode</em>
  ///     "<strong>;</strong>"</para>
  /// </remarks>
  // ================================================================================================
  public class UsingNamespaceNode : SyntaxNode<NamespaceScopeNode>
  {
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
      Parent = parent;
      TypeName = namespaceNode;
      Terminate(terminating);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the parent of this node.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public NamespaceScopeNode Parent { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the namespace belonging to this using directive.
    /// </summary>
    /// <value>The namespace.</value>
    // ----------------------------------------------------------------------------------------------
    public TypeOrNamespaceNode TypeName { get; private set; }

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
  }
}