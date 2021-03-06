// ================================================================================================
// IfPragmaState.cs
//
// Created: 2009.06.13, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.Ast;

namespace CSharpTreeBuilder.CSharpAstBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class describes the processing state of a #if..#elif..#else..#endif
  /// pragma block.
  /// </summary>
  // ================================================================================================
  internal sealed class IfPragmaState
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the "#if" pragma instance represented by this state.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public IfPragmaNode IfPragma;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Flag indicating that the "true" condition block is already found or not.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool TrueBlockFound;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Flag indicating that the #else block is already found or not.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool ElseBlockFound;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new #if pragma state using the specified token.
    /// </summary>
    /// <param name="ifPragma">The "#if" pragma represented by this state.</param>
    /// <param name="trueBlockFound">if set to <c>true</c> if true block found.</param>
    // ----------------------------------------------------------------------------------------------
    public IfPragmaState(IfPragmaNode ifPragma,bool trueBlockFound)
    {
      IfPragma = ifPragma;
      TrueBlockFound = trueBlockFound;
    }
  }
}