using System.IO;
using CSharpParser.ParserFiles;

namespace CSharpParser.ProjectModel
{
  public interface ICompilationErrorHandler
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Defines the output stream where errors should be written.
    /// </summary>
    // --------------------------------------------------------------------------------
    TextWriter ErrorStream { get; set; }
    
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Defines the format of message to write to the output stream.
    /// </summary>
    // --------------------------------------------------------------------------------
    string ErrorMessageFormat { get; set; }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Add a new error to the list of errors.
    /// </summary>
    /// <param name="code">Error code.</param>
    /// <param name="errorPoint">Token describing the error position.</param>
    /// <param name="description">Detailed error description.</param>
    // --------------------------------------------------------------------------------
    void Error(string code, Token errorPoint, string description);

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Add a new error to the list of errors.
    /// </summary>
    /// <param name="code">Error code.</param>
    /// <param name="errorPoint">Token describing the error position.</param>
    /// <param name="description">Detailed error description.</param>
    /// <param name="parameters">Error parameters.</param>
    // --------------------------------------------------------------------------------
    void Error(string code, Token errorPoint, string description,
               params object[] parameters);

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Add a new warning to the list of warnings.
    /// </summary>
    /// <param name="code">Warning code.</param>
    /// <param name="warningPoint">Token describing the warning position.</param>
    /// <param name="description">Detailed warning description.</param>
    // --------------------------------------------------------------------------------
    void Warning(string code, Token warningPoint, string description);

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Add a new warning to the list of warnings.
    /// </summary>
    /// <param name="code">Warning code.</param>
    /// <param name="warningPoint">Token describing the warning position.</param>
    /// <param name="description">Detailed warning description.</param>
    /// <param name="parameters">Warning parameters.</param>
    // --------------------------------------------------------------------------------
    void Warning(string code, Token warningPoint, string description,
               params object[] parameters);
  }
}
