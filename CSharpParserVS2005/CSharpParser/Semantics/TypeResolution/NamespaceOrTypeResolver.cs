using System;
using System.Collections.Generic;
using CSharpParser.ParserFiles;
using CSharpParser.ProjectModel;

namespace CSharpParser.Semantics
{
  // ==================================================================================
  /// <summary>
  /// This class provides methods that resolve namespace or type names according to
  /// the section 10.8 of the C# Language Specification 2.0
  /// </summary>
  // ==================================================================================
  public class NamespaceOrTypeResolver
  {
    #region Private fields

    private readonly CSharpSyntaxParser _Parser;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new instance of this class tied to the specified parser instance.
    /// </summary>
    /// <param name="parser">Parser instance.</param>
    // --------------------------------------------------------------------------------
    public NamespaceOrTypeResolver(CSharpSyntaxParser parser)
    {
      _Parser = parser;
    }

    #endregion

    #region Resolve method

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves the specified name within the given context.
    /// </summary>
    /// <param name="type">Type reference representing the name to be resolved.</param>
    /// <param name="contextType">Type of resolution context.</param>
    /// <param name="declarationScope">Current type declaration context.</param>
    /// <param name="parameterScope">Current type parameter declaration scope.</param>
    /// <returns>Name resolution information.</returns>
    /// <remarks>
    /// </remarks>
    // --------------------------------------------------------------------------------
    public NamespaceOrTypeResolverInfo Resolve(TypeReference type, 
      ResolutionContext contextType, ITypeDeclarationScope declarationScope,
      ITypeParameterScope parameterScope)
    {
      // --- Check all inputs
      if (type == null) throw new ArgumentNullException("type");
      if (declarationScope == null) throw new ArgumentNullException("declarationScope");

      // --- Create the resolution structure
      NamespaceOrTypeResolverInfo resolutionInfo = 
        new NamespaceOrTypeResolverInfo(this, type, contextType, 
        declarationScope, parameterScope);

      // --- Branch: "qualified-alias-member" | "namespace-or-type"
      if (type.IsGlobal) ResolveQualifiedAliasMember(resolutionInfo);
      else ResolveNamespaceOrTypeName(resolutionInfo);

      // --- At this point we resolved 0, 1 or more parts of the name. We go through all
      // --- resolved parts and resolve type arguments of generic types.
      foreach (TypeReference typePart in type.NameParts)
      {
        if (typePart.IsResolved && typePart.Arguments.Count > 0)
        {
          // --- Go through the generic arguments and resolve them
          foreach (TypeReference argument in typePart.Arguments)
          {
            if (!argument.IsResolved) Resolve(argument, contextType, 
              declarationScope, parameterScope);
          }
        }
      }

      // --- OK, we resolved this type as deep as we can.
      return resolutionInfo;
    }

    #endregion

    #region ResolveTypeName method

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves the specified name within the given context. Accepts only type names.
    /// </summary>
    /// <param name="type">Type reference representing the name to be resolved.</param>
    /// <param name="contextType">Type of resolution context.</param>
    /// <param name="declarationScope">Current type declaration context.</param>
    /// <param name="parameterScope">Current type parameter declaration scope.</param>
    /// <returns>Name resolution information.</returns>
    /// <remarks>
    /// If the specified name is a namespace or a type parameter, appropriate error
    /// is raised.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public NamespaceOrTypeResolverInfo ResolveTypeName(TypeReference type,
      ResolutionContext contextType, ITypeDeclarationScope declarationScope,
      ITypeParameterScope parameterScope)
    {
      NamespaceOrTypeResolverInfo info =
        Resolve(type, contextType, declarationScope, parameterScope);

      if (!info.IsResolved) return info;
      {
        // --- We expect a type and not a namespace.
        if (info.Target == ResolutionTarget.Namespace)
        {
          _Parser.Error0118(type.Token, type.FullName, "namespace", "type");
          type.Invalidate();
        }
      }
      return info;
    }

    #endregion

    #region ResolveQualifiedAliasMember method

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves a qualified alias member name.
    /// </summary>
    /// <param name="info">Resolution information</param>
    /// <remarks>
    /// <para>
    /// The method modified the resolution information accoding to resolution progress.
    /// </para>
    /// </remarks>
    // --------------------------------------------------------------------------------
    private void ResolveQualifiedAliasMember(NamespaceOrTypeResolverInfo info)
    {
      if (info == null) throw new ArgumentNullException("info");

      // --- Actually this should not happen, as the parser checks this situation...
      if (!info.Type.HasSubType)
      {
        throw new InvalidOperationException("Global name must have SubType.");
      }
      if (info.Type.Name == "global")
      {
        ResolveGlobalAliasMember(info);
      }
      else
      {
        ResolveNonGlobalAliasMember(info);
      }
    }

    #endregion

