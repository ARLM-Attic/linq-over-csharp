// ================================================================================================
// PropertyDeclarationNodeBase.cs
//
// Created: 2009.05.16, by Istvan Novak (DeepDiver)
// ================================================================================================
using CSharpFactory.ParserFiles;

namespace CSharpFactory.Syntax
{
  // ================================================================================================
  /// <summary>
  /// This class represents an abstract property node that can have up to two accessors.
  /// </summary>
  // ================================================================================================
  public abstract class PropertyDeclarationNodeBase : MemberDeclarationNode
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="PropertyDeclarationNodeBase"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    protected PropertyDeclarationNodeBase(Token start)
      : base(start)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the opening brace.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token OpenBrace { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the closing brace.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token CloseBrace { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the first accessor.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public AccessorNode FirstAccessor { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the second accessor.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public AccessorNode SecondAccessor { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the accessor with the specified name.
    /// </summary>
    /// <param name="name">The name of the accessor.</param>
    /// <returns>
    /// The accessor declaration, or null, if the specified accessor cannot be found.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    protected AccessorNode FindAccessor(string name)
    {
      if (FirstAccessor != null && FirstAccessor.Identifier == name) return FirstAccessor;
      if (SecondAccessor != null && SecondAccessor.Identifier == name) return SecondAccessor;
      return null;
    }
  }
}