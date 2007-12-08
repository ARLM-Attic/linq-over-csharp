public class MyClass
{
  // --- Valid nested types
  public class Nested1 {}
  private class Nested2 {}
  protected class Nested3 {}
  internal class Nested4 {}
  protected internal class Nested5 {}
  internal protected class Nested6 {}

  // --- Invalid accessors
  public private class Nested7 {}
  private protected class Nested8 {}
  internal public class Nested9 {}
  public private protected class Nested10 {}
}