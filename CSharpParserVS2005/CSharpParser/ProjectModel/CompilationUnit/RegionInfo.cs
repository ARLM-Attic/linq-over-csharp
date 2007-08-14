using System;
using System.Collections.Generic;
using System.Text;
using CSharpParser.Collections;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This class represent a collapsable region within the source file.
  /// </summary>
  // ==================================================================================
  public sealed class RegionInfo
  {
    #region Private fields

    private int _StartLine;
    private int _StartColumn;
    private int _EndLine;
    private int _EndColumn;
    private string _StartText;
    private string _EndText;

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the line where #region directive starts.
    /// </summary>
    // --------------------------------------------------------------------------------
    public int StartLine
    {
      get { return _StartLine; }
      set { _StartLine = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the column where #region directive starts.
    /// </summary>
    // --------------------------------------------------------------------------------
    public int StartColumn
    {
      get { return _StartColumn; }
      set { _StartColumn = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the line where #endregion directive starts.
    /// </summary>
    // --------------------------------------------------------------------------------
    public int EndLine
    {
      get { return _EndLine; }
      set { _EndLine = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the column where #endregion directive starts.
    /// </summary>
    // --------------------------------------------------------------------------------
    public int EndColumn
    {
      get { return _EndColumn; }
      set { _EndColumn = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the text of #region directive.
    /// </summary>
    // --------------------------------------------------------------------------------
    public string StartText
    {
      get { return _StartText; }
      set { _StartText = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the text of #endregion directive.
    /// </summary>
    // --------------------------------------------------------------------------------
    public string EndText
    {
      get { return _EndText; }
      set { _EndText = value; }
    }

    #endregion
  }


  // ==================================================================================
  /// <summary>
  /// This class represents a collection of region information.
  /// </summary>
  // ==================================================================================
  public class RegionInfoCollection : RestrictedCollection<RegionInfo>
  {
  }
}
