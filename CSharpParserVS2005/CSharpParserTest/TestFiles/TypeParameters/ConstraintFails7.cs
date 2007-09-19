public sealed class SealedClass {}

public class MyClass<A, B>
  where A: SealedClass
  where B: System.IO.DirectoryInfo
{}