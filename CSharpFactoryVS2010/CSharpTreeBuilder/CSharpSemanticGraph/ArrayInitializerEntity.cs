using System.Collections.Generic;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents an array initialier which is a list of expressions.
  /// </summary>
  // ================================================================================================
  public sealed class ArrayInitializerEntity : VariableInitializer
  {
    /// <summary>Backing field for VariableInitializers property.</summary>
    private readonly List<VariableInitializer> _VariableInitializers;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ArrayInitializerEntity"/> class.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ArrayInitializerEntity()
    {
      _VariableInitializers = new List<VariableInitializer>();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the collection of variable initializers.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public IEnumerable<VariableInitializer> VariableInitializers
    {
      get { return _VariableInitializers; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a variable initializer to the VariableInitializers collection.
    /// </summary>
    /// <param name="initializer">A variable initializer.</param>
    // ----------------------------------------------------------------------------------------------
    public void AddVariableInitializer(VariableInitializer initializer)
    {
      if (initializer != null)
      {
        _VariableInitializers.Add(initializer);
        initializer.Parent = this;
      }
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
      if (!visitor.Visit(this)) { return; }
    }

    #endregion
  }
}
