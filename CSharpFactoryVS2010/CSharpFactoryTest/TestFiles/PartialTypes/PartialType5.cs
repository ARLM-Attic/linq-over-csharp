delegate void A();
delegate void B();
delegate void C();
delegate void A(int param);
delegate void B(string param);

namespace MyNamespace
{
  delegate void A();
  delegate void B();
  delegate void C();

  namespace SubNamespace
  {
    delegate void A(int param);
    delegate void B(string param);
    delegate void C();
  }
}

namespace MyNamespace.SubNamespace
{
  delegate void A(int param);
  delegate void B(string param);
  delegate void C();
}