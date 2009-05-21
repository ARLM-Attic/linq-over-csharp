// ================================================================================================
// SyntaxTree.cs
//
// Created: 2009.03.13, by Istvan Novak (DeepDiver)
// ================================================================================================
namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This class represents the syntax tree as a result of the syntax parsing of a C# compilation 
  /// unit.
  /// </summary>
  // ================================================================================================
  public class SyntaxTree: ISyntaxTree
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="SyntaxTree"/> class.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public SyntaxTree()
    {
      SourceFileNodes = new SourceFileNodeCollection();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the source file nodes belonging to the syntax tree
    /// </summary>
    /// <value>The source file nodes.</value>
    // ----------------------------------------------------------------------------------------------
    public SourceFileNodeCollection SourceFileNodes { get; private set; }
  }
}