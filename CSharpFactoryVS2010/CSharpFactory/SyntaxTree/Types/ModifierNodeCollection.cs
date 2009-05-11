// ================================================================================================
// ModifierNodeCollection.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.Collections;
using CSharpFactory.ParserFiles;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This class represents a collection of modifiers.
  /// </summary>
  // ================================================================================================
  public sealed class ModifierNodeCollection : ImmutableCollection<ModifierNode>
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a new modifier specified by the token.
    /// </summary>
    /// <param name="t">The token for the modifier.</param>
    /// <returns>The newly created modifier.</returns>
    // ----------------------------------------------------------------------------------------------
    public ModifierNode Add(Token t)
    {
      var result = new ModifierNode(t);
      Add(result);
      return result;
    }
  }
}