using System;
using System.Collections.Generic;
using System.Text;
using AliasName = System.Text.Encoding;
using SecondAlias = Microsoft.Win32;

namespace CSharpParserTest.TestFiles
{
  using System;
  using System.Collections;
  using System.Data;

  namespace Level1
  {
    using System.Xml;

    namespace Level2
    {
      using System.Data;
      using SysData = System.Data.Sql;
    }
  }

  namespace Level1.A
  {
    using System.Xml;

    namespace Level2.A
    {
      namespace Level3
      {
        
      }
    }
  }

  namespace Level1
  {
    using System.Xml;

    namespace Level2
    {
      using System.Data;
      using System.Data.Sql;
    }
  }
}

namespace OtherNameSpace
{
  namespace Level1
  {
    using System.Xml;

    namespace Level2
    {
      using System.Data;
      using System.Data.Sql;
    }
  }
}