    #region ResolveGlobalAliasMember method

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves qualified alias member name within the "global::" namespace hierarchy.
    /// </summary>
    /// <param name="info">Resolution information</param>
    /// <remarks>
    /// <para>
    /// The method modified the resolution information accoding to resolution progress.
    /// </para>
    /// <para>
    /// During the resolution extern and using alias definitions are used according to
    /// the rules specified by section 16.7.
    /// </para>
    /// </remarks>
    // --------------------------------------------------------------------------------
    private void ResolveGlobalAliasMember(NamespaceOrTypeResolverInfo info)
    {
      if (info == null) throw new ArgumentNullException("info");

      // --- Resolve the remaining parts
      info.Hierarchy = info.CompilationUnit.GlobalHierarchy;
      info.CurrentPart.ResolveToNamespaceHierarchy(info.CompilationUnit.GlobalHierarchy);
      info.MoveToNextPart();
      info.Results.Merge(info.Hierarchy);
      ResolveNamespaceOrNameInForest(info);
      if (info.IsResolved) return;

      // --- Name cannot be fully resolved according to resolution roles
      info.Parser.Error0400(info.CurrentPart.Token, info.CurrentPart.Name);
    }

    #endregion

    #region ResolveNonGlobalAliasMember method

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves a non-global qualified alias member name.
    /// </summary>
    /// <param name="info">Resolution information</param>
    /// <remarks>
    /// <para>
    /// The method modified the resolution information accoding to resolution progress.
    /// </para>
    /// <para>
    /// During the resolution extern and using alias definitions are used according to
    /// the rules specified by section 16.7.
    /// </para>
    /// </remarks>
    // --------------------------------------------------------------------------------
    private void ResolveNonGlobalAliasMember(NamespaceOrTypeResolverInfo info)
    {
      if (info == null) throw new ArgumentNullException("info");
      
      // --- Cycle from the current declaration namespace to the global namespace
      SourceFile file = info.DeclarationScope.EnclosingSourceFile;
      NamespaceFragment ns = info.DeclarationScope.DeclaringNamespace;
      bool errorFound = false;
      bool resolved = false;
      foreach (ITypeDeclarationScope scope in file.GetScopesToOuter(ns))
      {
        // --- Check, if the current scope contains a using alias with the global name
        UsingClause usingClause = scope.Usings[info.CurrentPart.Name];
        if (usingClause != null && usingClause.IsResolved)
        {
          if (usingClause.IsResolvedToType)
          {
            // --- The name cannot be aliased to a type. Raise an error and go on with 
            // --- name resolution to find other errors.
            _Parser.Error0431(info.CurrentPart.Token, info.CurrentPart.Name);
            errorFound = true;
          }
          else
          {
            // --- The name is aliased to a namespace.
          }
          info.Results.Merge(usingClause.Resolvers);
          SetResultByResolutionNode(info.CurrentPart, usingClause.Resolvers[0]);
          resolved = true;
          break;
        }
        // --- Check, if the current scope contains an extern alias with the global name
        ExternalAlias externAlias = scope.ExternAliases[info.CurrentPart.Name];
        if (externAlias != null && externAlias.HasHierarchy)
        {
          info.Results.Merge(externAlias.Hierarchy);
          info.CurrentPart.ResolveToNamespaceHierarchy(externAlias.Hierarchy);
          resolved = true;
          break;
        }
      }

      // --- At this point we have 'resolved' with true, if the global name has been
      // --- successfully resolved.
      if (!resolved)
      {
        _Parser.Error0432(info.CurrentPart.Token, info.CurrentPart.Name);
        return;
      }

      // --- Resolve the remaining part of the name within the forest defined by the
      // --- global name

      info.MoveToNextPart();
      ResolveNamespaceOrNameInForest(info);
      if (info.IsResolved)
      {
        // --- The remaining partof the name is resolved, however we may have problems with
        // --- the global name.
        if (errorFound) info.Target = ResolutionTarget.Unresolved;
      }
      else
      {
        // --- Give an appropriate message
        if (info.CurrentPart.PrefixType.ResolvingNode is NamespaceResolutionNode)
        {
          _Parser.Error0234(info.CurrentPart.Token, info.CurrentPart.Name, 
            info.CurrentPart.PrefixType.ResolvingName);
        }
        else
        {
          _Parser.Error0426(info.CurrentPart.Token, info.CurrentPart.Name,
            info.CurrentPart.PrefixType.ResolvingName);
        }
      }
    }

    #endregion

