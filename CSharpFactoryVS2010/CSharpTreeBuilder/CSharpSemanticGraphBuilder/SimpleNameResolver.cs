using System;
using System.Collections.Generic;
using System.Linq;
using CSharpTreeBuilder.Ast;
using CSharpTreeBuilder.CSharpAstBuilder;
using CSharpTreeBuilder.CSharpSemanticGraph;
using CSharpTreeBuilder.ProjectContent;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class implements the resolution logic for simple names, as described in the spec §7.5.2
  /// </summary>
  // ================================================================================================
  public sealed class SimpleNameResolver : NamespaceOrTypeNameResolver
  {
    /// <summary>An object that implements the member lookup logic.</summary>
    private readonly MemberLookup _MemberLookup;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="SimpleNameResolver"/> class.
    /// </summary>
    /// <param name="errorHandler">Error handler object for error and warning reporting.</param>
    /// <param name="semanticGraph">The semantic graph that is the context of the type resolution.</param>
    // ----------------------------------------------------------------------------------------------
    public SimpleNameResolver(ICompilationErrorHandler errorHandler, SemanticGraph semanticGraph)
      : base( errorHandler, semanticGraph)
    {
      _MemberLookup = new MemberLookup(errorHandler, semanticGraph);
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Resolves a simple name to a semantic entity or a method group.
    /// </summary>
    /// <param name="simpleNameNode">An AST node that represents a simple-name.</param>
    /// <param name="simpleNameEntity">The semantic entity that represents the simple name.</param>
    /// <returns>The resolved semantic entity or method group, or null if couldn't resolve.</returns>
    // ----------------------------------------------------------------------------------------------
    public SimpleNameResult Resolve(SimpleNameNode simpleNameNode, SimpleNameExpressionEntity simpleNameEntity)
    {
      try
      {
        // A simple-name consists of an identifier, optionally followed by a type argument list:
        //   simple-name:
        //     identifier   type-argument-listopt
        // A simple-name is either of the form I or of the form I<A1, ..., AK>, where I is a single identifier 
        // and <A1, ..., AK> is an optional type-argument-list. When no type-argument-list is specified, consider K to be zero. 
        // The simple-name is evaluated [and classified as follows]:

        // First lets resolve all type arguments.
        var typeArguments = ResolveTypeArguments(simpleNameNode.Arguments, simpleNameEntity);

        // (Try to resolve to a local variable, parameter or constant.) 

        // TODO:
        // If K is zero and the simple-name appears within a block and if the block’s (or an enclosing block’s) 
        // local variable declaration space (§3.3) contains a local variable, parameter or constant with name I, 
        // then the simple-name refers to that local variable, parameter or constant 
        // [and is classified as a variable or value].

        // (Try to resolve to a method type parameter.)

        var typeParameterEntity = ResolveToMethodTypeParameter(simpleNameNode, simpleNameEntity);
        if (typeParameterEntity != null)
        {
          return new SimpleNameResult(typeParameterEntity);
        }

        // (Try to lookup in type declarations as a type parameter or type member.)

        var simpleNameResult = ResolveAtTypeDeclarationLevel(simpleNameNode, simpleNameEntity);
        if (simpleNameResult != null)
        {
          return simpleNameResult;
        }

        // (Try to lookup in namespace declarations as a type or a namespace.)

        var typeTagNode = new TypeTagNode(simpleNameNode.IdentifierToken,
                                          simpleNameNode.Arguments.Count > 0 ? simpleNameNode.Arguments : null);
        var namespaceOrTypeEntity = ResolveSingleTypeTagInNamespaces(typeTagNode, simpleNameEntity, simpleNameEntity, typeArguments);
        if (namespaceOrTypeEntity != null)
        {
          return new SimpleNameResult(namespaceOrTypeEntity);
        }

        throw new ApplicationException("NamespaceOrTypeEntity should not be null.");
      }
      catch (NamespaceOrTypeNameNotResolvedException)
      {
        // Otherwise, the simple-name is undefined and a compile-time error occurs.
        _ErrorHandler.Error("CS0103", simpleNameNode.StartToken, "The name '{0}' does not exist in the current context",
                            simpleNameNode.ToString());
      }
      catch (NamespaceOrTypeNameResolverException e)
      {
        TranslateExceptionToError(e, simpleNameNode.StartToken);
      }

      return null;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Translates an exception thrown during namespace-or-type-name resolution to an error 
    /// and reports it to the error handler object.
    /// </summary>
    /// <param name="e">An exception object.</param>
    /// <param name="errorToken">The token where the error occured.</param>
    // ----------------------------------------------------------------------------------------------
    protected override void TranslateExceptionToError(Exception e, Token errorToken)
    {
      if (e is StaticMemberExpectedException)
      {
        _ErrorHandler.Error("CS0120", errorToken,
                            string.Format("An object reference is required for the non-static field, method, or property '{0}'",
                                          (e as StaticMemberExpectedException).MemberEntity));
      }
      else
      {
        base.TranslateExceptionToError(e, errorToken);
      }
    }

    #region Private methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tries to resolve a simple name to a method type parameter.
    /// </summary>
    /// <param name="simpleNameNode">The simple name to resolve.</param>
    /// <param name="simpleNameEntity">The entity that represents the simple name.</param>
    /// <returns>A type parameter entity, or null if could not resolve.</returns>
    // ----------------------------------------------------------------------------------------------
    private static TypeParameterEntity ResolveToMethodTypeParameter(SimpleNameNode simpleNameNode, SimpleNameExpressionEntity simpleNameEntity)
    {
      // If K is zero ...
      if (!simpleNameNode.HasTypeArguments)
      {
        // ... and the simple-name appears within the body of a generic method declaration (§10.6) ...
        var genericMethodEntity = simpleNameEntity.GetEnclosingGenericMethodDeclaration();
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
    /// <param name="simpleNameEntity">The entity that represents the simple name.</param>
    /// <returns>A semantic entity, or null if could not resolve.</returns>
    // ----------------------------------------------------------------------------------------------
    private SimpleNameResult ResolveAtTypeDeclarationLevel(SimpleNameNode simpleNameNode, SimpleNameExpressionEntity simpleNameEntity)
    {
      // Otherwise, for each instance type T (§10.3.1), 
      // starting with the instance type of the immediately enclosing type declaration and 
      // continuing with the instance type of each enclosing class or struct declaration (if any):
      var T = simpleNameEntity.GetEnclosing<TypeEntity>();
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
            return new SimpleNameResult(foundTypeParameterEntity);
          }
        }

        // Otherwise, if a member lookup (§7.3) of I in T with K type arguments produces a match:
        var I = simpleNameNode.Identifier;
        var K = simpleNameNode.Arguments.Count;
        var memberLookupResult = _MemberLookup.Lookup(I, K, T, simpleNameEntity, false);

        if (!memberLookupResult.IsEmpty)
        {
          // If T is the instance type of the immediately enclosing class or struct type 
          // and the lookup identifies one or more methods,
          if (T == immediatelyEnclosingType && memberLookupResult.IsMethodGroup)
          {
            // the result is a method group with an associated instance expression of this. 
            return new SimpleNameResult(memberLookupResult.MethodGroup);

            // If a type argument list was specified, it is used in calling a generic method (§7.5.5.1).
          }

          // Otherwise, if T is the instance type of the immediately enclosing class or struct type, 
          // if the lookup identifies an instance member, ...
          if (T == immediatelyEnclosingType
            && memberLookupResult.IsSingleMember && memberLookupResult.SingleMember.IsInstanceMember)
          {
            // ... and if the reference occurs within the block of an instance constructor, 
            // an instance method, or an instance accessor, ...
            var enclosingMember = simpleNameEntity.GetEnclosing<NonTypeMemberEntity>();
            if (enclosingMember != null && enclosingMember.IsInstanceMember)
            {
              // ... the result is the same as a member access (§7.5.4) of the form this.I. 
              // This can only happen when K is zero.
              if (K == 0)
              {
                return new SimpleNameResult(memberLookupResult.SingleMember);
              }

              throw new ApplicationException(
                string.Format("At simple name node '{0}' expected 0 type arguments but found '{1}'.", simpleNameNode, K));
            }
          }

          // Otherwise, the result is the same as a member access (§7.5.4) of the form T.I or T.I<A1, ..., AK>. 
          var result = new SimpleNameResult(memberLookupResult.SingleMember);

          // In this case, it is a compile-time error for the simple-name to refer to an instance member.
          if (memberLookupResult.SingleMember.IsInstanceMember)
          {
            throw new StaticMemberExpectedException(memberLookupResult.SingleMember);
          }

          return result;
        }

        // "... and continuing with the instance type of each enclosing class or struct declaration (if any):"
        T = T.Parent.GetEnclosing<TypeEntity>();
      }

      return null;
    }

    #endregion
  }
}
