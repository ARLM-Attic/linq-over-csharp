using System;
using System.ComponentModel;

public class MyClass
{
  private CancelEventHandler _Handler;
  private EventHandler<CancelEventArgs> _SecondHandler;

  public event CancelEventHandler Handler
  {
    add { _Handler += value; }
    remove { _Handler -= value; }
  }

  public event EventHandler<CancelEventArgs> Handler
  {
    add { _SecondHandler += value; }
    remove { _SecondHandler -= value; }
  }
}