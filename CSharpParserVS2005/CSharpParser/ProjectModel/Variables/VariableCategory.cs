namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This enumeration type defines the variable categories.
  /// </summary>
  /// <remarks>
  /// For details see C# Language specification section 12.1
  /// </remarks>
  // ==================================================================================
  public enum VariableCategory
  {
    /// <summary>Static variable (S12.1.1)</summary>
    Static,
    /// <summary>Instance variable (S12.1.2)</summary>
    Instance,
    /// <summary>Array element (S12.1.3)</summary>
    ArrayElement,
    /// <summary>Value parameter (S12.1.4)</summary>
    ValueParameter,
    /// <summary>Reference parameter (S12.1.5)</summary>
    ReferenceParameter,
    /// <summary>Output parameter (S12.1.6)</summary>
    OutputParameter,
    /// <summary>Local variable (S12.1.7)</summary>
    Local
  }
}
