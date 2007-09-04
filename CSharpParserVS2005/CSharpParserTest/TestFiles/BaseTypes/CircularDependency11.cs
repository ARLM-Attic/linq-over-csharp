// --- There is no circular dependency even if it seems to be there!
class A
{
  class B : A { }
}