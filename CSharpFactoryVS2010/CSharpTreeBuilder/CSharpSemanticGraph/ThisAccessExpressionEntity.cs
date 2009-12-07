using System;
using CSharpTreeBuilder.CSharpSemanticGraphBuilder;
using CSharpTreeBuilder.ProjectContent;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a this access expression entity.
  /// </summary>
  // ================================================================================================
  public sealed class ThisAccessExpressionEntity : ExpressionEntity
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ThisAccessExpressionEntity"/> class.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ThisAccessExpressionEntity()
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ThisAccessExpressionEntity"/> class 
    /// by constructing it from a template instance.
    /// </summary>
    /// <param name="template">The template for the new instance.</param>
    /// <param name="typeParameterMap">The type parameter map of the new instance.</param>
    // ----------------------------------------------------------------------------------------------
    private ThisAccessExpressionEntity(ThisAccessExpressionEntity template, TypeParameterMap typeParameterMap)
      : base(template, typeParameterMap)
    {
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
      return new ThisAccessExpressionEntity(this, typeParameterMap);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Evaluates this expression.
    /// </summary>
    /// <param name="errorHandler">An error handler object.</param>
    // ----------------------------------------------------------------------------------------------
    public override void Evaluate(ICompilationErrorHandler errorHandler)
    {      
      // A this-access is permitted only in the block of an instance constructor, an instance method,
      // or an instance accessor. It has one of the following meanings:

      // When this is used in a primary-expression within an instance constructor of a class, 
      // it is classified as a value. 
      // The type of the value is the instance type (§10.3.1) of the class within which the usage occurs, 
      // and the value is a reference to the object being constructed.

      // When this is used in a primary-expression within an instance method or instance accessor of a class, 
      // it is classified as a value. 
      // The type of the value is the instance type (§10.3.1) of the class within which the usage occurs, 
      // and the value is a reference to the object for which the method or accessor was invoked.
      
      // When this is used in a primary-expression within an instance constructor of a struct, 
      // it is classified as a variable. 
      // The type of the variable is the instance type (§10.3.1) of the struct within which the usage occurs, 
      // and the variable represents the struct being constructed. 

      // When this is used in a primary-expression within an instance method or instance accessor of a struct,
      // it is classified as a variable. 
      // The type of the variable is the instance type (§10.3.1) of the struct within which the usage occurs.
      // - If the method or accessor is not an iterator (§10.14), 
      //   the this variable represents the struct for which the method or accessor was invoked, 
      //   and behaves exactly the same as a ref parameter of the struct type.
      // - If the method or accessor is an iterator, 
      //   the this variable represents a copy of the struct for which the method or accessor was invoked, 
      //   and behaves exactly the same as a value parameter of the struct type.

      var parentType = this.GetEnclosing<TypeEntity>();

      if (parentType is ClassEntity)
      {
        ExpressionResult = new ValueExpressionResult(parentType);
      }
      else if (parentType is StructEntity)
      {
        // TODO: this should be a VariableExpressionResult, we use ValueExpressionResult just as a quick shortcut.
        ExpressionResult = new ValueExpressionResult(parentType);
      }
      
      // Use of this in a primary-expression in a context other than the ones listed above is a compile-time error.
      // In particular, it is not possible to refer to this in a static method, a static property accessor, 
      // or in a variable-initializer of a field declaration.
      if (ExpressionResult == null)
      {
        // TODO: signal error
        throw new ApplicationException("Can't evaluate 'this' expression.");
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
    }

    #endregion
  }
}
