using System;
using System.Collections;
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
  public sealed class TypeParameterMap : IEnumerable<KeyValuePair<TypeParameterEntity, TypeEntity>>
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
    /// Initializes a new instance of the <see cref="SemanticEntity"/> class,
    /// with a collection of type parameters and null arguments.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeParameterMap(IEnumerable<TypeParameterEntity> typeParameters)
    {
      foreach (var typeParameter in typeParameters)
      {
        _Map.Add(typeParameter, null);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="SemanticEntity"/> class,
    /// with a collection of type parameters and the corresponding type arguments.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeParameterMap(IEnumerable<TypeParameterEntity> typeParameters, IEnumerable<TypeEntity> typeArguments)
    {
      if (typeParameters.Count() != typeArguments.Count())
      {
        throw new ArgumentException("Mismatch in type parameter and type argument count");
      }

      var typeArgumentList = typeArguments.ToList();
      int i = 0;
      foreach (var typeParameter in typeParameters)
      {
        _Map.Add(typeParameter, typeArgumentList[i]);
        i++;
      }
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
    /// Gets an enumerator for the underlying dictionary.
    /// </summary>
    /// <returns>An enumerator for the underlying dictionary.</returns>
    // ----------------------------------------------------------------------------------------------
    public IEnumerator<KeyValuePair<TypeParameterEntity, TypeEntity>> GetEnumerator()
    {
      return _Map.GetEnumerator();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets an enumerator for the underlying dictionary.
    /// </summary>
    /// <returns>An enumerator for the underlying dictionary.</returns>
    // ----------------------------------------------------------------------------------------------
    IEnumerator IEnumerable.GetEnumerator()
    {
      return _Map.GetEnumerator();
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

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Maps the type arguments in this map using another map, and returns a newly created map as result.
    /// using the other map.
    /// </summary>
    /// <param name="otherMap">A type parameter map.</param>
    /// <returns>A new type parameter map created by mapping the type arguments of this map 
    /// using the parameter map.</returns>
    // ----------------------------------------------------------------------------------------------
    public TypeParameterMap MapTypeArguments(TypeParameterMap otherMap)
    {
      var resultingTypeParameterMap = new TypeParameterMap();

      foreach (var keyValuePair in this._Map)
      {
        // Unbound type parameters are represented with null as arument, so they need special treatment
        var mappedTypeArgument = keyValuePair.Value == null
                                   ? keyValuePair.Key.GetMappedType(otherMap)
                                   : keyValuePair.Value.GetMappedType(otherMap);

        resultingTypeParameterMap.AddMapping(keyValuePair.Key, mappedTypeArgument);
      }

      return resultingTypeParameterMap;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Extends this map with another map and returns a new map.
    /// </summary>
    /// <param name="otherMap">A type parameter map.</param>
    /// <returns>A new type parameter map created by extending this map with the parameter map.</returns>
    // ----------------------------------------------------------------------------------------------
    public TypeParameterMap Extend(TypeParameterMap otherMap)
    {
      var resultingTypeParameterMap = new TypeParameterMap(this);

      foreach(var keyValuePair in otherMap._Map)
      {
        if (ContainsTypeParameter(keyValuePair.Key) && this[keyValuePair.Key] != keyValuePair.Value)
        {
          throw new ApplicationException(
            string.Format("Conflicting type argument at type parameter '{0}'. The arguments are: '{1}' and '{2}'",
                          keyValuePair.Key, this[keyValuePair.Key], keyValuePair.Value));
        }

        resultingTypeParameterMap.AddMapping(keyValuePair.Key, keyValuePair.Value);
      }

      return resultingTypeParameterMap;
    }
  }
}
