// For class and struct
// error CS0542: 'A': member names cannot be the same as their enclosing type

class C
{
  int C;

  int C { get; set; }

  int C() { return 0; }
}

struct S
{
  int S;

  int S { get; set; }

  int S() { return 0; }
}

// The restriction does not apply to enums and interfaces

enum E
{
  E
}

interface I1
{
  int I1 { get; }
}

interface I2
{
  int I2();
}
