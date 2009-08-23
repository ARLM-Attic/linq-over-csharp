// ================================================================================================
// CompilationMessageNode.cs
//
// Created: 2009.04.05, by Istvan Novak (DeepDiver)
// ================================================================================================
using System.Linq;
using System.Collections.Generic;
using CSharpTreeBuilder.Ast;
using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Cst
{
  // ================================================================================================
  /// <summary>
  /// This class represents a compilation message node.
  /// </summary>
  // ================================================================================================
  public class CompilationMessageNode: SemanticsNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="CompilationMessageNode"/> class.
    /// </summary>
    /// <param name="compilationUnitNode">The source file node.</param>
    /// <param name="code">The message code.</param>
    /// <param name="token">The message token.</param>
    /// <param name="description">The textual description.</param>
    // ----------------------------------------------------------------------------------------------
    public CompilationMessageNode(CompilationUnitNode compilationUnitNode, string code, Token token,
      string description)
      : base(compilationUnitNode)
    {
      Code = code;
      MessageToken = token;
      Description = description;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the code of the message.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string Code { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the descriptive text belonging to the message.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string Description { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the token belonging to the message.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token MessageToken { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the other optional message related tokens.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public IEnumerable<Token> RelatedTokens { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the optional parameters belonging to the message.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public IEnumerable<object> Parameters { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the line number of the error token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public int Line
    {
      get
      {
        return MessageToken == null ? -1 : MessageToken.Line + (ErrorLineOffset > 0 ? ErrorLineOffset : 0);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the columns number of the error token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public int Column
    {
      get
      {
        return MessageToken == null ? -1 : MessageToken.Column;
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the error line offset.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public int ErrorLineOffset { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the redirected source file name.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string RedirectedSourceFile { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the name of the source file.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string SourceFileName
    {
      get { return RedirectedSourceFile ?? CompilationUnitNode.Name; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Sets the error line offset to the sepcified value.
    /// </summary>
    /// <param name="offset">Error line offset.</param>
    // ----------------------------------------------------------------------------------------------
    internal void SetErrorLineOffset(int offset)
    {
      ErrorLineOffset = offset;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Redirects the source file.
    /// </summary>
    /// <param name="fileName">Name of the file.</param>
    // ----------------------------------------------------------------------------------------------
    internal void RedirectSourceFile(string fileName)
    {
      RedirectedSourceFile = fileName;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the string representation of this message.
    /// </summary>
    /// <returns>The string representation of this message.</returns>
    // ----------------------------------------------------------------------------------------------
    public string Message
    {
      get 
      {
        return Parameters == null ? Description : string.Format(Description, Parameters.ToArray());
      }
    }
  }
}