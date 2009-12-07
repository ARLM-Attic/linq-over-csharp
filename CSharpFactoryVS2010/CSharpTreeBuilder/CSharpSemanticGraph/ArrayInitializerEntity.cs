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
    #region State

    /// <summary>Backing field for VariableInitializers property.</summary>
    private readonly List<VariableInitializer> _VariableInitializers = new List<VariableInitializer>();

    #endregion

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ArrayInitializerEntity"/> class.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ArrayInitializerEntity()
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ArrayInitializerEntity"/> class 
    /// by constructing it from a template instance.
    /// </summary>
    /// <param name="template">The template for the new instance.</param>
    /// <param name="typeParameterMap">The type parameter map of the new instance.</param>
    // ----------------------------------------------------------------------------------------------
    private ArrayInitializerEntity(ArrayInitializerEntity template, TypeParameterMap typeParameterMap)
      : base(template, typeParameterMap)
    {
      foreach(var variableInitializer in template._VariableInitializers)
      {
        _VariableInitializers.Add((VariableInitializer)variableInitializer.GetGenericClone(typeParameterMap));
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new constructed entity.
    /// </summary>
    /// <param name="typeParameterMap">A collection of type parameters and associated type arguments.</param>
    /// <returns>
    /// A new semantic entity constructed from this entity using the specified type parameter map.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    protected override SemanticEntity ConstructNew(TypeParameterMap typeParameterMap)
    {
      return new ArrayInitializerEntity(this, typeParameterMap);
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
      visitor.Visit(this);
      base.AcceptVisitor(visitor);

      foreach (var variableInitializer in _VariableInitializers)
      {
        variableInitializer.AcceptVisitor(visitor);
      }
    }

    #endregion
  }
}
