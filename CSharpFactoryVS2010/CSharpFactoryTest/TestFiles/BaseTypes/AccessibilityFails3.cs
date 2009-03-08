public class A1
{
  private class B { }
  internal class C : B { }
  private interface D { }
  internal class E : D { }
}

public class A2
{
  protected class B { }
  internal class C : B { }
  protected interface D { }
  internal class E : D { }
}

public class A3
{
  internal class B { }
  internal class C : B { }
  internal interface D { }
  internal class E : D { }
}

public class A4
{
  protected internal class B { }
  internal class C : B { }
  protected internal interface D { }
  internal class E : D { }
}

internal class A5 { }
internal class B5 : A5 { }

internal interface A6 { }
internal class B6 : A6 { }
