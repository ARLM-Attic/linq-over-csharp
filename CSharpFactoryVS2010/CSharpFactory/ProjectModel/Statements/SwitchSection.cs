using System.Collections.Generic;
using CSharpFactory.ParserFiles;
using CSharpFactory.Semantics;

namespace CSharpFactory.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents a switch section in a switch statement.
  /// </summary>
  // ==================================================================================
  public sealed class SwitchSection : BlockStatement
  {
    #region Private fields

    private readonly List<Expression> _Labels = new List<Expression>();
    private bool _IsDefault;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new switch section declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser instance creating this element.</param>
    /// <param name="parent">Parent block of the statement.</param>
    // --------------------------------------------------------------------------------
    public SwitchSection(Token token, CSharpSyntaxParser parser, IBlockOwner parent)
      : base(token, parser, parent)
    {
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the flag indicating if this section has the "default" label or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsDefault
    {
      get { return _IsDefault; }
      set { _IsDefault = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the expressions indicating the label.
    /// </summary>
    // --------------------------------------------------------------------------------
    public List<Expression> Labels
    {
      get { return _Labels; }
    }

    #endregion

    #region Overridden methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Adds a new localVariable the block.
    /// </summary>
    /// <param name="localVariable">Variable to add to the block.</param>
    /// <remarks>
    /// The scope of variables in a switch section is the scope of the whole switch
    /// statement. So variables should be added to the block of the switch statement
    /// rather than to the section itself.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public override void Add(LocalVariable localVariable)
    {
      AddVariableToBlock(ParentBlock, localVariable);
    }

    #endregion

    #region Type resolution

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves all unresolved type references.
    /// </summary>
    /// <param name="contextType">Type of resolution context.</param>
    /// <param name="declarationScope">Current type declaration context.</param>
    /// <param name="parameterScope">Current type parameter declaration scope.</param>
    // --------------------------------------------------------------------------------
    public override void ResolveTypeReferences(ResolutionContext contextType, 
      ITypeDeclarationScope declarationScope, 
      ITypeParameterScope parameterScope)
    {
      base.ResolveTypeReferences(contextType, declarationScope, parameterScope);
      foreach (Expression expr in _Labels)
      {
        expr.ResolveTypeReferences(contextType, declarationScope, parameterScope);
      }
    }

    #endregion

  }
}