using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharpTreeBuilder.Ast;
using CSharpTreeBuilder.CSharpAstBuilder;
using CSharpTreeBuilder.CSharpSemanticGraph;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  public sealed class PartialEntityMerger 
  {
    //// ----------------------------------------------------------------------------------------------
    ///// <summary>
    ///// Merges a partial type declaration into a type entity.
    ///// </summary>
    ///// <param name="typeDeclaration">A type declaration AST node.</param>
    ///// <param name="typeEntity">A type entity.</param>
    ///// <param name="parentEntity">The parent of type entity.</param>
    ///// <returns>True if no error occured, false otherwise.</returns>
    //// ----------------------------------------------------------------------------------------------
    //private bool MergePartialTypeDeclaration(TypeDeclarationNode typeDeclaration, TypeEntity typeEntity, NamespaceOrTypeEntity parentEntity)
    //{
    //  var entityIsPartial = (typeEntity is ICanBePartial && ((ICanBePartial)typeEntity).IsPartial);

    //  // If nor the type entity neither this declaration is partial then report duplicate name error
    //  if (!entityIsPartial && !typeDeclaration.IsPartial)
    //  {
    //    ReportDuplicateNameError(parentEntity, typeDeclaration.StartToken, typeDeclaration.NameWithGenericDimensions);
    //    return false;
    //  }

    //  // If the entity is partial but this declaration is not (or vica versa) then report missing partial error
    //  if ((entityIsPartial && !typeDeclaration.IsPartial)
    //      || (!entityIsPartial && typeDeclaration.IsPartial))
    //  {
    //    Token typeEntityErrorPoint = null;
    //    if (typeEntity.SyntaxNodes.Count == 1 && typeEntity.SyntaxNodes[0] != null)
    //    {
    //      typeEntityErrorPoint = typeEntity.SyntaxNodes[0].StartToken;
    //    }

    //    var errorPoint = typeDeclaration.IsPartial ? typeEntityErrorPoint : typeDeclaration.StartToken;

    //    ErrorMissingPartialModifier(errorPoint, typeEntity.Name);
    //    return false;
    //  }

    //  // This partial type declaration must be merged with the already created type entity.
    //  // Base class: if present then must be the same --> it will be checked after base type resolution.
    //  // Base interfaces: union --> duplicates will be eliminated after base type resolution.
    //  AddBaseTypesToTypeEntity(typeEntity, typeDeclaration);

    //  // TODO:
    //  // Type parameters: must be the same (number, name and order)
    //  // Accesibility modifiers: must be the same
    //  // If one or more partial declarations of a class include an abstract modifier, the class is considered abstract 
    //  // If one or more partial declarations of a class include a sealed modifier, the class is considered sealed 
    //  // Type constraints: must be the same or completely missing
    //  // When the unsafe modifier is used on a partial type declaration, only that particular part is considered an unsafe context 
    //  // Attributes: combined
    //  // Members: union

    //  return true;
    //}

    //// ----------------------------------------------------------------------------------------------
    ///// <summary>
    ///// Merges a partial method declaration into a method entity.
    ///// </summary>
    ///// <param name="methodDeclaration">A method declaration AST node.</param>
    ///// <param name="methodEntity">A method entity.</param>
    ///// <param name="parentEntity">The parent of the method entity.</param>
    ///// <returns>True if no error occured, false otherwise.</returns>
    //// ----------------------------------------------------------------------------------------------
    //private bool MergePartialMethodDeclaration(MethodDeclarationNode methodDeclaration, MethodEntity methodEntity, TypeEntity parentEntity)
    //{
    //  // TODO checks:
    //  // error CS0756: A partial method may not have multiple defining declarations
    //  // error CS0757: A partial method may not have multiple implementing declarations
    //  //
    //  // If an implementing partial method declaration is given, a corresponding defining partial method declaration must exist,
    //  // and the declarations must match as specified in the following:
    //  // - The declarations must have the same modifiers (although not necessarily in the same order), 
    //  //   method name, number of type parameters and number of parameters.
    //  // - Corresponding parameters in the declarations must have the same modifiers (although not necessarily in the same order) 
    //  //   and the same types (modulo differences in type parameter names).
    //  // - Corresponding type parameters in the declarations must have the same constraints 
    //  //   (modulo differences in type parameter names).
    //  //


    //  // The semantics of the merging is that the implementing partial method declaration overwrites
    //  // the type parameter name and formal parameter names defined in the defining partial method declaration.
    //  if (methodDeclaration.Body != null)
    //  {
    //    // Remove the type parameters and formal parameters
    //    RemoveTypeParameters(methodEntity);
    //    RemoveParameters(methodEntity);

    //    // Add the type parameters and formal parameters
    //    AddTypeParametersToEntity(methodEntity, parentEntity, methodDeclaration.TypeParameters);
    //    AddParametersToOverloadableEntity(methodEntity, methodDeclaration.FormalParameters);

    //    // TODO: Add the body of the method
    //    methodEntity.Body = new BlockEntity();
    //  }

    //  // TODO: merging attributes
    //  // Method attributes: combined attributes of the defining and the implementing 
    //  //   partial method declaration in unspecified order. Duplicates are not removed.
    //  // Parameter attributes: combined attributes of the corresponding parameters of the defining 
    //  //   and the implementing partial method declaration in unspecified order. Duplicates are not removed. 

    //  return true;
    //}

      //    // In classes and structs: member name cannot be the same as the type name
      //if ((parentEntity is ClassEntity || parentEntity is StructEntity)
      //    && parentEntity.Name == node.Name)
      //{
      //  ErrorMemberNameAndTypeNameConflict(node.StartToken, node.Identifier);
      //  return;
      //}

      //// TODO: Partial methods cannot define access modifiers, but are implicitly private.

      //// Don't have to check CS0766 (Partial methods must have a void return type) 
      //// because the syntax analyzer makes sure that the return type is void.

      //// Partial methods' parameters cannot have the out modifier. 
      //if (node.IsPartial && MethodDeclarationHasOutParameter(node))
      //{
      //  _ErrorHandler.Error("CS0752", node.StartToken, "A partial method cannot have out parameters");
      //  return;
      //}

    //// ----------------------------------------------------------------------------------------------
    ///// <summary>
    ///// Gets a value indicating whether a method declaration has void return type.
    ///// </summary>
    ///// <param name="node">A method declaration AST node.</param>
    ///// <returns>True if the method declaration has void return type, false otherwise.</returns>
    //// ----------------------------------------------------------------------------------------------
    //private static bool MethodDeclarationHasVoidReturnType(MethodDeclarationNode node)
    //{
    //  return node != null
    //         && node.Type != null
    //         && node.Type.TypeName != null
    //         && node.Type.TypeName.TypeTags != null
    //         && node.Type.TypeName.TypeTags.Count == 1
    //         && node.Type.TypeName.TypeTags[0].Identifier == "void";
    //}

    //// ----------------------------------------------------------------------------------------------
    ///// <summary>
    ///// Gets a value indicating whether a method declaration has out parameter.
    ///// </summary>
    ///// <param name="node">A method declaration AST node.</param>
    ///// <returns>True if the method declaration has out parameter, false otherwise.</returns>
    //// ----------------------------------------------------------------------------------------------
    //private static bool MethodDeclarationHasOutParameter(MethodDeclarationNode node)
    //{
    //  bool result = false;

    //  foreach (var formalParameter in node.FormalParameters)
    //  {
    //    if (formalParameter.Modifier == FormalParameterModifier.Out)
    //    {
    //      result = true;
    //    }
    //  }

    //  return result;
    //}

    //// ----------------------------------------------------------------------------------------------
    ///// <summary>
    ///// Removes all type parameters from an entity.
    ///// </summary>
    ///// <param name="entity">An entity with type parameters.</param>
    //// ----------------------------------------------------------------------------------------------
    //private static void RemoveTypeParameters(ICanHaveTypeParameters entity)
    //{
    //  foreach (var typeParameter in entity.OwnTypeParameters)
    //  {
    //    entity.RemoveTypeParameter(typeParameter);
    //  }
    //}

    //// ----------------------------------------------------------------------------------------------
    ///// <summary>
    ///// Removes all parameters from an entity.
    ///// </summary>
    ///// <param name="entity">An entity with parameters.</param>
    //// ----------------------------------------------------------------------------------------------
    //private static void RemoveParameters(IOverloadableEntity entity)
    //{
    //  foreach (var parameter in entity.Parameters)
    //  {
    //    entity.RemoveParameter(parameter);
    //  }
    //}

  }
}
