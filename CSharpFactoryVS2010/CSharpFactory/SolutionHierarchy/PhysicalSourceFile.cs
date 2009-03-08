// ================================================================================================
// PhysicalSourceFile.cs
//
// Created: 2009.03.04, by Istvan Novak (DeepDiver)
// ================================================================================================
using System.IO;

namespace CSharpFactory.SolutionHierarchy
{
  // ================================================================================================
  /// <summary>
  /// This class represents a physical source file which can be read from the file system.
  /// </summary>
  // ================================================================================================
  public class PhysicalSourceFile : SourceFileBase
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="PhysicalSourceFile"/> class.
    /// </summary>
    /// <param name="filePath">Full file name</param>
    // ----------------------------------------------------------------------------------------------
    public PhysicalSourceFile(string filePath)
      : base(filePath)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the stream representing the content of the source file.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public override Stream Stream
    {
      get { return File.OpenText(Name).BaseStream; }
    }
  }
}