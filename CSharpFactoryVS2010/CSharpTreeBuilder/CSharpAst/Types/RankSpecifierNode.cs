using System.Text;
using CSharpTreeBuilder.CSharpAstBuilder;
using CSharpTreeBuilder.Collections;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents a rank specifier node: [ , ]
  /// </summary>
  // ================================================================================================
  public class RankSpecifierNode : SyntaxNode<ISyntaxNode>
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="RankSpecifierNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public RankSpecifierNode(Token start)
      : base(start)
    {
      Commas = new ImmutableCollection<Token>();
      OpenSquareBracket = start;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="RankSpecifierNode"/> class.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public RankSpecifierNode()
      : this(null)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the opening square bracket.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token OpenSquareBracket { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the collection of comma tokens.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ImmutableCollection<Token> Commas { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the closing square bracket.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token CloseSquareBracket { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the rank of the array.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public int Rank 
    { 
      get { return Commas.Count + 1; } 
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the string representation of this language element.
    /// </summary>
    /// <returns>Full name of the language element.</returns>
    // ----------------------------------------------------------------------------------------------
    public override string ToString()
    {
      var result = new StringBuilder();

      if (StartToken!=null)
      {
        result.Append('[');
        result.Append(',', Commas.Count);
        result.Append(']');
      }

      return result.Length == 0 ? GetType().ToString() : result.ToString();
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