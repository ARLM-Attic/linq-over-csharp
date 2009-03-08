using System;

public struct X: IDisposable
{
  public void Dispose()
  {
    throw new NotImplementedException();
  }
}

public struct Y: object {}

public class A {}

public struct Z: A {}