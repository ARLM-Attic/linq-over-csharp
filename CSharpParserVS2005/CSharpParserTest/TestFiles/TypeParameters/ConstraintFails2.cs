public class MyClass<A, B, C, D, E>
  where A: new(), new()
  where B: class, new(), new()
  where C: struct, new(), new(), new()
  where D: System.IO.BinaryReader, new(), new()
  where E: class, new(), System.IAsyncResult
{}
