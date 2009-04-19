// ================================================================================================
// FinalizerDeclarationNode.cs
//
// Created: 2009.04.13, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.ParserFiles;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This class declares a finalizer member.
  /// </summary>
  // ================================================================================================
  public class FinalizerDeclarationNode: MethodDeclarationNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="FinalizerDeclarationNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public FinalizerDeclarationNode(Token start)
      : base(start)
    {
    }
  }
}