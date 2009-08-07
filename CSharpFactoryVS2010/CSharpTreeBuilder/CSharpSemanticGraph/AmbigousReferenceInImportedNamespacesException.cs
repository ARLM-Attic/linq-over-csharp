using System;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This exception represents error CS0104: 'reference' is an ambiguous reference between 'identifier' and 'identifier'
  /// </summary>
  // ================================================================================================
  public sealed class AmbigousReferenceInImportedNamespacesException : Exception
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="AmbigousReferenceInImportedNamespacesException"/> class.
    /// </summary>
    /// <param name="reference">A name from source code that references a type.</param>
    /// <param name="identifier1">The fully qualified name of the first type found.</param>
    /// <param name="identifier2">The fully qualified name of the secound type found.</param>
    // ----------------------------------------------------------------------------------------------
    public AmbigousReferenceInImportedNamespacesException(string reference, string identifier1, string identifier2)
    {
      Reference = reference;
      Identifier1 = identifier1;
      Identifier2 = identifier2;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the name from source code that references a type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string Reference { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the fully qualified name of the first type found.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string Identifier1 { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets The fully qualified name of the second type found.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public string Identifier2 { get; private set; }
  }
}
