namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a class entity in the semantic graph.
  /// </summary>
  // ================================================================================================
  public sealed class ClassEntity : ChildTypeCapableTypeEntity
  {
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
    /// Gets the base class entity of this class.
    /// </summary>
    /// <remarks>
    /// It returns an entity, not a reference, so it can be successful only if the base type references are already resolved.
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    public ClassEntity BaseClassEntity
    {
      get
      {
        ClassEntity baseClassEntity = null;

        foreach (var baseType in BaseTypes)
        {
          if (baseType.ResolutionState==ResolutionState.Resolved && baseType.TargetEntity is ClassEntity)
          {
            baseClassEntity = baseType.TargetEntity as ClassEntity;
            break;
          }
        }

        return baseClassEntity;
      }
    }
  }
}