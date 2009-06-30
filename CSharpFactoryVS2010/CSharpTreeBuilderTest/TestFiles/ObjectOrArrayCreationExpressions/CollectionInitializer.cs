using System.Collections.Generic;

class CollectionInitializer
{
  void DummyMethod()
  {
    var a1 = new List<int>() {1, 2*3};
    var a2 = new Dictionary<int,string> {{4,"a"}, {5,"b"}};
  }
}

