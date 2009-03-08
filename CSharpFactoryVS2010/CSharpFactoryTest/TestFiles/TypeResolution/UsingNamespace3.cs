extern alias EA1;
extern alias EA2;

using System.IO;
using System.Data;
using EA1::SqlClient;
using MISSING1::Something;

namespace MyNamespace
{
  extern alias EA1;
  extern alias EA2;

  using System.Collections;
  using EA1::SqlClient;
  using MISSING1::Something;
  using System.Xml;

  namespace MySubNamespace
  {
    extern alias EA1;
    extern alias EA2;

    using EA1::SqlClient;
    using MISSING1::Something;
    using System.Collections;
    using System.Xml;
    using System.Data;
  }
}