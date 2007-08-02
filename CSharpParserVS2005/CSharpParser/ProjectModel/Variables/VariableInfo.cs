using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This structure contains information about variables declared in the source code.
  /// </summary>
  /// <remarks>
  /// For details see C# Language specification section 12.
  /// </remarks>
  // ==================================================================================
  public struct VariableInfo
  {
    /// <summary>
    /// Defines the category of the variable.
    /// </summary>
    public readonly VariableCategory Category;

    /// <summary>
    /// Signs if the variable is initially assigned or not.
    /// </summary>
    public readonly bool IsInitiallyAssigned;

    /// <summary>
    /// Stores the declaration point (token) of the variable.
    /// </summary>
    public readonly Token DeclarationPoint;

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates an initial variable information section.
    /// </summary>
    /// <param name="category">
    /// Category of the variable.
    /// </param>
    /// <param name="isInitiallyAssigned">
    /// Signs if the variable is initially assigned or not.
    /// </param>
    /// <param name="declarationPoint">
    /// Stores the declaration point (token) of the variable.
    /// </param>
    // --------------------------------------------------------------------------------
    public VariableInfo(VariableCategory category, bool isInitiallyAssigned, 
      Token declarationPoint)
    {
      Category = category;
      IsInitiallyAssigned = isInitiallyAssigned;
      DeclarationPoint = declarationPoint;
    }
  }
}
