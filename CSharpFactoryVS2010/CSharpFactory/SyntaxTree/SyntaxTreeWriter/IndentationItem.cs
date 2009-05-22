// ================================================================================================
// IndentationItem.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This item represent indentation output.
  /// </summary>
  // ================================================================================================
  public sealed class IndentationItem : OutputItem
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="IndentationItem"/> class.
    /// </summary>
    /// <param name="row">The row position of the item.</param>
    /// <param name="column">The column position of the item.</param>
    /// <param name="depth">The depth od indentation.</param>
    // ----------------------------------------------------------------------------------------------
    public IndentationItem(int row, int column, int depth)
      : base(row, column)
    {
      Depth = depth;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the depth of indentation.
    /// </summary>
    /// <value>The depth.</value>
    // ----------------------------------------------------------------------------------------------
    public int Depth { get; private set; }

    #region Overrides of OutputItem

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the text of this output item in the context of the specified options.
    /// </summary>
    /// <returns>
    /// Text of this output item in the context of the specified options.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    public override string GetText(SyntaxTreeOutputOptions options)
    {
      return string.Empty.PadRight(Depth*options.IndentationWidth, ' ');
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the length of this output item in the context of the specified options.
    /// </summary>
    /// <param name="options">Output options.</param>
    /// <returns>
    /// Length of this output item in the context of the specified options.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    public override int GetLength(SyntaxTreeOutputOptions options)
    {
      return Depth * options.IndentationWidth;
    }

    #endregion
  }
}