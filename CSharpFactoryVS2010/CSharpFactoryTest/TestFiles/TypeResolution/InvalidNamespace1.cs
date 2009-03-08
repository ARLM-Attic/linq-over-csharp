namespace MyNamespace
{
  namespace MyNamespace
  {
    // --- This class is valid
    class MyNamespace {}
  }

  // --- This definition is invalid, because namespace and type 
  // --- cannot have the same name within a namespace.
  class MyNamespace {}
}