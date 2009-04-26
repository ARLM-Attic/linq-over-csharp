// ================================================================================================
// CTypeMemberAccessNode.cs
//
// Created: 2009.04.26, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.ParserFiles;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This type is intended to be the base class of all member access operators.
  /// </summary>
  // ================================================================================================
  public abstract class MemberAccessNodeBase : PrimaryOperatorNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="MemberAccessNodeBase"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    protected MemberAccessNodeBase(Token start)
      : base(start)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the scope operand.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ExpressionNode ScopeOperand { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the member name.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public SimpleNameNode MemberName { get; internal set; }
  }
  
  // ================================================================================================
  /// <summary>
  /// This type represents a C-type member access operator ("->").
  /// </summary>
  // ================================================================================================
  public class CTypeMemberAccessNode : MemberAccessNodeBase
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="CTypeMemberAccessNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public CTypeMemberAccessNode(Token start)
      : base(start)
    {
    }
  }

  // ================================================================================================
  /// <summary>
  /// This type represents a member access operator (".").
  /// </summary>
  // ================================================================================================
  public class MemberAccessNode : MemberAccessNodeBase
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="MemberAccessNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public MemberAccessNode(Token start)
      : base(start)
    {
    }
  }
}