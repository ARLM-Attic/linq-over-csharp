using System.Diagnostics;

namespace Level1
{
  using System.IO;
  using System.ComponentModel;

  namespace Level2
  {
    namespace Level3
    {
      using System.Data;

      class A: FileStream
      {

        public A(string path, FileMode mode) : base(path, mode)
        {
        }
      }

      class B: DataSet {}

      class C: Process {}
    }
  }
}