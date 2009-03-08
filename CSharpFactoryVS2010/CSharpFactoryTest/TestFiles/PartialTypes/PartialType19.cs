using System.ComponentModel;
using System.Runtime.InteropServices;

[Description]
partial class A { }
[Guid("EFA410F9-9223-4fc5-9E6D-002A5D792F7B")]
partial class A { }

[Description]
partial class B { }
[Guid("EFA410F9-9223-4fc5-9E6D-002A5D792F7B")]
partial class B { }

class Master
{
  partial class A { }
  [Guid("EFA410F9-9223-4fc5-9E6D-002A5D792F7B")]
  partial class A { }

  [Description]
  partial class B { }
  [Guid("EFA410F9-9223-4fc5-9E6D-002A5D792F7B")]
  partial class B { }
}