    #region ResolveNamespaceOrTypeName method

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves a namespace or type name using the global hierarchy and the current 
    /// resolution context.
    /// </summary>
    /// <param name="info">Resolution information</param>
    /// <remarks>
    /// <para>
    /// The method modified the resolution information accoding to resolution progress.
    /// </para>
    /// </remarks>
    // --------------------------------------------------------------------------------
    private void ResolveNamespaceOrTypeName(NamespaceOrTypeResolverInfo info)
    {
      if (info == null) throw new ArgumentNullException("info");

      if (!info.CurrentPart.HasSubType)
      {
        // --- The name in if form of I or I<A1, ... Ak>
        // --- Step 1: Check for generic method parameter
        if (info.CurrentPart.Arguments.Count == 0)
        {
          if (ResolveGenericMethodParameter(info)) return;
        }

        // --- Step 2: Check if the name can be resolved using the current type body
        if (ResolveNameInTypeBody(info)) return;

        // --- Step 3: Try to resolve the name in the current scope with imported 
        // --- namespaces, using and extern aliases.
        ResolveSimpleNameInScope(info);
      }
      else
      {
        // --- The name in if form of N.I or N.I<A1, ..., Ak>. We cut the N from the name
        // --- and resolve it, later we use that name to look for the I<A1, ..., Ak> part.
        ResolveCompoundNameInScope(info);
      }
    }

    #endregion

