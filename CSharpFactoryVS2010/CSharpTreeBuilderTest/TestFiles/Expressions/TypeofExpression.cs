using System;
using myAlias = System.Collections.Generic;

public class TypeofExpression 
{
  public void DummyMethod()
  {
    Type t1 = typeof(int);
    Type t2 = typeof(Generic<,>);
    Type t3 = typeof(Generic<int,int>);
    Type t4 = typeof(myAlias::IList<>);
    Type t5 = typeof(Generic<,>.EmbeddedGeneric<>);
    Type t6 = typeof(void);
  }
}

public class Generic<T1,T2>
{
  public class EmbeddedGeneric<T>
  {}
}
