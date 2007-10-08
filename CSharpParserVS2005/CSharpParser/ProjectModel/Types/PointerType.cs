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
  public sealed class PointerType : SimpleExtendedType
  {
    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates an array type from the specified element type.
    /// </summary>
    /// <param name="elementType">Element type instance.</param>
    // --------------------------------------------------------------------------------
    public PointerType(ITypeAbstraction elementType) : base(elementType)
    {
    }

    #endregion

    #region Overridden methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is an array.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override bool IsArray
    {
      get { return false; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the Type is a pointer.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override bool IsPointer
    {
      get { return true; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the name of the current member.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override string Name
    {
      get { return GetElementType().Name + "*"; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this type is an unmanaged .NET runtime type or not
    /// </summary>
    // --------------------------------------------------------------------------------
    public override bool IsUnmanagedType
    {
      get { return true; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the parametrized name of the type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override string ParametrizedName
    {
      get { return TypeBase.GetParametrizedName(this) + "*"; }
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