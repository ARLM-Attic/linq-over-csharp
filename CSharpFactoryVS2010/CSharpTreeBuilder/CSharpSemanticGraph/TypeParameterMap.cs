using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a set of type parameters, optionally mapped to type arguments.
  /// </summary>
  // ================================================================================================
  public sealed class TypeParameterMap
  {
    /// <summary>
    /// A dictionary holding all type parameters as keys and type arguments as values.
    /// Values can be null.
    /// </summary>
    private Dictionary<TypeParameterEntity, TypeEntity> _Map;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="SemanticEntity"/> class.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeParameterMap(): this(null, null)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="SemanticEntity"/> class,
    /// with the specified type parameters.
    /// </summary>
    /// <param name="typeParameters">A collection of type parameters to add to the map.</param>
    // ----------------------------------------------------------------------------------------------
    public TypeParameterMap(IEnumerable<TypeParameterEntity> typeParameters)
      : this(typeParameters, null)
    { 
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="SemanticEntity"/> class,
    /// with the specified type parameters and corresponding type arguments.
    /// </summary>
    /// <param name="typeParameters">A collection of type parameters to add to the map.</param>
    /// <param name="typeArguments">A collection of type arguments to add to the map.</param>
    // ----------------------------------------------------------------------------------------------
    public TypeParameterMap(IEnumerable<TypeParameterEntity> typeParameters, IEnumerable<TypeEntity> typeArguments)
    {
      _Map = new Dictionary<TypeParameterEntity, TypeEntity>();

      // Add the specified type parameters to the map, with null as value (no corresponding type argument).
      if (typeParameters != null)
      {
        foreach (var typeParameter in typeParameters)
        {
          AddTypeParameter(typeParameter);
        }
      }

      // Add the specified type arguments to the map
      if (typeArguments != null)
      {
        var typeParameterList = _Map.Keys.ToList();
        int i = 0;
        
        foreach (var typeArgument in typeArguments)
        {
          if (i < typeParameterList.Count)
          {
            this[typeParameterList[i]] = typeArgument;
          }
          else
          {
            throw new ApplicationException("Too many type arguments.");
          }

          i++;
        }
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds a type parameter to the map, with null as type argument.
    /// </summary>
    /// <param name="typeParameter">A type parameter entity.</param>
    // ----------------------------------------------------------------------------------------------
    public void AddTypeParameter(TypeParameterEntity typeParameter)
    {
      if (ContainsTypeParameter(typeParameter))
      {
        throw new ApplicationException(string.Format("Type parameter '{0}' already exists in the map.", typeParameter));
      }
      else
      {
        _Map.Add(typeParameter, null);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the collection of type parameters in the map.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public IEnumerable<TypeParameterEntity> TypeParameters
    {
      get
      {
        return _Map.Keys;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the collection of type arguments in the map. Can contain null values.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public IEnumerable<TypeEntity> TypeArguments
    {
      get
      {
        return _Map.Values;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the map contains the specified type parameter.
    /// </summary>
    /// <param name="typeParameter">A type parameter entity.</param>
    /// <returns>True if the map contains the specified type parameter.</returns>
    // ----------------------------------------------------------------------------------------------
    public bool ContainsTypeParameter(TypeParameterEntity typeParameter)
    {
      return _Map.ContainsKey(typeParameter);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the type argument corresponding to the specified type parameter.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeEntity this[TypeParameterEntity typeParameter]
    {
      get
      {
        return _Map[typeParameter];
      }

      set
      {
        if (ContainsTypeParameter(typeParameter))
        {
          _Map[typeParameter] = value;
        }
        else
        {
          throw new ApplicationException(string.Format("Type parameter '{0}' not found in the map.", typeParameter));
        }
      }
    }

  }
}
