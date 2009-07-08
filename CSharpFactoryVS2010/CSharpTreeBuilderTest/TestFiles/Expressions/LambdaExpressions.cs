using System;
using System.Linq.Expressions;

class LambdaExpressions
{
  void DummyMethod()
  {
    // Lamba expression with block body, no parameters
    Dvoid d1 = () => {};

    // Lambda expression with expression body, no parameters
    Expression<Func<int>> d2 = () => 1;

    // Lambda expression with block body, explicit-anonymous-function-signature
    Dint d3 = (int i, ref int j, out int k) => { k = 1; return i; };

    // Lambda expression with expression body, implicit-anonymous-function-signature
    Expression<Func<int,int,int>> d4 = (i,j) =>  i+j;

    // Lambda expression are right-associative
    Expression<Func<Func<int>>> d5 = () => () => 2;
  }

  private delegate void Dvoid();
  private delegate int Dint(int i, ref int j, out int k);
}