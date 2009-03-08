using System.IO;
using CSharpFactory.ParserFiles;

namespace CSharpFactory.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This interface defines the behaviour of a compilation error handler object.
  /// </summary>
  // ==================================================================================
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

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Redirects line numbering and file name handling.
    /// </summary>
    /// <param name="currentLine">Current source line.</param>
    /// <param name="lineNumber">New line number.</param>
    /// <param name="fileName">Redirected filename.</param>
    // --------------------------------------------------------------------------------
    void Redirect(int currentLine, int lineNumber, string fileName);

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resets the line number and file name redirection.
    /// </summary>
    // --------------------------------------------------------------------------------
    void ResetRedirection();
  }
}
