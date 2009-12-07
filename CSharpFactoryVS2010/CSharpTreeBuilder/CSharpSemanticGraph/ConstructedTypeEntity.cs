using System;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This abstract class represents a type entity that is constructed from other types:
  /// array, pointer, nullable types, and constructed generic types.
  /// </summary>
  // ================================================================================================
  public abstract class ConstructedTypeEntity : TypeEntity
  {
    #region State

    /// <summary>Gets the underlying type that this type builds upon.</summary>
    public TypeEntity UnderlyingType { get; private set; }

    #endregion

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ConstructedTypeEntity"/> class.
    /// </summary>
    /// <param name="underlyingType">A type that this constructed type builds upon.</param>
    // ----------------------------------------------------------------------------------------------
    protected ConstructedTypeEntity(TypeEntity underlyingType)
      : base(null, underlyingType.Name)
    {
      if (underlyingType == null )
      {
        throw new ArgumentNullException("underlyingType");
      }

      UnderlyingType = underlyingType;
      Parent = underlyingType.Parent; 
    }

    // Constructed types are not generic-cloned so no copy constructor here.

    #region Visitor methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Accepts a visitor object, according to the Visitor pattern.
    /// </summary>
    /// <param name="visitor">A visitor object</param>
    // ----------------------------------------------------------------------------------------------
    public override void AcceptVisitor(SemanticGraphVisitor visitor)
    {
      visitor.Visit(this);
      base.AcceptVisitor(visitor);
    }

    #endregion
  }
}
