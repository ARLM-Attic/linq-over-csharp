using System.Collections.Generic;

namespace CSharpParser.ParserFiles.PPExpressions
{
  // ==================================================================================
  /// <summary>
  /// This enumeration signs how th preprocessor evaluation finished.
  /// </summary>
  // ==================================================================================
  public enum PPEvaluationStatus
  {
    /// <summary>Evaluation failed.</summary>
    Failed,
    /// <summary>Expression evaluated to true.</summary>
    True,
    /// <summary>Expression evaluated to false.</summary>
    False
  }

  // ==================================================================================
  /// <summary>
  /// This abstract type represents a preprocessor expression.
  /// </summary>
  // ==================================================================================
  public abstract class PPExpression
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Evaluates the expression using the specified conditional symbols.
    /// </summary>
    /// <param name="conditionalSymbols">List of conditional symbols defined.</param>
    /// <returns>Result of evaluation.</returns>
    // --------------------------------------------------------------------------------
    public abstract bool Evaluate(List<string> conditionalSymbols);

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the leftmost part of this expression.
    /// </summary>
    // --------------------------------------------------------------------------------
    public PPExpression LeftmostExpression
    {
      get
      {
        PPUnaryOperator unOp = this as PPUnaryOperator;
        if (unOp != null) return unOp.Operand.LeftmostExpression;
        else
        {
          PPBinaryOperator binOp = this as PPBinaryOperator;
          if (binOp != null) return binOp.LeftOperand.LeftmostExpression;
        }
        return this;
      }
    }
  }

  // ==================================================================================
  /// <summary>
  /// This type represents a "true" literal.
  /// </summary>
  // ==================================================================================
  public sealed class PPTrueLiteral : PPExpression
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Evaluates this preprocessor expression.
    /// </summary>
    /// <param name="conditionalSymbols">List of conditional symbols defined.</param>
    /// <returns>
    /// Always evaluates to true.
    /// </returns>
    // --------------------------------------------------------------------------------
    public override bool Evaluate(List<string> conditionalSymbols)
    {
      return true;
    }
  }

  // ==================================================================================
  /// <summary>
  /// This type represents a "false" literal.
  /// </summary>
  // ==================================================================================
  public sealed class PPFalseLiteral : PPExpression
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Evaluates this preprocessor expression.
    /// </summary>
    /// <param name="conditionalSymbols">List of conditional symbols defined.</param>
    /// <returns>
    /// Always evaluates to false.
    /// </returns>
    // --------------------------------------------------------------------------------
    public override bool Evaluate(List<string> conditionalSymbols)
    {
      return false;
    }
  }

  // ==================================================================================
  /// <summary>
  /// This type represents a symbol
  /// </summary>
  // ==================================================================================
  public sealed class PPSymbol : PPExpression
  {
    private readonly string _Symbol;

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new symbol.
    /// </summary>
    /// <param name="symbol">Symbol name.</param>
    // --------------------------------------------------------------------------------
    public PPSymbol(string symbol)
    {
      _Symbol = symbol;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the symbol.
    /// </summary>
    // --------------------------------------------------------------------------------
    public string Symbol
    {
      get { return _Symbol; }
    } 
    
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Evaluates this preprocessor symbol.
    /// </summary>
    /// <param name="conditionalSymbols">List of conditional symbols defined.</param>
    /// <returns>
    /// True, If the symbol defined; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    public override bool Evaluate(List<string> conditionalSymbols)
    {
      return conditionalSymbols.Contains(_Symbol);
    }
  }

  // ==================================================================================
  /// <summary>
  /// This abstract type represents a unary operator
  /// </summary>
  // ==================================================================================
  public abstract class PPUnaryOperator : PPExpression
  {
    private readonly PPExpression _Operand;

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new unary operator with the specified operand.
    /// </summary>
    /// <param name="operand">Operand of the unary operator.</param>
    // --------------------------------------------------------------------------------
    protected PPUnaryOperator(PPExpression operand)
    {
      _Operand = operand;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the operand of this unary operator.
    /// </summary>
    // --------------------------------------------------------------------------------
    public PPExpression Operand
    {
      get { return _Operand; }
    }
  }

  // ==================================================================================
  /// <summary>
  /// This type represents a unary operator
  /// </summary>
  // ==================================================================================
  public sealed class PPNotOperator : PPUnaryOperator
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new not operator with the specified operand.
    /// </summary>
    /// <param name="operand">Operand of the not operator.</param>
    // --------------------------------------------------------------------------------
    public PPNotOperator(PPExpression operand)
      : base(operand)
    {
    } 
    
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Evaluates this operator.
    /// </summary>
    /// <param name="conditionalSymbols">List of conditional symbols defined.</param>
    /// <returns>
    /// True, If the symbol defined; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    public override bool Evaluate(List<string> conditionalSymbols)
    {
      return !Operand.Evaluate(conditionalSymbols);
    }
  }

  // ==================================================================================
  /// <summary>
  /// This abstract type represents a binary operator
  /// </summary>
  // ==================================================================================
  public abstract class PPBinaryOperator : PPExpression
  {
    private PPExpression _LeftOperand;
    private PPExpression _RightOperand;

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the left operand of this unary operator.
    /// </summary>
    // --------------------------------------------------------------------------------
    public PPExpression LeftOperand
    {
      get { return _LeftOperand; }
      set { _LeftOperand = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the right operand of this unary operator.
    /// </summary>
    // --------------------------------------------------------------------------------
    public PPExpression RightOperand
    {
      get { return _RightOperand; }
      set { _RightOperand = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the leftmost expression that has a null left operand including this 
    /// operator.
    /// </summary>
    // --------------------------------------------------------------------------------
    public PPBinaryOperator LeftMostNonNull
    {
      get
      {
        PPBinaryOperator current = this;
        do
        {
          if (current._LeftOperand == null) return current;
          PPBinaryOperator next = current._LeftOperand as PPBinaryOperator;
          if (next == null) return current;
          current = next;
        } while (true);
      }
    }
  }

  // ==================================================================================
  /// <summary>
  /// This type represents an or operator
  /// </summary>
  // ==================================================================================
  public sealed class PPOrOperator : PPBinaryOperator
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Evaluates this operator.
    /// </summary>
    /// <param name="conditionalSymbols">List of conditional symbols defined.</param>
    /// <returns>
    /// True, If the symbol defined; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    public override bool Evaluate(List<string> conditionalSymbols)
    {
      return LeftOperand.Evaluate(conditionalSymbols) ||
        RightOperand.Evaluate(conditionalSymbols);
    }
  }

  // ==================================================================================
  /// <summary>
  /// This type represents an and operator
  /// </summary>
  // ==================================================================================
  public sealed class PPAndOperator : PPBinaryOperator
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Evaluates this operator.
    /// </summary>
    /// <param name="conditionalSymbols">List of conditional symbols defined.</param>
    /// <returns>
    /// True, If the symbol defined; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    public override bool Evaluate(List<string> conditionalSymbols)
    {
      return LeftOperand.Evaluate(conditionalSymbols) &&
        RightOperand.Evaluate(conditionalSymbols);
    }
  }

  // ==================================================================================
  /// <summary>
  /// This type represents an equality operator
  /// </summary>
  // ==================================================================================
  public sealed class PPEqualOperator : PPBinaryOperator
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Evaluates this operator.
    /// </summary>
    /// <param name="conditionalSymbols">List of conditional symbols defined.</param>
    /// <returns>
    /// True, If the symbol defined; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    public override bool Evaluate(List<string> conditionalSymbols)
    {
      return LeftOperand.Evaluate(conditionalSymbols) ==
        RightOperand.Evaluate(conditionalSymbols);
    }
  }

  // ==================================================================================
  /// <summary>
  /// This type represents an or operator
  /// </summary>
  // ==================================================================================
  public sealed class PPNotEqualOperator : PPBinaryOperator
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Evaluates this operator.
    /// </summary>
    /// <param name="conditionalSymbols">List of conditional symbols defined.</param>
    /// <returns>
    /// True, If the symbol defined; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    public override bool Evaluate(List<string> conditionalSymbols)
    {
      return LeftOperand.Evaluate(conditionalSymbols) !=
        RightOperand.Evaluate(conditionalSymbols);
    }
  }
}