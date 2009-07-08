using System.Collections.Generic;

class CollectionInitializer_Error
{
  void DummyMethod()
  {
    var a1 = new List<int>() {1, Capacity = 2};
  }
}

