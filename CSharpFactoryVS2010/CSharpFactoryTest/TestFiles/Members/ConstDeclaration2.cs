public class MyClass
{
  public const sbyte _1 = 1;
  public const byte _2 = 2;
  public const short _3 = 3;
  public const ushort _4 = 4;
  public const int _5 = 5;
  public const uint _6 = 6;
  public const long _7 = 7;
  public const ulong _8 = 8;
  public const char _9 = 'c';
  public const float _10 = 123.5F;
  public const double _11 = 123.5;
  public const decimal _12 = 123M;
  public const bool _13 = true;
  public const string _14 = "Hello";
  public const EnumType _15 = EnumType.Value1;
  public const EnumType _16 = 0;
  public const System.IO.BinaryReader _17 = null;

  // --- Error
  public const Struct _18 = null;
}

public enum EnumType { Value1 }

public struct Struct {}