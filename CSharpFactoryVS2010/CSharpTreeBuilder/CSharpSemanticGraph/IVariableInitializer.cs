using System.Collections.Generic;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This interface defines the behavior of a variable initializer.
  /// </summary>
  // ================================================================================================
  public interface IVariableInitializer
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this initializer is an expression.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    bool IsExpression { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this is an array initializer.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    bool IsArrayInitializer { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the expression, if this initializer is an expression. Null if it's not an expression.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    ExpressionEntity Expression { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the collection of variable initializers, if this is an array initializer. 
    /// Null if it's not an array initializer.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    IEnumerable<IVariableInitializer> VariableInitializers { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Accepts a visitor object, according to the Visitor pattern.
    /// </summary>
    /// <param name="visitor">A visitor object</param>
    // ----------------------------------------------------------------------------------------------
    void AcceptVisitor(SemanticGraphVisitor visitor);
  }
}
