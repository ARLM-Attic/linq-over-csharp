﻿using System;
namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This abstract class represents a using directive in the semantic graph.
  /// </summary>
  // ================================================================================================
  public abstract class UsingEntity : SemanticEntity, IHasLexicalScope
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="UsingEntity"/> class.
    /// </summary>
    /// <param name="lexicalScope">The region of program text where the using entity has effect.</param>
    // ----------------------------------------------------------------------------------------------
    protected UsingEntity(SourceRegion lexicalScope)
    {
      if (lexicalScope==null)
      {
        throw new ArgumentNullException("lexicalScope");
      }

      LexicalScope = lexicalScope;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the region of program text where this object has effect on.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public SourceRegion LexicalScope { get; private set; }
  }
}