    #region ResolveNamespaceOrNameInForest methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves a namespace or type name using the resolution tree nodes specified
    /// by the resolution information.
    /// </summary>
    /// <param name="info">Resolution information</param>
    /// <remarks>
    /// <para>
    /// The method modified the resolution information accoding to resolution progress.
    /// </para>
    /// <para>
    /// Uses the resolution tree nodes in the info.Result container. Seraches all nodes
    /// and retrieves the list of nodes that match the resolution. During the 
    /// resolution no extern and using alias definitions are used.
    /// </para>
    /// </remarks>
    // --------------------------------------------------------------------------------
    private void ResolveNamespaceOrNameInForest(NamespaceOrTypeResolverInfo info)
    {
      TypeReference lastResolved;
      ResolutionNodeList result;
      ResolveNamespaceOrNameInForest(info.CurrentPart, info.Results, out result, 
        out lastResolved);
      info.CurrentPart = lastResolved;
      info.SetResultNode(result);
      info.Evaluate();
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves a namespace or type name using the specified resolution tree nodes.
    /// </summary>
    /// <param name="type">Type reference representing the name.</param>
    /// <param name="forestNodes">Nodes to search for the name.</param>
    /// <param name="result">Results of the name resolution.</param>
    /// <param name="lastResolved">Last part of the name successfully resolved.</param>
    /// <remarks>
    /// <para>
    /// Searches all nodes and retrieves the list of nodes that match the resolution. 
    /// During the resolution no extern and using alias definitions are used.
    /// </para>
    /// </remarks>
    // --------------------------------------------------------------------------------
    private void ResolveNamespaceOrNameInForest(TypeReference type,
      ResolutionNodeList forestNodes, out ResolutionNodeList result, 
      out TypeReference lastResolved)
    {
      result = new ResolutionNodeList();
      lastResolved = type;
      int maxResolutionLength = 0;
      foreach (ResolutionNodeBase treeNode in forestNodes)
      {
        TypeReference carryOnPart;

        // --- Resolve the name in the current tree
        ResolutionNodeBase resolvingNode =
          ResolveNameOrNamespaceInTree(type, treeNode, out carryOnPart);
        if (resolvingNode == null) continue;

        // --- How far we resolved the node?
        int depth = resolvingNode.Depth - treeNode.Depth;
        if (depth == 0) continue;

        // --- This time we could not resolve the name as deep as before, forget
        // --- about this result.
        if (depth < maxResolutionLength) continue;

        // --- This time we resolved the name deeper then before. Forget about
        // --- partial results found before and register only this new result.
        if (depth > maxResolutionLength)
        {
          result.Clear();
          maxResolutionLength = depth;
        }

        // --- Add the result found to the list resolution nodes
        result.Add(resolvingNode);
        lastResolved = carryOnPart;
      }
    }

    #endregion

    #region ResolveNameOrNamespaceInTree method

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves a namespace or type name using the resolution tree node specified.
    /// </summary>
    /// <param name="type">Type reference representing the name to be found.</param>
    /// <param name="node">
    /// Node representing the tree where name should be resolved.
    /// </param>
    /// <param name="lastResolvedPart">
    /// Last part of the name successfully resolved.
    /// </param>
    /// <returns>
    /// The node of the resolution tree representing the resolving node of the name.
    /// Null, if the name cannot be resolved.
    /// </returns>
    /// <remarks>
    /// This method does not modify the info parameter, only reads its properties.
    /// </remarks>
    // --------------------------------------------------------------------------------
    private ResolutionNodeBase ResolveNameOrNamespaceInTree(
      TypeReference type,
      ResolutionNodeBase node,
      out TypeReference lastResolvedPart)
    {
      ResolutionNodeBase currentNode = node;
      lastResolvedPart = null;
      ResolutionNodeBase nextNode;
      do
      {
        // --- If the current node is a namespace and is not imported, we 
        // --- import its types.
        NamespaceResolutionNode nsNode = currentNode as NamespaceResolutionNode;
        if (nsNode != null) nsNode.ImportTypes();

        // --- Check, if the next part of the name can be resolved
        nextNode = currentNode.FindChild(type.ClrName);

        // --- No more parts can be resolved? Quit in this case.
        if (nextNode == null) return null;

        // --- Name part successfully resolved. 
        SetResultByResolutionNode(type, nextNode);

        // --- A this point we succesfully resolved the current part of the name.
        lastResolvedPart = type;
        currentNode = nextNode;
        type = type.SubType;
      } while (type != null);

      return nextNode;
    }

    #endregion

    #region ResolveGenericMethodParameter method

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves a generic method parameter.
    /// </summary>
    /// <param name="info">Resolution information</param>
    /// <returns>
    /// True, if a generic methodparameter has been found and successfully resolved;
    /// otherwise, false.
    /// </returns>
    /// <remarks>
    /// <para>
    /// If the current resolution context is not within a generic method, returns false.
    /// </para>
    /// <para>
    /// The method modified the resolution information accoding to resolution progress.
    /// </para>
    /// </remarks>
    // --------------------------------------------------------------------------------
    private bool ResolveGenericMethodParameter(NamespaceOrTypeResolverInfo info)
    {
      if (info.ParameterScope as MethodDeclaration == null) return false;
      TypeParameter typeParam;
      if (!info.ParameterScope.TypeParameters.TryGetValue(info.CurrentPart.Name, 
        out typeParam)) return false;

      // --- A this point we resolved the name to a method type parameter
      info.Target = ResolutionTarget.MethodTypeParameter;
      info.CurrentPart.ResolveToMethodTypeParameter(typeParam);
      return true;
    }

    #endregion

    #region ResolveNameInTypeBody method

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves a generic type parameter.
    /// </summary>
    /// <param name="info">Resolution information</param>
    /// <returns>
    /// True, if a generic methodparameter has been found and successfully resolved;
    /// otherwise, false.
    /// </returns>
    /// <remarks>
    /// <para>
    /// If there in no generic type in the current resolution context, returns false.
    /// </para>
    /// <para>
    /// The method modified the resolution information accoding to resolution progress.
    /// </para>
    /// </remarks>
    // --------------------------------------------------------------------------------
    private bool ResolveNameInTypeBody(NamespaceOrTypeResolverInfo info)
    {
      // --- Exit if the current context is not within a type declaration
      if (info.ParameterScope == null) return false;
      TypeDeclaration scope = info.ParameterScope as TypeDeclaration;
      if (scope == null) scope = info.ParameterScope.DeclaringType;
      if (scope == null) return false;

      // --- At this point we are within a type declaration
      while (scope != null)
      {
        // --- Step 1: Try to resolve as a type parameter
        TypeParameter typeParam;
        if (scope.TypeParameters.TryGetValue(info.CurrentPart.Name, out typeParam))
        {
          info.Target = ResolutionTarget.TypeParameter;
          info.CurrentPart.ResolveToTypeParameter(typeParam);
          return true;
        }

        // --- Step 2: Try to resolve name as a nested type
        ResolutionNodeBase node = scope.ResolverNode.
          FindSimpleNamespaceOrType(info.CurrentPart);
        if (node != null)
        {
          SetResultByResolutionNode(info.CurrentPart, node);
          info.SetResultNode(node);
          info.Evaluate();
          return true;
        }
        scope = scope.DeclaringType;
      }
      return false;
    }

    #endregion

    #region ResolveSimpleNameInScope method

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves a simple (one part) name in the current scope.
    /// </summary>
    /// <param name="info">Resolution information</param>
    /// <remarks>
    /// <para>
    /// This method check for the simple name in the scope from the enclosing 
    /// namespace to the global namespace.
    /// </para>
    /// <para>
    /// The method modified the resolution information accoding to resolution progress.
    /// </para>
    /// </remarks>
    // --------------------------------------------------------------------------------
    private void ResolveSimpleNameInScope(NamespaceOrTypeResolverInfo info)
    {
      // --- Cycle from the current declaration namespace to the global namespace
      SourceFile file = info.DeclarationScope.EnclosingSourceFile;
      NamespaceFragment ns = info.DeclarationScope.DeclaringNamespace;
      ITypeDeclarationScope scopeToCheck = null;
      foreach (ITypeDeclarationScope scope in file.GetScopesToOuter(ns))
      {
        bool resolved = false;
        bool needsModeCheck = false;
        // --- Check, if the name can be found in the current scope as a namespace
        // --- or a type name.
        ResolutionNodeList results = scope.FindSimpleName(info.CurrentPart);
        if (results.Count > 0)
        {
          // --- We found the a namespace ot type name but it needs some more checks
          resolved = true;
          scopeToCheck = scope;
          needsModeCheck = true;
        }
        // --- Check, if the name refers to a using-alias or an extern alias.
        else if (ResolveAliasInScope(info.CurrentPart, scope, out results))
        {
          // --- We found an alias and need no more checks.
          resolved = true;
        }
        // --- Check, if the name is within the imported (using) namespaces.
        else if (ResolveTypeNameWithUsings(info.CurrentPart, scope, out results))
        {
          // --- Exit, if a conflict found and cannot be resolved.
          if (!ResolveConflict(results, info.CurrentPart)) break;

          // --- The resolved node must be a type for successful name resolution.
          if (results[0] is TypeResolutionNode)
          {
            // --- We found a type and need no more checks.
            resolved = true;
          }
        }

        if (resolved)
        {
          SetResultByResolutionNode(info.CurrentPart, results[0]);
          info.SetResultNode(results);
          info.Evaluate();
          if (needsModeCheck) break;
          else return;
        }
      }

      // --- Quit, if we found no resolution.
      if (!info.IsResolved)
      {
        _Parser.Error0246(info.CurrentPart.Token, info.CurrentPart.Name);
        return;
      }

      // --- We are going to check the enclosing scope
      ITypeDeclarationScope foundInScope;

      // --- Check 1: We found a namespace and any of the enclosing namespaces 
      // --- contains an alias with the name found.
      if (info.Target == ResolutionTarget.Namespace
        && info.CurrentPart.Arguments.Count == 0)
      {
        if (scopeToCheck != null 
          && scopeToCheck.ContainsAlias(info.CurrentPart.Name, out foundInScope))
        {
          _Parser.Error0576(info.CurrentPart.Token, foundInScope.ScopeName, 
            info.CurrentPart.Name);
          info.Unresolve();
        }
        return;
      }

      // --- Check 2: We found a visible non-generic type and any of the enclosing
      // --- namespaces contains an alias with the name found.
      else if (info.Target == ResolutionTarget.Type 
        && info.CurrentPart.ResolvingType.IsVisible)
      {
        if (info.CurrentPart.Arguments.Count == 0 
          && scopeToCheck.ContainsAlias(info.CurrentPart.Name, out foundInScope))
        {
          _Parser.Error0576(info.CurrentPart.Token, foundInScope.ScopeName,
            info.CurrentPart.Name);
          info.Unresolve();
        }
        return;
      }
    }

    #endregion

    #region ResolveAliasInScope method

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves a simple (one part) name in the current scope as a using-alias or
    /// as an extern-alias.
    /// </summary>
    /// <param name="type">Type reference representing the name to resolve.</param>
    /// <param name="scope">Scope used to search for alias declaration.</param>
    /// <param name="results">Contains the nodes representing the alias.</param>
    /// <returns>
    /// True, if the alias found in the scope; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    private bool ResolveAliasInScope(TypeReference type, ITypeDeclarationScope scope,
      out ResolutionNodeList results)
    {
      results = null;

      // --- Generic names cannot be aliases.
      if (type.Arguments.Count > 0) return false;
      // --- Check for using-alias
      UsingClause usingAlias;
      if ((usingAlias = scope.Usings[type.Name]) != null && usingAlias.IsResolved)
      {
        // --- We have found a using alias
        results = usingAlias.Resolvers;
        return true;
      }

      // --- Check for extern-alias
      ExternalAlias externAlias;
      if ((externAlias = scope.ExternAliases[type.Name]) != null && externAlias.Hierarchy != null)
      {
        // --- We have found an external alias
        results = new ResolutionNodeList(externAlias.Hierarchy);
        return true;
      }
      return false;
    }

