using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpParserTest.TestFiles
{
  internal delegate void NewDelegate();

  class EventDeclarations
  {
    private delegate void MyEvent1<T, U>(T t, U u);
    protected delegate void MyEvent2(int a,string b);
    public delegate T MyEvent3<T>(T t, string b);

    public new event NewDelegate _Event1, _Event2;
    protected volatile int _IntField1, _IntField2 = 23;
    private event MyEvent1<List<int>, List<string>> _Event3;
  }
}
