// ================================================================================================
// ModifierNode.cs
//
// Created: 2009.04.02, by Istvan Novak (DeepDiver)
// ================================================================================================
using System;
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>This node represents a modifier.</summary>
  /// <remarks>
  /// 	<para>Syntax:</para>
  /// 	<blockquote style="MARGIN-RIGHT: 0px" dir="ltr">
  /// 		<para>( "<strong>new</strong>" | "<strong>public</strong>" |
  ///         "<strong>protected</strong>" | "<strong>internal</strong>" |
  ///         "<strong>private</strong>" | "<strong>unsafe</strong>" |
  ///         "<strong>static</strong>" | "<strong>volatile</strong>" |
  ///         "<strong>virtual</strong>" | "<strong>sealed</strong>" |
  ///         "<strong>override</strong>" | "<strong>abstract</strong>" |
  ///         "<strong>extern</strong>" | "<strong>readonly</strong>" )</para>
  /// 	</blockquote>
  /// 	<para>Representation:</para>
  /// 	<blockquote style="MARGIN-RIGHT: 0px" dir="ltr">
  /// 		<para>
  /// 			<see cref="ISyntaxNode.StartToken"/> stores the token representing the
  ///             modifier. <see cref="Value"/> stores the type of the modifier.
  ///         </para>
  /// 	</blockquote>
  /// </remarks>
  // ================================================================================================
  public class ModifierNode : SyntaxNode<ISyntaxNode>
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ModifierNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public ModifierNode(Token start)
      : base(start)
    {
      Terminate(start);
      switch (start.Value)
      {
        case "new":
          Value = ModifierType.New;
          break;
        case "public":
          Value = ModifierType.Public;
          break;
        case "protected":
          Value = ModifierType.Protected;
          break;
        case "internal":
          Value = ModifierType.Internal;
          break;
        case "private":
          Value = ModifierType.Private;
          break;
        case "unsafe":
          Value = ModifierType.Unsafe;
          break;
        case "static":
          Value = ModifierType.Static;
          break;
        case "volatile":
          Value = ModifierType.Volatile;
          break;
        case "virtual":
          Value = ModifierType.Virtual;
          break;
        case "sealed":
          Value = ModifierType.Sealed;
          break;
        case "override":
          Value = ModifierType.Override;
          break;
        case "abstract":
          Value = ModifierType.Abstract;
          break;
        case "extern":
          Value = ModifierType.Extern;
          break;
        case "readonly":
          Value = ModifierType.Readonly;
          break;
        default:
          throw new InvalidOperationException();
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the value of this modifier.
    /// </summary>
    /// <value>The value.</value>
    // ----------------------------------------------------------------------------------------------
    public ModifierType Value { get; private set; }

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
        MandatoryWhiteSpaceSegment.Default
        );
    }
  }
}