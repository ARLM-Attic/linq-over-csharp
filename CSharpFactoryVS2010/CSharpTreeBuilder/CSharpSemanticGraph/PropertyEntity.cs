using System.Collections.Generic;
using CSharpTreeBuilder.CSharpSemanticGraphBuilder;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a property member.
  /// </summary>
  // ================================================================================================
  public sealed class PropertyEntity : FunctionMemberWithAccessorsEntity
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="PropertyEntity"/> class.
    /// </summary>
    /// <param name="name">The name of the member.</param>
    /// <param name="isExplicitlyDefined">True, if the member is explicitly defined, false otherwise.</param>
    /// <param name="typeReference">A reference to the type of the member.</param>
    // ----------------------------------------------------------------------------------------------
    public PropertyEntity(string name, bool isExplicitlyDefined, SemanticEntityReference<TypeEntity> typeReference)
      : base(name, isExplicitlyDefined, typeReference)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the get accessor.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public AccessorEntity GetAccessor { get; set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the set accessor.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public AccessorEntity SetAccessor { get; set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the body of the function member.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public override IEnumerable<AccessorEntity> Accessors
    {
      get
      {
        var tempList = new List<AccessorEntity>();

        if (GetAccessor != null)
        {
          tempList.Add(GetAccessor);
        }

        if (SetAccessor != null)
        {
          tempList.Add(SetAccessor);
        }
        
        return tempList;
      }
    }
  }
}
