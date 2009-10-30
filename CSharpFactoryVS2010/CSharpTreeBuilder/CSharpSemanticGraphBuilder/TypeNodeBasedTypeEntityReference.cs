using System;
using System.Collections.Generic;
using CSharpTreeBuilder.Ast;
using CSharpTreeBuilder.CSharpSemanticGraph;
using CSharpTreeBuilder.ProjectContent;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class represents a reference to a type entity, based on a type-or-namespace AST node.
  /// </summary>
  // ================================================================================================
  public sealed class TypeNodeBasedTypeEntityReference : SyntaxNodeBasedSemanticEntityReference<TypeEntity, TypeNode>
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeNodeBasedTypeEntityReference"/> class.
    /// </summary>
    /// <param name="syntaxNode">A syntax node that will be resolved to a semantic entity.</param>
    // ----------------------------------------------------------------------------------------------
    public TypeNodeBasedTypeEntityReference(TypeNode syntaxNode)
      : base(syntaxNode)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Implements the resolution logic.
    /// </summary>
    /// <param name="context">A semantic entity that is the context of the resolution.</param>
    /// <param name="semanticGraph">The semantic graph.</param>
    /// <param name="errorHandler">An object for error and warning reporting.</param>
    /// <returns>The resolved entity, or null if could not resolve.</returns>
    // ----------------------------------------------------------------------------------------------
    protected override TypeEntity GetResolvedEntity(
      SemanticEntity context, SemanticGraph semanticGraph, ICompilationErrorHandler errorHandler)
    {
      return GetTypeEntityByTypeNode(SyntaxNode, context, semanticGraph, errorHandler);
    }

    #region Private methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Finds or constructs the type entity denoted by a type AST node.
    /// </summary>
    /// <param name="typeNode">A type AST node.</param>
    /// <param name="resolutionContextEntity">The entity that is the context of the resolution.</param>
    /// <param name="semanticGraph">The semantic graph.</param>
    /// <param name="errorHandler">An object for error and warning reporting.</param>
    /// <returns>A TypeEntity, or null if not found.</returns>
    // ----------------------------------------------------------------------------------------------
    private static TypeEntity GetTypeEntityByTypeNode(TypeNode typeNode, SemanticEntity resolutionContextEntity,
      SemanticGraph semanticGraph, ICompilationErrorHandler errorHandler)
    {
      // First, try to resolve as built-in type.
      TypeEntity typeEntity = ResolveBuiltInTypeName(typeNode.TypeName, semanticGraph);

      // If not found, then continue with the resolution
      if (typeEntity == null)
      {
        var namespaceOrTypeNameResolver = new NamespaceOrTypeNameResolver(errorHandler, semanticGraph);
        
        // Resolve the underlying type.
        typeEntity = namespaceOrTypeNameResolver.ResolveToTypeEntity(typeNode.TypeName, resolutionContextEntity);

        // If no success then just return null. Errors were already signaled in NamespaceOrTypeNameResolver.
        if (typeEntity==null)
        {
          return null;
        }
      }

      // If the AST node has a nullable type indicating token, then create nullable type.
      if (typeNode.NullableToken != null)
      {
        typeEntity = ConstructedTypeHelper.GetConstructedGenericType(semanticGraph.NullableGenericTypeDefinition, 
          new List<TypeEntity>() {typeEntity});
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
