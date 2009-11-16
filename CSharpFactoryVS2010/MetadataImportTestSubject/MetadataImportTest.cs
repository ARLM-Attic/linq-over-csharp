// The resulting assembly from this project is used as a test subject in metadata import unit tests.
using N;

public class Class0 : BaseClass, Interface0<Class0>
{
  public const string a1 = "a1";
  internal static string a2;
  protected string a3;
  protected internal string a4;
  private string a5;

  public int P1 { get; set; }
  // TODO: uncomment if support for explicitly implemented interface members is implemented
  //int Interface0<Class0>.P1 { get; set; }
  static int P2 { get; set; }
  public virtual int P3 { get; private set; }
  public override int P4 { get { return 0; } }
  public override sealed int P5 { set { } }
  public override int P6 { get; set; }

  public void M1<T1, T2, T3>(Class0 a, ref Class0 b, out Class0 c)
    where T1 : PublicClass, A.B.IInterface1, new()
    where T2 : class
    where T3 : struct
  {
    c = default(Class0);
  }

  public void M1() { }
  void Interface0<Class0>.M1() { }
  public override void M2() { }
  public override sealed void M3() { }
  public static void M4() { }
}

public abstract class BaseClass
{
  public virtual int P4 { get { return 0; } }
  public virtual int P5 { set { } }
  public abstract int P6 { get; set; }

  public virtual void M2() { }
  public abstract void M3();
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
  }

  public delegate void Delegate1();

  public class Generic1<T1, T2>
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

  public interface Interface0<T> where T : class, new()
  {
    int P1 { get; set; }
    void M1();
  }
}