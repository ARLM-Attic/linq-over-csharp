namespace CSharpFactory.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This interface defines the behaviour of language elements that can be variables.
  /// </summary>
  /// <remarks>
  /// For details see C# Language specification section 12.
  /// </remarks>
  // ==================================================================================
  public interface IVariableInfo
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Defines the category of the variable.
    /// </summary>
    // --------------------------------------------------------------------------------
    VariableCategory Category { get; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Signs if the variable is initially assigned or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    bool IsInitiallyAssigned { get; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Stores the declaration point (token) of the variable.
    /// </summary>
    // --------------------------------------------------------------------------------
    int DeclarationPosition { get; }
  }
}
