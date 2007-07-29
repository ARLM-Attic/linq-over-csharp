using System.Collections.Generic;
using CSharpParser.ParserFiles;
using CSharpParser.Semantics;

namespace CSharpParser.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This type represents a switch section in a switch statement.
  /// </summary>
  // ==================================================================================
  public sealed class SwitchSection : BlockStatement
  {
    #region Private fields

    private List<Expression> _Labels = new List<Expression>();
    private bool _IsDefault;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new "fixed" statement declaration.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    // --------------------------------------------------------------------------------
    public SwitchSection(Token token)
      : base(token)
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

    #region Type resolution

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves all unresolved type references.
    /// </summary>
    /// <param name="contextType">Type of context where the resolution occurs.</param>
    /// <param name="contextInstance">Instance of the context.</param>
    // --------------------------------------------------------------------------------
    public override void ResolveTypeReferences(ResolutionContext contextType,
      IResolutionRequired contextInstance)
    {
      base.ResolveTypeReferences(contextType, contextInstance);
      foreach (Expression expr in _Labels)
      {
        expr.ResolveTypeReferences(contextType, contextInstance);
      }
    }

    #endregion

  }
}