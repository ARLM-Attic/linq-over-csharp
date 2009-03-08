public class MyClass<A, B, C>
  where A : struct, B, C
  where B : System.IAsyncResult
  where C : System.IO.BinaryReader
{ }

