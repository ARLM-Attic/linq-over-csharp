// ================================================================================================
// ReferencedUnit.cs
//
// Created: 2009.05.21, by Istvan Novak (DeepDiver)
// ================================================================================================
namespace CSharpTreeBuilder.ProjectContent
{
  // ================================================================================================
  /// <summary>
  /// This type represents an abstract compilation reference.
  /// </summary>
  /// <remarks>
  /// Compilation reference is a referenced assembly, a referenced project, or anything
  /// that holds information about types and namespaces external to the compilation
  /// unit.
  /// </remarks>
  // ================================================================================================
  public abstract class ReferencedUnit
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new compilation reference with the specified name.
    /// </summary>
    /// <param name="name">Name of the compilation reference.</param>
    // ----------------------------------------------------------------------------------------------
    protected ReferencedUnit(string name)
    {
      Name = name;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the name of the compilation reference.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string Name { get; private set; }
  }
}