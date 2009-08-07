using System;
using CSharpTreeBuilder.Ast;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a region in a compilation unit.
  /// </summary>
  // ================================================================================================
  public class SourceRegion
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="SourceRegion"/> class.
    /// </summary>
    /// <param name="fromSourcePoint">The starting point of the region.</param>
    /// <param name="toSourcePoint">The ending point of the region.</param>
    // ----------------------------------------------------------------------------------------------
    public SourceRegion(SourcePoint fromSourcePoint, SourcePoint toSourcePoint)
    {
      if (fromSourcePoint.CompilationUnit != toSourcePoint.CompilationUnit)
      {
        throw new ArgumentException(
          "The starting point and the ending point of a region must belong to the same compilation unit.");
      }

      FromSourcePoint = fromSourcePoint;
      ToSourcePoint = toSourcePoint;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the starting point of the region.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public SourcePoint FromSourcePoint { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the ending point of the region.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public SourcePoint ToSourcePoint { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the compilation unit where this source region belongs to.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public CompilationUnitNode CompilationUnit
    {
      get { return FromSourcePoint.CompilationUnit; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Returns a value indicating whether a source point is inside this source region.
    /// </summary>
    /// <param name="sourcePoint">A source point.</param>
    /// <returns>True if the source point is inside this source region, false otherwise.</returns>
    // ----------------------------------------------------------------------------------------------
    public bool Contains(SourcePoint sourcePoint)
    {
      return sourcePoint.CompilationUnit == this.CompilationUnit
             && sourcePoint.Position > 0  // because a missing position is represented with -1
             && FromSourcePoint.Position <= sourcePoint.Position
             && sourcePoint.Position <= ToSourcePoint.Position;
    }
  }
}
