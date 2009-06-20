using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// A fluent interface for creating TypeTagNode objects, implemented with extension methods.
  /// </summary>
  // ================================================================================================
  public static class TypeTagNodeFluentInterface
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds type arguments to a TypeTagNode.
    /// </summary>
    /// <param name="node">A TypeTagNode node</param>
    /// <param name="types">Any number of types.</param>
    /// <returns>The TypeTagNode parameter is returned to enable method chaining.</returns>
    // ----------------------------------------------------------------------------------------------
    public static TypeTagNode TypeArg(this TypeTagNode node, params TypeOrNamespaceNode[] types)
    {
      foreach (var type in types)
      {
        node.AddArgument(type);
      }
      return node;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Adds type arguments to a TypeTagNode.
    /// </summary>
    /// <param name="node">A TypeTagNode node</param>
    /// <param name="typeNames">Any number of typenames.</param>
    /// <returns>The TypeTagNode parameter is returned to enable method chaining.</returns>
    // ----------------------------------------------------------------------------------------------
    public static TypeTagNode TypeArg(this TypeTagNode node, params string[] typeNames)
    {
      foreach (var typeName in typeNames)
      {
        var typeOrNamespaceNode = new TypeOrNamespaceNode();
        typeOrNamespaceNode.AddTypeTag(new TypeTagNode(typeName));
        node.TypeArg(typeOrNamespaceNode);
      }
      return node;
    }
  }
}
