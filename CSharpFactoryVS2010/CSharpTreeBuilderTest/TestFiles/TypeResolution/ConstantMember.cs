class A
{
  const int a = 0, b = 1;

  const D d = null; // D is a delegate, so "d" is invocable
}

delegate void D();