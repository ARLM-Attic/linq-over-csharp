using CSharpTreeBuilder.Ast;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a point in a compilation unit.
  /// </summary>
  // ================================================================================================
  public class SourcePoint
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="SourcePoint"/> class.
    /// </summary>
    /// <param name="compilationUnit">A compilation unit.</param>
    /// <param name="position">A character position in the compilation unit.</param>
    // ----------------------------------------------------------------------------------------------
    public SourcePoint(CompilationUnitNode compilationUnit, int position)
    {
      CompilationUnit = compilationUnit;
      Position = position;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// A compilation unit.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public CompilationUnitNode CompilationUnit { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// A character position in a compilation unit.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public int Position { get; private set; }
  }
}
