using System;
using CSharpTreeBuilder.Ast;
using CSharpTreeBuilder.CSharpSemanticGraphBuilder;
using CSharpTreeBuilder.ProjectContent;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a simple name expression entity.
  /// </summary>
  // ================================================================================================
  public class SimpleNameExpressionEntity : ExpressionEntity
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="SimpleNameExpressionEntity"/> class.
    /// </summary>
    /// <param name="simpleNameNode">A simple name AST node.</param>
    // ----------------------------------------------------------------------------------------------
    public SimpleNameExpressionEntity(SimpleNameNode simpleNameNode)
    {
      if (simpleNameNode == null)
      {
        throw new ArgumentNullException("simpleNameNode");
      }

      SimpleNameNode = simpleNameNode;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the simple name AST node.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public SimpleNameNode SimpleNameNode { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the result of the simple name resolution.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public SimpleNameResult SimpleNameResult { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Evaluates this expression.
    /// </summary>
    /// <param name="errorHandler">An error handler object.</param>
    // ----------------------------------------------------------------------------------------------
    public override void Evaluate(ICompilationErrorHandler errorHandler)
    {
      // First resolve the simple name
      
      if (SimpleNameNode != null)
      {
        var simpleNameResolver = new SimpleNameResolver(errorHandler, SemanticGraph);
        SimpleNameResult = simpleNameResolver.Resolve(SimpleNameNode, this);
      }
      
      // Then determine the expression result

      if (SimpleNameResult != null)
      {
        if (SimpleNameResult.IsMethodGroup)
        {
          ExpressionResult = new MethodGroupExpressionResult(SimpleNameResult.MethodGroup);
        }

        // TODO: separate variable and value result

        else if (SimpleNameResult.SingleEntity is IVariableEntity)
        {
          ExpressionResult = new VariableExpressionResult(SimpleNameResult.SingleEntity as IVariableEntity);
        }
        else if (SimpleNameResult.SingleEntity is TypeEntity)
        {
          ExpressionResult = new TypeExpressionResult(SimpleNameResult.SingleEntity as TypeEntity);
        }
        else if (SimpleNameResult.SingleEntity is NamespaceEntity)
        {
          ExpressionResult = new NamespaceExpressionResult(SimpleNameResult.SingleEntity as NamespaceEntity);
        }

        // TODO
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
