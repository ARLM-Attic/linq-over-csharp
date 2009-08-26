using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a method.
  /// </summary>
  // ================================================================================================
  public sealed class MethodEntity : FunctionMemberWithBodyEntity, IOverloadableMember
  {
    /// <summary>Backing field for Parameters property.</summary>
    private readonly List<ParameterEntity> _Parameters;

    /// <summary>Backing field for AllTypeParameters property to disallow direct adding or removing.</summary>
    private readonly List<TypeParameterEntity> _AllTypeParameters;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="MethodEntity"/> class.
    /// </summary>
    /// <param name="name">The name of the member.</param>
    /// <param name="isExplicitlyDefined">A value indicating whether the member is explicitly defined.</param>
    /// <param name="isAbstract">A value indicating whether the function member is abstract.</param>
    /// <param name="isPartial">A value indicating whether this method is partial.</param>
    // ----------------------------------------------------------------------------------------------
    public MethodEntity(string name, bool isExplicitlyDefined, bool isAbstract, bool isPartial)
      : base(name, isExplicitlyDefined, isAbstract)
    {
      _Parameters = new List<ParameterEntity>();
      _AllTypeParameters = new List<TypeParameterEntity>();
      IsPartial = isPartial;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this method was declared as partial.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsPartial { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an iterate-only collection of parameters.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public IEnumerable<ParameterEntity> Parameters
    {
      get { return _Parameters; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a parameter.
    /// </summary>
    /// <param name="parameterEntity">A parameter entity.</param>
    // ----------------------------------------------------------------------------------------------
    public void AddParameter(ParameterEntity parameterEntity)
    {
      if (parameterEntity != null)
      {
        parameterEntity.Parent = this;
        _Parameters.Add(parameterEntity);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the signature of the member.
    /// </summary>
    /// <remarks>
    /// The signature of a method consists of the name of the method, the number of type parameters 
    /// and the type and kind (value, reference, or output) of each of its formal parameters, 
    /// considered in the order left to right.
    /// The signature of a method does not include the return type.
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    public string Signature
    {
      get
      {
        var stringBuilder = new StringBuilder(DistinctiveName);
        
        stringBuilder.Append('(');

        bool isFirst = true;
        foreach (var parameter in Parameters)
        {
          if (isFirst)
          {
            isFirst = false;
          }
          else
          {
            stringBuilder.Append(", ");
          }

          switch (parameter.Mode)
          {
            case (ParameterMode.Reference):
              stringBuilder.Append("ref ");
              break;
            case (ParameterMode.Output):
              stringBuilder.Append("out ");
              break;
            default:
              break;
          }

          stringBuilder.Append(parameter.Type.RootNamespace.Name);
          stringBuilder.Append("::");
          stringBuilder.Append(parameter.Type.FullyQualifiedName);
        }

        stringBuilder.Append(')');

        return stringBuilder.ToString();
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a read-only list of all type parameters of this method (parent's + own).
    /// Empty list for non-generic methods.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ReadOnlyCollection<TypeParameterEntity> AllTypeParameters
    {
      get { return _AllTypeParameters.AsReadOnly(); }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a read-only collection of the own type parameters of this method (excluding parents' params). 
    /// Empty list for non-generic methods.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ReadOnlyCollection<TypeParameterEntity> OwnTypeParameters
    {
      get
      {
        return (Parent is GenericCapableTypeEntity
                  ? _AllTypeParameters.Skip((Parent as GenericCapableTypeEntity).AllTypeParameters.Count)
                  : _AllTypeParameters)
          .ToList().AsReadOnly();
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the distinctive name of the entity, which is unique for all entities in a declaration space.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public override string DistinctiveName
    {
      get
      {
        return OwnTypeParameters.Count == 0
                 ? Name
                 : string.Format("{0}`{1}", Name, OwnTypeParameters.Count);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this is a generic method. 
    /// </summary>
    /// <remarks>It's a generic method if it has any type parameters.</remarks>
    // ----------------------------------------------------------------------------------------------
    public bool IsGeneric
    {
      get { return AllTypeParameters.Count() > 0; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a type parameter to this method.
    /// </summary>
    /// <param name="typeParameterEntity">The type parameter entity.</param>
    // ----------------------------------------------------------------------------------------------
    public void AddTypeParameter(TypeParameterEntity typeParameterEntity)
    {
      _AllTypeParameters.Add(typeParameterEntity);

      // If the type parameter is inherited, then the Parent property is already set, and we leave it as is.
      if (typeParameterEntity.Parent == null)
      {
        typeParameterEntity.Parent = this;
      }

      // A type parameter defined in a nested type/method can hide a type parameter inherited from parents
      var nameTableEntry = DeclarationSpace[typeParameterEntity.Name];

      if (nameTableEntry != null
        && nameTableEntry.State == NameTableEntryState.Definite
        && nameTableEntry.Entity is TypeParameterEntity
        && ((TypeParameterEntity)nameTableEntry.Entity).Parent != this)
      {
        DeclarationSpace.Redefine(typeParameterEntity);
      }
      else
      {
        DeclarationSpace.Define(typeParameterEntity);
      }
    }
  }
}
