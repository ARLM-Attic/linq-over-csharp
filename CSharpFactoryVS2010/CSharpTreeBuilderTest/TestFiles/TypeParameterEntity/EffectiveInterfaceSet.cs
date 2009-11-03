public class P<TP1, TP2>
  where TP1 : I3
  where TP2 : I4
{
  public class A<T1, T21, T22, T31, T32, T41, T42>
    // T1: If T has no secondary-constraints, its effective interface set is empty.
    // T2: If T has interface-type constraints but no type-parameter constraints,
    // its effective interface set is its set of interface-type constraints.
    where T21 : I1
    where T22 : I1, I2
    // T31, T32: If T has no interface-type constraints but has type-parameter constraints, 
    // its effective interface set is the union of the effective interface sets of its type-parameter constraints.
    where T31 : TP1
    where T32 : TP1, TP2
    // T41, T42: If T has both interface-type constraints and type-parameter constraints, 
    // its effective interface set is the union of its set of interface-type constraints 
    // and the effective interface sets of its type-parameter constraints.
    where T41 : I1, TP1
    where T42 : I1, I2, TP1, TP2
  {
  }
}

public interface I1
{}

public interface I2
{}

public interface I3
{ }

public interface I4
{ }
