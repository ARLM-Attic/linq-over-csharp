using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpParserTest.TestFiles
{
  interface ExplicitIntf
  {
    int this[int index1, string index2] { get; }
  }

  public abstract class PropertyDeclarations: ExplicitIntf
  {
    private int _IntProp1;
    private int _IntProp2;
    private string _StringProp3;

    public int IntProp1
    {
      get { return _IntProp1; }
      protected set { _IntProp1 = value; }
    }

    public int IntProp2
    {
      get { return _IntProp2; }
      private set { _IntProp2 = value; }
    }

    public string StringProp3
    {
      set { _StringProp3 = value; }
    }

    public string this[int index1, string index2]
    {
      get { return String.Empty; }
    }

    #region ExplicitIntf Members

    int ExplicitIntf.this[int index1, string index2]
    {
      get { throw new Exception("The method or operation is not implemented."); }
    }

    #endregion

    public abstract int IntProp3
    { 
      get;
      set;
    }

  }
}
