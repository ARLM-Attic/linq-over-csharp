// ================================================================================================
// TypeDeclarationNode.cs
//
// Created: 2009.04.07, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.ParserFiles;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This class is the base class of all syntax tree nodes representing a type declaration.
  /// </summary>
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
      NameToken = name;
      BaseTypes = new TypeOrNamespaceNodeCollection();
      NestedTypes = new TypeDeclarationNodeCollection();
      MemberDeclarations = new MemberDeclarationNodeCollection();
    }

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
    /// Gets a value indicating whether this instance is partial type declaration.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this instance is partial; otherwise, <c>false</c>.
    /// </value>
    // ----------------------------------------------------------------------------------------------
    public bool IsPartial { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the token representing the nameof the type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token NameToken { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the name of the type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string Name
    {
      get { return NameToken.val; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the collection of base types.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeOrNamespaceNodeCollection BaseTypes { get; private set; }

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
  }
}