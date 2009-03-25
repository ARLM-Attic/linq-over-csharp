// ================================================================================================
// OutputItemSerializer.cs
//
// Created: 2009.03.24, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.ParserFiles;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// The output item serializer is responsible for rendering OutputSegment instances into
  /// OutputItem instances.
  /// </summary>
  // ================================================================================================
  public sealed class OutputItemSerializer
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="OutputItemSerializer"/> class.
    /// </summary>
    /// <param name="options">The options.</param>
    public OutputItemSerializer(SyntaxTreeOutputOptions options)
    {
      OutputOptions = options;
      OutputItems = new OutputItemCollection();
      CurrentRow = 0;
      CurrentColumn = 0;
    }

    /// <summary>
    /// Gets or sets the output options.
    /// </summary>
    public SyntaxTreeOutputOptions OutputOptions { get; private set; }

    /// <summary>
    /// Gets or sets the output items.
    /// </summary>
    /// <value>The output items.</value>
    public OutputItemCollection OutputItems { get; private set; }

    /// <summary>
    /// Gets or sets the current row.
    /// </summary>
    /// <value>The current row.</value>
    public int CurrentRow { get; internal set; }
    
    /// <summary>
    /// Gets or sets the current column.
    /// </summary>
    /// <value>The current column.</value>
    public int CurrentColumn { get; internal set; }

    /// <summary>
    /// Appends the specified segment.
    /// </summary>
    /// <param name="segment">The segment.</param>
    public void Append(OutputSegment segment)
    {
      foreach (var element in segment.OutputSegmentElements)
      {
        var text = element as string;
        if (text != null)
        {
          Append(text);
          continue;
        }

        // --- Append a token
        var token = element as Token;
        if (token != null)
        {
          Append(token);
          continue;
        }

        // --- Append a mandatory whitespace
        var whiteSpace = element as WhiteSpaceSegment;
        if (whiteSpace != null)
        {
          Append(whiteSpace);
          continue;
        }

        // --- Append a nested output segment
        var outputSegment = element as OutputSegment;
        if (outputSegment != null)
        {
          Append(outputSegment);
          continue;
        }
      }
    }

    #region Helper function

    private void Append(string text)
    {
      OutputItems.Add(new TextOutputItem(CurrentRow, CurrentColumn, text));
      CurrentColumn += text.Length;
    }

    /// <summary>
    /// Appends the specified token.
    /// </summary>
    /// <param name="token">The token.</param>
    private void Append(Token token)
    {
      
    }

    /// <summary>
    /// Appends the specified white space.
    /// </summary>
    /// <param name="whiteSpace">The white space.</param>
    private void Append(WhiteSpaceSegment whiteSpace)
    {
      
    }

    #endregion

  }
}