using System;
using CSharpTreeBuilder.CSharpSemanticGraph;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This exception is raised when an entity cannot be registered in a declaration space 
  /// because it conflicts with an already registered entity.
  /// </summary>
  // ================================================================================================
  public sealed class DeclarationConflictException : Exception
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="DeclarationConflictException"/> class.
    /// </summary>
    /// <param name="alreadyDeclaredEntity">The already declared entity that caused the conflict.</param>
    /// <param name="newEntity">The entity that couldn't be registered because of the conflict.</param>
    // ----------------------------------------------------------------------------------------------
    public DeclarationConflictException(INamedEntity alreadyDeclaredEntity, INamedEntity newEntity)
    {
      AlreadyDeclaredEntity = alreadyDeclaredEntity;
      NewEntity = newEntity;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the already declared entity that caused the conflict.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public INamedEntity AlreadyDeclaredEntity { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the entity that couldn't be registered because of the conflict.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public INamedEntity NewEntity { get; private set; }
  }
}
