using System;
using System.Collections.Generic;
using CSharpTreeBuilder.Ast;
using CSharpTreeBuilder.CSharpSemanticGraph;
using CSharpTreeBuilder.ProjectContent;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class represents a resolver that resolves a SimpleNameNode to an ExpressionResult.
  /// </summary>
  /// <remarks>
  /// This class implements the resolution logic for simple names, as described in the spec §7.5.2
  /// </remarks>
  // ================================================================================================
  public sealed class SimpleNameNodeResolver : SyntaxNodeToExpressionResultResolver<SimpleNameNode>
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="SimpleNameNodeResolver"/> class.
    /// </summary>
    /// <param name="syntaxNode">The syntax node to be resolved.</param>
    // ----------------------------------------------------------------------------------------------
    public SimpleNameNodeResolver(SimpleNameNode syntaxNode)
      : base(syntaxNode)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="SimpleNameNodeResolver"/> class 
    /// by constructing it from a template instance.
    /// </summary>
    /// <param name="template">The template for the new instance.</param>
    /// <param name="typeParameterMap">The type parameter map of the new instance.</param>
    // ----------------------------------------------------------------------------------------------
    private SimpleNameNodeResolver(SimpleNameNodeResolver template, TypeParameterMap typeParameterMap)
      :base(template, typeParameterMap)
    {
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new resolver.
    /// </summary>
    /// <param name="typeParameterMap">A collection of type parameters and associated type arguments.</param>
    /// <returns>
    /// A new resolver constructed from this resolver using the specified type parameter map.
    /// </returns>
    // ----------------------------------------------------------------------------------------------
    protected override Resolver<ExpressionResult> ConstructNew(TypeParameterMap typeParameterMap)
    {
      return new SimpleNameNodeResolver(this, typeParameterMap);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Implements the resolution logic.
    /// </summary>
    /// <param name="context">A semantic entity that is the context of the resolution.</param>
    /// <param name="errorHandler">The object used for error reporting.</param>
    /// <returns>The resolved entity, or null if could not resolve.</returns>
    // ----------------------------------------------------------------------------------------------
    protected override ExpressionResult InternalGetResolvedEntity(ISemanticEntity context, ICompilationErrorHandler errorHandler)
    {
      // A simple-name consists of an identifier, optionally followed by a type argument list:
      //   simple-name:
      //     identifier   type-argument-listopt
      // A simple-name is either of the form I or of the form I<A1, ..., AK>, where I is a single identifier 
      // and <A1, ..., AK> is an optional type-argument-list. When no type-argument-list is specified, consider K to be zero. 
      // The simple-name is evaluated [and classified as follows]:

      // First lets resolve all type arguments.
      var typeArguments = new List<TypeEntity>();
      foreach (var typeNode in SyntaxNode.Arguments)
      {
        var typeNodeResolver = new TypeNodeToTypeEntityResolver(typeNode);
        typeNodeResolver.Resolve(context, errorHandler);
        if (typeNodeResolver.Target != null)
        {
          typeArguments.Add(typeNodeResolver.Target);
        }
      }

      // (Try to resolve to a local variable, parameter or constant.) 

      var expressionResult = ResolveAtLocalDeclarationSpace(SyntaxNode, context);
      if (expressionResult != null)
      {
        return expressionResult;
      }
      
      // (Try to resolve to a method type parameter.)

      var typeParameterEntity = ResolveToMethodTypeParameter(SyntaxNode, context);
      if (typeParameterEntity != null)
      {
        return new TypeExpressionResult(typeParameterEntity);
      }

      // (Try to lookup in type declarations as a type parameter or type member.)

      expressionResult = ResolveAtTypeDeclarationLevel(SyntaxNode, context, errorHandler);
      if (expressionResult != null)
      {
        return expressionResult;
      }

      // (Try to lookup in namespace declarations as a type or a namespace.)

      var typeTagNode = new TypeTagNode(SyntaxNode.IdentifierToken,
                                        SyntaxNode.Arguments.Count > 0 ? SyntaxNode.Arguments : null);
      
      NamespaceOrTypeEntity namespaceOrTypeEntity = null;

      try
      {
        namespaceOrTypeEntity = 
          NamespaceOrTypeNameResolutionAlgorithm.ResolveSingleTypeTagInNamespaces(typeTagNode, context, context, typeArguments, errorHandler);
      }
      catch (NamespaceOrTypeNameNotResolvedException)
      {
        // We swallow this exception to continue simple name resolution.
      }

      if (namespaceOrTypeEntity != null)
      {
        if (namespaceOrTypeEntity is NamespaceEntity)
        {
          return new NamespaceExpressionResult(namespaceOrTypeEntity as NamespaceEntity);
        }
        return new TypeExpressionResult(namespaceOrTypeEntity as TypeEntity);
      }

      // Otherwise, the simple-name is undefined and a compile-time error occurs.
      throw new SimpleNameUndefinedException(SyntaxNode.ToString());
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Translates an exception thrown during namespace-or-type-name resolution to an error that
    /// can be reported to an ICompilationErrorHandler.
    /// </summary>
    /// <param name="e">An exception object.</param>
    /// <param name="errorHandler">The object used for error reporting.</param>
    /// <remarks>If can't translate the exception then delegates it to its base.</remarks>
    // ----------------------------------------------------------------------------------------------
    protected override void TranslateExceptionToError(ResolverException e, ICompilationErrorHandler errorHandler)
    {
      var errorToken = SyntaxNode.StartToken;

      if (e is SimpleNameUndefinedException)
      {
        errorHandler.Error("CS0103", errorToken, "The name '{0}' does not exist in the current context",
                           (e as SimpleNameUndefinedException).SimpleName);
      }
      else if (e is StaticMemberExpectedException)
      {
        errorHandler.Error("CS0120", errorToken,
                           string.Format("An object reference is required for the non-static field, method, or property '{0}'",
                                         (e as StaticMemberExpectedException).MemberEntity));
      }
      else
      {
        base.TranslateExceptionToError(e, errorHandler);
      }
    }

    #region Private methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tries to resolve a simple name to a method type parameter.
    /// </summary>
    /// <param name="simpleNameNode">The simple name to resolve.</param>
    /// <param name="context">The resolution context entity.</param>
    /// <returns>A type parameter entity, or null if could not resolve.</returns>
    // ----------------------------------------------------------------------------------------------
    private static ExpressionResult ResolveAtLocalDeclarationSpace(
      SimpleNameNode simpleNameNode,
      ISemanticEntity context)
    {
      // If K is zero ...
      if (!simpleNameNode.HasTypeArguments)
      {
        // and the simple-name appears within a block [or any IDefinesLocalVariableDeclarationSpace entity]
        var enclosingBlock = context.GetEnclosing<IDefinesLocalVariableDeclarationSpace>();

        while (enclosingBlock != null)
        {
          // ... and if the block’s (or an enclosing block’s) local variable declaration space (§3.3) 
          // contains a local variable, parameter or constant with name I, 
          var namedEntity = enclosingBlock.GetDeclaredEntityByName(simpleNameNode.Identifier);
          if (namedEntity != null && namedEntity is IVariableEntity)
          {
            // then the simple-name refers to that local variable, parameter or constant 
            // and is classified as a variable or value.
            return new ValueExpressionResult((namedEntity as IVariableEntity).Type);
          }

          enclosingBlock = enclosingBlock.Parent.GetEnclosing<IDefinesLocalVariableDeclarationSpace>();
        }
      }

      return null;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tries to resolve a simple name to a method type parameter.
    /// </summary>
    /// <param name="simpleNameNode">The simple name to resolve.</param>
    /// <param name="context">The resolution context entity.</param>
    /// <returns>A type parameter entity, or null if could not resolve.</returns>
    // ----------------------------------------------------------------------------------------------
    private static TypeParameterEntity ResolveToMethodTypeParameter(
      SimpleNameNode simpleNameNode,
      ISemanticEntity context)
    {
      // If K is zero ...
      if (!simpleNameNode.HasTypeArguments)
      {
        // ... and the simple-name appears within the body of a generic method declaration (§10.6) ...
        var genericMethodEntity = context.GetEnclosingGenericMethodDeclaration();
        if (genericMethodEntity != null)
        {
          // ... and if that declaration includes a type parameter (§10.1.3) with name I, ...
          var methodTypeParameter = genericMethodEntity.GetOwnTypeParameterByName(simpleNameNode.Identifier);
          if (methodTypeParameter != null)
          {
            // ... then the simple-name refers to that type parameter.
            return methodTypeParameter;
          }
        }
      }

      return null;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tries to resolve a simple name at the type declaration level, to a type parameter or type member.
    /// </summary>
    /// <param name="simpleNameNode">The simple name to resolve.</param>
    /// <param name="context">The resolution context entity.</param>
    /// <param name="errorHandler">The object used for error reporting.</param>
    /// <returns>A semantic entity, or null if could not resolve.</returns>
    // ----------------------------------------------------------------------------------------------
    private static ExpressionResult ResolveAtTypeDeclarationLevel(
      SimpleNameNode simpleNameNode, 
      ISemanticEntity context,
      ICompilationErrorHandler errorHandler)
    {
      // Otherwise, for each instance type T (§10.3.1), 
      // starting with the instance type of the immediately enclosing type declaration and 
      // continuing with the instance type of each enclosing class or struct declaration (if any):
      var T = context.GetEnclosing<TypeEntity>();
      var immediatelyEnclosingType = T;

      while (T != null)
      {
        // If K is zero ...
        if (!simpleNameNode.HasTypeArguments)
        {
          // ... and the declaration of T includes a type parameter with name I, ...
          var foundTypeParameterEntity = (T is GenericCapableTypeEntity)
            ? (T as GenericCapableTypeEntity).GetOwnTypeParameterByName(simpleNameNode.Identifier)
            : null;

          if (foundTypeParameterEntity != null)
          {
            // then the simple-name refers to that type parameter.
            return new TypeExpressionResult(foundTypeParameterEntity);
          }
        }

        // Otherwise, if a member lookup (§7.3) of I in T with K type arguments produces a match:
        var I = simpleNameNode.Identifier;
        var A = NamespaceOrTypeNameResolutionAlgorithm.ResolveTypeArguments(simpleNameNode.Arguments, context, errorHandler);
        var K = simpleNameNode.Arguments.Count;
        var memberLookupResult = MemberLookup.Lookup(I, K, T, context, false);

        if (!memberLookupResult.IsEmpty)
        {
          // If T is the instance type of the immediately enclosing class or struct type 
          // and the lookup identifies one or more methods,
          if (T == immediatelyEnclosingType && memberLookupResult.IsMethodGroup)
          {
            // the result is a method group with an associated instance expression of this. 
            return new MethodGroupExpressionResult(memberLookupResult.MethodGroup, new ValueExpressionResult(T));

            // If a type argument list was specified, it is used in calling a generic method (§7.5.5.1).
          }

          // Otherwise, if T is the instance type of the immediately enclosing class or struct type, 
          // if the lookup identifies an instance member, ...
          if (T == immediatelyEnclosingType
            && memberLookupResult.IsSingleMember && memberLookupResult.SingleMember.IsInstanceMember)
          {
            // ... and if the reference occurs within the block of an instance constructor, 
            // an instance method, or an instance accessor, ...
            var enclosingMember = context.GetEnclosing<NonTypeMemberEntity>();
            if (enclosingMember != null && enclosingMember.IsInstanceMember)
            {
              // ... the result is the same as a member access (§7.5.4) of the form this.I. 
              // This can only happen when K is zero.
              if (K == 0)
              {
                // Create a temporal this access entity and evaluate it, just to be able to use it in ResolveMemberAccess call.
                var thisAccessEntity = new ThisAccessExpressionEntity {Parent = context};
                thisAccessEntity.Evaluate(errorHandler);

                return MemberAccessNodeResolver.ResolveMemberAccess(thisAccessEntity.ExpressionResult, I, A, K, context, errorHandler);
              }

              throw new ApplicationException(
                string.Format("At simple name node '{0}' expected 0 type arguments but found '{1}'.", simpleNameNode, K));
            }
          }

          // Otherwise, the result is the same as a member access (§7.5.4) of the form T.I or T.I<A1, ..., AK>. 

          // In this case, it is a compile-time error for the simple-name to refer to an instance member.
          if (memberLookupResult.SingleMember.IsInstanceMember)
          {
            throw new StaticMemberExpectedException(memberLookupResult.SingleMember);
          }

          return MemberAccessNodeResolver.ResolveMemberAccess(new TypeExpressionResult(T), I, A, K, context, errorHandler);
        }

        // "... and continuing with the instance type of each enclosing class or struct declaration (if any):"
        T = T.Parent.GetEnclosing<TypeEntity>();
      }

      return null;
    }

    #endregion

  }
}
