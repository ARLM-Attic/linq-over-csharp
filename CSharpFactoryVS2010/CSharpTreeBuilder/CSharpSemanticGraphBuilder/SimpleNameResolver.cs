using System;
using System.Collections.Generic;
using System.Linq;
using CSharpTreeBuilder.Ast;
using CSharpTreeBuilder.CSharpSemanticGraph;
using CSharpTreeBuilder.ProjectContent;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class implements the resolution logic for simple names, as described in the spec §7.5.2
  /// </summary>
  // ================================================================================================
  public sealed class SimpleNameResolver
  {
    /// <summary>Error handler object for error and warning reporting.</summary>
    private readonly ICompilationErrorHandler _ErrorHandler;

    /// <summary>The semantic graph that is the context of the type resolution.</summary>
    private readonly SemanticGraph _SemanticGraph;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="SimpleNameResolver"/> class.
    /// </summary>
    /// <param name="errorHandler">Error handler object for error and warning reporting.</param>
    /// <param name="semanticGraph">The semantic graph that is the context of the type resolution.</param>
    // ----------------------------------------------------------------------------------------------
    public SimpleNameResolver(ICompilationErrorHandler errorHandler, SemanticGraph semanticGraph)
    {
      _ErrorHandler = errorHandler;
      _SemanticGraph = semanticGraph;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Resolves a simple name to a semantic entity.
    /// </summary>
    /// <param name="simpleName">An AST node that represents a simple-name.</param>
    /// <param name="resolutionContextEntity">The semantic entity that is the context of the resolution.</param>
    /// <returns>The resolved semantic entity, or null if couldn't be resolved.</returns>
    // ----------------------------------------------------------------------------------------------
    public SemanticEntity Resolve(SimpleNameNode simpleName, SemanticEntity resolutionContextEntity)
    {
      // A simple-name consists of an identifier, optionally followed by a type argument list:
      //   simple-name:
      //     identifier   type-argument-listopt
      // A simple-name is either of the form I or of the form I<A1, ..., AK>, where I is a single identifier 
      // and <A1, ..., AK> is an optional type-argument-list. When no type-argument-list is specified, consider K to be zero. 
      // The simple-name is evaluated [and classified as follows]:

      // (Try to resolve to a local variable, parameter or constant.) 

      // TODO:
      // If K is zero and the simple-name appears within a block and if the block’s (or an enclosing block’s) 
      // local variable declaration space (§3.3) contains a local variable, parameter or constant with name I, 
      // then the simple-name refers to that local variable, parameter or constant 
      // [and is classified as a variable or value].

      // (Try to resolve to a method type parameter.)

      var typeParameterEntity = ResolveToMethodTypeParameter(simpleName, resolutionContextEntity);
      if (typeParameterEntity != null)
      {
        return typeParameterEntity;
      }

      // (Try to lookup in type declarations as a type parameter or type member.)

      var typeParameterOrTypeMember = ResolveAtTypeDeclarationLevel(simpleName, resolutionContextEntity);
      if (typeParameterOrTypeMember != null)
      {
        return typeParameterOrTypeMember;
      }

      // (Try to lookup in namespace declarations as a type or a namespace.)

      // TODO

      // Otherwise, the simple-name is undefined and a compile-time error occurs.
      _ErrorHandler.Error("CS0103", simpleName.StartToken, "The name '{0}' does not exist in the current context",
                          simpleName.ToString());

      return null;
    }

    #region Private methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Tries to resolve a simple name to a method type parameter.
    /// </summary>
    /// <param name="simpleName">The simple name to resolve.</param>
    /// <param name="resolutionContextEntity">The context of the resolution.</param>
    /// <returns>A type parameter entity, or null if could not resolve.</returns>
    // ----------------------------------------------------------------------------------------------
    private static TypeParameterEntity ResolveToMethodTypeParameter(SimpleNameNode simpleName, SemanticEntity resolutionContextEntity)
    {
      // If K is zero ...
      if (!simpleName.HasTypeArguments)
      {
        // ... and the simple-name appears within the body of a generic method declaration (§10.6) ...
        var genericMethodEntity = resolutionContextEntity.GetEnclosingGenericMethodDeclaration();
        if (genericMethodEntity != null)
        {
          // ... and if that declaration includes a type parameter (§10.1.3) with name I, ...
          var methodTypeParameter = genericMethodEntity.GetOwnTypeParameterByName(simpleName.Identifier);
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
    /// <param name="simpleName">The simple name to resolve.</param>
    /// <param name="resolutionContextEntity">The context of the resolution.</param>
    /// <returns>A semantic entity, or null if could not resolve.</returns>
    // ----------------------------------------------------------------------------------------------
    private SemanticEntity ResolveAtTypeDeclarationLevel(SimpleNameNode simpleName, SemanticEntity resolutionContextEntity)
    {
      // Otherwise, for each instance type T (§10.3.1), 
      // starting with the instance type of the immediately enclosing type declaration and 
      // continuing with the instance type of each enclosing class or struct declaration (if any):
      var typeContext = resolutionContextEntity.GetEnclosing<TypeEntity>();

      while (typeContext != null)
      {
        // If K is zero ...
        if (!simpleName.HasTypeArguments)
        {
          // ... and the declaration of T includes a type parameter with name I, ...
          var foundTypeParameterEntity = (typeContext is GenericCapableTypeEntity)
            ? (typeContext as GenericCapableTypeEntity).GetOwnTypeParameterByName(simpleName.Identifier)
            : null;

          if (foundTypeParameterEntity != null)
          {
            // then the simple-name refers to that type parameter.
            return foundTypeParameterEntity;
          }
        }

        // - Otherwise, if a member lookup (§7.3) of I in T with K type arguments produces a match:
        //   - If T is the instance type of the immediately enclosing class or struct type and the lookup identifies one or more methods, the result is a method group with an associated instance expression of this. If a type argument list was specified, it is used in calling a generic method (§7.5.5.1).
        //   - Otherwise, if T is the instance type of the immediately enclosing class or struct type, if the lookup identifies an instance member, and if the reference occurs within the block of an instance constructor, an instance method, or an instance accessor, the result is the same as a member access (§7.5.4) of the form this.I. This can only happen when K is zero.
        //   - Otherwise, the result is the same as a member access (§7.5.4) of the form T.I or T.I<A1, ..., AK>. In this case, it is a compile-time error for the simple-name to refer to an instance member.

        // "... and continuing with the instance type of each enclosing class or struct declaration (if any):"
        typeContext = typeContext.Parent.GetEnclosing<TypeEntity>();
      }

      return null;
    }

    #endregion
  }
}
