// ====================================================================================
// IReadOnlySupport.cs
//
// Created by: NI, 2006.12.08
// ====================================================================================

namespace CSharpFactory.Collections
{
  // ====================================================================================
  /// <summary>
  /// This interface defines the behaviour of objects that support read-only behaviour.
  /// </summary>
  /// <remarks>
  /// 	<para>Supporting read-only behaviour means the following responsibility:</para>
  /// 	<list type="bullet">
  /// 		<item>
  ///             Each object has an <see cref="IsReadOnly"/> property to query the
  ///             object instance if it is immutable (proeprty gives a <strong>true</strong>
  ///             value) or changeable (property gives a <strong>false</strong> value).
  ///         </item>
  /// 		<item>
  ///             The object has a <see cref="MakeReadOnly"/> method that makes the
  ///             object immutable. Once an object has been changed to read-only it cannot be
  ///             made changeable again.
  ///         </item>
  /// 	</list>
  /// </remarks>
  // ====================================================================================
  public interface IReadOnlySupport
  {
    // ------------------------------------------------------------------------------------
    /// <summary>
    /// Signs if the object is in read-only state or not.
    /// </summary>
    /// <value>
    /// 	<strong>True</strong>, if the object is immutable; otherwise,
    /// <strong>false</strong>.
    /// </value>
    /// <seealso cref="MakeReadOnly">MakeReadOnly Method</seealso>
    // ------------------------------------------------------------------------------------
    bool IsReadOnly { get; }

    // ------------------------------------------------------------------------------------
    /// <summary>
    /// Sets the object's state to read-only.
    /// </summary>
    /// <example>
    /// Once this object has been set to read-only it cannotbe set to changeable
    /// again.
    /// </example>
    /// <seealso cref="IsReadOnly">IsReadOnly Property</seealso>
    // ------------------------------------------------------------------------------------
    void MakeReadOnly();
  }
}
