// ================================================================================================
// SpaceAroundSegment.cs
//
// Created: 2009.03.27, by Istvan Novak (DeepDiver)
// ================================================================================================
using System;
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This segment handles spaces
  /// </summary>
  // ================================================================================================
  public class SpaceAroundSegment : ControlSegment
  {
    private readonly Token Token;
    private readonly SpaceType Type;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// This enumeration defines the types of "space around" segments.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    private enum SpaceType
    {
      AssignmentOp,
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Represents a space around assignment operators.
    /// </summary>
    /// <param name="token">Assignment operator token.</param>
    /// <returns>
    /// The newly created "space around" segment.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    public static SpaceAroundSegment AssignmentOp(Token token)
    {
      return new SpaceAroundSegment(token, SpaceType.AssignmentOp);  
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="SpaceAroundSegment"/> class.
    /// </summary>
    /// <param name="token">The token surrounding with spaces.</param>
    /// <param name="type">The type of token surrounded.</param>
    // ----------------------------------------------------------------------------------------------
    private SpaceAroundSegment(Token token, SpaceType type)
    {
      Token = token;
      Type = type;
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
        case SpaceType.AssignmentOp:
          useSpace = serializer.OutputOptions.SpaceAroundAssignmentOps;
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      if (useSpace) serializer.Append(" ");
      serializer.Append(Token);
      if (useSpace) serializer.Append(" ");
    }
  }
}