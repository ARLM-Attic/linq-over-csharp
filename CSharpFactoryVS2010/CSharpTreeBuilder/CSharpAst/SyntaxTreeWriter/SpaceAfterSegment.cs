// ================================================================================================
// SpaceAfterSegment.cs
//
// Created: 2009.05.29, by Istvan Novak (DeepDiver)
// ================================================================================================
using System;
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This segment handles spaces after tokens.
  /// </summary>
  // ================================================================================================
  public class SpaceAfterSegment : ControlSegment
  {
    private readonly SpaceType Type;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="SpaceAfterSegment"/> class.
    /// </summary>
    /// <param name="token">The token surrounding with spaces.</param>
    /// <param name="type">The type of token surrounded.</param>
    // ----------------------------------------------------------------------------------------------
    private SpaceAfterSegment(SpaceType type)
    {
      Type = type;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Represents a "space before colon in attributes" operator.
    /// </summary>
    /// <returns>
    /// The newly created "space before" segment.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    public static SpaceAfterSegment AfterColonInAttributes()
    {
      return new SpaceAfterSegment(SpaceType.AfterColonInAttributes);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Represents a "space before colon in attributes" operator.
    /// </summary>
    /// <returns>
    /// The newly created "space before" segment.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    public static SpaceAfterSegment AfterComma()
    {
      return new SpaceAfterSegment(SpaceType.AfterComma);
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
        case SpaceType.AfterColonInAttributes:
          useSpace = serializer.OutputOptions.SpaceAfterColonInAttributes;
          break;
        case SpaceType.AfterComma:
          useSpace = serializer.OutputOptions.SpaceAfterComma;
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      if (useSpace) serializer.Append(" ");
    }
  }
}