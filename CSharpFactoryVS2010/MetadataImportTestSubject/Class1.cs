﻿// The resulting assembly from this project is used as a test subject in metadata import unit tests.

public class Class0
{
  int A;

  int B { get { return A; } set { A = value; } }

  int C { get; set; }
}

namespace A.B
{
  public class Class1
  {
    public class SubClass1
    {}
  }

  public enum Enum1
  {
  }

  public struct Struct1
  {
    
  }

  public interface IInterface1
  {
    
  }

  public delegate void Delegate1();

  public class Generic1<T1,T2>
  {}
}