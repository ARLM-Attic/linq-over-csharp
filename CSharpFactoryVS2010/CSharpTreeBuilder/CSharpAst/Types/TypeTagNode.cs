// ================================================================================================
// TypeTagNode.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
using System.Text;
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>This class represents a type tag within a compound type or namespace name.</summary>
  /// <remarks>
  /// 	<para>For example, in the type name "System.Text.Encodig" "System", "Text" and
  ///     "Encoding" are represented by type tags.</para>
  /// 	<para>Syntax:</para>
  /// 	<blockquote style="MARGIN-RIGHT: 0px" dir="ltr">
  /// 		<para><em>identifier</em> [ <em>TypeArgumentListNode</em> ]</para>
  /// 	</blockquote>
  /// 	<para>Representation:</para>
  /// 	<blockquote style="MARGIN-RIGHT: 0px" dir="ltr">
  /// 		<para>
  /// 			<em>identifier</em>: <see cref="IdentifierToken"/><br/>
  /// 			<em>TypeArgumentListNode</em>: <see cref="Arguments"/>
  /// 		</para>
  /// 	</blockquote>
  /// </remarks>
  // ================================================================================================
  public class TypeTagNode : SyntaxNode<NamespaceOrTypeNameNode>, IIdentifierSupport, ITypeArguments
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeTagNode"/> class.
    /// </summary>
    /// <param name="identifierToken">Identifier token.</param>
    /// <param name="argumentListNode">The argument list node.</param>
    // ----------------------------------------------------------------------------------------------
    public TypeTagNode(Token identifierToken, TypeNodeCollection argumentListNode)
      : base(identifierToken)
    {
      IdentifierToken = identifierToken;
      Arguments = argumentListNode;
      if (Arguments == null)
      {
        Arguments = new TypeNodeCollection();
      }
      Terminate(argumentListNode == null ? IdentifierToken : argumentListNode.TerminatingToken);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeTagNode"/> class with only an identifier token.
    /// </summary>
    /// <param name="identifierToken">Identifier token.</param>
    // ----------------------------------------------------------------------------------------------
    public TypeTagNode(Token identifierToken)
      : this(identifierToken, null)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeTagNode"/> class with only an identifier string.
    /// </summary>
    /// <param name="identifier">Identifier string.</param>
    // ----------------------------------------------------------------------------------------------
    public TypeTagNode(string identifier)
      : this(new Token(identifier), null)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the identifier token.
    /// </summary>
    /// <value>The identifier token.</value>
    // ----------------------------------------------------------------------------------------------
    public Token IdentifierToken { get; set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the identifier name.
    /// </summary>
    /// <value>The identifier name.</value>
    // ----------------------------------------------------------------------------------------------
    public string Identifier
    {
      get { return IdentifierToken.Value; }
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
    /// Gets or sets the node providing type arguments.
    /// </summary>
    /// <value>The arguments.</value>
    // ----------------------------------------------------------------------------------------------
    public TypeNodeCollection Arguments { get; internal set; }

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
      get { return Arguments != null && Arguments.Count > 0; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this is an unbound generic type (has empty type arguments).
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsUnbound
    {
      get { return HasTypeArguments && Arguments[0].IsEmpty; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating the number of generic dimensions.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public int GenericDimensions
    {
      get { return Arguments.Count; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the typetag name with generic dimensions (eg. A`1).
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string NameWithGenericDimensions
    {
      get
      {
        return GenericDimensions > 0 ? Identifier + "`" + GenericDimensions : Identifier;
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
        SeparatorToken,
        IdentifierToken,
        Arguments
        );
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

      if (Identifier!= null)
      {
        result.Append(Identifier);
      }

      if (Arguments.Count>0)
      {
        result.Append('<');
      }

      bool firstItem = true;
      foreach (var argument in Arguments)
      {
        if (firstItem) { firstItem = false; } else { result.Append(','); }
        result.Append(argument.ToString());
      }

      if (Arguments.Count > 0)
      {
        result.Append('>');
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

      foreach (var argument in Arguments)
      {
        argument.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}