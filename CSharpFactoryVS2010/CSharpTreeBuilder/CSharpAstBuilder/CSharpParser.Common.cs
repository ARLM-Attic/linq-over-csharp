// ================================================================================================
// CSharpAstBuilder.Common.cs
//
// Created: 2009.05.22, by Istvan Novak (DeepDiver)
// ================================================================================================
using System;
using System.Collections;
using System.IO;
using CSharpTreeBuilder.Ast;
using CSharpTreeBuilder.ProjectContent;

namespace CSharpTreeBuilder.CSharpAstBuilder
{
  // ================================================================================================
  /// <summary>
  /// Commom parts of the AST Builder
  /// </summary>
  // ================================================================================================
  public partial class CSharpParser
  {
    #region These constants are used within the parser independently of C# syntax.

    /// <summary>Represents the "true" value in the token set table.</summary>
    const bool T = true;

    // ReSharper disable InconsistentNaming
    /// <summary>Represents the "false" value in the token set table.</summary>
    const bool x = false;

    /// <summary>
    /// Represents the minimum distance (measured by tokens) between two error points 
    /// that are taken into account as separate errors.
    /// </summary>
    const int MinimumDistanceOfSeparateErrors = 2;

    /// <summary>Represents the last recognized token.</summary>
    private Token t;

    /// <summary>Represents the lookahead token.</summary>
    private Token la;

    /// <summary>Represents the current distance from the last error.</summary>
    private int errDist = MinimumDistanceOfSeparateErrors;
    // ReSharper restore InconsistentNaming

    #endregion

    #region Fields used by the parser

    // ReSharper disable InconsistentNaming
    /// <summary>Token ID for #if preprocessor directive.</summary>
    public const int ppIfKind = _ppIf;
    /// <summary>Token ID for #elif preprocessor directive.</summary>
    public const int ppElifKind = _ppElif;
    /// <summary>Token ID for #else preprocessor directive.</summary>
    public const int ppElseKind = _ppElse;
    /// <summary>Token ID for #endif preprocessor directive.</summary>
    public const int ppEndifKind = _ppEndif;
    /// <summary>Token ID for EOF.</summary>
    public const int EOFKind = _EOF;
    // ReSharper restore InconsistentNaming

    #endregion

    #region Lifecycle methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new instance of this parser using the specified scanner, compilation
    /// uint and file.
    /// </summary>
    /// <param name="scanner">The scanner used to scan tokens.</param>
    /// <param name="sourceFileNode">File used by this parser instance.</param>
    // ----------------------------------------------------------------------------------------------
    public CSharpParser(Scanner scanner, SourceFileNode sourceFileNode)
    {
      SuppressErrors = false;
      Scanner = scanner;
      SourceFileNode = sourceFileNode;
    }

    #endregion

