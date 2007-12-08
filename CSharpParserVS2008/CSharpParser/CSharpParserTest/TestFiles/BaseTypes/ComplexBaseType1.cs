using System.Collections.Generic;

public class A<U, V>: Dictionary<List<U>, Dictionary<int, V>> {}
public class B<U, V> : Dictionary<List<U>, Dictionary<int, V>> { }
public class C<U, V> : B<List<U>, Dictionary<int, V>> { }
