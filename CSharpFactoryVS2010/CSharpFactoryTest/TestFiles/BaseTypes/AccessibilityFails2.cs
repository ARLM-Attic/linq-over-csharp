public class A1
{
  private class B { }
  protected class C : B { }
  private interface D { }
  protected class E : D { }
}

public class A2
{
  protected class B { }
  protected class C : B { }
  protected interface D { }
  protected class E : D { }
}

public class A3
{
  internal class B { }
  protected class C : B { }
  internal interface D { }
  protected class E : D { }
}

public class A4
{
  protected internal class B { }
  protected class C : B { }
  protected internal interface D { }
  protected class E : D { }
}
