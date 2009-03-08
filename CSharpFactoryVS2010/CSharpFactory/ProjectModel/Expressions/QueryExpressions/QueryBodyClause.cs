using CSharpFactory.Collections;
using CSharpFactory.ParserFiles;

namespace CSharpFactory.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This abstract type a clause that can be a clause of a query body.
  /// </summary>
  // ==================================================================================
  public abstract class QueryBodyClause : LanguageElement
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new query body clause.
    /// </summary>
    /// <param name="token">Token providing position information.</param>
    /// <param name="parser">Parser instance creating this element.</param>
    // --------------------------------------------------------------------------------
    protected QueryBodyClause(Token token, CSharpSyntaxParser parser)
      : base(token, parser)
    {
    }
  }

  // ==================================================================================
  /// <summary>
  /// This type represents a collection query body clause declarations.
  /// </summary>
  // ==================================================================================
  public sealed class QueryBodyClauseCollection : ImmutableCollection<QueryBodyClause>
  {
  }
}