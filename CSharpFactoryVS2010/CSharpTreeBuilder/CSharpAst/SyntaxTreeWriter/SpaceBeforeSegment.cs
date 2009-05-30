// ================================================================================================
// SpaceBeforeSegment.cs
//
// Created: 2009.05.29, by Istvan Novak (DeepDiver)
// ================================================================================================
using System;
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This segment handles spaces before tokens.
  /// </summary>
  // ================================================================================================
  public class SpaceBeforeSegment : ControlSegment
  {
    private readonly Token Token;
    private readonly SpaceType Type;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="SpaceBeforeSegment"/> class.
    /// </summary>
    /// <param name="token">The token surrounding with spaces.</param>
    /// <param name="type">The type of token surrounded.</param>
    // ----------------------------------------------------------------------------------------------
    private SpaceBeforeSegment(Token token, SpaceType type)
    {
      Token = token;
      Type = type;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Represents a "space before colon in attributes" operator.
    /// </summary>
    /// <param name="token">Token to put space before.</param>
    /// <returns>
    /// The newly created "space before" segment.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    public static SpaceBeforeSegment BeforeColonInAttributes(Token token)
    {
      return new SpaceBeforeSegment(token, SpaceType.BeforeColonInAttributes);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Represents a "space before colon in attributes" operator.
    /// </summary>
    /// <param name="token">Token to put space before.</param>
    /// <returns>
    /// The newly created "space before" segment.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    public static SpaceBeforeSegment BeforeComma(Token token)
    {
      return new SpaceBeforeSegment(token, SpaceType.BeforeComma);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Controls the state of the specified OutputItemSerializer.
    /// </summary>
    /// <param name="serializer">The serializer to control.</param>
    // ----------------------------------------------------------------------------------------------
    public override void Control(OutputItemSerializer serializer)
    {
      bool useSpace;
      switch (Type)
      {
        case SpaceType.BeforeColonInAttributes:
          useSpace = serializer.OutputOptions.SpaceBeforeColonInAttributes;
          break;
        case SpaceType.BeforeComma:
          useSpace = serializer.OutputOptions.SpaceBeforeComma;
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      if (useSpace) serializer.Append(" ");
      serializer.Append(Token);
    }
  }
}