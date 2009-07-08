using System.Linq;

class QueryExpressions
{
  void DummyMethod()
  {
    // simplest query with select: from-clause select-clause
    var a1 = from i1 in array
             select i1;

    // simplest query with group: from-clause group-clause
    var a2 = from int i2 in array
             group i2 by -i2;

    // query with all possible clauses 
    var a3 = from i in array
             from int j in array
             let k = j
             where true
             join l in array on i equals l
             join int p in array on i equals p
             join n in array on i equals n into o
             join int q in array on i equals q into r
             orderby k ascending, j descending
             select i
             into m
               select m;
  }

  private int[] array = new int[] { 1 };
}