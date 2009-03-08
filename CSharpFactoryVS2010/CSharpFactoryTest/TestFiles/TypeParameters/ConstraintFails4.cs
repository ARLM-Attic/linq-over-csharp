public class MyClass<A, B, C, D>
  where A: System.IAsyncResult, System.IO.BinaryReader
  where B : System.IO.BinaryReader, System.IO.BinaryWriter
  where C : System.IAsyncResult, System.IO.BinaryReader, System.IO.BinaryWriter
  where D : System.IAsyncResult, System.IO.BinaryReader, System.IO.BinaryWriter, System.IDisposable
{ }