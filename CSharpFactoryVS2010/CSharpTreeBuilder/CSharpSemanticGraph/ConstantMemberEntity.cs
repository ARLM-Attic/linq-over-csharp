using System;
using System.Collections.Generic;
using CSharpTreeBuilder.CSharpSemanticGraphBuilder;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a constant member of a type.
  /// </summary>
  // ================================================================================================
  public class ConstantMemberEntity : NonTypeMemberEntity, IHasExpressions
  {
    #region State

    /// <summary>Gets the reference to the type of the field.</summary>
    public Resolver<TypeEntity> TypeReference { get; private set; }

    /// <summary>Gets the initializer expression of the constant.</summary>
    public ExpressionEntity InitializerExpression { get; private set; }

    #endregion

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
      Resolver<TypeEntity> type,
      string name)
      : 
      base(isDeclaredInSource, accessibility, name)
    {
      TypeReference = type;
      _IsStatic = true;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ConstantMemberEntity"/> class 
    /// by constructing it from a template instance.
    /// </summary>
    /// <param name="template">The template for the new instance.</param>
    /// <param name="typeParameterMap">The type parameter map of the new instance.</param>
    // ----------------------------------------------------------------------------------------------
    protected ConstantMemberEntity(ConstantMemberEntity template, TypeParameterMap typeParameterMap)
      : base(template, typeParameterMap)
    {
      if (template.TypeReference != null)
      {
        TypeReference = (Resolver<TypeEntity>)template.TypeReference.GetGenericClone(typeParameterMap);
      }

      if (template.InitializerExpression != null)
      {
        InitializerExpression = (ExpressionEntity) template.InitializerExpression.GetGenericClone(typeParameterMap);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new constructed entity.
    /// </summary>
    /// <param name="typeParameterMap">A collection of type parameters and associated type arguments.</param>
    /// <returns>
    /// A new semantic entity constructed from this entity using the specified type parameter map.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    protected override SemanticEntity ConstructNew(TypeParameterMap typeParameterMap)
    {
      return new ConstantMemberEntity(this, typeParameterMap);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a child entity.
    /// </summary>
    /// <param name="entity">A child entity.</param>
    // ----------------------------------------------------------------------------------------------
    public override void AddChild(ISemanticEntity entity)
    {
      if (entity is ExpressionEntity)
      {
        AddExpression(entity as ExpressionEntity);
      }
      else
      {
        base.AddChild(entity);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type of the field.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeEntity Type
    {
      get
      {
        return TypeReference != null ? TypeReference.Target : null;
      }
    }

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

      if (expressionEntity != null)
      {
        expressionEntity.Parent = this;
      }
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
        return InitializerExpression == null
          ? new List<ExpressionEntity>()
          : new List<ExpressionEntity> { InitializerExpression };
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this member is invocable.
    /// </summary>
    /// <remarks>A member is invocable if it's a method or event, 
    /// or if it is a constant, field or property of a delegate type.</remarks>
    // ----------------------------------------------------------------------------------------------
    public override bool IsInvocable
    {
      get
      {
        return (Type != null) && Type is DelegateEntity;
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
      visitor.Visit(this);
      base.AcceptVisitor(visitor);

      if (InitializerExpression != null)
      {
        InitializerExpression.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}
