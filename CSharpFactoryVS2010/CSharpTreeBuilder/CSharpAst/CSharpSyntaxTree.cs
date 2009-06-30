// ================================================================================================
// CSharpSyntaxTree.cs
//
// Created: 2009.03.13, by Istvan Novak (DeepDiver)
// ================================================================================================
namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents the syntax tree as a result of the syntax parsing of a C# compilation 
  /// unit.
  /// </summary>
  // ================================================================================================
  public class CSharpSyntaxTree : ICSharpSyntaxTree
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="CSharpSyntaxTree"/> class.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public CSharpSyntaxTree()
    {
      CompilationUnitNodes = new CompilationUnitNodeCollection();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the source file nodes belonging to the syntax tree
    /// </summary>
    /// <value>The source file nodes.</value>
    // ----------------------------------------------------------------------------------------------
    public CompilationUnitNodeCollection CompilationUnitNodes { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Resets the syntax tree so that it could be built from the beginning.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public void Reset()
    {
      CompilationUnitNodes.Clear();
    }
  }
}