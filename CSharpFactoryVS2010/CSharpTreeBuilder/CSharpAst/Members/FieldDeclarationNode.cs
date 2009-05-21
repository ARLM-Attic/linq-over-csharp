// ================================================================================================
// FieldDeclarationNode.cs
//
// Created: 2009.05.16, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This class represents a field member declaration
  /// </summary>
  // ================================================================================================
  public class FieldDeclarationNode : MemberDeclarationNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="FieldDeclarationNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public FieldDeclarationNode(Token start)
      : base(start)
    {
      FieldTags = new FieldTagNodeCollection();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance is event field.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this instance is event field; otherwise, <c>false</c>.
    /// </value>
    // ----------------------------------------------------------------------------------------------
    public bool IsEventField { get { return StartToken.Value == "event"; } }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the field tags belonging to this declaration.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public FieldTagNodeCollection FieldTags { get; private set; }
  }
}