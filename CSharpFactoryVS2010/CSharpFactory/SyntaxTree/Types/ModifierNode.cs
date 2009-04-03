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
        case "readonly":
          Value = ModifierType.Readonly;
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
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// The "new" modifier
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    New,

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// The "public" modifier
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    Public,

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// The "protected" modifier
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    Protected,

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// The "internal" modifier
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    Internal,

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// The "private" modifier
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    Private,

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// The "unsafe" modifier
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    Unsafe,

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// The "static" modifier
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    Static,

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// The "readonly" modifier
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    Readonly,

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// The "volatile" modifier
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    Volatile,

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// The "virtual" modifier
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    Virtual,

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// The "sealed" modifier
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    Sealed,

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// The "override" modifier
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    Override,

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// The "abstract" modifier
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    Abstract,

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// The "extern" modifier
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    Extern
  }

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