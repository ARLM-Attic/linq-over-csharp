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
    /// <summary>No modifiers specified.</summary>
    none = 0x0000,

    /// <summary>"new" modifier has been used.</summary>
    @new = 0x0001,
    /// <summary>"public" modifier has been used.</summary>
    @public = 0x0002,
    /// <summary>"protected" modifier has been used.</summary>
    @protected = 0x0004,
    /// <summary>"internal" modifier has been used.</summary>
    @internal = 0x0008,
    /// <summary>"private" modifier has been used.</summary>
    @private = 0x0010,
    /// <summary>"unsafe" modifier has been used.</summary>
    @unsafe = 0x0020,
    /// <summary>"static" modifier has been used.</summary>
    @static = 0x0040,
    /// <summary>"readonly" modifier has been used.</summary>
    @readonly = 0x0080,
    /// <summary>"volatile" modifier has been used.</summary>
    @volatile = 0x0100,
    /// <summary>"virtual" modifier has been used.</summary>
    @virtual = 0x0200,
    /// <summary>"sealed" modifier has been used.</summary>
    @sealed = 0x0400,
    /// <summary>"override" modifier has been used.</summary>
    @override = 0x0800,
    /// <summary>"abstract" modifier has been used.</summary>
    @abstract = 0x1000,
    /// <summary>"extern" modifier has been used.</summary>
    @extern = 0x2000,

    /// <summary>Set of visibility modifiers</summary>
    VisibilityAccessors = @public | @protected | @internal | @private,
    /// <summary>"protected internal" modifier</summary>
    ProtectedInternal = @protected | @internal,

    /// <summary>Set of modifiers a field can have.</summary>
    fields = VisibilityAccessors | @new | @unsafe | @static | @readonly | @volatile,

    /// <summary>Set of modifiers an event property accessor can have.</summary>
    propEvntMeths = VisibilityAccessors | @new | @unsafe | @static | @virtual | @sealed | @override | @abstract | @extern,
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
    private readonly CSharpSyntaxParser _Parser;

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
    /// <param name="token">Token representing the modifier</param>
    // --------------------------------------------------------------------------------
    public void Add(Modifier m, Token token)
    {
      if ((_Value & m) == 0) _Value |= m;
      else _Parser.Error1004(token, m.ToString().ToLower());
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
