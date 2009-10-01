using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a parameter filter, ie a type + kind pair.
  /// </summary>
  // ================================================================================================
  public sealed class ParameterFilter
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ParameterFilter"/> class.
    /// </summary>
    /// <param name="type">The type of the parameter</param>
    /// <param name="kind">The kind of the parameter</param>
    // ----------------------------------------------------------------------------------------------
    public ParameterFilter(TypeEntity type, ParameterKind kind)
    {
      Type = type;
      Kind = kind;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type of the parameter.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeEntity Type { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the kind of the parameter.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ParameterKind Kind { get; private set; }
  }
}
