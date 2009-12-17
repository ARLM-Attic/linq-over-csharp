// ================================================================================================
// AccessorNode.cs
//
// Created: 2009.05.16, by Istvan Novak (DeepDiver)
// ================================================================================================
using System;
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class declares and accessor that can be used in properties and event properties.
  /// </summary>
  // ================================================================================================
  public class AccessorNode : MemberWithBodyDeclarationNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="AccessorNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public AccessorNode(Token start)
      : base(start)
    {
      IdentifierToken = start;

      switch (start.Value)
      {
        case "get":
          AccessorKind = AccessorKind.Get;
          break;
        case "set":
          AccessorKind = AccessorKind.Set;
          break;
        case "add":
          AccessorKind = AccessorKind.Add;
          break;
        case "remove":
          AccessorKind = AccessorKind.Remove;
          break;
        default:
          throw new ApplicationException("Unrecognized accessor kind");
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the accessor kind.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public AccessorKind AccessorKind { get; private set; }

    #region Visitor methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Accepts a visitor object, according to the Visitor pattern.
    /// </summary>
    /// <param name="visitor">A visitor object</param>
    // ----------------------------------------------------------------------------------------------
    public override void AcceptVisitor(ISyntaxNodeVisitor visitor)
    {
      if (!visitor.Visit(this))
      {
        return;
      }

      if (Body!=null)
      {
        Body.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}