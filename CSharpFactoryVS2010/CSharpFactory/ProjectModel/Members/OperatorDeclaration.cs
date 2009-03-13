using System;
using System.Collections.Generic;
using System.Text;
using CSharpFactory.Collections;
using CSharpFactory.ParserFiles;
using CSharpFactory.Semantics;
using Token=CSharpFactory.ParserFiles.Token;

namespace CSharpFactory.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents an operator member declaration.
  /// </summary>
  // ==================================================================================
  public class OperatorDeclaration : MethodDeclaration
  {
    #region Private fields

    private ParserFiles.Operator _Operator;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new operator declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="declaringType">Type declaring this member.</param>
    // --------------------------------------------------------------------------------
    public OperatorDeclaration(Token token, TypeDeclaration declaringType)
      : base(token, declaringType)
    {
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the name of this member
    /// </summary>
    /// <remarks>
    /// Adds an "op_" prefix before the operator name to avoid collision with 
    /// constructors having the same parameter signature.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public override string Name
    {
      get { return "op_" + base.Name; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the operator code.
    /// </summary>
    // --------------------------------------------------------------------------------
    public Operator Operator
    {
      get { return _Operator; }
      set { _Operator = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this operator is a unary operator
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsUnary
    {
      get
      {
        return 
          ((_Operator == Operator.Plus || _Operator == Operator.Minus) 
            && FormalParameters.Count == 1) ||
          _Operator == Operator.Not || 
          _Operator == Operator.BitwiseNot || 
          _Operator == Operator.Decrement || 
          _Operator == Operator.Increment || 
          _Operator == Operator.True || 
          _Operator == Operator.False;
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this operator is a binary operator
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsBinary
    {
      get { return !IsUnary; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the signature to test for pairing operators.
    /// </summary>
    // --------------------------------------------------------------------------------
    public string PairingSignature
    {
      get
      {
        StringBuilder sb = new StringBuilder();
        foreach (FormalParameter param in FormalParameters)
        {
          if (!param.Type.IsResolvedToType) continue;
          if (sb.Length > 0) sb.Append(", ");
          sb.Append(param.Type.ParametrizedName);
        }
        return sb.ToString();
      }
    }

    #endregion

    #region Semantic checks

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks the semantics for the specified field declaration.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override void CheckSemantics()
    {
      CheckGeneralMemberSemantics();
      CheckCommonOperatorSemantics();

      // --- No more checks, if the resulting type is not resolved.
      if (!ResultingType.TailIsType) return;

      // --- Operator cannot return void
      if (TypeBase.IsSame(ResultingType.Tail.TypeInstance, typeof(void)))
      {
        Parser.Error0590(Token);
        Invalidate();
      }

      // --- Check operator specific rules
      if (IsUnary)
        CheckUnaryOperator();
      else
        CheckBinaryOperator();
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks if the unary operator definition is correct.
    /// </summary>
    // --------------------------------------------------------------------------------
    private void CheckUnaryOperator()
    {
      // --- Unary operator must have exactly one formal parameter.
      if (FormalParameters.Count != 1)
      {
        Parser.Error1535(Token, Name);
        Invalidate();
        return;
      }

      // --- No more check, if the type is not resolved.
      if (!FormalParameters[0].Type.IsResolvedToType) return;

      // --- Parameter type must be the containing type or its Nullable version.
      ITypeAbstraction unaryArg = FormalParameters[0].Type.TypeInstance;
      if (!CheckContainingType(unaryArg))
      {
        Parser.Error0562(Token);
      }

      if (_Operator == Operator.Increment || _Operator == Operator.Decrement)
      {
        // --- Return type must be the containing type or its derived type.
        if (!TypeBase.IsSameOrInheritsFrom(ResultingType.Tail.TypeInstance,
          unaryArg))
        {
          Parser.Error0448(Token);
        }
      }
      else if (_Operator == Operator.True || _Operator == Operator.False)
      {
        // --- Return type must be bool
        if (!TypeBase.IsSame(ResultingType.Tail.TypeInstance, typeof(bool)))
        {
          Parser.Error0215(Token);
        }
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks if the binary operator definition is correct.
    /// </summary>
    // --------------------------------------------------------------------------------
    private void CheckBinaryOperator()
    {
      // --- Binary operator must have exactly two formal parameter.
      if (FormalParameters.Count != 2)
      {
        Parser.Error1534(Token, Name);
        Invalidate();
        return;
      }

      // --- No more check, if the types are not resolved.
      if (!FormalParameters[0].Type.IsResolvedToType) return;
      if (!FormalParameters[1].Type.IsResolvedToType) return;

      // --- 
      if (_Operator == Operator.LeftShift || _Operator == Operator.RightShift)
      {
        // --- Shift operators must have the enclosing type as their first parameter
        // --- and int as their second parameter.
      }
      else
      {
        // --- Non-shift operators must have either the first or the second parameter
        // --- the same as their containing type.
        bool firstOk = CheckContainingType(FormalParameters[0].Type.TypeInstance);
        bool secondOk = CheckContainingType(FormalParameters[1].Type.TypeInstance);
        if (!firstOk && !secondOk)
        {
          Parser.Error0563(Token);
        }
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks, if the type of the parameter matches with the containing type.
    /// </summary>
    /// <returns>
    /// True, if the type of the parameter is the containing type or its nullable 
    /// version; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    private bool CheckContainingType(ITypeAbstraction param)
    {
      if (param == DeclaringType) return true;
      if (!TypeBase.IsSame(param, typeof(Nullable<>)) && !(param is NullableType)) 
        return false;
      List<ITypeAbstraction> typeArguments = param.GetGenericArguments();
      return typeArguments.Count == 1 && typeArguments[0] == DeclaringType;
    }

    #endregion
  }

  // ==================================================================================
  /// <summary>
  /// This type defines a collection of operator declarations that can be indexed by the
  /// signature of the method.
  /// </summary>
  // ==================================================================================
  public class OperatorDeclarationCollection : ImmutableIndexedCollection<OperatorDeclaration>
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Defines the key to be used by the indexing.
    /// </summary>
    /// <param name="item">OperatorDeclaration item.</param>
    /// <returns>
    /// Signature of the operator declaration.
    /// </returns>
    // --------------------------------------------------------------------------------
    protected override string GetKeyOfItem(OperatorDeclaration item)
    {
      return item.Signature;
    }
  }
}
