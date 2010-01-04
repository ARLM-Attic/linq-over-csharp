class A : Base1, I1
{
  static A a = null;

  static object i1 = a;   // From any reference-type to object.
  static Base1 i2 = a;    // From any class-type S to any class-type T, provided S is derived from T.
  static Base2 i3 = a;    // From any class-type S to any class-type T, provided S is derived from T. (indirectly)
  static NonBase i4 = a;  // Fails
  static I1 i5 = a;       // From any class-type S to any interface-type T, provided S implements T.
  static I2 i6 = a;       // From any class-type S to any interface-type T, provided S implements T. (indirectly)
  static I2 i7 = i5;      // From any interface-type S to any interface-type T, provided S is derived from T.
  static INotImplemented i8 = a;  // Fails

  static A[] arr1;
  static A[,] arr2;

  static A[] arr3 = arr1;           // Success
  static Base1[] arr4 = arr1;       // Success
  static Base2[] arr5 = arr1;       // Success
  static A[] arr6 = arr2;           // Fails, rank mismatch
  static System.Array arr7 = arr2;  // From any array-type to System.Array and the interfaces it implements.
  static System.ICloneable arr8 = arr2; // From any array-type to System.Array and the interfaces it implements.

  // - From a single-dimensional array type S[] to System.Collections.Generic.IList<T> and its base interfaces, 
  //   provided that there is an implicit identity or reference conversion from S to T.
  static System.Collections.Generic.IList<A> arr9 = arr1;
  static System.Collections.Generic.IList<Base1> arr10 = arr1;
  static System.Collections.Generic.IEnumerable<A> arr11 = arr1;
  static System.Collections.IEnumerable arr12 = arr1;
  static System.Collections.Generic.IEnumerable<Base1> arr13 = arr1;
  static System.Collections.Generic.IEnumerable<NonBase> arr14 = arr1; // error CS0029: Cannot implicitly convert type 'A[]' to 'System.Collections.Generic.IEnumerable<NonBase>'
}

class Base1 : Base2
{ }

class Base2
{ }

class NonBase
{ }

interface I1 : I2
{ }

interface I2
{ }

interface INotImplemented
{ }