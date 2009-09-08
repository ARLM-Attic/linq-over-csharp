// ================================================================================================
// LocalVariableNode.cs
//
// Created: 2009.05.06, by Istvan Novak (DeepDiver)
// ================================================================================================
namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents a local variable declaration
  /// </summary>
  // ================================================================================================
  public class LocalVariableNode : SyntaxNode<ISyntaxNode>
  {
    // --- Backing fields
    private TypeNode _Type;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="LocalVariableNode"/> class.
    /// </summary>
    /// <param name="typeNode">The type node.</param>
    // ----------------------------------------------------------------------------------------------
    public LocalVariableNode(TypeNode typeNode)
      : base(typeNode.StartToken)
    {
      Type = typeNode;
      VariableTags = new LocalVariableTagNodeCollection { ParentNode = this };
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the name of the type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeNode Type
    {
      get { return _Type; }
      internal set
      {
        _Type = value;
        if (_Type != null) _Type.ParentNode = this;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this is an implicit type declaration.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this instance is implicit; otherwise, <c>false</c>.
    /// </value>
    // ----------------------------------------------------------------------------------------------
    public bool IsImplicit
    {
      get { return Type.TypeName.TypeTags.Count == 1 && Type.StartToken.Value == "var"; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets variable tags within this variable declaration.
    /// </summary>
    /// <value>The variables.</value>
    // ----------------------------------------------------------------------------------------------
    public LocalVariableTagNodeCollection VariableTags { get; private set; }

    #region Visitor methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Accepts a visitor object, according to the Visitor pattern.
    /// </summary>
    /// <param name="visitor">A visitor object</param>
    // ----------------------------------------------------------------------------------------------
    public override void AcceptVisitor(ISyntaxNodeVisitor visitor)
    {
      if (!visitor.Visit(this)) { return; }

      if (Type!=null)
      {
        Type.AcceptVisitor(visitor);
      }

      foreach (var variableTag in VariableTags)
      {
        variableTag.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}