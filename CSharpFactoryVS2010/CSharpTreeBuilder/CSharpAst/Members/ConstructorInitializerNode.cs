using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class is the abstract base class of constructor initializers :base(args) and :this(args). 
  /// </summary>
  // ================================================================================================
  public abstract class ConstructorInitializerNode : SyntaxNode<ConstructorDeclarationNode>
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ConstructorInitializerNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    protected ConstructorInitializerNode(Token start)
      : base(start)
    {
      ColonToken = start;
      Arguments = new ArgumentNodeCollection { ParentNode = this };
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the colon token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token ColonToken { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the "base" or "this" token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token BaseOrThisToken { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets if it's a base access type initalizer
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public virtual bool IsBaseAccess { get { return false; } }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets if it's a this access type initalizer
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public virtual bool IsThisAccess { get { return false; } }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the list of arguments of the initializer.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ArgumentNodeCollection Arguments { get; internal set; }

    #region Visitor methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Accepts a visitor object, according to the Visitor pattern.
    /// </summary>
    /// <param name="visitor">A visitor object</param>
    // ----------------------------------------------------------------------------------------------
    public override void AcceptVisitor(ISyntaxNodeVisitor visitor)
    {
      foreach (var argument in Arguments)
      {
        argument.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}