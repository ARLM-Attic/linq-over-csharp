// ================================================================================================
// SyntaxTreeOutputOptions.cs
//
// Created: 2009.03.21, by Istvan Novak (DeepDiver)
// ================================================================================================
using System;
using CSharpFactory.Collections;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This class represents the options used when outputting a syntax tree.
  /// </summary>
  // ================================================================================================
  public class SyntaxTreeOutputOptions: IReadOnlySupport
  {
    #region Private fields

    // --- Generic formatting fields
    private bool _UseOriginalPositions;
    private bool _UseTabs;
    private int _Indentation;

    // --- Bracing styles
    private BracingStyle _TypeAndNamespaceBraces;
    private BracingStyle _MethodBraces;
    private BracingStyle _AnonymousMethodBraces;
    private BracingStyle _CaseLabelBraces;
    private BracingStyle _InitializerBraces;
    private BracingStyle _OtherBraces;
    private ForceBracingStyle _ForceBracesInIfElse;
    private ForceBracingStyle _ForceBracesInFor;
    private ForceBracingStyle _ForceBracesInForEach;
    private ForceBracingStyle _ForceBracesInWhile;
    private ForceBracingStyle _ForceBracesInUsing;
    private ForceBracingStyle _ForceBracesInFixed;

    #endregion

    #region Lifecycle methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the default output formatting settings
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public static SyntaxTreeOutputOptions Default;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes the <see cref="SyntaxTreeOutputOptions"/> class.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    static SyntaxTreeOutputOptions()
    {
      Default =
        new SyntaxTreeOutputOptions
          {
            UseOriginalPositions = true,
            Indentation = 2
          };
      Default.MakeReadOnly();
    }

    #endregion

    #region IReadOnlySupport implementation

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Signs if the object is in read-only state or not.
    /// </summary>
    /// <value>
    /// 	<strong>True</strong>, if the object is immutable; otherwise,
    /// <strong>false</strong>.
    /// </value>
    /// <seealso cref="MakeReadOnly">MakeReadOnly Method</seealso>
    // ----------------------------------------------------------------------------------------------
    public bool IsReadOnly { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Sets the object's state to read-only.
    /// </summary>
    /// <example>
    /// Once this object has been set to read-only it cannotbe set to changeable
    /// again.
    /// </example>
    /// <seealso cref="IsReadOnly">IsReadOnly Property</seealso>
    // ----------------------------------------------------------------------------------------------
    public void MakeReadOnly()
    {
      IsReadOnly = true;
    }

    #endregion

    #region General identation properties

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets a value indicating whether original token positions should be used.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if original positions are to be used; otherwise, <c>false</c>.
    /// </value>
    // ----------------------------------------------------------------------------------------------
    public bool UseOriginalPositions
    {
      get { return _UseOriginalPositions; }
      set
      {
        CheckReadonly();
        _UseOriginalPositions = value;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets a value indicating whether tabs should be used.
    /// </summary>
    /// <value><c>true</c> if tabs should be used; otherwise, <c>false</c>.</value>
    // ----------------------------------------------------------------------------------------------
    public bool UseTabs
    {
      get { return _UseTabs; }
      set
      {
        CheckReadonly();
        _UseTabs = value;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the number of positions used for indentation.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public int Indentation
    {
      get { return _Indentation; }
      set
      {
        CheckReadonly();
        _Indentation = value;
      }
    }

    #endregion

    #region Braces options

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets how braces should be used for types and namespaces.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public BracingStyle TypeAndNamespaceBraces
    {
      get { return _TypeAndNamespaceBraces; }
      set
      {
        CheckReadonly();
        _TypeAndNamespaceBraces = value;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets how braces should be used for methods.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public BracingStyle MethodBraces
    {
      get { return _MethodBraces; }
      set
      {
        CheckReadonly();
        _MethodBraces = value;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets how braces should be used for anonymous methods.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public BracingStyle AnonymousMethodBraces
    {
      get { return _AnonymousMethodBraces; }
      set
      {
        CheckReadonly();
        _AnonymousMethodBraces = value;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets how braces should be used for switch case labels.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public BracingStyle CaseLabelBraces
    {
      get { return _CaseLabelBraces; }
      set
      {
        CheckReadonly();
        _CaseLabelBraces = value;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets how braces should be used for object and array initializers.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public BracingStyle InitializerBraces
    {
      get { return _InitializerBraces; }
      set
      {
        CheckReadonly();
        _InitializerBraces = value;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets how braces should be used for other language constructs.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public BracingStyle OtherBraces
    {
      get { return _OtherBraces; }
      set
      {
        CheckReadonly();
        _OtherBraces = value;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets how braces should be forced for "if...else" statements.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ForceBracingStyle ForceBracesInIfElse
    {
      get { return _ForceBracesInIfElse; }
      set
      {
        CheckReadonly();
        _ForceBracesInIfElse = value;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets how braces should be forced for "for" statements.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ForceBracingStyle ForceBracesInFor
    {
      get { return _ForceBracesInFor; }
      set
      {
        CheckReadonly();
        _ForceBracesInFor = value;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets how braces should be forced for "foreach" statements.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ForceBracingStyle ForceBracesInForEach
    {
      get { return _ForceBracesInForEach; }
      set
      {
        CheckReadonly();
        _ForceBracesInForEach = value;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets how braces should be forced for "while" statements.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ForceBracingStyle ForceBracesInWhile
    {
      get { return _ForceBracesInWhile; }
      set
      {
        CheckReadonly();
        _ForceBracesInWhile = value;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets how braces should be forced for "using" statements.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ForceBracingStyle ForceBracesInUsing
    {
      get { return _ForceBracesInUsing; }
      set
      {
        CheckReadonly();
        _ForceBracesInUsing = value;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets how braces should be forced for "fixed" statements.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ForceBracingStyle ForceBracesInFixed
    {
      get { return _ForceBracesInFixed; }
      set
      {
        CheckReadonly();
        _ForceBracesInFixed = value;
      }
    }

    #endregion

    #region Helper methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Checks if the readonly flag is set.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    private void CheckReadonly()
    {
      if (IsReadOnly) throw new InvalidOperationException();
    }

    #endregion
  }

  #region BracingStyle enumeration

  // ================================================================================================
  /// <summary>
  /// This enumeration defines the output styles used for bracing.
  /// </summary>
  // ================================================================================================
  public enum BracingStyle
  {
    /// <summary>
    /// At next line with the current indentation position.
    /// </summary>
    NexLineBSD,
    /// <summary>
    /// At next line indented with one position.
    /// </summary>
    NextLineGNU,
    /// <summary>
    /// At next line indented with one position, and child items are indented together with the brace.
    /// </summary>
    NextLineWhitesmiths,
    /// <summary>
    /// Directly at the end of the line with no space.
    /// </summary>
    EndOfLineNoSpace,
    /// <summary>
    /// Directly at the end of the line with one space.
    /// </summary>
    EndOfLineKAndR
  }

  #endregion 

  #region ForceBracingStyle

  // ================================================================================================
  /// <summary>
  /// This enumeration defines how braces should be forced.
  /// </summary>
  // ================================================================================================
  public enum ForceBracingStyle
  {
    /// <summary>
    /// Remove braces if possible
    /// </summary>
    Remove,
    /// <summary>
    /// Keep braces as those are used in the original source
    /// </summary>
    DoNotChange,
    /// <summary>
    /// Add braces even if those are not used in the original source
    /// </summary>
    Add,
    /// <summary>
    /// Use braces only if single-statement block spread across multiple lines
    /// </summary>
    UseForMultiline
  }

  #endregion
}