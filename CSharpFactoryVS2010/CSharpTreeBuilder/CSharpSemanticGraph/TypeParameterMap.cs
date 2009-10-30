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
    private Dictionary<TypeParameterEntity, TypeEntity> _Map = new Dictionary<TypeParameterEntity, TypeEntity>();

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="SemanticEntity"/> class, with an empty map.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeParameterMap()
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="SemanticEntity"/> class
    /// by copying data from another map.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeParameterMap(TypeParameterMap source)
    {
      foreach (var keyValuePair in source._Map)
      {
        _Map.Add(keyValuePair.Key, keyValuePair.Value);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="SemanticEntity"/> class,
    /// from an existing map and a collection of type parameters.
    /// </summary>
    /// <param name="sourceMap">An existing map.</param>
    /// <param name="typeParameters">A collection of type parameters.</param>
    // ----------------------------------------------------------------------------------------------
    public TypeParameterMap(TypeParameterMap sourceMap, IEnumerable<TypeParameterEntity> typeParameters)
      : this(sourceMap)
    {
      // Add the specified type parameters to the map, with null value (ie. no corresponding type argument).
      if (typeParameters != null)
      {
        foreach (var typeParameter in typeParameters)
        {
          AddTypeParameter(typeParameter);
        }
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="SemanticEntity"/> class,
    /// from an existing map and a collection of type arguments that will replace null values in the map.
    /// </summary>
    /// <param name="sourceMap">An existing map.</param>
    /// <param name="typeArguments">A collection of type arguments.</param>
    // ----------------------------------------------------------------------------------------------
    public TypeParameterMap(TypeParameterMap sourceMap, IEnumerable<TypeEntity> typeArguments)
      : this(sourceMap)
    {
      if (typeArguments != null)
      {
        var typeParameterList = _Map.Keys.ToList();
        int i = 0;

        foreach (var typeArgument in typeArguments)
        {
          while (i < typeParameterList.Count && _Map[typeParameterList[i]] != null)
          {
            i++;
          }

          if (i == typeParameterList.Count)
          {
            throw new ApplicationException("Null slot not found for type argument.");
          }

          _Map[typeParameterList[i]] = typeArgument;

          i++;
        }
      }
    }
    
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an empty type parameter map (a new instance).
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public static TypeParameterMap Empty
    {
      get
      {
        return new TypeParameterMap();
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
    /// Adds a type parameter and a type argument to the map.
    /// </summary>
    /// <param name="typeParameter">A type parameter entity.</param>
    /// <param name="typeArgument">A type entity.</param>
    // ----------------------------------------------------------------------------------------------
    public void AddMapping(TypeParameterEntity typeParameter, TypeEntity typeArgument)
    {
      if (ContainsTypeParameter(typeParameter))
      {
        throw new ApplicationException(string.Format("Type parameter '{0}' already exists in the map.", typeParameter));
      }
      else
      {
        _Map.Add(typeParameter, typeArgument);
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

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the number of mappings in the map.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public int Count
    {
      get
      {
        return _Map.Count;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this map is empty (no type parameters).
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsEmpty
    {
      get
      {
        return _Map.Keys.Count == 0;
      }
    }
  }
}
