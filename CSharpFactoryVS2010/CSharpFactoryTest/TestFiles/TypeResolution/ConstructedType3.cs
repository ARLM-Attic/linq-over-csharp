public unsafe class MyClass
{
  private byte* a;
  private sbyte* b;
  private short* c;
  private ushort* d;
  private int* e;
  private uint* f;
  private long* g;
  private ulong* h;
  private char* i;
  private float* j;
  private double* k;
  private decimal* l;
  private bool* m;
  private System.IO.FileAccess* n;
  private bool*** o;
  private C* _Ok1;
  private B* _Err1;
  private string* _Err2;
  private D* _Err3;
  private E* _Ok2;
}

public unsafe class B {}

public unsafe struct C
{
  public bool* a;
  public int* b;
}

public unsafe struct D
{
  public bool* a;
  public string b;
}

public struct E {}