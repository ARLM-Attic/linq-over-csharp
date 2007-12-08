public class A1
{
  private class B { }
  private class C : B { }
  private interface D { }
  private class E : D { }
}

public class A2
{
  protected class B { }
  private class C : B { }
  protected interface D { }
  private class E : D { }
}

public class A3
{
  internal class B { }
  private class C : B { }
  internal interface D { }
  private class E : D { }
}

public class A4
{
  protected internal class B { }
  private class C : B { }
  protected internal interface D { }
  private class E : D { }
}
