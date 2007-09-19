public class MyClass<A, B, C>
  where A : B, C
  where B : struct, System.IAsyncResult
  where C : class
{ }

public class MyClass2<A, B, C, D>
  where A : B, C
  where B : struct, System.IAsyncResult
  where C : System.IConvertible, D
  where D : struct, System.IFormattable
{ }