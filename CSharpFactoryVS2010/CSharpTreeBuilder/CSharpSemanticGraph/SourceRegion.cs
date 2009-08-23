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

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a dummy source region.
    /// </summary>
    /// <returns>A dummy source region.</returns>
    // ----------------------------------------------------------------------------------------------
    public static SourceRegion GetDummy()
    {
      return new SourceRegion(SourcePoint.GetDummy(),SourcePoint.GetDummy());
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Compares to objects for equality.
    /// </summary>
    /// <param name="obj">An object to compare to.</param>
    /// <returns>True if obj and this object are equal. False otherwise.</returns>
    // ----------------------------------------------------------------------------------------------
    public override bool Equals(object obj)
    {
      // The new implementation of Equals should follow all the guarantees of Equals:
      // x.Equals(x) returns true.
      // x.Equals(y) returns the same value as y.Equals(x).
      // if (x.Equals(y) && y.Equals(z)) returns true, then x.Equals(z) returns true.
      // Successive invocations of x.Equals(y) return the same value as long as the objects referenced by x and y are not modified.
      // x.Equals(null) returns false (for non-nullable value types only).
      // The new implementation of Equals should not throw exceptions. 
      // It is recommended that any class that overrides Equals also override Object.GetHashCode. 
      // It is also recommended that in addition to implementing Equals(object), 
      // any class also implement Equals(type) for their own type, to enhance performance.

      if (obj == null)
      {
        return false;
      }

      var sourceRegion = obj as SourceRegion;
      if (sourceRegion == null)
      {
        return false;
      }

      return (sourceRegion.FromSourcePoint == FromSourcePoint) && (sourceRegion.ToSourcePoint == ToSourcePoint);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Overrides Object.GetHashCode.
    /// </summary>
    /// <returns>The hash code of the object</returns>
    // ----------------------------------------------------------------------------------------------
    public override int GetHashCode()
    {
      return base.GetHashCode();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Compares to objects for equality.
    /// </summary>
    /// <param name="sourceRegion">A sourceRegion to compare to.</param>
    /// <returns>True if sourceRegion and this object are equal. False otherwise.</returns>
    // ----------------------------------------------------------------------------------------------
    public bool Equals(SourceRegion sourceRegion)
    {
      if (sourceRegion == null)
      {
        return false;
      }

      return (sourceRegion.FromSourcePoint == FromSourcePoint) && (sourceRegion.ToSourcePoint == ToSourcePoint);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Overrides the equality operator for SourceRegion.
    /// </summary>
    /// <param name="a">A source point.</param>
    /// <param name="b">A source point.</param>
    /// <returns>True if the two source regions equal.</returns>
    // ----------------------------------------------------------------------------------------------
    public static bool operator ==(SourceRegion a, SourceRegion b)
    {
      // If both are null, or both are same instance, return true.
      if (ReferenceEquals(a, b))
      {
        return true;
      }

      // If one is null, but not both, return false.
      if (((object)a == null) || ((object)b == null))
      {
        return false;
      }

      // Return true if the fields match:
      return (a.FromSourcePoint == b.FromSourcePoint) && (a.ToSourcePoint == b.ToSourcePoint);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Overrides the non-equality operator for SourceRegion.
    /// </summary>
    /// <param name="a">A source point.</param>
    /// <param name="b">A source point.</param>
    /// <returns>True if the two source regions don't equal.</returns>
    // ----------------------------------------------------------------------------------------------
    public static bool operator !=(SourceRegion a, SourceRegion b)
    {
      return !(a == b);
    }

  }
}
