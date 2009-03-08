using System;

interface B : IDisposable, D, IServiceProvider { }
interface C : B, IAsyncResult, ICustomFormatter, IFormatProvider { }
interface D : IFormattable, C, IEquatable<string>, ICloneable { }

class Master
{
  class Inner
  {
    interface B : IDisposable, D, IServiceProvider { }
    interface C : B, IAsyncResult, ICustomFormatter, IFormatProvider { }
    interface D : IFormattable, C, IEquatable<string>, ICloneable { }
  }
}