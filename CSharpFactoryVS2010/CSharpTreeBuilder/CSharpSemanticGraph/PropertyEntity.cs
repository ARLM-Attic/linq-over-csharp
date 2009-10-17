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
    #region State

    /// <summary>Backing field for GetAccessor property.</summary>
    private AccessorEntity _GetAccessor;

    /// <summary>Backing field for SetAccessor property.</summary>
    private AccessorEntity _SetAccessor;

    /// <summary>Backing field for SetAccessor property.</summary>
    private FieldEntity _AutoImplementedField;


    /// <summary>Gets a value indictaing whether this property is auto-implemented.</summary>
    public bool IsAutoImplemented { get; private set; }

    #endregion

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="PropertyEntity"/> class.
    /// </summary>
    /// <param name="isDeclaredInSource">True if the member is explicitly declared in source code, false otherwise.</param>
    /// <param name="accessibility">The declared accessibility of the member. Can be null.</param>
    /// <param name="isStatic">A value indicating whether this property is static.</param>
    /// <param name="typeReference">A reference to the type of the member.</param>
    /// <param name="interfaceReference">
    /// A reference to in interface, if the member is explicitly implemented interface member.
    /// Null otherwise.
    /// </param>
    /// <param name="name">The name of the member.</param>
    /// <param name="isAutoImplemented">A value indicating whether this property is auto-implemented.</param>
    // ----------------------------------------------------------------------------------------------
    public PropertyEntity(
      bool isDeclaredInSource,
      AccessibilityKind? accessibility,
      bool isStatic,
      SemanticEntityReference<TypeEntity> typeReference,
      SemanticEntityReference<TypeEntity> interfaceReference,
      string name, 
      bool isAutoImplemented)
      : 
      base(isDeclaredInSource, accessibility, typeReference, interfaceReference, name)
    {
      _IsStatic = isStatic;
      IsAutoImplemented = isAutoImplemented;
      
      // If the property is auto-implemented then create the backing field.
      if (isAutoImplemented)
      {
        // csc.exe uses '<{PropertyName}>k_BackingField' but we don't follow it, 
        // because the uniqueness of the field name gets tricky with explicitly implemented interface members.
        // We just use a good old GUID, because it's unique, and its name has no significance at all.
        var fieldName = System.Guid.NewGuid().ToString();
        AutoImplementedField = new FieldEntity(false, AccessibilityKind.Private, isStatic, typeReference, fieldName, null);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="PropertyEntity"/> class 
    /// by deep copying from another instance.
    /// </summary>
    /// <param name="source">The object whose state will be copied to the new object.</param>
    // ----------------------------------------------------------------------------------------------
    public PropertyEntity(PropertyEntity source)
      : base(source)
    {
      if (source.GetAccessor != null)
      {
        GetAccessor = (AccessorEntity)source.GetAccessor.Clone();
      }
      
      if (source.SetAccessor != null)
      {
        SetAccessor = (AccessorEntity)source.SetAccessor.Clone();
      }

      if (AutoImplementedField != null)
      {
        AutoImplementedField = (FieldEntity)source.AutoImplementedField.Clone();
      }

      IsAutoImplemented = source.IsAutoImplemented;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a deep copy of the semantic subtree starting at this entity.
    /// </summary>
    /// <returns>The deep clone of this entity and its semantic subtree.</returns>
    // ----------------------------------------------------------------------------------------------
    public override object Clone()
    {
      return new PropertyEntity(this);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the auto-implemented field. Returns null if the property is not auto-implemented.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public FieldEntity AutoImplementedField
    {
      get 
      { 
        return _AutoImplementedField; 
      }

      private set
      {
        _AutoImplementedField = value;

        if (_AutoImplementedField != null)
        {
          _AutoImplementedField.Parent = this;
        }
      }
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
    /// Gets a value indicating whether this member is invocable.
    /// </summary>
    /// <remarks>A member is invocable if it's a method or event, 
    /// or if it is a constant, field or property of a delegate type.</remarks>
    // ----------------------------------------------------------------------------------------------
    public override bool IsInvocable
    {
      get
      {
        return (Type != null) && Type.IsDelegateType;
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
