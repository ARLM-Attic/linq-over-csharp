using System.Collections.Generic;
using System;
using System.Linq;
using System.Linq.Expressions;

unsafe class ExpressionVisitorTest : ExpressionVisitorTestBase
{
  void DummyMethod()
  {
    // The collection initializer below is used to list all kinds of expressions
    var a = new List<object>()
              {
                // array-creation-expression
                new int[1,2] {{1, 1}},
                // true-literal
                true,
                //false-literal
                false,
                // char-literal
                'a',
                // decimal-literal
                1M,
                // double-literal
                1D,
                // int32-literal
                1,
                // int64-literal
                1L,
                // null-literal
                null,
                // single-literal
                1F,
                // string-literal
                "a",
                // uint32-literal
                1U,
                // uint64-literal
                1UL,
                // simple-name
                myList,
                // parenthesized-expression
                (1),
                // primary-expression-member-access
                myList.Capacity,
                // pointer-member-access (inside an invocation-expression)
                pp->GetType(),
                // predefined-type-member-access
                int.MinValue,
                // qualified-alias-member-access
                global::ExpressionVisitorTest.myStaticList,
                // invocation-expression
                GenericMethod<int>(1, 1),
                // element-access
                myArray[1, 1],
                // this-access
                this,
                // base-member-access
                base.baseField,
                // base-element-access
                base[1, 1],
                // post-increment-expression
                p++,
                // post-decrement-expression
                p--,
                // object-creation-expression (with collection initializer)
                new Dictionary<int, int> {{1, 1}, {1, 1}},
                // object-creation-expression (with argument-list and object-initializer
                new ExpressionVisitorTest(1, 1) {p = 1, baseField = 1},
                // anonymous-object-creation-expression
                new {p, myList.Capacity, base.baseField, A = 1},
                // typeof-expression (with unbound typename)
                typeof (List<>),
                // sizeof-expression
                sizeof (int),
                // checked-expression
                checked (1),
                // unchecked-expression
                unchecked (1),
                // default-value-expression
                default(int),
                // anonymous-method-expression (wrapped in a cast)
                (Func<int, int, int>) delegate(int i1, int i2) { return 1; },
                // lambda-expression (with implicit signature, expression body) (wrapped in a cast + parens)
                (Expression<Func<int, int, int>>) ((x, y) => x + y),
                // lambda-expression (with explicit signature, block body) (wrapped in a cast + parens)
                (Func<int, int, int>) ((int i1, int i2) => { return 1; }),
                // unary-expression
                -1,
                // pre-increment-expression
                ++p,
                // pre-decrement-expression
                --p,
                // cast-expression
                (int) 1,
                // binary-expression
                1 + 1,
                // type-testing-expression
                1 is int,
                // conditional-expression
                true ? 1 : 2,
                // assignment (wrapped in parens, otherwise assignment is not permitted here)
                (p = 1),
                // query-expression
                from int i in myList
                from int j in myList
                let k = j
                where true
                join l in myList on i equals l
                join n in myList on i equals n into o
                orderby k ascending , j descending
                select i
                into m
                  group m by 0
              };
  }

  private ExpressionVisitorTest(int i1, int i2)
  {
  }
  
  private int p;

  private int* pp;

  private List<int> myList = new List<int>();

  private static List<int> myStaticList = new List<int>();

  private int[,] myArray = new int[2,2];

  private int GenericMethod<T>(int i1, int i2)
  {
    return 1;
  }
}

class ExpressionVisitorTestBase
{
  protected int baseField;

  public int this[int i, int j]
  {
    get { return 0; }
  }
}
