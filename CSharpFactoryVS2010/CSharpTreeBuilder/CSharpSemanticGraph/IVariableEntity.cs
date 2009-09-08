using CSharpTreeBuilder.CSharpSemanticGraphBuilder;

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
    /// Gets the type reference of the variable.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    SemanticEntityReference<TypeEntity> TypeReference { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type of the variable.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    TypeEntity Type { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this variable is an array. 
    /// Null if the type of the variable is not yet resolved.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    bool? IsArray { get; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the initializer of the variable.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    IVariableInitializer Initializer { get; }
  }
}
