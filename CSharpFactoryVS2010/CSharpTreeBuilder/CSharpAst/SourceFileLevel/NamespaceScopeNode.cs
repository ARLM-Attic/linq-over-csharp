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
  // ================================================================================================
  public abstract class NamespaceScopeNode : SyntaxNode<NamespaceScopeNode>
  {
    #region Lifecycle methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="NamespaceScopeNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    protected NamespaceScopeNode(Token start) : base(start)
    {
      UsingNodes = new UsingNodeCollection();
      ExternAliaseNodes = new ExternAliasNodeCollection();
      NamespaceDeclarations = new NamespaceDeclarationNodeCollection();
      TypeDeclarations = new TypeDeclarationNodeCollection();
      InScopeDeclarations = new ScopeNodeCollection();
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
    public ExternAliasNodeCollection ExternAliaseNodes { get; private set; }

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
    internal ScopeNodeCollection InScopeDeclarations { get; private set; }

    #endregion

    #region Public operations

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Create a using node and add it to the source file node.
    /// </summary>
    /// <param name="start">The start.</param>
    /// <param name="namespaceNode">The namespace node.</param>
    /// <param name="terminating">The terminating token.</param>
    /// <returns>The newly created using node.</returns>
    // ----------------------------------------------------------------------------------------------
    public UsingNamespaceNode AddUsing(Token start, TypeOrNamespaceNode namespaceNode, Token terminating)
    {
      var node = new UsingNamespaceNode(this, start, namespaceNode, terminating);
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
    /// <param name="typeName">Name of the type.</param>
    /// <param name="terminating">The terminating token.</param>
    /// <returns>The newly created using node.</returns>
    // ----------------------------------------------------------------------------------------------
    public UsingNamespaceNode AddUsingWithAlias(Token start, Token alias, Token equalToken,
                                       TypeOrNamespaceNode typeName, Token terminating)
    {
      var node = new UsingAliasNode(this, start, alias, equalToken, typeName, terminating);
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
    public void AddExternAlias(Token start, Token alias, Token identifier,
                                          Token terminating)
    {
      ExternAliaseNodes.Add(new ExternAliasNode(this, start, alias, identifier, terminating));
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
        ExternAliaseNodes,
        UsingNodes,
        InScopeDeclarations
        );
    }

    #endregion
  }
}