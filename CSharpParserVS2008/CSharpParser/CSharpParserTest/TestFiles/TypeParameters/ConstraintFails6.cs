using System;

public class MyClass<A, B, C, D>
  where A : class, IAsyncResult, System.IAsyncResult
  where B : IAsyncResult, System.IAsyncResult, new()
  where C : struct, IDisposable, System.IAsyncResult, IAsyncResult, IDisposable
  where D : class, A, A, B, B
{ }