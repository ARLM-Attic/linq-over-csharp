// ================================================================================================
// AccessorNode.cs
//
// Created: 2009.05.16, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.ParserFiles;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class declares and accessor that can be used in properties and event properties.
  /// </summary>
  // ================================================================================================
  public class AccessorNode: MemberWithBodyDeclarationNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="AccessorNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public AccessorNode(Token start)
      : base(start)
    {
      IdentifierToken = start;
    }
  }
}