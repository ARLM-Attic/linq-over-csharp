// ================================================================================================
// SyntaxTreeTextWriter.cs
//
// Created: 2009.03.21, by Istvan Novak (DeepDiver)
// ================================================================================================
using System.Collections.Generic;
using System.IO;
using CSharpTreeBuilder.ProjectContent;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents a syntax tree writer that writes out the syntax tree into text files.
  /// </summary>
  // ================================================================================================
  public class SyntaxTreeTextWriter : SyntaxTreeWriter
  {
    #region Private fields

    private int _CurrentRow;
    private int _CurrentColumn;

    #endregion

    #region Lifecycle methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="SyntaxTreeTextWriter"/> class.
    /// </summary>
    /// <param name="tree">The syntax tree this writer uses.</param>
    /// <param name="provider">The project provider instance.</param>
    /// <param name="options">The options used for output.</param>
    // ----------------------------------------------------------------------------------------------
    public SyntaxTreeTextWriter(ISyntaxTree tree, ProjectProviderBase provider, SyntaxTreeOutputOptions options)
      : base(tree, provider, options)
    {
      WorkingFolder = @".\Output";
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="SyntaxTreeTextWriter"/> class.
    /// </summary>
    /// <param name="tree">The tree.</param>
    /// <param name="provider">The provider.</param>
    // ----------------------------------------------------------------------------------------------
    public SyntaxTreeTextWriter(ISyntaxTree tree, ProjectProviderBase provider)
      : this(tree, provider, null)
    {
    }

    #endregion

    #region Public members

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the working folder where the output files should be written.
    /// </summary>
    /// <value>The working folder.</value>
    // ----------------------------------------------------------------------------------------------
    public string WorkingFolder { get; set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Writes the list of items to the output.
    /// </summary>
    /// <param name="sourceFile">The source file.</param>
    /// <param name="items">The items to be written to the output.</param>
    // ----------------------------------------------------------------------------------------------
    protected override void WriteOutput(SourceFileNode sourceFile, OutputItemCollection items)
    {
      var expectedPrefix = ProjectProvider.WorkingFolder.Trim();
      if (!expectedPrefix.EndsWith("\\")) expectedPrefix += "\\";
      expectedPrefix = expectedPrefix.ToLower();
      var outputFolder = Path.IsPathRooted(WorkingFolder)
                           ? WorkingFolder
                           : Path.Combine(ProjectProvider.WorkingFolder, WorkingFolder);
      var outputFile = sourceFile.FullName + ".out.cs";
      if (sourceFile.FullName.ToLower().StartsWith(expectedPrefix))
      {
        outputFile = Path.Combine(outputFolder,
                                  sourceFile.FullName.Substring(expectedPrefix.Length));
      }
      var path = Path.GetDirectoryName(outputFile);
      if (!Directory.Exists(path)) Directory.CreateDirectory(path);
      TextWriter tw = File.CreateText(outputFile);
      WriteItems(items, tw);
      tw.Close();
    }

    #endregion

    #region Helper methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Writes the output items to the specified text writer.
    /// </summary>
    /// <param name="items">The output items to write out.</param>
    /// <param name="writer">The text writer to use.</param>
    // ----------------------------------------------------------------------------------------------
    private void WriteItems(IEnumerable<OutputItem> items, TextWriter writer)
    {
      _CurrentRow = 0;
      _CurrentColumn = 0;
      foreach (var item in items)
      {
        WriteLeadingLines(item, writer);
        WriteLeadingWhitespace(item, writer);
        writer.Write(item.GetText(OutputOptions));
        _CurrentColumn += item.GetLength(OutputOptions);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Writes the leading lines.
    /// </summary>
    /// <param name="item">The item to write the leading lines for.</param>
    /// <param name="writer">The text writer to use.</param>
    // ----------------------------------------------------------------------------------------------
    private void WriteLeadingLines(OutputItem item, TextWriter writer)
    {
      while (item.Row > _CurrentRow)
      {
        writer.WriteLine();
        _CurrentRow++;
        _CurrentColumn = 0;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Writes the leading white spaces.
    /// </summary>
    /// <param name="item">The item to write the leading lines for.</param>
    /// <param name="writer">The text writer to use.</param>
    // ----------------------------------------------------------------------------------------------
    private void WriteLeadingWhitespace(OutputItem item, TextWriter writer)
    {
      var whiteSpaceLength = item.Column - _CurrentColumn;
      if (whiteSpaceLength > 0)
      {
        writer.Write(string.Empty.PadRight(whiteSpaceLength, ' '));
        _CurrentColumn += whiteSpaceLength;
      }
    }

    #endregion
  }
}