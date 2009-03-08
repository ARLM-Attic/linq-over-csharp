using System.Text;
using CSharpFactory.Collections;
using CSharpFactory.ParserFiles;
using CSharpFactory.Semantics;

namespace CSharpFactory.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents a cast operator declaration.
  /// </summary>
  // ==================================================================================
  public sealed class CastOperatorDeclaration : MethodDeclaration
  {
    #region Private fields

    private bool _IsExplicit;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new cast operator declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="declaringType">Type declaring this member.</param>
    // --------------------------------------------------------------------------------
    public CastOperatorDeclaration(Token token, TypeDeclaration declaringType)
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
      get { return "castOp_" + base.Name; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the flag indicating if this is an explicit operator or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsExplicit
    {
      get { return _IsExplicit; }
      set { _IsExplicit = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this is an implicit operator or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsImplicit
    {
      get { return !_IsExplicit; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the signature of this cast operator
    /// </summary>
    // --------------------------------------------------------------------------------
    public string OperatorSignature
    {
      get
      {
        StringBuilder sb = new StringBuilder();
        if (ResultingType.IsResolvedToType)
          sb.Append(ResultingType.ParametrizedName);
        else
          sb.Append(ResultingType.Name);
        sb.Append("(");
        if (FormalParameters.Count > 0)
        {
          if (FormalParameters[0].Type.IsResolvedToType)
            sb.Append(FormalParameters[0].Type.ParametrizedName);
          else
            sb.Append(FormalParameters[0].Type.Name);
        }
        sb.Append(")");
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

      // --- Operator cannot cast to void
      if (TypeBase.IsSame(ResultingType.Tail.TypeInstance, typeof(void)))
      {
        Parser.Error1547(Token, "void");
        Invalidate();
      }

      // --- Cast operator is an unary operator, must have exactly one parameter
      if (FormalParameters.Count != 1)
      {
        Parser.Error1535(Token, Name);
        Invalidate();
        return;
      }

      // --- Obtain the non-nullable equivalents of the input and output types
      ITypeAbstraction resultType =
        TypeBase.GetNonNullableElement(ResultingType.Tail.TypeInstance);
      ITypeAbstraction inputType = 
        TypeBase.GetNonNullableElement(FormalParameters[0].Type.TypeInstance);

      // --- At least one of the elements must be the enclosing type.
      if (!TypeBase.IsSame(resultType, DeclaringType) &&
        !TypeBase.IsSame(inputType, DeclaringType))
      {
        Parser.Error0556(Token);
        return;
      }

      // --- Input and output types must be different
      if (TypeBase.IsSame(resultType, inputType))
      {
        Parser.Error0555(Token);
        return;
      }

      // --- Nor the input neither the result type cannot be interfaces.
      if (resultType.IsInterface || inputType.IsInterface)
      {
        Parser.Error0552(Token, Signature);
      }
    }

    #endregion

  }

  // ==================================================================================
  /// <summary>
  /// This type defines a collection of cast operator declarations that can be indexed 
  /// by the signature of the method.
  /// </summary>
  // ==================================================================================
  public class CastOperatorDeclarationCollection : 
    RestrictedIndexedCollection<CastOperatorDeclaration>
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Defines the key to be used by the indexing.
    /// </summary>
    /// <param name="item">CastOperatorDeclaration item.</param>
    /// <returns>
    /// Signature of the cast operator declaration.
    /// </returns>
    // --------------------------------------------------------------------------------
    protected override string GetKeyOfItem(CastOperatorDeclaration item)
    {
      return item.Signature;
    }
  }
}
