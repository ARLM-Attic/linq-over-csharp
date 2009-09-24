// The resulting assembly from this project is used as a test subject in metadata import unit tests.

public class Class0 : BaseClass, A.B.IInterface1
{
  public const string a1 = "a1";
  internal static string a2;
  protected string a3;
  protected internal string a4;
  private string a5;

  public int P1 { get; set; }

  void A.B.IInterface1.M1() { }

  public void M1(int a, ref int b, out int c)
  {
    c = 0;
  }

  public void M1<T>() where T : PublicClass, A.B.IInterface1, new()
  { }

  public override void M2() { }
  public override void M3() { }
  public new void M4() { }
}

public abstract class BaseClass
{
  public virtual void M2() { }
  public abstract void M3();
  public void M4() { }
}

namespace A.B
{
  public class Class1
  {
    public class SubClass1
    {
    }
  }

  public enum Enum1
  {
    EnumValue1,
    EnumValue2 = 5
  }

  public struct Struct1
  {
  }

  public interface IInterface1
  {
    void M1();
  }

  public delegate void Delegate1();

  public class Generic1<T1,T2>
  {}
}

public class PublicClass
{
  public class PublicNestedClass
  { }

  internal class InternalNestedClass
  { }

  protected class ProtectedNestedClass
  { }
  
  protected internal class ProtectedInternalNestedClass
  { }
  
  private class PrivateNestedClass
  { }

  public static int publicNestedMember;
  internal static int internalNestedMember;
  protected static int protectedNestedMember;
  protected internal static int protectedInternalNestedMember;
  private static int privateNestedCMember;

}

internal class InternalClass
{
  public class PublicNestedInInternalClass
  { }
}

static class StaticClass
{ }

abstract class AbstractClass
{ }

sealed class SealedClass
{ }

namespace N
{
  public class PublicClass { }
  internal class InternalClass { }
}