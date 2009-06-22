using System;
using System.Runtime.InteropServices;

[Obsolete]
public delegate void MyDelegate<T1, T2>([In] int a, int b)
  where T1 : struct
  where T2 : new();