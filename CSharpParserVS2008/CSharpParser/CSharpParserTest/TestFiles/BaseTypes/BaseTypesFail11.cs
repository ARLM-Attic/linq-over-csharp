using System;

interface A: IDisposable, ICloneable, IDisposable, ICloneable {}
class B: ICloneable, IDisposable, ICloneable {}