// ================================================================================================
// SourceFileNode.cs
//
// Created: 2009.03.13, by Istvan Novak (DeepDiver)
// ================================================================================================
using System.IO;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This type defines a source file node in the syntax tree.
  /// </summary>
  /// <remarks>
  /// 	<para>Syntax:</para>
  /// 	<blockquote style="MARGIN-RIGHT: 0px" dir="ltr">
  /// 		<para>{ <em>UsingNamespaceNode</em> | <em>UsingAliasNode</em> } {
  ///         <em>ExternAliasNode</em> } { <em>AttributeDecorationNode</em> }<br/>
  ///         { <em>NamespaceDeclarationNode</em> | <em>TypeDeclarationNode</em> }</para>
  /// 	</blockquote>
  /// 	<para>Representation:</para>
  /// 	<blockquote style="MARGIN-RIGHT: 0px" dir="ltr">
  /// 		<para>
  ///             { <em>UsingNamespaceNode</em> | <em>UsingAliasNode</em> }: see <see cref="NamespaceScopeNode"/><br/>
  ///             { <em>ExternAliasNode</em> }: see <see cref="NamespaceScopeNode"/><br/>
  ///             { <em>AttributeDecorationNode</em> }: <see cref="GlobalAttributes"/><br/>
  ///             { <em>NamespaceDeclarationNode</em> | <em>TypeDeclarationNode</em> }: see
  ///             <see cref="NamespaceScopeNode"/><br/>
  ///             { <em>NamespaceDeclarationNode</em> }: see <see cref="NamespaceScopeNode"/><br/>
  ///             { <em>TypeDeclarationNode</em> }: see <see cref="NamespaceScopeNode"/>
  /// 		</para>
  /// 	</blockquote>
  /// </remarks>
  // ================================================================================================
  public sealed class SourceFileNode : NamespaceScopeNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="SourceFileNode"/> class.
    /// </summary>
    /// <param name="fullName">The full name of the source file.</param>
    // ----------------------------------------------------------------------------------------------
    public SourceFileNode(string fullName) : base(null, null)
    {
      Name = Path.GetFileName(fullName);
      FullName = fullName;
      GlobalAttributes = new AttributeDecorationNodeCollection {ParentNode = this};
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the name of the source file.
    /// </summary>
    /// <value>The name.</value>
    // ----------------------------------------------------------------------------------------------
    public string Name { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the full name of the source file.
    /// </summary>
    /// <value>The full name.</value>
    // ----------------------------------------------------------------------------------------------
    public string FullName { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the global attributes belonging to this source file.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public AttributeDecorationNodeCollection GlobalAttributes { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates the output items describing this source file.
    /// </summary>
    /// <param name="options">The output formatting options.</param>
    /// <returns>Colection of output items to be written.</returns>
    // ----------------------------------------------------------------------------------------------
    public OutputItemCollection CreateOutput(SyntaxTreeOutputOptions options)
    {
      var serializer = new OutputItemSerializer(options);
      serializer.Append(GetOutputSegment());
      return serializer.OutputItems;
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
        ExternAliasNodes,
        UsingNodes,
        GlobalAttributes,
        InScopeDeclarations
        );
    }
  }
}