// ================================================================================================
// BraceSegment.cs
//
// Created: 2009.03.27, by Istvan Novak (DeepDiver)
// ================================================================================================
using System;
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This control segment provides operations to handle braces.
  /// </summary>
  // ================================================================================================
  public class BraceSegment : ControlSegment
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Defines the type of the brace to use during formatting.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    private enum BraceType
    {
      OpenType, 
      CloseType
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Opening brace for types and namespaces.
    /// </summary>
    /// <param name="token">The brace token.</param>
    /// <returns>
    /// The newly created brace segment
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    public static BraceSegment OpenType(Token token)
    {
      return new BraceSegment(token, BraceType.OpenType); 
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Closing brace for types and namespaces.
    /// </summary>
    /// <param name="token">The brace token.</param>
    /// <returns>
    /// The newly created brace segment
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    public static BraceSegment CloseType(Token token)
    {
      return new BraceSegment(token, BraceType.CloseType);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Avoid external instantiation
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    private BraceSegment(Token t, BraceType type) 
    {
      Token = t;
      Type = type;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the token representing a brace.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    private Token Token { get; set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the brace type
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    private BraceType Type { get; set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Controls the state of the specified OutputItemSerializer.
    /// </summary>
    /// <param name="serializer">The serializer to control.</param>
    // ----------------------------------------------------------------------------------------------
    public override void Control(OutputItemSerializer serializer)
    {
      if (Type == BraceType.OpenType) 
        OpenBrace(serializer, serializer.OutputOptions.TypeAndNamespaceBraces);
      else if (Type == BraceType.CloseType)
        CloseBrace(serializer, serializer.OutputOptions.TypeAndNamespaceBraces);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Places an opening brace according to the style.
    /// </summary>
    /// <param name="serializer">The serializer.</param>
    /// <param name="style">The style to apply.</param>
    // ----------------------------------------------------------------------------------------------
    private void OpenBrace(OutputItemSerializer serializer, BracingStyle style)
    {
      switch (style)
      {
        case BracingStyle.NextLineBsd:
          serializer.ForceNewLine();
          serializer.ApplyIndentation();
          serializer.Append(Token);
          serializer.ForceNewLine();
          serializer.IncrementIndentation();
          break;
        case BracingStyle.NextLineGnu:
          serializer.ForceNewLine();
          serializer.IncrementIndentation();
          serializer.ApplyIndentation();
          serializer.Append(Token);
          serializer.ForceNewLine();
          serializer.IncrementIndentation();
          break;
        case BracingStyle.NextLineWhitesmiths:
          serializer.ForceNewLine();
          serializer.IncrementIndentation();
          serializer.ApplyIndentation();
          serializer.Append(Token);
          serializer.ForceNewLine();
          break;
        case BracingStyle.EndOfLineNoSpace:
          serializer.Append(Token);
          serializer.ForceNewLine();
          serializer.IncrementIndentation();
          break;
        case BracingStyle.EndOfLineKAndR:
          serializer.Append(" ");
          serializer.Append(Token);
          serializer.ForceNewLine();
          serializer.IncrementIndentation();
          break;
        default:
          throw new ArgumentOutOfRangeException("style");
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Places a closing brace according to the style.
    /// </summary>
    /// <param name="serializer">The serializer.</param>
    /// <param name="style">The style to apply.</param>
    // ----------------------------------------------------------------------------------------------
    private void CloseBrace(OutputItemSerializer serializer, BracingStyle style)
    {
      switch (style)
      {
        case BracingStyle.NextLineBsd:
          serializer.DecrementIndentation();
          serializer.ApplyIndentation();
          serializer.Append(Token);
          serializer.ForceNewLine();
          break;
        case BracingStyle.NextLineGnu:
          serializer.DecrementIndentation();
          serializer.ApplyIndentation();
          serializer.Append(Token);
          serializer.ForceNewLine();
          serializer.DecrementIndentation();
          break;
        case BracingStyle.NextLineWhitesmiths:
          serializer.ApplyIndentation();
          serializer.DecrementIndentation();
          serializer.Append(Token);
          serializer.ForceNewLine();
          break;
        case BracingStyle.EndOfLineNoSpace:
          serializer.DecrementIndentation();
          serializer.ApplyIndentation();
          serializer.Append(Token);
          serializer.ForceNewLine();
          break;
        case BracingStyle.EndOfLineKAndR:
          serializer.DecrementIndentation();
          serializer.ApplyIndentation();
          serializer.Append(Token);
          serializer.ForceNewLine();
          break;
        default:
          throw new ArgumentOutOfRangeException("style");
      }
    }
  }
}