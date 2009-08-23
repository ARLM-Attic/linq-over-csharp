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
      TypeEntity typeEntity = FindBuiltInTypeByTypeNode(typeNode, semanticGraph);

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
        typeEntity = ConstructedTypeHelper.GetConstructedNullableType(typeEntity, semanticGraph);
      }

      // If the AST node has pointer token(s), then create pointer type(s).
      bool isFirstStar = true;
      foreach (var pointerToken in typeNode.PointerTokens)
      {
        // If it's pointer to unknown type (void*) then the first '*' should be swallowed, because that's part of 'void*'
        if (typeEntity is PointerToUnknownTypeEntity && isFirstStar)
        {
          isFirstStar = false;
        }
        else
        {
          typeEntity = ConstructedTypeHelper.GetConstructedPointerType(typeEntity);
        }
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
    /// Tries to find a built-in type (or void*) that matches a given type AST node.
    /// </summary>
    /// <param name="typeNode">A type AST node.</param>
    /// <param name="semanticGraph">The semantic graph.</param>
    /// <returns>A TypeEntity, or null if not found.</returns>
    // ----------------------------------------------------------------------------------------------
    private static TypeEntity FindBuiltInTypeByTypeNode(TypeNode typeNode, SemanticGraph semanticGraph)
    {
      // If the name is not a one-part-long name, then not a builtin type
      if (typeNode.TypeName.TypeTags.Count != 1)
      {
        return null;
      }

      TypeEntity typeEntity = null;

      string identifier = typeNode.TypeName.TypeTags[0].Identifier;

      // Resolve 'void*'
      if (identifier == "void" && typeNode.PointerTokens.Count > 0)
      {
        typeEntity = semanticGraph.PointerToUnknownType;
      }
      else
      {
        // Resolve built-in types
        typeEntity = semanticGraph.GetBuiltInTypeByName(identifier);
      }

      return typeEntity;
    }

    #endregion
  }
}
