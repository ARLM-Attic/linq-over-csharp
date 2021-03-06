// ================================================================================================
// TypeOrMemberDeclarationNode.cs
//
// Created: 2009.04.03, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This abstract class is intended to be the base class of or type and member declaration.
  /// </summary>
  /// <remarks>
  /// 	<para>Representation:</para>
  /// 	<para>
  ///         { <em>TypeParameterNode</em> }: <see cref="TypeParameters"/><br/>
  ///         { <em>TypeParameterConstraintNode</em> } : <see cref="TypeParameterConstraints"/>
  /// 	</para>
  /// </remarks>
  // ================================================================================================
  public abstract class TypeOrMemberDeclarationNode :
    AttributedDeclarationNode,
    ITypeParameterHolder,
    IIdentifierSupport
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeOrMemberDeclarationNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    protected TypeOrMemberDeclarationNode(Token start) : base(start)
    {
      TypeParameters = new TypeParameterNodeCollection { ParentNode = this };
      TypeParameterConstraints = new TypeParameterConstraintNodeCollection { ParentNode = this };
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the modifiers of this declaration node.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ModifierNodeCollection Modifiers { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the name of the type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public virtual string Name
    {
      get { return IdentifierToken.Value; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the name with generic dimensions (eg. A`1).
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string NameWithGenericDimensions
    {
      get
      {
        return TypeParameters.Count > 0 ? Identifier + "`" + TypeParameters.Count : Identifier;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the partial modifier token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token PartialToken { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance is partial type declaration.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsPartial
    {
      get { return PartialToken != null; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the identifier token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token IdentifierToken { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the alias identifier.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string Identifier
    {
      get { return IdentifierToken == null ? string.Empty : IdentifierToken.Value; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance has identifier.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this instance has identifier; otherwise, <c>false</c>.
    /// </value>
    // ----------------------------------------------------------------------------------------------
    public bool HasIdentifier
    {
      get { return IdentifierToken != null; }
    }

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
      get { return TypeParameters.StartToken != null; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the collection of type parameters.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeParameterNodeCollection TypeParameters { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type parameter constraints.
    /// </summary>
    /// <value>The type parameter constraints.</value>
    // ----------------------------------------------------------------------------------------------
    public TypeParameterConstraintNodeCollection TypeParameterConstraints { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the output segments representing the base type list.
    /// </summary>
    /// <returns></returns>
    // ----------------------------------------------------------------------------------------------
    protected virtual OutputSegment GetDeclarationSegments()
    {
      return null;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the output segment representing this syntax node.
    /// </summary>
    /// <returns>
    /// The OutputSegment instance describing this syntax node, or null; if the node has no output.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    public override OutputSegment GetOutputSegment()
    {
      return new OutputSegment(
        base.GetOutputSegment(),
        Modifiers,
        PartialToken,
        StartToken,
        MandatoryWhiteSpaceSegment.Default,
        IdentifierToken,
        TypeParameters,
        GetDeclarationSegments(),
        ForceNewLineSegment.Default,
        IndentationSegment.Increment,
        TypeParameterConstraints,
        IndentationSegment.Decrement
        );
    }
  }
}