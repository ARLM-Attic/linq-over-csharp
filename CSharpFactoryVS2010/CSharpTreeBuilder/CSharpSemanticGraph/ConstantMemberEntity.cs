using System;
using System.Collections.Generic;
using CSharpTreeBuilder.CSharpSemanticGraphBuilder;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a constant member a type.
  /// </summary>
  // ================================================================================================
  public class ConstantMemberEntity : MemberEntity, IHasExpressions
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ConstantMemberEntity"/> class.
    /// </summary>
    /// <param name="isDeclaredInSource">True if the member is explicitly declared in source code, false otherwise.</param>
    /// <param name="accessibility">The declared accessibility of the member. Can be null.</param>
    /// <param name="type">The type of the member (a type entity reference).</param>
    /// <param name="name">The name of the member.</param>
    // ----------------------------------------------------------------------------------------------
    public ConstantMemberEntity(
      bool isDeclaredInSource, 
      AccessibilityKind? accessibility, 
      SemanticEntityReference<TypeEntity> type,
      string name)
      : 
      base(isDeclaredInSource, accessibility, name)
    {
      TypeReference = type;
      _IsStatic = true;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type of the field.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public SemanticEntityReference<TypeEntity> TypeReference { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type of the field.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeEntity Type
    {
      get { return TypeReference == null ? null : TypeReference.TargetEntity; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the initializer expression of the constant.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ExpressionEntity InitializerExpression { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets a value indicating whether this member belongs to the class itself
    /// (as opposed to an instance object).
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public override bool IsStatic
    {
      set { throw new InvalidOperationException("Constant members are always static."); }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Sets the initializer expression.
    /// </summary>
    /// <param name="expressionEntity">An expression entity.</param>
    // ----------------------------------------------------------------------------------------------
    public void AddExpression(ExpressionEntity expressionEntity)
    {
      InitializerExpression = expressionEntity;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an iterate-only collection of child expressions.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public IEnumerable<ExpressionEntity> Expressions
    {
      get
      {
        return new List<ExpressionEntity> {InitializerExpression};
      }
    }

    #region Visitor methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Accepts a visitor object, according to the Visitor pattern.
    /// </summary>
    /// <param name="visitor">A visitor object</param>
    // ----------------------------------------------------------------------------------------------
    public override void AcceptVisitor(SemanticGraphVisitor visitor)
    {
      if (!visitor.Visit(this)) { return; }

      if (InitializerExpression!=null)
      {
        InitializerExpression.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}