    #region Public properties

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the source file node.
    /// </summary>
    /// <value>The source file node.</value>
    // ----------------------------------------------------------------------------------------------
    public SourceFileNode SourceFileNode { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the scanner used by this parser.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Scanner Scanner { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the lookahead token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token Lookahead
    {
      get { return la; } 
    }

    #endregion

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Expects the next token to be as defined by tokenKind.
    /// </summary>
    /// <param name="tokenKind">Expected kind of token.</param>
    /// <remarks>
    /// If the token does not match with the kind specified, raises a syntax error.
    /// </remarks>
    // --------------------------------------------------------------------------------
    void Expect(int tokenKind)
    {
      if (la.kind == tokenKind) Get();
      else
      {
        SynErr(tokenKind);
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks if the lookahead token can be a start for the specified tokenKind.
    /// </summary>
    /// <param name="tokenKind">Kind of token to examine.</param>
    /// <returns>
    /// True, if the lookahead token can be a strat for tokenKind; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    bool StartOf(int tokenKind)
    {
      return _StartupSet[tokenKind, la.kind];
    }

    #region Token sets

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a BitArray with the specified values set.
    /// </summary>
    /// <param name="values">Values to set in the BitArray</param>
    /// <returns></returns>
    // --------------------------------------------------------------------------------
    static BitArray NewSet(params int[] values)
    {
      var a = new BitArray(maxT);
      foreach (var item in values) a[item] = true;
      return a;
    }

    // ReSharper disable InconsistentNaming

    /// <summary>Tokens representing type shortcuts</summary>
    private static readonly BitArray typeKW =
      NewSet(_char, _bool, _object, _string, _sbyte, _byte, _short,
             _ushort, _int, _uint, _long, _ulong, _float, _double, _decimal);

    /// <summary>
    /// Tokens representing unary header tokens
    /// </summary>
    private static readonly BitArray
      unaryHead = NewSet(_plus, _minus, _not, _tilde, _times, _inc, _dec, _and);

    /// <summary>
    /// Tokens representing assignment start tokens
    /// </summary>
    private static readonly BitArray
      assnStartOp = NewSet(_plus, _minus, _not, _tilde, _times);

    /// <summary>Tokens that can follow a cast operation</summary>
    private static readonly BitArray
      castFollower = NewSet(_tilde, _not, _lpar, _ident,
                            /* literals */
                            _intCon, _realCon, _charCon, _stringCon,
                            /* any keyword expect as and is */
                            _abstract, _base, _bool, _break, _byte, _case, _catch,
                            _char, _checked, _class, _const, _continue, _decimal, _default,
                            _delegate, _do, _double, _else, _enum, _event, _explicit,
                            _extern, _false, _finally, _fixed, _float, _for, _foreach,
                            _goto, _if, _implicit, _in, _int, _interface, _internal,
                            _lock, _long, _namespace, _new, _null, _object, _operator,
                            _out, _override, _params, _private, _protected, _public,
                            _readonly, _ref, _return, _sbyte, _sealed, _short, _sizeof,
                            _stackalloc, _static, _string, _struct, _switch, _this, _throw,
                            _true, _try, _typeof, _uint, _ulong, _unchecked, _unsafe,
                            _ushort, _usingKW, _virtual, _void, _volatile, _while
        );

    /// <summary>Tokens that can follow argument lists</summary>
    private static readonly BitArray
      typArgLstFol = NewSet(_lpar, _rpar, _rbrack, _colon, _scolon, _comma, _dot,
                            _question, _eq, _neq);

    /// <summary>Reserved C# keywords</summary>
    private static readonly BitArray
      keyword = NewSet(_abstract, _as, _base, _bool, _break, _byte, _case, _catch,
                       _char, _checked, _class, _const, _continue, _decimal, _default,
                       _delegate, _do, _double, _else, _enum, _event, _explicit,
                       _extern, _false, _finally, _fixed, _float, _for, _foreach,
                       _goto, _if, _implicit, _in, _int, _interface, _internal,
                       _is, _lock, _long, _namespace, _new, _null, _object, _operator,
                       _out, _override, _params, _private, _protected, _public,
                       _readonly, _ref, _return, _sbyte, _sealed, _short, _sizeof,
                       _stackalloc, _static, _string, _struct, _switch, _this, _throw,
                       _true, _try, _typeof, _uint, _ulong, _unchecked, _unsafe,
                       _ushort, _usingKW, _virtual, _void, _volatile, _while);

    /// <summary>Assignment operators</summary>
    private static readonly BitArray
      assgnOps = NewSet(_assgn, _plusassgn, _minusassgn, _timesassgn, _divassgn,
                     _modassgn, _andassgn, _orassgn, _xorassgn, _lshassgn);
                 // rshassgn: ">" ">="  no whitespace allowed
    // ReSharper restore InconsistentNaming

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Return the n-th token after the current lookahead token. 
    /// </summary>
    /// <param name="n">Number of tokens to skip.</param>
    /// <returns>n-th token after the current lookahead token.</returns>
    // --------------------------------------------------------------------------------
    Token Peek(int n)
    {
      Scanner.ResetPeek();
      var token = la;
      while (n > 0) { token = Scanner.Peek(); n--; }
      return token;
    }

    #endregion
    
    #region Common methods used from outside

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks if the pragma is the first token in the current line.
    /// </summary>
    /// <param name="symbol">Token representing the pragma.</param>
    /// <returns>True, if the pragma is the first token; otherwise, false.</returns>
    // --------------------------------------------------------------------------------
    public bool CheckTokenIsFirstInLine(Token symbol)
    {
      int oldPos = Scanner.Buffer.Pos;
      bool wsOnly = true;
      for (int i = symbol.col - 1; i >= 1; i--)
      {
        Scanner.Buffer.Pos = symbol.pos - i;
        int ch = Scanner.Buffer.Peek();
        wsOnly &= (ch == ' ' || (ch >= 9 && ch <= 13));
      }
      Scanner.Buffer.Pos = oldPos;
      return wsOnly;
    }

    #endregion

    #region Utility methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Terminates the specified node.
    /// </summary>
    /// <param name="node">The node to terminate.</param>
    /// <remarks>
    /// If the node is null, the operation aborts silently.
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    private void Terminate(ISyntaxNode node)
    {
      if (node != null) node.Terminate(t);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Starts the specified node.
    /// </summary>
    /// <param name="node">The node to start.</param>
    /// <remarks>
    /// If the node is null, the operation aborts silently.
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    private void Start<TNode, TParent>(SyntaxNodeCollection<TNode, TParent> node)
      where TNode : class, ISyntaxNode
      where TParent: class, ISyntaxNode
    {
      if (node != null) node.StartToken = t;
    }

    // --------------------------------------------------------------------------------------------
    /// <summary>
    /// Builds the ast for source file.
    /// </summary>
    /// <param name="sourceNode">The source file node to build the AST for.</param>
    /// <param name="project">The error handler instance.</param>
    // --------------------------------------------------------------------------------------------
    public static void BuildAstForSourceFile(SourceFileNode sourceNode, 
      CSharpProject project)
    {
      // --- Open the stream
      var stream = File.OpenText(sourceNode.FullName).BaseStream;
      try
      {
        // --- Create a scanner with token processor events and a parser using that scanner
        var scanner = new Scanner(stream);
        var parser = new CSharpParser(scanner, sourceNode) {ErrorHandler = project, Project = project};
        scanner.NewLineReached += parser.NewLineReached;
        scanner.TokenScanned += parser.TokenScanned;
        scanner.WhitespaceScanned += parser.WhitespaceScanned;
        
        // --- Parse the source file
        parser.Parse();

        // --- Sign that all tokens have been added to the symbol stream
        sourceNode.SymbolStream.FreezeStream();
        sourceNode.LastScannedPosition = scanner.Position;

        // --- Release event handlers
        scanner.NewLineReached -= parser.NewLineReached;
        scanner.TokenScanned -= parser.TokenScanned;
        scanner.WhitespaceScanned -= parser.WhitespaceScanned;
      }
      finally
      {
        // --- Anyway, close the source stream
        stream.Close();
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Binds left and right operands to a binary operator.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    private void BindBinaryOperator(BinaryOperatorNode opNode, ExpressionNode unaryNode,
      BinaryOperatorNodeBase rgNode)
    {
      if (rgNode == null)
      {
        opNode.RightOperand = unaryNode;
      }
      else
      {
        opNode.RightOperand = rgNode;
        rgNode.LeftOperand = unaryNode;
      }
      opNode.Terminate(t);
    }

    #endregion

    #region Scanner event response methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Responds to the "whitespaces scanned" event of the scanner.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">
    /// The <see cref="CSharpTreeBuilder.CSharpAstBuilder.WhitespaceScannedEventArgs"/> 
    /// instance containing the event data.
    /// </param>
    // ----------------------------------------------------------------------------------------------
    private void WhitespaceScanned(object sender, WhitespaceScannedEventArgs e)
    {
      SourceFileNode.SymbolStream.AddWhitespaceToStream(e.Whitespace);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Responds to the "token scanned" event of the scanner.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">
    /// The <see cref="CSharpTreeBuilder.CSharpAstBuilder.TokenScannedEventArgs"/> instance 
    /// containing the event data.
    /// </param>
    // ----------------------------------------------------------------------------------------------
    private void TokenScanned(object sender, TokenScannedEventArgs e)
    {
      SourceFileNode.SymbolStream.AddTokenToStream(e.Token);
      e.Token.TokenizedStreamPosition = SourceFileNode.SymbolStream.CurrentPosition;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Responds to the "new line reached" event of the scanner.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">
    /// The <see cref="CSharpTreeBuilder.CSharpAstBuilder.NewLineReachedEventArgs"/> instance 
    /// containing the event data.
    /// </param>
    // ----------------------------------------------------------------------------------------------
    private void NewLineReached(object sender, NewLineReachedEventArgs e)
    {
      // --- Nothing to do in the case of first line.
      if (e.LineNumber == 1) return;
      SourceFileNode.SymbolStream.AddEndOfLineToStream(e.LineNumber);
    }

    #endregion
  }
}