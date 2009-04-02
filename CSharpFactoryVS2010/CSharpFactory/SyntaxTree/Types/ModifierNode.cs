// ================================================================================================
// ModifierNode.cs
//
// Created: 2009.04.02, by Istvan Novak (DeepDiver)
// ================================================================================================
using System;
using CSharpFactory.Collections;
using CSharpFactory.ParserFiles;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This node represents a modifier.
  /// </summary>
  // ================================================================================================
  public class ModifierNode: SyntaxNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the value of this modifier.
    /// </summary>
    /// <value>The value.</value>
    // ----------------------------------------------------------------------------------------------
    public ModifierType Value { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ModifierNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public ModifierNode(Token start)
      : base(start)
    {
      Terminate(start);
      switch (start.val)
      {
        case "new":
          Value = ModifierType.New;
          break;
        case "public":
          Value = ModifierType.Public;
          break;
        case "protected":
          Value = ModifierType.Protected;
          break;
        case "internal":
          Value = ModifierType.Internal;
          break;
        case "private":
          Value = ModifierType.Private;
          break;
        case "unsafe":
          Value = ModifierType.Unsafe;
          break;
        case "static":
          Value = ModifierType.Static;
          break;
        case "volatile":
          Value = ModifierType.Volatile;
          break;
        case "virtual":
          Value = ModifierType.Virtual;
          break;
        case "sealed":
          Value = ModifierType.Sealed;
          break;
        case "override":
          Value = ModifierType.Override;
          break;
        case "abstract":
          Value = ModifierType.Abstract;
          break;
        case "extern":
          Value = ModifierType.Extern;
          break;
        default:
          throw new InvalidOperationException();
      }
    }
  }

  // ================================================================================================
  /// <summary>
  /// Gets the type of the modifier.
  /// </summary>
  // ================================================================================================
  public enum ModifierType
  {
    New,
    Public,
    Protected,
    Internal,
    Private,
    Unsafe,
    Static,
    Readonly,
    Volatile,
    Virtual,
    Sealed,
    Override,
    Abstract,
    Extern
  }

  // ================================================================================================
  /// <summary>
  /// This class represents a collection of modifiers.
  /// </summary>
  // ================================================================================================
  public sealed class ModifierNodeCollection : ImmutableCollection<ModifierNode>
  {
  }
}