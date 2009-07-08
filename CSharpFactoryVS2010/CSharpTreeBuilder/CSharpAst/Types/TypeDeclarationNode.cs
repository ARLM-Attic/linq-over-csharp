// ================================================================================================
// TypeDeclarationNode.cs
//
// Created: 2009.04.07, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpTreeBuilder.Collections;
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class is the base class of all syntax tree nodes representing a type declaration.
  /// </summary>
  /// <remarks>
  /// 	<para>Representation:</para>
  /// 	<blockquote style="MARGIN-RIGHT: 0px" dir="ltr">
  /// 		<para>
  /// 			<em>identifier</em>: <see cref="TypeOrMemberDeclarationNode.IdentifierToken"/><br/>
  ///             "<strong>:</strong>": <see cref="ColonToken"/> (optional separator for
  ///             base types)<br/>
  ///             { <em>TypeOrNamespaceNode</em> }: <see cref="BaseTypes"/><br/>
  ///             { <em>TypeOrMemberNode</em> }: <see cref="NestedDeclarations"/><br/>
  ///             { <em>TypeDeclarationNode</em> }: <see cref="NestedTypes"/><br/>
  ///             { <em>MemberDeclarationNode</em> }: <see cref="MemberDeclarations"/>
  /// 		</para>
  /// 	</blockquote>
  /// </remarks>
  // ================================================================================================
  public abstract class TypeDeclarationNode : TypeOrMemberDeclarationNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeDeclarationNode"/> class.
    /// </summary>
    /// <param name="start">The start token of the declaration.</param>
    /// <param name="name">The name of the delcaration.</param>
    // ----------------------------------------------------------------------------------------------
    protected TypeDeclarationNode(Token start, Token name)
      : base(start)
    {
      IdentifierToken = name;
      BaseTypes = new TypeOrNamespaceNodeCollection { ParentNode = this };
      NestedDeclarations = new TypeOrMemberNodeCollection { ParentNode = this };
      NestedTypes = new TypeDeclarationNodeCollection { ParentNode = this };
      MemberDeclarations = new MemberDeclarationNodeCollection { ParentNode = this };
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the colon token separating the type and its base classes.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token ColonToken { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the type declaring this type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeDeclarationNode DeclaringType { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the declaring namespace.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public NamespaceScopeNode DeclaringNamespace { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the collection of base types.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeOrNamespaceNodeCollection BaseTypes { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the nested declarations.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeOrMemberNodeCollection NestedDeclarations { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the collection of nested types.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeDeclarationNodeCollection NestedTypes { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the collection of member declarations.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public MemberDeclarationNodeCollection MemberDeclarations { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the output segments representing the base type list.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    protected override OutputSegment GetDeclarationSegments()
    {
      return new OutputSegment(
        SpaceBeforeSegment.BeforeBaseTypeColon(ColonToken),
        SpaceAfterSegment.AfterBaseTypeColon(),
        BaseTypes
        );
    }
  }
}