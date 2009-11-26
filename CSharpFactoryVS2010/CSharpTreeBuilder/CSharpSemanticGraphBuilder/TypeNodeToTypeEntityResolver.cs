using CSharpTreeBuilder.Ast;
using CSharpTreeBuilder.CSharpSemanticGraph;
using CSharpTreeBuilder.ProjectContent;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class represents a resolver that resolves a TypeNode to a TypeEntity.
  /// </summary>
  // ================================================================================================
  public sealed class TypeNodeToTypeEntityResolver : SyntaxNodeResolver<TypeEntity, TypeNode>
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeNodeToTypeEntityResolver"/> class.
    /// </summary>
    /// <param name="syntaxNode">A syntax node that be resolved.</param>
    // ----------------------------------------------------------------------------------------------
    public TypeNodeToTypeEntityResolver(TypeNode syntaxNode)
      : base(syntaxNode)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Implements the resolution logic.
    /// </summary>
    /// <param name="context">A semantic entity that is the context of the resolution.</param>
    /// <param name="errorHandler">The object used for error reporting.</param>
    /// <returns>The resolved entity, or null if could not resolve.</returns>
    // ----------------------------------------------------------------------------------------------
    protected override TypeEntity GetResolvedEntity(ISemanticEntity context, ICompilationErrorHandler errorHandler)
    {
      return GetTypeEntityByTypeNode(SyntaxNode, context, errorHandler);
    }

    #region Private methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Finds or constructs the type entity denoted by a type AST node.
    /// </summary>
    /// <param name="typeNode">A type AST node.</param>
    /// <param name="resolutionContextEntity">The entity that is the context of the resolution.</param>
    /// <param name="errorHandler">An object for error and warning reporting.</param>
    /// <returns>A TypeEntity, or null if not found.</returns>
    // ----------------------------------------------------------------------------------------------
    private static TypeEntity GetTypeEntityByTypeNode(
      TypeNode typeNode, 
      ISemanticEntity resolutionContextEntity,
      ICompilationErrorHandler errorHandler)
    {
      var semanticGraph = resolutionContextEntity.SemanticGraph;

      // First, try to resolve as built-in type.
      TypeEntity typeEntity = ResolveBuiltInTypeName(typeNode.TypeName, semanticGraph);

      // If not found, then continue with the resolution
      if (typeEntity == null)
      {
        // Resolve the underlying type.
        var namespaceOrTypeNameResolver = new NamespaceOrTypeNameNodeToTypeEntityResolver(typeNode.TypeName);
        namespaceOrTypeNameResolver.Resolve(resolutionContextEntity, errorHandler);
        typeEntity = namespaceOrTypeNameResolver.Target;

        // If no success then just return null. Errors were already signaled in NamespaceOrTypeNameResolver.
        if (typeEntity == null)
        {
          return null;
        }
      }

      // If the AST node has a nullable type indicating token, then create nullable type.
      if (typeNode.NullableToken != null)
      {
        var typeParameterMap = new TypeParameterMap(semanticGraph.NullableGenericTypeDefinition.OwnTypeParameters, new[] {typeEntity});
        typeEntity = semanticGraph.NullableGenericTypeDefinition.GetGenericClone(typeParameterMap) as TypeEntity;
      }

      // If the AST node has pointer token(s), then create pointer type(s).
      foreach (var pointerToken in typeNode.PointerTokens)
      {
        typeEntity = ConstructedTypeHelper.GetConstructedPointerType(typeEntity);
      }

      // If the AST node has rank specifier(s), then create array type(s).
      foreach (var rankSpecifier in typeNode.RankSpecifiers)
      {
        typeEntity = ConstructedTypeHelper.GetConstructedArrayType(typeEntity, rankSpecifier.Rank);
      }

      return typeEntity;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tries to resolve a namespace-or-type-name AST node to a builtin type.
    /// </summary>
    /// <param name="typeNameNode">A namespace-or-type-name AST node.</param>
    /// <param name="semanticGraph">The semantic graph.</param>
    /// <returns>A TypeEntity, or null if not found.</returns>
    // ----------------------------------------------------------------------------------------------
    private static TypeEntity ResolveBuiltInTypeName(NamespaceOrTypeNameNode typeNameNode, SemanticGraph semanticGraph)
    {
      // If the name is not a one-part-long name, then not a builtin type
      if (typeNameNode.TypeTags.Count != 1)
      {
        return null;
      }

      System.Type type = null;

      switch (typeNameNode.TypeTags[0].Identifier)
      {
        case "bool":
          type = typeof(bool);
          break;
        case "byte":
          type = typeof(byte);
          break;
        case "char":
          type = typeof(char);
          break;
        case "decimal":
          type = typeof(decimal);
          break;
        case "double":
          type = typeof(double);
          break;
        case "float":
          type = typeof(float);
          break;
        case "int":
          type = typeof(int);
          break;
        case "long":
          type = typeof(long);
          break;
        case "object":
          type = typeof(object);
          break;
        case "sbyte":
          type = typeof(sbyte);
          break;
        case "short":
          type = typeof(short);
          break;
        case "string":
          type = typeof(string);
          break;
        case "uint":
          type = typeof(uint);
          break;
        case "ulong":
          type = typeof(ulong);
          break;
        case "ushort":
          type = typeof(ushort);
          break;
        case "void":
          type = typeof(void);
          break;
        default:
          // Not a builtin type
          break;
      }

      if (type != null)
      {
        return semanticGraph.GetEntityByMetadataObject(type) as TypeEntity;
      }

      return null;
    }

    #endregion
  }
}
