using System;
using System.IO;
using System.Collections.Generic;

class MyClass
{
  int MyMethod()
  {
    // method body is a block statement

    // empty-statement
    label1:
    ;

    // local-variable-declaration
    label2:
    int a1, a2 = 0;
    var v1 = 0;

    // local-constant-declaration
    label3:
    const int c1 = 0, c2 = 0;

    // expression-statement
    label4:
    v1++;

    // if-statement
    label5:
    if (true)
    {
    }
    else
    {
    }

    // switch-statement
    label6:
    switch (v1)
    {
      case 0:
      case 1:
        // break-statement
        break;
      default:
        break;
    }

    // while-statement
    label7:
    while (false)
    {
    }

    // do-statement
    label8:
    do
    {
    } while (false);

    // for-statement (with local-variable-declaration)
    label9:
    for (int i = 0; false; i++)
      // continue-statement
      continue;

    // for-statement (with statement-expression-list)
    label10:
    for (v1++, v1++; false; v1++, v1++)
    {
    }

    // foreach-statement
    label11:
    foreach (int i in new int[] {0})
    {
    }

    // try-statement
    label12:
    try
    {
    }
    catch (Exception e)
    {
    }
    catch
    {
    }
    finally
    {
    }

    // checked-statement
    label13:
    checked
    {
    }

    // unchecked-statement
    label14:
    unchecked
    {
    }

    // lock-statement
    label15:
    lock (this)
    {
    }

    // using-statement
    label16:
    using (Stream s = null)
    {
    }

    // return-statement
    labl17:
    return 0;

    // goto-statement
    label18:
    goto label1;

    // throw-statement
    label19:
    throw new Exception();

    // unsafe-statement
    label20:
    unsafe
    {
      string s = "a";

      // fixed-statement
      label21:
      fixed (char* p1 = s, p2 = s)
      {
      }
    }
  }

  IEnumerable<int> IteratorMethod()
  {
  // yield-return-statement
  label_1:
    yield return 0;

    // yield-break-statement
  label_2:
    yield break;
  }
}

