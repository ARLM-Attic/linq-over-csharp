using CSharpParser.Semantics;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents an array type constructed from a declaration in the 
  /// source code.
  /// </summary>
  // ==================================================================================
  public sealed class NullableType : GenericType
  {
    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates an array type from the specified element type.
    /// </summary>
    /// <param name="structType">Nullable type</param>
    // --------------------------------------------------------------------------------
    public NullableType(ITypeAbstraction structType): 
      base(NetBinaryType.Nullable, structType)
    {
    }

    #endregion
  }
}