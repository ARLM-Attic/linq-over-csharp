// ================================================================================================
// TypeOrMemberDeclarationNode.cs
//
// Created: 2009.04.03, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.Collections;
using CSharpFactory.ParserFiles;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This class is intended to be the base class of or type and member declaration.
  /// </summary>
  // ================================================================================================
  public abstract class TypeOrMemberDeclarationNode : AttributedDeclarationNode,
    ITypeParameterHolder
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeOrMemberDeclarationNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    protected TypeOrMemberDeclarationNode(Token start) : base(start)
    {
      TypeParameters = new ImmutableCollection<TypeParameterNode>();
      TypeParameterConstraints = new TypeParameterConstraintNodeCollection();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the modifiers of this declaration node.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ModifierNodeCollection Modifiers { get; internal set; }

    #region Implementation of ITypeParameterHolder

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance has type parameters.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this instance has type parameters; otherwise, <c>false</c>.
    /// </value>
    // ----------------------------------------------------------------------------------------------
    public bool HasTypeParameters
    {
      get { return OpenSign != null; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the open sign token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token OpenSign { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Sets the open sign token.
    /// </summary>
    /// <param name="token">The token.</param>
    // ----------------------------------------------------------------------------------------------
    public void SetOpenSign(Token token)
    {
      OpenSign = token;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the close sign token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token CloseSign { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Sets the close sign token.
    /// </summary>
    /// <param name="token">The token.</param>
    // ----------------------------------------------------------------------------------------------
    public void SetCloseSign(Token token)
    {
      CloseSign = token;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the collection of type parameters.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ImmutableCollection<TypeParameterNode> TypeParameters { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type parameter constraints.
    /// </summary>
    /// <value>The type parameter constraints.</value>
    // ----------------------------------------------------------------------------------------------
    public TypeParameterConstraintNodeCollection TypeParameterConstraints { get; private set; }

    #endregion
  }
}