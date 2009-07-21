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
  public sealed class BuiltInTypeEntity : TypeEntity
  {
  }
}
