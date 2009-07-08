using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents the 'base.member'  expression
  /// </summary>
  // ================================================================================================
  public sealed class BaseMemberAccessNode : BaseAccessNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="BaseMemberAccessNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public BaseMemberAccessNode(Token start)
      : base(start)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the member name.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public SimpleNameNode MemberName { get; internal set; }
  }
}