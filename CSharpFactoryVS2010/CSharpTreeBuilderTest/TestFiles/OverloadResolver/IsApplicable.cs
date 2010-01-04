class A
{
  void M1(int p1)
  {}

  void M2(long p1)
  { }

  void M3(int p1, long p2, char p3)
  { }

  void M4(int p1, ref long p2, out char p3)
  {
    p3 = 'a';
  }

  void T()
  {
    int a1 = 1;
    long a2 = 2;
    char a3;

    M1(1);
    M2(1L);
    M3(1, 2, '3');
    
    M4(1, ref a2, out a3);
    M4(a1, ref a2, out a3);
    M4(a1, ref a1, out a3);  // error CS1503: Argument '2': cannot convert from 'ref int' to 'ref long'
    M4(a1, a2, out a3);      // error CS1620: Argument '2' must be passed with the 'ref' keyword
  }
}