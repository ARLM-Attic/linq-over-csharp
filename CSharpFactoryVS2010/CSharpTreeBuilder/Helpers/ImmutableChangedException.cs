// ================================================================================================
// ImmutableChangedException.cs
//
// Created: 2009.05.21, by Istvan Novak (DeepDiver)
// ================================================================================================
using System;
using System.Runtime.Serialization;
using CSharpTreeBuilder.Properties;

namespace CSharpTreeBuilder.Collections
{
  // ====================================================================================
  /// <summary>
  /// This exception is raised whan the state of an immutable object is about to
  /// change.
  /// </summary>
  /// <remarks>
  ///     This type implements all the constructors the <see cref="System.Exception"/>
  ///     type has. The default constructor automatically sets a message that indicates the
  ///     cause of this exception.
  /// </remarks>
  // ====================================================================================
  public class ImmutableChangedException : InvalidOperationException
  {
    // ------------------------------------------------------------------------------------
    /// <summary>
    ///     Initializes a new instance of the <see cref="ImmutableChangedException"/>
    ///     class.
    /// </summary>
    // ------------------------------------------------------------------------------------
    public ImmutableChangedException()
      : base(Resources.ImmutableChanged)
    {
    }

    // ------------------------------------------------------------------------------------
    /// <summary>
    ///     Initializes a new instance of the <see cref="ImmutableChangedException"/>
    ///     class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    // ------------------------------------------------------------------------------------
    public ImmutableChangedException(string message)
      : base(message)
    {
    }

    // ------------------------------------------------------------------------------------
    /// <summary>
    ///     Initializes a new instance of the <see cref="ImmutableChangedException"/>
    ///     class with serialized data.
    /// </summary>
    /// <param name="info">
    ///     The <see cref="SerializationInfo"/> that holds the serialized object data
    ///     about the exception being thrown.
    /// </param>
    /// <param name="context">
    ///     The <see cref="StreamingContext"/> that contains contextual information about
    ///     the source or destination.
    /// </param>
    // ------------------------------------------------------------------------------------
    public ImmutableChangedException(SerializationInfo info, StreamingContext context)
      : 
      base(info, context)
    {
    }

    // ------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ImmutableChangedException"/>
    /// class with a specified error message and a reference to the inner exception that is
    /// the cause of this exception.
    /// </summary>
    /// <returns>
    /// The exception that is the cause of the current exception, or a null reference
    /// (<b>Nothing</b> in Visual Basic) if no inner exception is specified.
    /// </returns>
    /// <param name="message">
    /// The error message that explains the reason for the exception.
    /// </param>
    /// <param name="innerException">Exception embedded into this instance.</param>
    // ------------------------------------------------------------------------------------
    public ImmutableChangedException(string message, Exception innerException)
      : 
      base(message, innerException)
    {
    }
  }
}
