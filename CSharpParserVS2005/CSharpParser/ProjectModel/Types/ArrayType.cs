using System.Collections.Generic;
using CSharpParser.Semantics;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents an array type constructed from a declaration in the 
  /// source code.
  /// </summary>
  // ==================================================================================
  public sealed class ArrayType : SimpleExtendedType
  {
    #region Private fields

    private readonly int _Rank; 

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates an array type from the specified element type.
    /// </summary>
    /// <param name="elementType">Element type instance.</param>
    /// <param name="rank">Array rank</param>
    // --------------------------------------------------------------------------------
    public ArrayType(ITypeAbstraction elementType, int rank)
      : base(elementType)
    {
      _Rank = rank;
    }

    #endregion

    #region Overridden methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the number of dimensions of an array type.
    /// </summary>
    /// <returns>Number of array dimensions.</returns>
    // --------------------------------------------------------------------------------
    public override int GetArrayRank()
    {
      return _Rank;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is an array.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override bool IsArray
    {
      get { return true; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is a pointer.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override bool IsPointer
    {
      get { return false; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the name of the current member.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override string Name
    {
      get
      {
        return GetElementType().Name + "[" + string.Empty.PadRight(_Rank - 1, ',') + "]";
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this type is an unmanaged .NET runtime type or not
    /// </summary>
    // --------------------------------------------------------------------------------
    public override bool IsUnmanagedType
    {
      get { return false; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the parametrized name of the type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override string ParametrizedName
    {
      get
      {
        return TypeBase.GetParametrizedName(this) + 
          "[" + string.Empty.PadRight(_Rank - 1, ',') + "]";
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a list representing the generic arguments of a type.
    /// </summary>
    /// <returns>
    /// Arguments of a generic typeor generic type declaration.
    /// </returns>
    // --------------------------------------------------------------------------------
    public override List<ITypeAbstraction> GetGenericArguments()
    {
      return NetBinaryType.EmptyTypes;
    }
    
    #endregion
  }
}