    #endregion

    #region ResolveTypeNameWithUsings method

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves a type represented by the specified type reference within the 
    /// namespaces imported by the using directives of the current scope.
    /// </summary>
    /// <param name="type">Type reference representing the name to be found.</param>
    /// <param name="scope">Scope used to serarch.</param>
    /// <param name="results">Contains the nodes representing the type found.</param>
    /// <returns>
    /// True, if the name found in namespaces of this scope; otherwise, false.
    /// </returns>
    /// <remarks>
    /// This method looks only for types within the current namespaces.
    /// </remarks>
    // --------------------------------------------------------------------------------
    private bool ResolveTypeNameWithUsings(TypeReference type, ITypeDeclarationScope scope,
      out ResolutionNodeList results)
    {
      // --- We make a list of nodes represented by the set of using clauses.
      ResolutionNodeList forestNodes = new ResolutionNodeList();
      foreach (UsingClause usingClause in scope.Usings)
      {
        if (usingClause.IsResolved && !usingClause.HasAlias)
        {
          forestNodes.Merge(usingClause.Resolvers);
        }
      }

      // --- Search the forest of namespaces defined by the using clauses
      TypeReference lastResolved;
      ResolveNamespaceOrNameInForest(type, forestNodes, out results, out lastResolved);
      return results.Count > 0;
    }

