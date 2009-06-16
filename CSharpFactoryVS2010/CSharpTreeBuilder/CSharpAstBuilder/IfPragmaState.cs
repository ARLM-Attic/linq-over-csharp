// ================================================================================================
// IfPragmaState.cs
//
// Created: 2009.06.13, by Istvan Novak (DeepDiver)
// ================================================================================================
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
    /// Token holding the error/success position information.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token PragmaPosition;

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
    /// <param name="pragmaPosition"></param>
    // ----------------------------------------------------------------------------------------------
    public IfPragmaState(Token pragmaPosition)
    {
      PragmaPosition = pragmaPosition;
      TrueBlockFound = false;
    }
  }
}