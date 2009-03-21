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

    private bool _UseOriginalPositions;
    private bool _UseTabs;
    private int _Indentation;
    private BracingStyle _TypeAndNamespaceBraces;
    private BracingStyle _MethodBraces;
    private BracingStyle _AnonymousMethodBraces;
    private BracingStyle _CaseLabelBraces;
    private BracingStyle _InitializerBraces;
    private BracingStyle _OtherBraces;

    #endregion

    #region Lifecycle methods

    public static SyntaxTreeOutputOptions Default;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes the <see cref="SyntaxTreeOutputOptions"/> class.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    static SyntaxTreeOutputOptions()
    {
      Default = new SyntaxTreeOutputOptions();
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

    public bool UseOriginalPositions
    {
      get { return _UseOriginalPositions; }
      set
      {
        CheckReadonly();
        _UseOriginalPositions = value;
      }
    }

    public bool UseTabs
    {
      get { return _UseTabs; }
      set
      {
        CheckReadonly();
        _UseTabs = value;
      }
    }

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

    public BracingStyle TypeAndNamespaceBraces
    {
      get { return _TypeAndNamespaceBraces; }
      set
      {
        CheckReadonly();
        _TypeAndNamespaceBraces = value;
      }
    }

    public BracingStyle MethodBraces
    {
      get { return _MethodBraces; }
      set
      {
        CheckReadonly();
        _MethodBraces = value;
      }
    }

    public BracingStyle AnonymousMethodBraces
    {
      get { return _AnonymousMethodBraces; }
      set
      {
        CheckReadonly();
        _AnonymousMethodBraces = value;
      }
    }

    public BracingStyle CaseLabelBraces
    {
      get { return _CaseLabelBraces; }
      set
      {
        CheckReadonly();
        _CaseLabelBraces = value;
      }
    }

    public BracingStyle InitializerBraces
    {
      get { return _InitializerBraces; }
      set
      {
        CheckReadonly();
        _InitializerBraces = value;
      }
    }

    public BracingStyle OtherBraces
    {
      get { return _OtherBraces; }
      set
      {
        CheckReadonly();
        _OtherBraces = value;
      }
    }

    #endregion

    #region Helper methods

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
    /// 
    /// </summary>
    Remove,
    /// <summary>
    /// 
    /// </summary>
    DoNotChange,
    /// <summary>
    /// 
    /// </summary>
    Add,
    /// <summary>
    /// 
    /// </summary>
    UseForMultiline
  }

  #endregion
}