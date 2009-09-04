class A : C.I<int>
{
  int B { get; set; }

  int C.I<int>.B { get; set; }
}

namespace C
{
  public interface I<T>
  {
    int B { get; set; }
  }
}