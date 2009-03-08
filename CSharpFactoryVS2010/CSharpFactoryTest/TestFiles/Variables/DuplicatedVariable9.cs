using System.Data.SqlClient;

public class MyClass
{
  public MyClass()
  {
    using (SqlConnection conn1 = new SqlConnection())
    {
      int conn1 = 1;      
    }
    using (SqlConnection conn1 = new SqlConnection(), conn2 = new SqlConnection())
    {
      int conn1 = 1;
      SqlConnection conn2 = null;
    }
    long conn2 = 0;
  }
}