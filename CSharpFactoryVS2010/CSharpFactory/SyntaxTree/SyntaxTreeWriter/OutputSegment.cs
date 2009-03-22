// ================================================================================================
// OutputSegment.cs
//
// Created: 2009.03.22, by Istvan Novak (DeepDiver)
// ================================================================================================
namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This class represents an output segment that is used to translate it to output items in the
  /// context of a SyntaxTreeWriter.
  /// </summary>
  // ================================================================================================
  public class OutputSegment
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="OutputSegment"/> class.
    /// </summary>
    /// <param name="elements">The elements.</param>
    // ----------------------------------------------------------------------------------------------
    public OutputSegment(params object[] elements)
    {
      OutputSegmentElements = elements;  
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the output segment elements.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public object[] OutputSegmentElements { get; private set; }   
  }
}