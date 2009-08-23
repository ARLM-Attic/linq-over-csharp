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

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a dummy source point.
    /// </summary>
    /// <returns>A dummy source point.</returns>
    // ----------------------------------------------------------------------------------------------
    public static SourcePoint GetDummy()
    {
      return new SourcePoint(null, -1);
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

      var sourcePoint = obj as SourcePoint;
      if (sourcePoint == null)
      {
        return false;
      }

      return (sourcePoint.CompilationUnit == CompilationUnit) && (sourcePoint.Position == Position);
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
    /// <param name="sourcePoint">A sourcePoint to compare to.</param>
    /// <returns>True if obj and this object are equal. False otherwise.</returns>
    // ----------------------------------------------------------------------------------------------
    public bool Equals(SourcePoint sourcePoint)
    {
      if (sourcePoint == null)
      {
        return false;
      }

      return (sourcePoint.CompilationUnit == CompilationUnit) && (sourcePoint.Position == Position);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Overrides the equality operator for SourcePoints.
    /// </summary>
    /// <param name="a">A source point.</param>
    /// <param name="b">A source point.</param>
    /// <returns>True if the two source points equal.</returns>
    // ----------------------------------------------------------------------------------------------
    public static bool operator ==(SourcePoint a, SourcePoint b)
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
      return (a.CompilationUnit == b.CompilationUnit) && (a.Position == b.Position);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Overrides the non-equality operator for SourcePoints.
    /// </summary>
    /// <param name="a">A source point.</param>
    /// <param name="b">A source point.</param>
    /// <returns>True if the two source points don't equal.</returns>
    // ----------------------------------------------------------------------------------------------
    public static bool operator !=(SourcePoint a, SourcePoint b)
    {
      return !(a == b);
    }

  }
}
