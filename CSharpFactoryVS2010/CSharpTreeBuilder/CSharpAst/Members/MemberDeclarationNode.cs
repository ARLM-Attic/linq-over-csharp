// ================================================================================================
// MemberDeclarationNode.cs
//
// Created: 2009.04.08, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class is an abstraction root for all type member declarations.
  /// </summary>
  // ================================================================================================
  public abstract class MemberDeclarationNode : TypeOrMemberDeclarationNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="MemberDeclarationNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    protected MemberDeclarationNode(Token start)
      : base(start)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the type of the member.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeOrNamespaceNode TypeName { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the member name of this method.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeOrNamespaceNode MemberName { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance has simple name.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool HasSimpleName
    {
      get { return MemberName != null && MemberName.TypeTags.Count == 1; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the name of method.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public override string Name
    {
      get
      {
        return MemberName != null && MemberName.TypeTags.Count > 0
                 ? MemberName.TypeTags[MemberName.TypeTags.Count - 1].Identifier
                 : Identifier;
      }
    }
  }
}