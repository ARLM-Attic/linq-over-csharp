using System;
using CSharpParser.Collections;
using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents an operator member declaration.
  /// </summary>
  // ==================================================================================
  public class OperatorDeclaration : MethodDeclaration
  {
    #region Private fields

    private Operator _Operator;

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
          ((_Operator == Operator.plus || _Operator == Operator.minus) 
            && FormalParameters.Count == 1) ||
          _Operator == Operator.not || 
          _Operator == Operator.tilde || 
          _Operator == Operator.dec || 
          _Operator == Operator.inc || 
          _Operator == Operator.@true || 
          _Operator == Operator.@false;
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

      AbstractNotAllowed();
      VirtualNotAllowed();
      OverrideNotAllowed();
      SealedNotAllowed();
      ReadOnlyNotAllowed();
      VolatileNotAllowed();
      NewNotAllowed();

      // --- Only "public static" declaration is allowed.
      if (!HasDefaultVisibility && DeclaredVisibility != Visibility.Public)
      {
        Parser.Error0106(Token, Visibility.ToString().ToLower()); 
      }
      if (DeclaredVisibility != Visibility.Public || !IsStatic)
      {
        Parser.Error0558(Token, Signature);
        Invalidate();
      }

      // --- Operators cannot have "ref" or "out" parameter modifiers.
      foreach (FormalParameter param in FormalParameters)
      {
        if (param.Kind != FormalParameterKind.In) Parser.Error0631(Token);
      }

      // --- No more checks, if the resulting type is not resolved.
      if (!ResultingType.RightMostPart.IsResolvedToType) return;

      // --- Operator cannot return void
      if (ResultingType.RightMostPart.ResolvingType.TypeObject == typeof(void))
      {
        Parser.Error0590(Token);
        Invalidate();
      }

      // --- Check unary operators
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
    }

    #endregion
  }

  // ==================================================================================
  /// <summary>
  /// This type defines a collection of operator declarations that can be indexed by the
  /// signature of the method.
  /// </summary>
  // ==================================================================================
  public class OperatorDeclarationCollection : RestrictedIndexedCollection<OperatorDeclaration>
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
