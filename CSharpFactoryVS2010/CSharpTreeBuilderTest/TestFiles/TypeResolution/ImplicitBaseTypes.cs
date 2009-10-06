class A1 // implicitly : System.Object
{ }

struct A2 // implicitly: System.ValueType
{ }

enum A3 // implicitly: System.Enum
{ }

delegate void A4(); // implicitly: System.MulticastDelegate

class A5<T>
{ }

struct A6<T>
{ }

class A8
{
  A5<int> a5;
  A6<int> a6;
}