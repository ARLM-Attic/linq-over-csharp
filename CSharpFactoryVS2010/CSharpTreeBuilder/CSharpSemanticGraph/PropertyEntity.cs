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
    /// <summary>Template for creating auto-implemented field names.</summary>
    private const string _AutoImplementedFieldNameTemplate = "<{0}>k_BackingField";

    /// <summary>Backing field for GetAccessor property.</summary>
    private AccessorEntity _GetAccessor;

    /// <summary>Backing field for SetAccessor property.</summary>
    private AccessorEntity _SetAccessor;

    /// <summary>Backing field for SetAccessor property.</summary>
    private readonly FieldEntity _AutoImplementedField;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="PropertyEntity"/> class.
    /// </summary>
    /// <param name="name">The name of the member.</param>
    /// <param name="isExplicitlyDefined">True, if the member is explicitly defined, false otherwise.</param>
    /// <param name="typeReference">A reference to the type of the member.</param>
    /// <param name="isStatic">A value indicating whether this property is static.</param>
    /// <param name="isAutoImplemented">A value indicating whether this property is auto-implemented.</param>
    // ----------------------------------------------------------------------------------------------
    public PropertyEntity(
      string name, bool isExplicitlyDefined, SemanticEntityReference<TypeEntity> typeReference, bool isStatic, bool isAutoImplemented)
      : base(name, isExplicitlyDefined, typeReference)
    {
      IsStatic = isStatic;
      IsAutoImplemented = isAutoImplemented;
      
      // If the property is auto-implemented then create the backing field.
      if (isAutoImplemented)
      {
        _AutoImplementedField = new FieldEntity(string.Format(_AutoImplementedFieldNameTemplate, name),
                                                false, typeReference, isStatic);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the property is static.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsStatic { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indictaing whether this property is auto-implemented.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsAutoImplemented { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the auto-implemented field. Null if the property is not auto-implemented.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public FieldEntity AutoImplementedField
    {
      get { return _AutoImplementedField; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the get accessor.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public AccessorEntity GetAccessor
    {
      get { return _GetAccessor; }
      set
      {
        if (value != null)
        {
          value.Parent = this;
        }
        _GetAccessor = value;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the set accessor.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public AccessorEntity SetAccessor
    {
      get { return _SetAccessor; }
      set
      {
        if (value != null)
        {
          value.Parent = this;
        }
        _SetAccessor = value;
      }
    }

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

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the parent entity.
    /// </summary>
    /// <remarks>Also set the parent of the backing field.</remarks>
    // ----------------------------------------------------------------------------------------------
    public override SemanticEntity Parent
    {
      set
      {
        base.Parent = value;

        // If the property is auto-implemented, then the backing field has to be added to the parent type.
        if (IsAutoImplemented && value is TypeEntity)
        {
          var parentType = value as TypeEntity;
          if (parentType.GetMember<FieldEntity>(_AutoImplementedField.Name) == null)
          {
            parentType.AddMember(_AutoImplementedField);
          }
        }
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
    }

    #endregion
  }
}
