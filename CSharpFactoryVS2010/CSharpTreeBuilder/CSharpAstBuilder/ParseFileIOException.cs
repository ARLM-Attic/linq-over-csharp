// ================================================================================================
// ParseFileIOException.cs
//
// Created: 2009.05.22, by Istvan Novak (DeepDiver)
// ================================================================================================
using System;
using System.Runtime.Serialization;

namespace CSharpTreeBuilder.CSharpAstBuilder
{
  // ================================================================================================
  /// <summary>
  /// Defines the exception to be raised when a parse file cannot be opened.
  /// </summary>
  // ================================================================================================
  public class ParseFileIOException : Exception
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ParseFileIOException"/> class.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ParseFileIOException()
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ParseFileIOException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    // ----------------------------------------------------------------------------------------------
    public ParseFileIOException(string message)
      : base(message)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ParseFileIOException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="innerException">The inner exception.</param>
    // ----------------------------------------------------------------------------------------------
    public ParseFileIOException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ParseFileIOException"/> class.
    /// </summary>
    /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
    /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is null. </exception>
    /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0). </exception>
    // ----------------------------------------------------------------------------------------------
    protected ParseFileIOException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}