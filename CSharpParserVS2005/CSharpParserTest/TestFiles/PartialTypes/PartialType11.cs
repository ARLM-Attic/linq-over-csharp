delegate void A();
delegate void B();
delegate void C();
delegate void A<T>(T param);
delegate void B<T>(string param, T t);

namespace MyNamespace
{
  delegate void A();
  delegate void B();
  delegate void C();

  namespace SubNamespace
  {
    delegate void A<T>(T param);
    delegate void B<T>(T param);
    delegate void C<T>();
  }
}

namespace MyNamespace.SubNamespace
{
  delegate void A<T,U>(T param, U t);
  delegate void B<T, U>(T param);
  delegate void C<T, U>();
}