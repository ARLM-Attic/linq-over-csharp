using System;

namespace CSharpTreeBuilder.CSharpSemanticGraph
{
  // ================================================================================================
  /// <summary>
  /// This class represents a builtin C# type.
  /// </summary>
  /// <remarks>
  /// Builtin types are identified through reserved words, but these reserved words are simply aliases
  /// for predefined types in the System namespace. These are: sbyte, byte, short, ushort, int, uint,
  /// long, ulong, char, float, double, bool, decimal, bool, string, object.
  /// </remarks>
  // ================================================================================================
  public sealed class BuiltInTypeEntity : TypeEntity, IAliasType
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="BuiltInTypeEntity"/> class.
    /// </summary>
    /// <param name="builtInType">A built in type.</param>
    // ----------------------------------------------------------------------------------------------
    public BuiltInTypeEntity(BuiltInType builtInType)
    {
      BuiltInType = builtInType;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the built in type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public BuiltInType BuiltInType { get; private set; }
    
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the alias type.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public TypeEntity Alias
    {
      get
      {
        throw new NotImplementedException();
      }
    }
  }
}
