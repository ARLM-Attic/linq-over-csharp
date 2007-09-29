namespace CSharpParser.CodeExplorer.TreeNodes
{
  // ====================================================================================
  /// <summary>
  /// This interface indicates that an object supports providing information about 
  /// itself to display in a PropertyGrid.
  /// </summary>
  // ====================================================================================
  internal interface IPropertyPanelSupport
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the object to display in the PropertyGrid.
    /// </summary>
    // --------------------------------------------------------------------------------
    object GetSelectedObject();
  }
}
