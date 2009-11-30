using System;
using System.Collections.Generic;
using CSharpTreeBuilder.Ast;
using CSharpTreeBuilder.CSharpSemanticGraph;
using CSharpTreeBuilder.ProjectContent;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class represents a resolver that resolves a MemberAccessNode to an ExpressionResult.
  /// </summary>
  /// <remarks>
  /// This class implements the member access resolution logic, as described in the spec §7.5.4
  /// </remarks>
  // ================================================================================================
  public sealed class MemberAccessNodeResolver : SyntaxNodeResolver<ExpressionResult, MemberAccessNode>
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="MemberAccessNodeResolver"/> class.
    /// </summary>
    /// <param name="syntaxNode">The syntax node to be resolved.</param>
    // ----------------------------------------------------------------------------------------------
    public MemberAccessNodeResolver(MemberAccessNode syntaxNode)
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
    protected override ExpressionResult GetResolvedEntity(ISemanticEntity context, ICompilationErrorHandler errorHandler)
    {
      // A member-access is either of the form E.I or of the form E.I<A1, ..., AK>, 
      // where E is a primary-expression, I is a single identifier and <A1, ..., AK> is an optional type-argument-list. 
      // When no type-argument-list is specified, consider K to be zero. 

      ExpressionResult E = null;
      var I = SyntaxNode.Identifier;
      var A = NamespaceOrTypeNameResolutionAlgorithm.ResolveTypeArguments(SyntaxNode.Arguments, context, errorHandler);
      var K = A.Count;

      // Determine the value of E based on the type of this member access expression.
      if (context is PrimaryMemberAccessExpressionEntity)
      {
        E = (context as PrimaryMemberAccessExpressionEntity).ChildExpression.ExpressionResult;
      }
      else if (context is QualifiedAliasMemberAccessExpressionEntity)
      {
        E = (context as QualifiedAliasMemberAccessExpressionEntity).QualifiedAliasMemberNodeResolver.Target;
      }
      else if (context is PredefinedTypeMemberAccessExpressionEntity)
      {
        E = new TypeExpressionResult((context as PredefinedTypeMemberAccessExpressionEntity).PredefinedTypeEntity);
      }
      else
      {
        throw new ApplicationException(string.Format("Unexpected context type: {0}", context.GetType()));
      }

      // If E in not resolved yet then can't continue.
      if (E == null)
      {
        return null;
      }

      return ResolveMemberAccess(E, I, A, K, errorHandler);
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

      if (e is ObjectReferenceRequiredException)
      {
        errorHandler.Error("CS0120", errorToken,
                           "An object reference is required for the non-static field, method, or property '{0}'",
                           (e as ObjectReferenceRequiredException).MemberName );
      }
      else
      {
        base.TranslateExceptionToError(e, errorHandler);
      }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Applies the member access resolution algorithm described in the spec §7.5.4
    /// </summary>
    /// <param name="E">An ExpressionResult object representing the subject of the member access.</param>
    /// <param name="I">The name of the accessed member.</param>
    /// <param name="A">A collection of type arguments belonging to I.</param>
    /// <param name="K">The number of type arguments.</param>
    /// <param name="errorHandler">An error handler object.</param>
    /// <returns>An ExpressionResult object representing the result of the member access.</returns>
    // ----------------------------------------------------------------------------------------------
    public static ExpressionResult ResolveMemberAccess(ExpressionResult E, string I, IEnumerable<TypeEntity> A, int K,
          ICompilationErrorHandler errorHandler)
    {
      // A member-access is either of the form E.I or of the form E.I<A1, ..., AK>, 
      // where E is a primary-expression, I is a single identifier and <A1, ..., AK> is an optional type-argument-list. 
      // When no type-argument-list is specified, consider K to be zero. 

      // The member-access is evaluated and classified as follows:
      ExpressionResult expressionResult = null;

      // Try to resolve to a nested namespace entity.
      expressionResult = ResolveToNestedNamespace(E, I, K);

      // Try to resolve to a non-nested type entity.
      if (expressionResult == null)
      {
        expressionResult = ResolveToNonNestedType(E, I, A, K);
      }

      // Try to resolve to a type member.
      if (expressionResult == null)
      {
        expressionResult = ResolveToTypeMember(E, I, A, K, errorHandler);
      }

      //•	If E is a property access, indexer access, variable, or value, the type of which is T, and a member lookup (§7.3) of I in T with K type arguments produces a match, then E.I is evaluated and classified as follows:
      //o	First, if E is a property or indexer access, then the value of the property or indexer access is obtained (§7.1.1) and E is reclassified as a value.
      //o	If I identifies one or more methods, then the result is a method group with an associated instance expression of E. If a type argument list was specified, it is used in calling a generic method (§7.5.5.1).
      //o	If I identifies an instance property, then the result is a property access with an associated instance expression of E. 
      //o	If T is a class-type and I identifies an instance field of that class-type:
      //•	If the value of E is null, then a System.NullReferenceException is thrown.
      //•	Otherwise, if the field is readonly and the reference occurs outside an instance constructor of the class in which the field is declared, then the result is a value, namely the value of the field I in the object referenced by E.
      //•	Otherwise, the result is a variable, namely the field I in the object referenced by E.
      //o	If T is a struct-type and I identifies an instance field of that struct-type:
      //•	If E is a value, or if the field is readonly and the reference occurs outside an instance constructor of the struct in which the field is declared, then the result is a value, namely the value of the field I in the struct instance given by E.
      //•	Otherwise, the result is a variable, namely the field I in the struct instance given by E.
      //o	If I identifies an instance event:
      //•	If the reference occurs within the class or struct in which the event is declared, and the event was declared without event-accessor-declarations (§10.8), then E.I is processed exactly as if I was an instance field.
      //•	Otherwise, the result is an event access with an associated instance expression of E.
      //•	Otherwise, an attempt is made to process E.I as an extension method invocation (§7.5.5.2). If this fails, E.I is an invalid member reference, and a compile-time error occurs.

      return expressionResult;
    }

    #region Private methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tries to resolve the member access expression to a nested namespace entity.
    /// </summary>
    /// <param name="E">The child expression.</param>
    /// <param name="I">The member name.</param>
    /// <param name="K">The number of type arguments of the member name.</param>
    /// <returns>A namespace expression result or null if could not resolve.</returns>
    // ----------------------------------------------------------------------------------------------
    private static ExpressionResult ResolveToNestedNamespace(ExpressionResult E, string I, int K)
    {
      // If K is zero and E is a namespace ...
      if (K == 0 && E is NamespaceExpressionResult)
      {
        // ... and E contains a nested namespace with name I, ...
        var parentNamespace = (E as NamespaceExpressionResult).Namespace;
        var childNamespace = parentNamespace.GetChildNamespace(I);
        if (childNamespace != null)
        {
          // ... then the result is that namespace.
          return new NamespaceExpressionResult(childNamespace);
        }
      }

      return null;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tries to resolve the member access expression to a non-nested type entity.
    /// </summary>
    /// <param name="E">The child expression.</param>
    /// <param name="I">The member name.</param>
    /// <param name="A">A collection of type argument entities.</param>
    /// <param name="K">The number of type arguments of the member name.</param>
    /// <returns>A type expression result or null if could not resolve.</returns>
    // ----------------------------------------------------------------------------------------------
    private static ExpressionResult ResolveToNonNestedType(ExpressionResult E, string I, IEnumerable<TypeEntity> A, int K)
    {
      // Otherwise, if E is a namespace ... 
      if (E is NamespaceExpressionResult)
      {
        // ... and E contains an accessible type having name I and K type parameters, 
        var parentNamespace = (E as NamespaceExpressionResult).Namespace;
        var childType = parentNamespace.GetSingleChildType<TypeEntity>(I, K);
        if (childType != null)
        {
          // then the result is that type constructed with the given type arguments.
          var constructedType = NamespaceOrTypeNameResolutionAlgorithm.GetConstructedTypeEntity(childType, A);
          return new TypeExpressionResult(constructedType);
        }
      }

      return null;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tries to resolve the member access expression to a type member entity.
    /// </summary>
    /// <param name="E">The child expression.</param>
    /// <param name="I">The member name.</param>
    /// <param name="A">A collection of type argument entities.</param>
    /// <param name="K">The number of type arguments of the member name.</param>
    /// <param name="errorHandler">Error handler object.</param>
    /// <returns>An expression result or null if could not resolve.</returns>
    // ----------------------------------------------------------------------------------------------
    private static ExpressionResult ResolveToTypeMember(ExpressionResult E, string I, IEnumerable<TypeEntity> A, int K,
      ICompilationErrorHandler errorHandler)
    {
      // If E is a predefined-type or a primary-expression classified as a type, 
      if (E is TypeExpressionResult)
      {
        var typeEntity = (E as TypeExpressionResult).Type;

        // if E is not a type parameter, 
        if (!(typeEntity is TypeParameterEntity))
        {
          // and if a member lookup (§7.3) of I in E with K type parameters produces a match, 
          var memberLookup = new MemberLookup(errorHandler, typeEntity.SemanticGraph);
          var memberLookupResult = memberLookup.Lookup(I, K, typeEntity, typeEntity, false);
          if (memberLookupResult != null)
          {
            // then E.I is evaluated and classified as follows:

            // If I identifies a type, 
            if (memberLookupResult.SingleMember is TypeEntity)
            {
              // then the result is that type constructed with the given type arguments.
              var constructedType = NamespaceOrTypeNameResolutionAlgorithm.GetConstructedTypeEntity(memberLookupResult.SingleMember as TypeEntity, A);
              return new TypeExpressionResult(constructedType);
            }

            // If I identifies one or more methods, 
            if (memberLookupResult.IsMethodGroup)
            {
              // then the result is a method group with no associated instance expression.
              return new MethodGroupExpressionResult(memberLookupResult.MethodGroup, null);

              // If a type argument list was specified, it is used in calling a generic method (§7.5.5.1).
              // TODO: do we care here about method calls?
            }

            // If I identifies a static property, 
            if (memberLookupResult.SingleMember is PropertyEntity
              && (memberLookupResult.SingleMember as PropertyEntity).IsStatic)
            {
              // then the result is a property access with no associated instance expression.
              return new PropertyAccessExpressionResult(memberLookupResult.SingleMember as PropertyEntity, null);
            }

            // If I identifies a static field:
            if (memberLookupResult.SingleMember is FieldEntity
              && (memberLookupResult.SingleMember as FieldEntity).IsStatic)
            {
              // If the field is readonly 
              // TODO: and the reference occurs outside the static constructor of the class or struct in which the field is declared, 
              if ((memberLookupResult.SingleMember as FieldEntity).IsReadOnly)
              {
                // then the result is a value, namely the value of the static field I in E.
                return new ValueExpressionResult((memberLookupResult.SingleMember as FieldEntity).Type);
              }
              else
              {
                // Otherwise, the result is a variable, namely the static field I in E.
                return new VariableExpressionResult(memberLookupResult.SingleMember as FieldEntity);
              }
            }

            // TODO:
            // If I identifies a static event:
            //   If the reference occurs within the class or struct in which the event is declared, and the event was declared without event-accessor-declarations (§10.8), then E.I is processed exactly as if I were a static field.
            //   Otherwise, the result is an event access with no associated instance expression.

            // If I identifies a constant ...
            if (memberLookupResult.SingleMember is ConstantMemberEntity)
            {
              // then the result is a value, namely the value of that constant. 
              return new ValueExpressionResult((memberLookupResult.SingleMember as ConstantMemberEntity).Type);
            }

            // If I identifies an enumeration member then the result is a value, namely the value of that enumeration member.
            // --> This case is already handled above, because an EnumMemberEntity is a subclass of ConstantMemberEntity.

            // Otherwise, E.I is an invalid member reference, and a compile-time error occurs.
            throw new ObjectReferenceRequiredException(typeEntity.ToString() + "." + I);
          }
        }
      }

      return null;
    }

    #endregion

  }
}
