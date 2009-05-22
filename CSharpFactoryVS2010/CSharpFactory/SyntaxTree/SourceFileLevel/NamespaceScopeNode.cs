// ================================================================================================
// NamespaceScopeNode.cs
//
// Created: 2009.03.27, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.Collections;
using CSharpFactory.ParserFiles;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This abstract class defines the behavior of a scope where exter aliases, using directives and 
  /// namespaces can be declared.
  /// </summary>
  // ================================================================================================
  public abstract class NamespaceScopeNode: SyntaxNode
  {
    #region Lifecycle methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="NamespaceScopeNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    protected NamespaceScopeNode(Token start): base(start)
    {
      UsingNodes = new ImmutableCollection<UsingNode>();
      UsingWithAliasNodes = new ImmutableCollection<UsingWithAliasNode>();
      ExternAliaseNodes = new ImmutableCollection<ExternAliasNode>();
      NamespaceDeclarations = new NamespaceDeclarationNodeCollection();
      TypeDeclarations = new TypeDeclarationNodeCollection();
    }

    #endregion

    #region Public Properties

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the using clauses belonging to the source file (only without using alias)
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ImmutableCollection<UsingNode> UsingNodes { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the using clauses belonging to the source file (only with using alias)
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ImmutableCollection<UsingWithAliasNode> UsingWithAliasNodes { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the extern alias clauses belonging to the source file.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ImmutableCollection<ExternAliasNode> ExternAliaseNodes { get; private set; }

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
    public UsingNode AddUsing(Token start, TypeOrNamespaceNode namespaceNode, Token terminating)
    {
      var node = new UsingNode(this, start, namespaceNode, terminating);
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
    public UsingNode AddUsingWithAlias(Token start, Token alias, Token equalToken,
      TypeOrNamespaceNode typeName, Token terminating)
    {
      var node = new UsingWithAliasNode(this, start, alias, equalToken, typeName, terminating);
      UsingNodes.Add(node);
      UsingWithAliasNodes.Add(node);
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
      var node = new ExternAliasNode(this, start, alias, identifier, terminating);
      ExternAliaseNodes.Add(node);
      return node;
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
        NamespaceDeclarations
        );
    }

    #endregion
  }
}