using System;
using System.Collections.Generic;

public interface A: IDisposable {}
public interface B: A, IEnumerable<int> {}
public class C {}
public interface D: A, B, C {}
public interface E : object { }
