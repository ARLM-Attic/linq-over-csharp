// ================================================================================================
// TextOutputItem.cs
//
// Created: 2009.05.11, by Istvan Novak (DeepDiver)
// ================================================================================================
namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This item represent text output.
  /// </summary>
  // ================================================================================================
  public sealed class TextOutputItem : OutputItem
  {
    private readonly string _Text;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TextOutputItem"/> class.
    /// </summary>
    /// <param name="row">The row position of the item.</param>
    /// <param name="column">The column position of the item.</param>
    /// <param name="text">The text.</param>
    // ----------------------------------------------------------------------------------------------
    public TextOutputItem(int row, int column, string text)
      : base(row, column)
    {
      _Text = text;
    }

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
      return _Text;
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
      return _Text.Length;
    }

    #endregion
  }
}