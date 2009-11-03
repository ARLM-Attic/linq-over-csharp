public class P<TP1, TP2>
  where TP1 : B
  where TP2 : D
{
  public class A<T1, T2, T3, T41, T42, T51, T52, T6>
    // T1: If T has no primary constraints or type parameter constraints, its effective base class is object.
    // T2: If T has the value type constraint, its effective base class is System.ValueType.
    where T2 : struct
    // T3: If T has a class-type constraint C but no type-parameter constraints, its effective base class is C.
    where T3 : C
    // T41, T42: If T has no class-type constraint but has one (T41) or more (T42) type-parameter constraints, 
    // its effective base class is the most encompassed type (§6.4.2) 
    // in the set of effective base classes of its type-parameter constraints. 
    where T41 : TP1
    where T42 : TP1, TP2
    // T51, T52: If T has both a class-type constraint and one or more type-parameter constraints, 
    // its effective base class is the most encompassed type (§6.4.2) 
    // in the set consisting of the class-type constraint of T and the effective base classes of its type-parameter constraints.
    where T51 : C, TP1
    where T52 : C, TP1, TP2
    // T6: If T has the reference type constraint but no class-type constraints, its effective base class is object.
    where T6 : class
  {
  }
}

public class B
{}

public class C : B
{}

public class D : C
{}
