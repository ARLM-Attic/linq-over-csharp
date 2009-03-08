namespace Test
{
  // --- Valid access modifiers
  struct Struct0 { }
  public struct Struct1 { }
  internal struct Struct2 { }

  // --- Invalid access modifiers
  protected struct Struct3 { }
  private struct Struct4 { }
  protected internal struct Struct5 { }
  internal protected struct Struct6 { }
}
