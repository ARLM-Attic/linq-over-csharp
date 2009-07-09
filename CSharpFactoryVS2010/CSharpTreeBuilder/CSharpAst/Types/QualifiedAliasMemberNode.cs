using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents a qualified alias member node.
  /// identifier :: identifier [type-arg-list]
  /// </summary>
  // ================================================================================================
  public class QualifiedAliasMemberNode : SyntaxNode<ISyntaxNode>, IIdentifierSupport, ITypeArguments
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="QualifiedAliasMemberNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public QualifiedAliasMemberNode(Token start)
      : base(start)
    {
      QualifierToken = start;
      Arguments = new TypeOrNamespaceNodeCollection();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the qualifier token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token QualifierToken { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the qualifier name.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string Qualifier
    {
      get { return QualifierToken == null ? string.Empty : QualifierToken.Value; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the qualifier separator (::) token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token QualifierSeparatorToken { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the identifier token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token IdentifierToken { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the identifier name.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string Identifier
    {
      get { return IdentifierToken == null ? string.Empty : IdentifierToken.Value; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance has identifier.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this instance has identifier; otherwise, <c>false</c>.
    /// </value>
    // ----------------------------------------------------------------------------------------------
    public bool HasIdentifier
    {
      get { return IdentifierToken != null; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the node providing type arguments.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeOrNamespaceNodeCollection Arguments { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance has type arguments.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this instance has type arguments; otherwise, <c>false</c>.
    /// </value>
    // ----------------------------------------------------------------------------------------------
    public bool HasTypeArguments
    {
      get { return Arguments != null; }
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

      foreach (var argument in Arguments)
      {
        argument.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}