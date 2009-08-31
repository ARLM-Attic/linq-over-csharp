using System.Collections.Generic;
using System.Text;
using System;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This type represents the signature of an overloadable member entity
  /// (method, instance constructor, indexer or operator)
  /// </summary>
  /// <remarks>
  /// <para>
  /// The signature of a method consists of the name of the method, the number of type parameters 
  /// and the type and kind (value, reference, or output) of each of its formal parameters, 
  /// considered in the order left to right. 
  /// The signature of a method specifically does not include the return type, 
  /// the params modifier that may be specified for the right-most parameter, 
  /// nor the optional type parameter constraints.
  /// </para>
  /// <para>
  /// Although out and ref parameter modifiers are considered part of a signature, 
  /// members declared in a single type cannot differ in signature solely by ref and out. 
  /// A compile-time error occurs if two members are declared in the same type with signatures 
  /// that would be the same if all parameters in both methods with out modifiers were changed to ref modifiers. 
  /// For other purposes of signature matching (e.g., hiding or overriding), 
  /// ref and out are considered part of the signature and do not match each other.
  /// </para>
  /// </remarks>
  // ================================================================================================
  public class Signature
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="Signature"/> type.
    /// </summary>
    /// <param name="name">The name part of the signature.</param>
    /// <param name="typeParameterCount">The number of type parameters.</param>
    /// <param name="parameters">The parameter list.</param>
    // ----------------------------------------------------------------------------------------------
    public Signature(string name, int typeParameterCount, IEnumerable<ParameterEntity> parameters)
    {
      if (name == null)
      {
        throw new ArgumentNullException("name");
      }

      Name = name;
      TypeParameterCount = typeParameterCount;
      Parameters = parameters ?? new List<ParameterEntity>();
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the name part of the signature.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string Name { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the number of type parameters.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public int TypeParameterCount { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the parameter list.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public IEnumerable<ParameterEntity> Parameters { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the string representation of the object.
    /// </summary>
    /// <returns>The string representation of the object.</returns>
    // ----------------------------------------------------------------------------------------------
    public override string ToString()
    {
      if (string.IsNullOrEmpty(Name))
      {
        return null;
      }

      var stringBuilder = new StringBuilder(Name);

      if (TypeParameterCount > 0)
      {
        stringBuilder.Append('`');
        stringBuilder.Append(TypeParameterCount);
      }

      stringBuilder.Append('(');

      if (Parameters != null)
      {
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

          switch (parameter.Kind)
          {
            case (ParameterKind.Reference):
              stringBuilder.Append("ref ");
              break;
            case (ParameterKind.Output):
              stringBuilder.Append("out ");
              break;
            default:
              break;
          }

          if (parameter.Type != null)
          {
            stringBuilder.Append(parameter.Type.ToString());
          }
          else
          {
            stringBuilder.Append("?");
          }
        }
      }

      stringBuilder.Append(')');

      return stringBuilder.ToString();
    }
  }
}