    #endregion

    #region ResolveCompoundNameInScope

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves a compound (N.I form) name in the current scope.
    /// </summary>
    /// <param name="info">Resolution information</param>
    /// <remarks>
    /// <para>
    /// This method check for the simple name in the scope from the enclosing 
    /// namespace to the global namespace.
    /// </para>
    /// <para>
    /// The method modified the resolution information accoding to resolution progress.
    /// </para>
    /// </remarks>
    // --------------------------------------------------------------------------------
    private void ResolveCompoundNameInScope(NamespaceOrTypeResolverInfo info)
    {
      // --- This name is in N.I<A1, ..., Ak> form. First we resolve the N part, later
      // --- use it to search for the I<A1, ..., Ak> part.

      // --- Serach for the I<A1, ..., Ak> part
      TypeReference partI = info.Type.RightMostPart;
      TypeReference partNLast = partI.PrefixType;
      try
      {
        // --- Cut the 'N' and 'I' part
        partNLast.SubType = null;

        // --- Resolve the 'N' part
        ResolveNamespaceOrTypeName(info);
        if (!info.IsResolved) return;

        // --- At this point 'N' is successfully resolved. Try to resolve 'I' in the 
        // --- context of 'N'
        ResolutionNodeList results;
        TypeReference lastResolved;
        ResolveNamespaceOrNameInForest(partI, info.Results, out results, out lastResolved);
        if (results.Count > 0)
        {
          info.CurrentPart = lastResolved;
          info.SetResultNode(results);
          info.Evaluate();
          return;
        }
        
        // --- At this point type cannot be resolved. Raise an appropriate error message
        if (info.Target == ResolutionTarget.Namespace)
        {
          _Parser.Error0234(partI.Token, partI.Name, info.Type.FullName);
        }
        else
        {
          _Parser.Error0426(partI.Token, partI.Name, info.Type.FullName);
        }
      }
      finally
      {
        // --- Merge the N and I part again
        partNLast.SubType = partI;
      }
    }

    #endregion

