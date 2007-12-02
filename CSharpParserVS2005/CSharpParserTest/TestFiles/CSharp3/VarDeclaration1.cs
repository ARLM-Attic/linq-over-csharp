using System;
using System.Data.SqlClient;

class MyClass
{
  public MyClass()
  {
    var minCount = 10;
    var maxCount = minCount + 100;
    for (var i = 0; i < maxCount; i++)
    {
      var result = i + 1;
      foreach (var ch in "Hello World")
      {
        Console.WriteLine(ch);
      }
    }
    using (var sqlConn = new SqlConnection())
    {
      Console.WriteLine(sqlConn.ToString());
    }
  }
}