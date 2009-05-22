// ================================================================================================
// OutputSegment.cs
//
// Created: 2009.03.22, by Istvan Novak (DeepDiver)
// ================================================================================================
using System.Collections.Generic;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents an output segment that is used to translate it to output items in the
  /// context of a SyntaxTreeWriter.
  /// </summary>
  // ================================================================================================
  public class OutputSegment
  {
    private readonly List<object> _Elements;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="OutputSegment"/> class.
    /// </summary>
    /// <param name="elements">The elements.</param>
    // ----------------------------------------------------------------------------------------------
    public OutputSegment(params object[] elements)
    {
      _Elements = new List<object>(elements);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the output segment elements.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public IEnumerable<object> OutputSegmentElements
    {
      get { return _Elements; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Appends the specified element to the collection of output segment elements.
    /// </summary>
    /// <param name="element">The element to append.</param>
    // ----------------------------------------------------------------------------------------------
    public void Append(object element)
    {
      _Elements.Add(element);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Appends the specified elements to the collection of output segment elements.
    /// </summary>
    /// <param name="elements">The elements to append.</param>
    // ----------------------------------------------------------------------------------------------
    public void Append(params object[] elements)
    {
      _Elements.AddRange(elements);
    }
  }
}