    #region Other methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Sets the resolver of a type reference according to the specified 
    /// resolution node.
    /// </summary>
    /// <param name="type">Type reference</param>
    /// <param name="node">Resolver node</param>
    // --------------------------------------------------------------------------------
    private void SetResultByResolutionNode(TypeReference type, ResolutionNodeBase node)
    {
      if (type == null) throw new ArgumentNullException("type");
      if (node == null) return;

      // --- At this point the specified type part is resolved.
      if (node.IsNamespace)
      {
        type.ResolveToNamespace(node);
      }
      else if (node.IsType)
      {
        type.ResolveToType(node as TypeResolutionNode);
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks for conflict within the specified result set and resolves them.
    /// </summary>
    /// <param name="results">Results of the name resolution.</param>
    /// <param name="typePart">
    /// Type reference representing the name that should be used in messages.
    /// </param>
    /// <returns>
    /// True, if there are no conflicts or the conflicts have been resolved;
    /// otherwise, false.
    /// </returns>
    /// <remarks>
    /// If there is a promoted types (type that wins in a conflict), then demoted
    /// types are removed from results. In case of type promotion an appropriate 
    /// warning is raised.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public bool ResolveConflict(ResolutionNodeList results, 
      TypeReference typePart)
    {
      // --- No conflict?
      if (results.Count == 1) return true;

      // --- In other cases we may have a conflict.
      // --- Separate the resolved names into four categories:
      // --- Category 1: Namespace declarations in referenced assemblies
      // --- Category 2: Namespace declarations in our source code
      // --- Category 3: Type declarations in referenced assemblies
      // --- Category 4: Type declaration in our source code
      List<NamespaceResolutionNode> nsInAsm = new List<NamespaceResolutionNode>();
      List<NamespaceResolutionNode> nsInCode = new List<NamespaceResolutionNode>();
      List<TypeResolutionNode> tyInAsm = new List<TypeResolutionNode>();
      List<TypeResolutionNode> tyInCode = new List<TypeResolutionNode>();
      foreach (ResolutionNodeBase item in results)
      {
        NamespaceResolutionNode nsNode = item as NamespaceResolutionNode;
        if (nsNode != null)
        {
          if (nsNode.DefinedInSource) nsInCode.Add(nsNode);
          else nsInAsm.Add(nsNode);
        }
        TypeResolutionNode tyNode = item as TypeResolutionNode;
        if (tyNode != null)
        {
          if (tyNode.Resolver.DeclaringUnit is ReferencedCompilation) tyInCode.Add(tyNode);
          else tyInAsm.Add(tyNode);
        }
      }

      // --- Check for potential namespace conflicts
      // --- Namespace conflict 1: Only namespace found? This means no conflict.
      if (nsInCode.Count + nsInAsm.Count > 0 && tyInCode.Count + tyInAsm.Count == 0)
      {
        return true;
      }

      // --- Namespace conflict 2: Assembly namespace/source type conflict?
      if (nsInAsm.Count > 0 && tyInCode.Count > 0)
      {
        _Parser.Warning0437(typePart.Token, tyInCode[0].Name,
                           nsInAsm[0].Name);
        ResolutionNodeBase winner = tyInCode[0];
        results.Clear();
        results.Add(winner);
        SetResultByResolutionNode(typePart, winner);
        return true;
      }

      // --- Namespace conflict 3: Assembly namespace/assembly type conflict?
      if (nsInAsm.Count > 0 && tyInAsm.Count > 0)
      {
        _Parser.Error0434(typePart.Token, nsInAsm[0].FullName, "referenced assembly",
                           tyInAsm[0].FullName, tyInAsm[0].Resolver.DeclaringUnit.Name);
        ResolutionNodeBase winner = tyInCode[0];
        results.Clear();
        results.Add(winner);
        SetResultByResolutionNode(typePart, winner);
        return true;
      }

      // --- Check potential type conflicts
      // --- At this point tyInCode.Count equals to 0 or 1, because if the source would
      // --- have defined the same type more than once, we had also detected it.
      // --- Type conflicts can arrive only if we found the type in any assembly.
      if (tyInAsm.Count == 0) return true;

      // --- Further we work only with the count visible types in the assembly.
      List<TypeResolutionNode> visibleTypes = new List<TypeResolutionNode>();
      foreach (TypeResolutionNode node in tyInAsm)
      {
        if (node.Resolver.IsVisible) visibleTypes.Add(node);
      }

      if (visibleTypes.Count > 1)
      {
        // --- That is a real type conflict.
        // --- This point we have more than one visible types.
        _Parser.Error0433(typePart.Token, typePart.Name,
                          visibleTypes[0].Resolver.DeclaringUnit.Name,
                          visibleTypes[1].Resolver.DeclaringUnit.Name);
        return false;
      }

      if (visibleTypes.Count == 1) 
      {
        ResolutionNodeBase winner;
        if (nsInCode.Count > 0)
        {
          // --- Assembly type/source namespace conflict!
          _Parser.Warning0435(typePart.Token, nsInCode[0].Name,
                              tyInAsm[0].Resolver.DeclaringUnit.Name);
          winner = nsInCode[0];
        }
        else if (tyInCode.Count > 0)
        {
          // --- Assembly type/source type conflict!
          _Parser.Warning0436(typePart.Token, tyInCode[0].Resolver.FullName,
                              tyInAsm[0].Resolver.DeclaringUnit.Name);
          winner = tyInCode[0];
        }
        else
        {
          return true;
        }
        results.Clear();
        results.Add(winner);
        SetResultByResolutionNode(typePart, winner);
        return true;
      }

      // --- If we are here, there is a conflict case we did not handle...
      throw new InvalidOperationException(
        String.Format("Name conflict not handled. tyInAsm={0}, tyInCode={1}, " +
                      "nsInAsm={2}, nsInCode={3}",
                      tyInAsm.Count, tyInCode.Count, nsInAsm.Count, nsInCode.Count));
    }

    #endregion
  }

  #region NamespaceOrTypeResolverInfo

  // ==================================================================================
  /// <summary>
  /// This class contains all information that is required and used during the 
  /// resolution of a namespace name or a type name.
  /// </summary>
  // ==================================================================================
  public sealed class NamespaceOrTypeResolverInfo
  {
    #region Private fields

    // --- Input fields
    private readonly NamespaceOrTypeResolver _Resolver;
    private readonly TypeReference _Type;
    private readonly ResolutionContext _ContextType;
    private readonly ITypeDeclarationScope _DeclarationScope;
    private readonly ITypeParameterScope _ParameterScope;

