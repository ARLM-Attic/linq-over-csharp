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

      // We have to collect all type arguments, because constructed generic types need their parents's type arguments as well.
      var typeArgumentNodes = GetTypeArgumentNodesFromTypeOrNamespaceNode(typeNode.TypeName);

      // If there are type arguments, but the found entity is not a generic type definition, then it's an error.
      if (typeArgumentNodes.Count > 0 && !(typeEntity is GenericCapableTypeEntity))
      {
        throw new ApplicationException(
          string.Format("Expected to find GenericCapableTypeEntity, but found '{0}'.", typeEntity.GetType()));
      }

      // If there are type arguments then we have to create a constructed generic type.
      if (typeArgumentNodes.Count > 0 && typeEntity is GenericCapableTypeEntity)
      {
        // Resolve all type arguments
        var typeArguments = new List<TypeEntity>();
        foreach (var typeArgumentSyntaxNode in typeArgumentNodes)
        {
          var typeArgumentRef = new TypeNodeBasedTypeEntityReference(typeArgumentSyntaxNode);
          var typeArgument = GetTypeEntityByTypeNode(typeArgumentRef.SyntaxNode, resolutionContextEntity, semanticGraph, errorHandler);
          if (typeArgument == null)
          {
            // No need to signal error here, because the GetTypeEntityByTypeOrNamespaceNode method already signaled it, just bail out.
            return null;
          }
          typeArguments.Add(typeArgument);
        }

        typeEntity = ConstructedTypeHelper.GetConstructedGenericType(typeEntity as GenericCapableTypeEntity, typeArguments);
        if (typeEntity == null)
        {
          throw new ApplicationException("GetConstructedGenericType returned null.");
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
        typeEntity = ConstructedTypeHelper.GetConstructedArrayType(typeEntity, rankSpecifier.Rank, semanticGraph);
      }

      return typeEntity;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Extracts the list of type arguments from a namespace-or-type-name AST node.
    /// </summary>
    /// <param name="namespaceOrTypeNameNode">A namespace-or-type-name node</param>
    /// <returns>The list of type arguments.</returns>
    // ----------------------------------------------------------------------------------------------
    private static TypeNodeCollection GetTypeArgumentNodesFromTypeOrNamespaceNode(NamespaceOrTypeNameNode namespaceOrTypeNameNode)
    {
      var typeArgumentNodes = new TypeNodeCollection();

      foreach (var typeTag in namespaceOrTypeNameNode.TypeTags)
      {
        if (typeTag.HasTypeArguments)
        {
          foreach (var argument in typeTag.Arguments)
          {
            typeArgumentNodes.Add(argument);
          }
        }
      }

      return typeArgumentNodes;
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

      System.Type aliasedType = null;

      switch (typeNameNode.TypeTags[0].Identifier)
      {
        case "bool":
          aliasedType = typeof(bool);
          break;
        case "byte":
          aliasedType = typeof(byte);
          break;
        case "char":
          aliasedType = typeof(char);
          break;
        case "decimal":
          aliasedType = typeof(decimal);
          break;
        case "double":
          aliasedType = typeof(double);
          break;
        case "float":
          aliasedType = typeof(float);
          break;
        case "int":
          aliasedType = typeof(int);
          break;
        case "long":
          aliasedType = typeof(long);
          break;
        case "object":
          aliasedType = typeof(object);
          break;
        case "sbyte":
          aliasedType = typeof(sbyte);
          break;
        case "short":
          aliasedType = typeof(short);
          break;
        case "string":
          aliasedType = typeof(string);
          break;
        case "uint":
          aliasedType = typeof(uint);
          break;
        case "ulong":
          aliasedType = typeof(ulong);
          break;
        case "ushort":
          aliasedType = typeof(ushort);
          break;
        case "void":
          aliasedType = typeof(void);
          break;
        default:
          // Not a builtin type
          break;
      }

      if (aliasedType != null)
      {
        return semanticGraph.GetEntityByMetadataObject(aliasedType) as TypeEntity;
      }

      return null;
    }

    #endregion
  }
}
