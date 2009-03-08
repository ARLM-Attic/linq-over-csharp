using System;

namespace CSharpFactory.ProjectModel
{
  #region ParseEventArgs

  // ==================================================================================
  /// <summary>
  /// This class defines an event argument class to pass information to event handlers
  /// related to parse events.
  /// </summary>
  // ==================================================================================
  public class ParseEventArgs : EventArgs
  {
    private readonly CompilationUnit _Unit;

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new instance of the event argument class for the specified 
    /// compilation unit.
    /// </summary>
    /// <param name="unit">Compilation unit</param>
    // --------------------------------------------------------------------------------
    public ParseEventArgs(CompilationUnit unit)
    {
      _Unit = unit;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the compilation unit related to this instance.
    /// </summary>
    // --------------------------------------------------------------------------------
    public CompilationUnit Unit
    {
      get { return _Unit; }
    }
  }

  #endregion

  #region ParseCancelEventArgs

  // ==================================================================================
  /// <summary>
  /// This class defines an event argument class to pass information to event handlers
  /// related to parse events. The event can be used to cancel the parsing process
  /// by setting the Cancel property to true.
  /// </summary>
  // ==================================================================================
  public class ParseCancelEventArgs : ParseEventArgs
  {
    private bool _Cancel;

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new instance of the event argument class for the specified 
    /// compilation unit.
    /// </summary>
    /// <param name="unit">Compilation unit</param>
    /// <remarks>
    /// The Cancel property is set to false by default.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public ParseCancelEventArgs(CompilationUnit unit)
      : base(unit)
    {
      _Cancel = false;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the property indicating if the further phases of parsing
    /// should be cancelled or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool Cancel
    {
      get { return _Cancel; }
      set { _Cancel = value; }
    }
  }

  #endregion

  #region ParseReferencedUnitEventArgs

  // ==================================================================================
  /// <summary>
  /// This class defines an event argument class to pass information to event handlers
  /// related to referenced compilation unit events.
  /// </summary>
  // ==================================================================================
  public class ParseReferencedUnitEventArgs : ParseCancelEventArgs
  {
    private readonly CompilationUnit _ReferencedUnit;

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new instance of the event argument class for the specified 
    /// referenced compilation unit.
    /// </summary>
    /// <param name="unit">Compilation unit</param>
    /// <param name="referencedUnit">Referenced compilation unit.</param>
    /// <remarks>
    /// The Cancel property is set to false by default.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public ParseReferencedUnitEventArgs(CompilationUnit unit, CompilationUnit referencedUnit) 
      : base(unit)
    {
      _ReferencedUnit = referencedUnit;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the referenced compilation unit.
    /// </summary>
    // --------------------------------------------------------------------------------
    public CompilationUnit ReferencedUnit
    {
      get { return _ReferencedUnit; }
    }
  }

  #endregion

  #region

  // ==================================================================================
  /// <summary>
  /// This class defines an event argument class to pass information to event handlers
  /// related to source file events.
  /// </summary>
  // ==================================================================================
  public class ParseFileEventArgs : ParseCancelEventArgs
  {
    private readonly SourceFile _File;

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new instance of the event argument class for the specified 
    /// source file.
    /// </summary>
    /// <param name="unit">Compilation unit</param>
    /// <param name="file">Source file information</param>
    /// <remarks>
    /// The Cancel property is set to false by default.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public ParseFileEventArgs(CompilationUnit unit, SourceFile file)
      : base(unit)
    {
      _File = file;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the source file information..
    /// </summary>
    // --------------------------------------------------------------------------------
    public SourceFile File
    {
      get { return _File; }
    }
  }

  #endregion
}