class A
{
  E e1 = 0;     // success
  E? e2 = 0;    // success
  X<E> e3 = 0;  // fails, not enum, or nullable enum
  E e4 = 1;     // fails, not decimal zero
}

enum E { }

class X<T> { }
