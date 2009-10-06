class A
{
  static int? i1 = 1;
  static float? i2 = 2;
  static double? i3 = i2;
  static float? i4 = i3;  // fails
}
