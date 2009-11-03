// A type parameter is known to be a reference type 
// if it has the reference type constraint or its effective base class is not object or System.ValueType.

public class A<T1, T2, T3>
  where T1: class   // true
  where T2: struct  // false
  where T3: B       // true
{
}

public class B
{}
