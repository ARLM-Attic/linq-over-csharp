// ================================================================================================
// TypeWithBodyDeclarationNode.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents type declarations having bodies.
  /// </summary>
  /// <remarks>
  /// 	<para>Representation:</para>
  /// 	<blockquote style="MARGIN-RIGHT: 0px" dir="ltr">
  /// 		<para>
  ///             "<strong>{</strong>": <see cref="OpenBrace"/><br/>
  ///             "<strong>}</strong>": <see cref="CloseBrace"/>
  /// 		</para>
  /// 	</blockquote>
  /// </remarks>
  // ================================================================================================
  public abstract class TypeWithBodyDeclarationNode : TypeDeclarationNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeWithBodyDeclarationNode"/> class.
    /// </summary>
    /// <param name="start">The start token of the declaration.</param>
    /// <param name="name">The name of the declaration.</param>
    // ----------------------------------------------------------------------------------------------
    protected TypeWithBodyDeclarationNode(Token start, Token name)
      : base(start, name)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the opening brace token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token OpenBrace { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the closing brace token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token CloseBrace { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the output segments representing the type declaration body.
    /// </summary>
    /// <returns>Output segments representing the type declaration body.</returns>
    // ----------------------------------------------------------------------------------------------
    protected virtual OutputSegment GetBodySegments()
    {
      return new OutputSegment(MemberDeclarations);  
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the output segment.
    /// </summary>
    /// <returns></returns>
    // ----------------------------------------------------------------------------------------------
    public override OutputSegment GetOutputSegment()
    {
      return new OutputSegment(
        base.GetOutputSegment(),
        BraceSegment.OpenType(OpenBrace),
        GetBodySegments(),
        BraceSegment.CloseType(CloseBrace)
        );
    }
  }
}