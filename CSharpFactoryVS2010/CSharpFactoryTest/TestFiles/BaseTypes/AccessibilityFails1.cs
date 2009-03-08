public class A1
{
  private class B { }
  public class C: B {}
  private interface D {}
  public class E: D {}
}

public class A2
{
  protected class B { }
  public class C : B { }
  protected interface D { }
  public class E : D { }
}

public class A3
{
  internal class B { }
  public class C : B { }
  internal interface D { }
  public class E : D { }
}

public class A4
{
  protected internal class B { }
  public class C : B { }
  protected internal interface D { }
  public class E : D { }
}

internal class A5 {}
public class B5: A5 {}

internal interface A6 { }
public class B6 : A6 { }