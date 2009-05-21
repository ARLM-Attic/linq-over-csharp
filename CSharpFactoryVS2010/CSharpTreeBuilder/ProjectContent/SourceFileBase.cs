// ================================================================================================
// SourceFileBase.cs
//
// Created: 2009.05.21, by Istvan Novak (DeepDiver)
// ================================================================================================
using System.IO;

namespace CSharpTreeBuilder.ProjectContent
{
  // ================================================================================================
  /// <summary>
  /// This class represents an abstract source file where the content of the source files comes 
  /// either from a physical or a virtual stream.
  /// </summary>
  // ================================================================================================
  public abstract class SourceFileBase
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="SourceFileBase"/> class.
    /// </summary>
    /// <param name="filePath">Full file name</param>
    // ----------------------------------------------------------------------------------------------
    public SourceFileBase(string filePath)
    {
      FullName = filePath;
      Name = Path.GetFileName(filePath);
      Folder = Path.GetDirectoryName(filePath);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the name of the file
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string Name { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the folder of the file
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string Folder { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the full name of the file
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string FullName { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the stream representing the content of the source file.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public abstract Stream Stream { get; }
  }
}
