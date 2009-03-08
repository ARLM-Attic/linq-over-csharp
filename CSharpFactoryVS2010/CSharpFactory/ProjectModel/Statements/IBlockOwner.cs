using System.Collections.Generic;

namespace CSharpFactory.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This interface marks all the language elements that can have statement blocks
  /// within themselves.
  /// </summary>
  // ==================================================================================
  public interface IBlockOwner
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the element owning the block;
    /// </summary>
    // --------------------------------------------------------------------------------
    IBlockOwner Owner { get; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the parent block of this block;
    /// </summary>
    // --------------------------------------------------------------------------------
    IBlockOwner ParentBlock { get; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the statement block belonging to the language element.
    /// </summary>
    // --------------------------------------------------------------------------------
    StatementCollection Statements { get; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Adds a new statement to the statement block.
    /// </summary>
    /// <param name="statement">Statement to add.</param>
    // --------------------------------------------------------------------------------
    void Add(Statement statement);

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the list ob child block in this one.
    /// </summary>
    // --------------------------------------------------------------------------------
    List<IBlockOwner> ChildBlocks { get; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the collection of variables belonging to this block.
    /// </summary>
    // --------------------------------------------------------------------------------
    VariableCollection Variables { get; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Adds a new localVariable the block.
    /// </summary>
    /// <param name="localVariable">Variable to add to the block.</param>
    // --------------------------------------------------------------------------------
    void Add(LocalVariable localVariable);
  }
}
