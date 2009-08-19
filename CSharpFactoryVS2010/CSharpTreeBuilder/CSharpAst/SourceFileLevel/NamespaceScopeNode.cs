// ================================================================================================
// NamespaceScopeNode.cs
//
// Created: 2009.03.27, by Istvan Novak (DeepDiver)
// ================================================================================================
using System.Collections.Generic;
using System.Linq;
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This abstract class defines the behavior of a scope where exter aliases, using directives and 
  /// namespaces can be declared.
  /// </summary>
  /// <remarks>
  /// 	<para>This class is used to represent source files and namespace
  ///     declarations.</para>
  /// 	<para>Syntax:</para>
  /// 	<blockquote style="MARGIN-RIGHT: 0px" dir="ltr">
  /// 		<para>{ <em>UsingNamespaceNode</em> | <em>UsingAliasNode</em> } {
  ///         <em>ExternAliasNode</em> } { <em>NamespaceDeclarationNode</em> |
  ///         <em>TypeDeclarationNode</em> }</para>
  /// 	</blockquote>
  /// 	<para>Representation:</para>
  /// 	<blockquote style="MARGIN-RIGHT: 0px" dir="ltr">
  /// 		<para>
  ///             { <em>UsingNamespaceNode</em> | <em>UsingAliasNode</em> }: <see cref="UsingNodes"/><br/>
  ///             { <em>ExternAliasNode</em> }: <see cref="ExternAliasNodes"/><br/>
  ///             { <em>NamespaceDeclarationNode</em> | <em>TypeDeclarationNode</em> }:
  ///             <see cref="InScopeDeclarations"/><br/>
  ///             { <em>NamespaceDeclarationNode</em> }: <see cref="NamespaceDeclarations"/><br/>
  ///             { <em>TypeDeclarationNode</em> }: <see cref="TypeDeclarations"/>
  /// 		</para>
  /// 	</blockquote>
  /// </remarks>
  // ================================================================================================
  public abstract class NamespaceScopeNode : SyntaxNode<NamespaceScopeNode>
  {
    #region Lifecycle methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="NamespaceScopeNode"/> class.
    /// </summary>
    /// <param name="parent">The parent namespace scope node.</param>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    protected NamespaceScopeNode(NamespaceScopeNode parent, Token start)
      : base(start)
    {
      ParentNode = parent;
      UsingNodes = new UsingNodeCollection {ParentNode = this};
      ExternAliasNodes = new ExternAliasNodeCollection { ParentNode = this };
      NamespaceDeclarations = new NamespaceDeclarationNodeCollection { ParentNode = this };
      TypeDeclarations = new TypeDeclarationNodeCollection { ParentNode = this };
      InScopeDeclarations = new ScopeNodeCollection { ParentNode = this };
    }

    #endregion

    #region Public Properties

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the using clauses belonging to the source file (only without using alias)
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public UsingNodeCollection UsingNodes { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the using clauses belonging to the source file (only with using alias)
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public IEnumerable<UsingAliasNode> UsingWithAliasNodes
    {
      get
      {
        return UsingNodes
          .Where(node => node.GetType() == typeof (UsingAliasNode))
          .Cast<UsingAliasNode>();
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the extern alias clauses belonging to the source file.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ExternAliasNodeCollection ExternAliasNodes { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the namespace declarations belonging to this source file.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public NamespaceDeclarationNodeCollection NamespaceDeclarations { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the collection of nested types.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeDeclarationNodeCollection TypeDeclarations { get; private set; }

    #endregion

    #region Protected members

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the in-scope declarations.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ScopeNodeCollection InScopeDeclarations { get; private set; }

    #endregion

    #region Public operations

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Create a using node and add it to the source file node.
    /// </summary>
    /// <param name="start">The start.</param>
    /// <param name="namespaceOrTypeName">The namespace or type name node.</param>
    /// <param name="terminating">The terminating token.</param>
    /// <returns>The newly created using node.</returns>
    // ----------------------------------------------------------------------------------------------
    public UsingNamespaceNode AddUsing(Token start, NamespaceOrTypeNameNode namespaceOrTypeName, Token terminating)
    {
      var node = new UsingNamespaceNode(this, start, namespaceOrTypeName, terminating);
      UsingNodes.Add(node);
      return node;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Create a using node and add it to the source file node.
    /// </summary>
    /// <param name="start">The start.</param>
    /// <param name="alias">AliasToken of the using clause</param>
    /// <param name="equalToken">The equal token.</param>
    /// <param name="namespaceOrTypeName">The namespace or type name node.</param>
    /// <param name="terminating">The terminating token.</param>
    /// <returns>The newly created using node.</returns>
    // ----------------------------------------------------------------------------------------------
    public UsingNamespaceNode AddUsingWithAlias(Token start, Token alias, Token equalToken,
                                       NamespaceOrTypeNameNode namespaceOrTypeName, Token terminating)
    {
      var node = new UsingAliasNode(this, start, alias, equalToken, namespaceOrTypeName, terminating);
      UsingNodes.Add(node);
      return node;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Create an extern alias node and add it to the source file node.
    /// </summary>
    /// <param name="start">The start token.</param>
    /// <param name="alias">The alias token.</param>
    /// <param name="identifier">The identifier token.</param>
    /// <param name="terminating">The terminating token.</param>
    /// <returns></returns>
    // ----------------------------------------------------------------------------------------------
    public ExternAliasNode AddExternAlias(Token start, Token alias, Token identifier,
                                          Token terminating)
    {
      var externAlias = new ExternAliasNode(this, start, alias, identifier, terminating);
      ExternAliasNodes.Add(externAlias);
      return externAlias;
    }

    #endregion

    #region Output methods

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
        InScopeDeclarations
        );
    }

    #endregion
  }
}