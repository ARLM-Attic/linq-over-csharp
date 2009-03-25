// ================================================================================================
// SyntaxTreeTextWriter.cs
//
// Created: 2009.03.21, by Istvan Novak (DeepDiver)
// ================================================================================================
using System.Collections.Generic;
using System.IO;
using CSharpFactory.ProjectContent;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This class represents a syntax tree writer that writes out the syntax tree into text files.
  /// </summary>
  // ================================================================================================
  public class SyntaxTreeTextWriter : SyntaxTreeWriter
  {
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

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the working folder where the output files should be written.
    /// </summary>
    /// <value>The working folder.</value>
    // ----------------------------------------------------------------------------------------------
    public string WorkingFolder { get; set; }

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

    private int _CurrentRow;
    private int _CurrentColumn;

    private void WriteItems(IEnumerable<OutputItem> items, TextWriter writer)
    {
      _CurrentRow = 0;
      _CurrentColumn = 0;
      foreach (var item in items)
      {
        WriteLeadingLines(item, writer);
        WriteLeadingWhitespace(item, writer);
        if (item is IndentationItem)
        {
          var length = (item as IndentationItem).Depth*OutputOptions.Indentation;
          writer.Write(string.Empty.PadRight(length, ' '));
          _CurrentColumn += length;
        }
        else if (item is TextOutputItem)
        {
          var text = (item as TextOutputItem).Text;
          writer.Write(text);
          _CurrentColumn += text.Length;
        }
      }
    }

    private void WriteLeadingLines(OutputItem item, TextWriter writer)
    {
      while (item.Row > _CurrentRow)
      {
        writer.WriteLine();
        _CurrentRow++;
      }
    }

    private void WriteLeadingWhitespace(OutputItem item, TextWriter writer)
    {
      var whiteSpaceLength = item.Column - _CurrentColumn;
      if (whiteSpaceLength > 0)
      {
        writer.Write(string.Empty.PadRight(whiteSpaceLength, ' '));
        _CurrentColumn += whiteSpaceLength;
      }
    }
  }
}