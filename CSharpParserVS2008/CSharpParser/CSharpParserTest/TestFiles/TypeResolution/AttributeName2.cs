using System;

[AttributeUsage(AttributeTargets.All)]
public class X : Attribute { }

[AttributeUsage(AttributeTargets.All)]
public class XAttribute : Attribute { }

[X] // error: ambiguity
class Class1 { }

[XAttribute] // refers to XAttribute
class Class2 { }

[@X] // refers to X
class Class3 { }

[@XAttribute] // refers to XAttribute
class Class4 { }
