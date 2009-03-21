// ================================================================================================
// SyntaxTreeTextWriter.cs
//
// Created: 2009.03.21, by Istvan Novak (DeepDiver)
// ================================================================================================
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

    public string WorkingFolder { get; set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Writes out the whole syntax tree.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public override void WriteTree()
    {
      var expectedPrefix = ProjectProvider.WorkingFolder.Trim();
      if (!expectedPrefix.EndsWith("\\")) expectedPrefix += "\\";
      expectedPrefix = expectedPrefix.ToLower();
      var outputFolder = Path.IsPathRooted(WorkingFolder)
                           ? WorkingFolder
                           : Path.Combine(ProjectProvider.WorkingFolder, WorkingFolder);
      foreach (var sourceFile in SyntaxTree.SourceFileNodes)
      {
        var outputFile = sourceFile.FullName + ".out.cs";
        if (sourceFile.FullName.ToLower().StartsWith(expectedPrefix))
        {
          outputFile = Path.Combine(outputFolder, 
            sourceFile.FullName.Substring(expectedPrefix.Length));
        }
        var path = Path.GetDirectoryName(outputFile);
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        TextWriter tw = File.CreateText(outputFile);
        tw.WriteLine("// --- " + sourceFile.Name);
        tw.Close();
      }
    }
  }
}