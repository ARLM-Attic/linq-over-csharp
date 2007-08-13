using System;

namespace CSharpParser.ParserFiles
{
  // ==================================================================================
  /// <summary>
  /// This enumeration enlists all the native modifiers that can be used to modify
  /// the visibility, invocation and inheritance properties of type and member
  /// declarations.
  /// </summary>
  /// <remarks>
  /// The enumeration also contains the possible combinations of these modifiers 
  /// according to their usage context.
  /// </remarks>
  // ==================================================================================
  [Flags]
  public enum Modifier
  {
    // --- No modifier specified
    none = 0x0000,

    // --- Simple modifiers
    @new = 0x0001, 
    @public = 0x0002, 
    @protected = 0x0004, 
    @internal = 0x0008,
    @private = 0x0010, 
    @unsafe = 0x0020, 
    @static = 0x0040, 
    @readonly = 0x0080,
    @volatile = 0x0100, 
    @virtual = 0x0200, 
    @sealed = 0x0400, 
    @override = 0x0800,
    @abstract = 0x1000, 
    @extern = 0x2000,

    // --- Visibility modifiers
    VisibilityAccessors = @public | @protected | @internal | @private,
    ProtectedInternal = @protected | @internal,

    // --- Modifiers applicable for classes
    classes = VisibilityAccessors | @new | @unsafe | @abstract | @sealed | @static,

    // --- Modifiers applicable for non-class types
    nonClassTypes = VisibilityAccessors | @new | @unsafe,

    // --- Modifiers applicable for constants
    constants = VisibilityAccessors | @new ,

    // --- Modifiers applicable for fields
    fields = VisibilityAccessors | @new | @unsafe | @static | @readonly | @volatile,

    // --- Modifiers applicable for property event methods
    propEvntMeths = VisibilityAccessors | @new | @unsafe | @static | @virtual | @sealed | @override | @abstract | @extern,

    // --- Modifiers applicable for property accessors: the two set cannot overlap each other
    accessorsPossib1 = @private,
    accessorsPossib2 = @protected | @internal,

    // --- Modifiers applicable for indexers
    indexers = VisibilityAccessors | @new | @unsafe | @virtual | @sealed | @override | @abstract | @extern,

    // --- Modifiers applicable for operator overloads
    operators = @public | @unsafe | @static | @extern,

    // --- Mandatory modifiers for operator overloads
    operatorsMust = @public | @static,

    // --- Modifiers applicable for constructors
    constructors = VisibilityAccessors | @unsafe | @extern,

    // --- Modifiers applicable for static constructors
    staticConstr = @extern | @static,

    // --- Mandatory modifiers for static constructors
    staticConstrMust = @static,

    // --- Modifiers applicable for destructors
    destructors = @extern | @unsafe,
    all = 0x3fff
  }

  // ==================================================================================
  /// <summary>
  /// This class is responsible for handling operations on the instances of the 
  /// Modifier enumeration.
  /// </summary>
  // ==================================================================================
  class Modifiers
  {
    #region Private fields

    private Modifier _Value = Modifier.none;
    private CSharpSyntaxParser _Parser;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates an instance of this class that can communicate with the specified
    /// Parser instance.
    /// </summary>
    /// <param name="parser">The Parser instance used for communication</param>
    /// <remarks>
    /// A Modifiers instance can send error messages to the Parser instance.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public Modifiers(CSharpSyntaxParser parser)
    {
      _Parser = parser;
    }

    #endregion

    #region Public Properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the value of this modifier.
    /// </summary>
    // --------------------------------------------------------------------------------
    public Modifier Value
    {
      get { return _Value; }
    }

    #endregion

    #region Public methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Adds a modifier to the existing set of modifiers.
    /// </summary>
    /// <param name="m">Modifier to add.</param>
    // --------------------------------------------------------------------------------
    public void Add(Modifier m)
    {
      if ((_Value & m) == 0) _Value |= m;
      else _Parser.Error("UNDEF", new Token(), "modifier " + m + " already defined");
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Adds a set of modifiers to the existing set of modifiers.
    /// </summary>
    /// <param name="m">Modifiers to add.</param>
    // --------------------------------------------------------------------------------
    public void Add(Modifiers m)
    {
      Add(m._Value);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if no modifiers has been set.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsNone
    {
      get { return _Value == Modifier.none; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks, if the current set of modifiers has only the allowed modifiers and
    /// no others.
    /// </summary>
    /// <param name="allowed">The set of allowed modifiers.</param>
    // --------------------------------------------------------------------------------
    public void Check(Modifier allowed)
    {
      Modifier wrong = _Value & (allowed ^ Modifier.all);
      if (wrong != Modifier.none)
        _Parser.Error("UNDEF", new Token(), "modifier(s) " + wrong + " not allowed here");
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks, if the current set of modifiers has only the allowed modifiers and
    /// no others. There are two set of allowed modifiers, the current set can contain
    /// either the first set or the second one but cannot contain modifiers from both
    /// sets.
    /// </summary>
    /// <param name="allowEither">First set of modifiers</param>
    /// <param name="allowOr">Second set of modifiers.</param>
    /// <remarks>The two set of allowed modifiers cannot overlap each other.</remarks>
    // --------------------------------------------------------------------------------
    public void Check(Modifier allowEither, Modifier allowOr)
    {
      Modifier wrong = _Value & ((allowEither | allowOr) ^ Modifier.all);
      if ((allowEither & allowOr) != Modifier.none)
      {
        _Parser.Error("UNDEF", new Token(), "modifiers provided must not overlap");
      }
      else if (wrong != Modifier.none)
      {
        _Parser.Error("UNDEF", new Token(), "modifier(s) " + wrong + " not allowed here");
      }
      else if (((_Value & allowEither) != Modifier.none) && ((_Value & allowOr) != Modifier.none))
      {
        _Parser.Error("UNDEF", new Token(), "modifier(s) may either be " + allowEither + " or " + allowOr);
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks if the current set of modifiers has all the specified modifiers.
    /// </summary>
    /// <param name="mustHave">Modifiers that the current value must have.</param>
    // --------------------------------------------------------------------------------
    public void CheckMust(Modifier mustHave)
    {
      Modifier missing = (_Value & mustHave) ^ mustHave;
      if (missing != Modifier.none)
      {
        _Parser.Error("UNDEF", new Token(), "modifier(s) " + missing + " must be applied here");
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if the current value of modifiers has the set of 
    /// specified modifiers.
    /// </summary>
    /// <param name="mod">Set of modifiers to check.</param>
    /// <returns>
    /// True, if the current value has the specified set; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    public bool Has(Modifier mod)
    {
      return (_Value & mod) == mod;
    }

    #endregion
  }
}
