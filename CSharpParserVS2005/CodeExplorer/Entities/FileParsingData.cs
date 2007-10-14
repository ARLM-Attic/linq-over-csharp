namespace CSharpParser.CodeExplorer.Entities
{
  // ==================================================================================
  /// <summary>
  /// This class represents an entry about the parsing time statistics.
  /// </summary>
  // ==================================================================================
  public sealed class FileParsingData
  {
    private string _FileName;
    private double _TimeFromStart;
    private double _ParseTime;

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the name of the file.
    /// </summary>
    // --------------------------------------------------------------------------------
    public string FileName
    {
      get { return _FileName; }
      set { _FileName = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the time ellapsed from the start of parsing.
    /// </summary>
    // --------------------------------------------------------------------------------
    public double TimeFromStart
    {
      get { return _TimeFromStart; }
      set { _TimeFromStart = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the time required to parse this file syntactically
    /// </summary>
    // --------------------------------------------------------------------------------
    public double ParseTime
    {
      get { return _ParseTime; }
      set { _ParseTime = value; }
    }
  }
}