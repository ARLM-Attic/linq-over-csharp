namespace Test
{
  // --- Valid access modifiers
  class Class0 {}
  public class Class1 {}
  internal class Class2 {}

  // --- Invalid access modifiers
  protected class Class3 {}
  private class Class4 {}
  protected internal class Class5 {}
  internal protected class Class6 {}
}
