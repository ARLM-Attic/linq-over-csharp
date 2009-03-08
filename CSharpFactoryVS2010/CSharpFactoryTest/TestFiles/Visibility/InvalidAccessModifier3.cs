namespace Test
{
  struct Declaring
  {
    // --- Valid access modifiers
    struct Struct0 { }
    public struct Struct1 { }
    internal struct Struct2 { }
    private struct Struct4 { }
    void Method1() { }
    public void Method2() { }
    internal void Method3() { }
    private void Method4() { }

    protected void Method5() { }
    protected internal void Method6() {}
    internal protected void Method7() { }

    // --- Invalid access modifiers
    protected struct Struct3 { }
    protected internal struct Struct5 { }
    internal protected struct Struct6 { }
  }
}
