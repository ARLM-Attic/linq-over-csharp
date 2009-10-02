class A : C.I<int>
{
  int B { get; set; }

  int C.I<int>.B { get; set; }

  D DP { get; set; } // D is a delegate, so DP is invocable
}

namespace C
{
  public interface I<T>
  {
    int B { get; set; }
  }
}

delegate void D();