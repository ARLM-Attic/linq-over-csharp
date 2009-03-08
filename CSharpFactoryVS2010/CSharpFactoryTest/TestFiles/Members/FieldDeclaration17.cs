public class MyClass
{
  private volatile byte _byte;
  private volatile sbyte _sbyte;
  private volatile short _short;
  private volatile ushort _ushort;
  private volatile int _int;
  private volatile uint _uint;
  private volatile char _char;
  private volatile float _float;
  private volatile bool _bool;
  private volatile string _string;
  private volatile ByteEnum _1;
  private volatile SByteEnum _2;
  private volatile ShortEnum _3;
  private volatile UShortEnum _4;
  private volatile IntEnum _5;
  private volatile UIntEnum _6;
  
  // --- This is not valid
  private volatile double _double; 
  private volatile long _long;
  private volatile ulong _ulong;
  private volatile LongEnum _LongEnum;
  private volatile ULongEnum _ULongEnum;
}

public enum ByteEnum: byte {}
public enum SByteEnum : sbyte { }
public enum ShortEnum : short { }
public enum UShortEnum : ushort { }
public enum IntEnum : int { }
public enum UIntEnum : uint { }
public enum LongEnum : long { }
public enum ULongEnum : ulong { }


