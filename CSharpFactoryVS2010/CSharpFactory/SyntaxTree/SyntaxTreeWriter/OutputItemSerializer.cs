// ================================================================================================
// OutputItemSerializer.cs
//
// Created: 2009.03.24, by Istvan Novak (DeepDiver)
// ================================================================================================
using System.Collections;
using CSharpFactory.ParserFiles;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// The output item serializer is responsible for rendering OutputSegment instances into
  /// OutputItem instances.
  /// </summary>
  // ================================================================================================
  public sealed class OutputItemSerializer
  {
    #region Private fields

    private bool _MandatoryWhiteSpaceNeeded;
    private int _IndentationDepth;
    private int _LastRowPosition;

    #endregion

    #region Lifecycle methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="OutputItemSerializer"/> class.
    /// </summary>
    /// <param name="options">The options.</param>
    // ----------------------------------------------------------------------------------------------
    public OutputItemSerializer(SyntaxTreeOutputOptions options)
    {
      OutputOptions = options;
      OutputItems = new OutputItemCollection();
      CurrentRow = 0;
      CurrentColumn = 0;
    }

    #endregion

    #region Public properties

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the output options.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public SyntaxTreeOutputOptions OutputOptions { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the output items.
    /// </summary>
    /// <value>The output items.</value>
    // ----------------------------------------------------------------------------------------------
    public OutputItemCollection OutputItems { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the current row.
    /// </summary>
    /// <value>The current row.</value>
    // ----------------------------------------------------------------------------------------------
    public int CurrentRow { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the current column.
    /// </summary>
    /// <value>The current column.</value>
    // ----------------------------------------------------------------------------------------------
    public int CurrentColumn { get; internal set; }

    #endregion

    #region Public methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Appends the specified segment.
    /// </summary>
    /// <param name="segment">The segment.</param>
    // ----------------------------------------------------------------------------------------------
    public void Append(OutputSegment segment)
    {
      if (segment == null) return;
      foreach (var element in segment.OutputSegmentElements)
      {
        Append(element);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Appends the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    // ----------------------------------------------------------------------------------------------
    public void Append(object element)
    {
      // --- Append a simple string
      var text = element as string;
      if (text != null)
      {
        Append(text);
        return;
      }

      // --- Append a token
      var token = element as Token;
      if (token != null)
      {
        Append(token);
        return;
      }

      // --- Append a SyntaxNode
      var syntaxNode = element as SyntaxNode;
      if (syntaxNode != null)
      {
        Append(syntaxNode.GetOutputSegment());
        return;
      }

      // --- Append enumerables
      var enumerable = element as IEnumerable;
      if (enumerable != null)
      {
        foreach (var item in enumerable) Append(item);
      }

      // --- Handle control segments
      var controlSegment = element as ControlSegment;
      if (controlSegment != null)
      {
        controlSegment.Control(this);  
      }

      // --- Append a nested output segment
      var outputSegment = element as OutputSegment;
      if (outputSegment != null)
      {
        Append(outputSegment);
        return;
      }
    }

    #endregion

    #region Internal methods and properties

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Signs the mandatory white space.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    internal void SignMandatoryWhiteSpace()
    {
      _MandatoryWhiteSpaceNeeded = true;  
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Forces the new line.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    internal void ForceNewLine()
    {
      if (OutputOptions.UseOriginalPositions) return;
      CurrentRow++;
      _LastRowPosition++;
      CurrentColumn = 0;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Applies the current indentation
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    internal void ApplyIndentation()
    {
      if (!OutputOptions.UseOriginalPositions && CurrentColumn == 0)
      {
        CurrentColumn += OutputOptions.IndentationWidth*_IndentationDepth;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Increases the indentation depth.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    internal void IncrementIndentation()
    {
      _IndentationDepth++;
      if (_IndentationDepth > 100) _IndentationDepth = 100;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Decreases the indentation depth.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    internal void DecrementIndentation()
    {
      _IndentationDepth--;
      if (_IndentationDepth < 0) _IndentationDepth = 0;
    }

    #endregion

    #region Helper function

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Appends the specified output item.
    /// </summary>
    /// <param name="item">The item to append.</param>
    // ----------------------------------------------------------------------------------------------
    private void AppendOutputItem(OutputItem item)
    {
      // --- Insert a mandatory whitespaceif required
      if (_MandatoryWhiteSpaceNeeded && CurrentRow >= item.Row && CurrentColumn >= item.Column)
      {
        OutputItems.Add(new TextOutputItem(item.Row, item.Column, " "));
        CurrentColumn++;
      }
      OutputItems.Add(item);
      CurrentColumn += item.GetLength(OutputOptions);
      _LastRowPosition = CurrentRow;
      _MandatoryWhiteSpaceNeeded = false;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Appends the specified text.
    /// </summary>
    /// <param name="text">The text to append.</param>
    // ----------------------------------------------------------------------------------------------
    private void Append(string text)
    {
      AppendOutputItem(new TextOutputItem(CurrentRow, CurrentColumn, text));
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Appends the specified token.
    /// </summary>
    /// <param name="token">The token to append.</param>
    // ----------------------------------------------------------------------------------------------
    private void Append(Token token)
    {
      TextOutputItem item;
      if (OutputOptions.UseOriginalPositions && token.line >= 0 && token.col >= 0)
      {
        // --- Original token positions should be used 
        item = new TextOutputItem(token.line - 1, token.col - 1, token.val);
      }
      else
      {
        // --- Token should be positioned
        if (OutputOptions.KeepLineBreaks && token.line >= 0 && (token.line - 1) > _LastRowPosition)
        {
          // --- Line breaks muts be kept
          CurrentRow += (token.line - 1) - _LastRowPosition;
        }
        item = new TextOutputItem(CurrentRow, CurrentColumn, token.val);
      }
      AppendOutputItem(item);
    }

    #endregion
  }
}