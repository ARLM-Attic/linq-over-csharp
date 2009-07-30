namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This interface defines the behavior of a variable.
  /// </summary>
  /// <remarks>
  /// Variables represent storage locations, and every variable has a type 
  /// that determines what values can be stored in the variable.
  /// There are several kinds of variables: 
  /// including fields, array elements, local variables, and parameters.
  /// </remarks>
  // ================================================================================================
  public interface IVariableEntity
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type of the variable.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    SemanticEntityReference<TypeEntity> Type { get; }
  }
}
