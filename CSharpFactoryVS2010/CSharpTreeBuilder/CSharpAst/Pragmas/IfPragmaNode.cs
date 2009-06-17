// ================================================================================================
// IfPragmaNode.cs
//
// Created: 2009.06.16, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents an "#if" pragma node.
  /// </summary>
  // ================================================================================================
  public class IfPragmaNode : ConditionalPragmaNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="IfPragmaNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public IfPragmaNode(Token start)
      : base(start)
    {
      ElseIfPragmas = new ElseIfPragmaNodeCollection {ParentNode = this};
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the collection of "#elif" pragmas belonging to this #if pragma.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ElseIfPragmaNodeCollection ElseIfPragmas { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the "#else" pragmas belonging to this #if pragma.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ElsePragmaNode ElsePragma { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the "#endif" pragmas belonging to this #if pragma.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public EndIfPragmaNode EndIfPragma { get; internal set; }
  }
}