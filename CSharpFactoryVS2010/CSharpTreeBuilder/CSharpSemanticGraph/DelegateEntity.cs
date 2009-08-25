using CSharpTreeBuilder.CSharpSemanticGraphBuilder;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a delegate entity in the semantic graph.
  /// </summary>
  // ================================================================================================
  public sealed class DelegateEntity : GenericCapableTypeEntity
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="DelegateEntity"/> class.
    /// </summary>
    /// <param name="name">The name of the entity.</param>
    // ----------------------------------------------------------------------------------------------
    public DelegateEntity(string name)
      : base(name)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the reference to the return type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public SemanticEntityReference<TypeEntity> ReturnTypeReference { get; set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the return type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeEntity ReturnType 
    { 
      get
      {
        return ReturnTypeReference == null ? null : ReturnTypeReference.TargetEntity;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this type is a reference type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public override bool IsReferenceType
    {
      get { return true; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this type is a delegate type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public override bool IsDelegateType
    {
      get { return true; }
    }

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