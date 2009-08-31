using System;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This exception is raised when a type and its member have the same name.
  /// </summary>
  // ================================================================================================
  public sealed class TypeNameMemberNameConflictException : Exception
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeNameMemberNameConflictException"/> class.
    /// </summary>
    /// <param name="name">The conflicting name.</param>
    // ----------------------------------------------------------------------------------------------
    public TypeNameMemberNameConflictException(string name)
    {
      Name = name;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the conflicting name.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string Name { get; private set; }
  }
}
