namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This abstract class represents a variable initializer.
  /// </summary>
  // ================================================================================================
  public abstract class VariableInitializer : SemanticEntity
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="VariableInitializer"/> class.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    protected VariableInitializer()
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="VariableInitializer"/> class 
    /// by constructing it from a template instance.
    /// </summary>
    /// <param name="template">The template for the new instance.</param>
    /// <param name="typeParameterMap">The type parameter map of the new instance.</param>
    // ----------------------------------------------------------------------------------------------
    protected VariableInitializer(VariableInitializer template, TypeParameterMap typeParameterMap)
      : base(template, typeParameterMap)
    {
    }
  }
}
