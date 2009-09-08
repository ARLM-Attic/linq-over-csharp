using System;
using System.Collections.Generic;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents an array initialier which is a list of expressions.
  /// </summary>
  // ================================================================================================
  public sealed class ArrayInitializerEntity : SemanticEntity, IVariableInitializer
  {
    /// <summary>Backing field for VariableInitializers property.</summary>
    private readonly List<IVariableInitializer> _VariableInitializers;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ArrayInitializerEntity"/> class.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ArrayInitializerEntity()
    {
      _VariableInitializers = new List<IVariableInitializer>();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this initializer is an expression.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsExpression
    {
      get { return false; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this is an array initializer.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsArrayInitializer
    {
      get { return true; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the expression, if this initializer is an expression. Null if it's not an expression.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ExpressionEntity Expression
    {
      get { return null; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the collection of variable initializers.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public IEnumerable<IVariableInitializer> VariableInitializers
    {
      get { return _VariableInitializers; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a variable initializer to the VariableInitializers collection.
    /// </summary>
    /// <param name="initializer">A variable initializer.</param>
    // ----------------------------------------------------------------------------------------------
    public void AddVariableInitializer(IVariableInitializer initializer)
    {
      _VariableInitializers.Add(initializer);
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
    }

    #endregion
  }
}