    // --- Resolution state fields
    private NamespaceHierarchy _Hierarchy;
    private TypeReference _CurrentPart;
    private ResolutionTarget _Target;
    private readonly ResolutionNodeList _Results = new ResolutionNodeList();

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new instance of this resolution information using the specified type
    /// and context information.
    /// </summary>
    /// <param name="resolver">Resolver that uses this information.</param>
    /// <param name="type">Type reference representing the name to be resolved.</param>
    /// <param name="contextType">Type of resolution context.</param>
    /// <param name="declarationScope">Current type declaration context.</param>
    /// <param name="parameterScope">Current type parameter declaration scope.</param>
    // --------------------------------------------------------------------------------
    public NamespaceOrTypeResolverInfo(NamespaceOrTypeResolver resolver,
      TypeReference type, ResolutionContext contextType,
      ITypeDeclarationScope declarationScope,
      ITypeParameterScope parameterScope)
    {
      _Resolver = resolver;
      _Type = type;
      _ContextType = contextType;
      _DeclarationScope = declarationScope;
      _ParameterScope = parameterScope;
      _Hierarchy = null;
      _CurrentPart = type;
      _Target = ResolutionTarget.Unresolved;
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type reference representing the name to be resolved.
    /// </summary>
    // --------------------------------------------------------------------------------
    public TypeReference Type
    {
      get { return _Type; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type of resolution context.
    /// </summary>
    // --------------------------------------------------------------------------------
    public ResolutionContext ContextType
    {
      get { return _ContextType; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the current type declaration scope
    /// </summary>
    // --------------------------------------------------------------------------------
    public ITypeDeclarationScope DeclarationScope
    {
      get { return _DeclarationScope; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the current type parameter declaration scope
    /// </summary>
    // --------------------------------------------------------------------------------
    public ITypeParameterScope ParameterScope
    {
      get { return _ParameterScope; }
    } 
    
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the hierarchy used to resolve the name.
    /// </summary>
    // --------------------------------------------------------------------------------
    public NamespaceHierarchy Hierarchy
    {
      get { return _Hierarchy; }
      internal set { _Hierarchy = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the part of the name that is currently under resolution.
    /// </summary>
    // --------------------------------------------------------------------------------
    public TypeReference CurrentPart
    {
      get { return _CurrentPart; }
      internal set { _CurrentPart = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the target of the resolution.
    /// </summary>
    // --------------------------------------------------------------------------------
    public ResolutionTarget Target
    {
      get { return _Target; }
      internal set { _Target = value; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the flag indicating that name has been fully resolved.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsResolved
    {
      get { return _Target != ResolutionTarget.Unresolved; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the resolution tree nodes representing the result of resolution.
    /// </summary>
    // --------------------------------------------------------------------------------
    public ResolutionNodeList Results
    {
      get { return _Results; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the parser.
    /// </summary>
    // --------------------------------------------------------------------------------
    public CSharpSyntaxParser Parser
    {
      get { return _Type.Parser; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the compliation unit.
    /// </summary>
    // --------------------------------------------------------------------------------
    public CompilationUnit CompilationUnit
    {
      get { return _Type.Parser.CompilationUnit; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if there are any parts to resolve or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool HasPartsLeft
    {
      get { return _Type.SubType != null; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the resolver currently set.
    /// </summary>
    // --------------------------------------------------------------------------------
    public ResolutionNodeBase Result
    {
      get { return _Results[0]; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks, if the name is resolved to a type within the souce code.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsResolvedToSourceType
    {
      get
      {
        return IsResolved &&
          (
            (
              _Target != ResolutionTarget.NamespaceHierarchy &&
              _Target != ResolutionTarget.Namespace &&
              _Target == ResolutionTarget.Type
            )
            || Result.Root == CompilationUnit.SourceResolutionTree
          );
      }
    }

    #endregion

    #region Public methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Moves to the next part to resolve.
    /// </summary>
    /// <returns>
    /// True, if move is successful; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    public bool MoveToNextPart()
    {
      if (_CurrentPart.SubType == null) return false;
      _CurrentPart = _Type.SubType;
      return true;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Sets the nodes representing resolution results to the collection of nodes
    /// specified.
    /// </summary>
    /// <param name="list">Collection of nodes.</param>
    // --------------------------------------------------------------------------------
    public void SetResultNode(IEnumerable<ResolutionNodeBase> list)
    {
      _Results.Clear();
      _Results.Merge(list);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Sets the node representing resolution result to the specified node.
    /// </summary>
    /// <param name="node">Node resolving the name.</param>
    // --------------------------------------------------------------------------------
    public void SetResultNode(ResolutionNodeBase node)
    {
      _Results.Clear();
      _Results.Add(node);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Evaluates the current state of the resolution.
    /// </summary>
    /// <remarks>
    /// If all name parts are resolved, signs the full resolution. If there are more
    /// resulting nodes, resolves namespace/type/type ambiguity conflicts.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public void Evaluate()
    {
      // --- Evaluate only if type is fully resolved.
      if (_CurrentPart.SubType != null) return;

      // --- No resolution found?
      if (_Results.Count == 0)
      {
        _Target = ResolutionTarget.Unresolved;
        return;
      }

      // --- At this point Result contains all nodes. 
      _Target = _CurrentPart.Target;
      if (!_Resolver.ResolveConflict(_Results, _CurrentPart))
      {
        _Target = ResolutionTarget.Unresolved;
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Signs that the name is not resolved.
    /// </summary>
    // --------------------------------------------------------------------------------
    public void Unresolve()
    {
      _CurrentPart.Unresolve();
      _Target = ResolutionTarget.Unresolved;
    }

    #endregion

  }

  #endregion
}