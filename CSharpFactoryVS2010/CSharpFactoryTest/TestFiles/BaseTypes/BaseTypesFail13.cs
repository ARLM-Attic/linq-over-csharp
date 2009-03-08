using System;

public static class A {}
public class B {}
public class C: A {}

public static class D: B, IDisposable {}
public static class E: System.Object {}