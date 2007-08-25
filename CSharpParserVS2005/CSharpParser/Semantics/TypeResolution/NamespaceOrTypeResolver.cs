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
    /// <param name="contextObject">Object representing the current context.</param>
    /// <returns>Name resolution information.</returns>
    /// <remarks>
    /// </remarks>
    // --------------------------------------------------------------------------------
    public  NamespaceOrTypeResolverInfo Resolve(TypeReference type, 
      ResolutionContext contextType, ITypeDeclarationScope contextObject)
    {
      // --- Check all inputs
      if (type == null) throw new ArgumentNullException("type");
      if (contextObject == null) throw new ArgumentNullException("contextObject");

      // --- Create the resolution structure
      NamespaceOrTypeResolverInfo resolutionInfo = 
        new NamespaceOrTypeResolverInfo(this, type, contextType, contextObject);

      // --- Branch: "qualified-alias-member" | "namespace-or-type"
      if (type.IsGlobal) ResolveQualifiedAliasMember(resolutionInfo);
      else ResolveNamespaceOrTypeName(resolutionInfo);
      return resolutionInfo;
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

      if (!info.Type.HasSubType)
      {
        // --- The name in if form of I or I<A1, ... Ak>
        // --- Check for generic method parameter
        if (info.Type.Arguments.Count == 0)
        {
          if (ResolveGenericMethodParameter(info)) return;
        }

        // --- Check for generic type parameter
        if (ResolveGenericTypeParameter(info)) return;
        ResolveSimpleNameInScope(info);
        if (info.IsResolved) return;

        // --- Name cannot be fully resolved according to resolution roles
        // info.Parser.Error0400(info.CurrentPart.Token, info.CurrentPart.Name);
      }
      else
      {
        // --- The name in if form of N.I or N.I<A1, ... Ak>
        
      }
    }

    #endregion

    #region ResolveNamespaceOrNameInForest

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
    /// Uses the resolution tree nodes in the info.Result container. Results all node
    /// and retrieves the list of nodes that match the resolution. During the 
    /// resolution no extern and using alias definitions are used.
    /// </para>
    /// </remarks>
    // --------------------------------------------------------------------------------
    private void ResolveNamespaceOrNameInForest(NamespaceOrTypeResolverInfo info)
    {
      // --- Init name resolution
      ResolutionNodeList result = new ResolutionNodeList();
      TypeReference lastResolved = info.CurrentPart;
      int maxResolutionLength = 0;
      foreach (ResolutionNodeBase treeNode in info.Results)
      {
        TypeReference carryOnPart;

        // --- Resolve the name in the current tree
        ResolutionNodeBase resolvingNode = 
          ResolveNameOrNamespaceInTree(info, treeNode, out carryOnPart);
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
      info.CurrentPart = lastResolved;
      info.SetResultNode(result);
      info.Evaluate();
    }

    #endregion

    #region ResolveNameOrNamespaceInTree method

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Resolves a namespace or type name using the resolution tree node specified.
    /// </summary>
    /// <param name="info">Resolution information</param>
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
      NamespaceOrTypeResolverInfo info, 
      ResolutionNodeBase node, 
      out TypeReference lastResolvedPart)
    {
      TypeReference type = info.CurrentPart;
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
        nextNode = currentNode.FindChild(type.Name);

        // --- No more parts can be resolved? Quit in this case.
        if (nextNode == null) return null;

        // --- If the current node is a TypeNameResolutionNode, we must look for the 
        // --- next TypeResolutionNode.
        TypeNameResolutionNode nameNode = nextNode as TypeNameResolutionNode;
        if (nameNode != null)
        {
          // --- We are dealing with a type.
          nextNode = nameNode.FindChild(type.Arguments.Count.ToString());
          if (nextNode == null)
          {
            // --- We found the part name but not the one with correct number of
            // --- type parameters.
            return null;
          }
        }

        // --- Name part successfully resolved. 
        SetResolutionResult(type, nextNode);

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
      return false;
    }

    #endregion

    #region ResolveGenericTypeParameter method

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
    private static bool ResolveGenericTypeParameter(NamespaceOrTypeResolverInfo info)
    {
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
      SourceFile file = info.ContextObject.EnclosingSourceFile;
      NamespaceFragment ns = info.ContextObject.EnclosingNamespace;
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
          needsModeCheck = true;
        }
        // --- Check, if the name refers to a using-alias or an extern alias.
        else if (ResolveAliasInScope(info.CurrentPart, scope, out results))
        {
          // --- We found an alias and need no more checks.
          resolved = true;
        }
        // --- Check, if the name is within the imported (using) namespaces.

        if (resolved)
        {
          SetResolutionResult(info.CurrentPart, results[0]);
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
      ITypeDeclarationScope scopeToCheck =
        ((ITypeDeclarationScope)(info.ContextObject.EnclosingNamespace)) ?? file;
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

    #region Other methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Sets the resolver of a type reference according to the specified 
    /// resolution node.
    /// </summary>
    /// <param name="type">Type reference</param>
    /// <param name="node">Resolver node</param>
    // --------------------------------------------------------------------------------
    private void SetResolutionResult(TypeReference type, ResolutionNodeBase node)
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
    /// Type reference representing the name that should be used in messages.</param>
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
    private bool ResolveConflict(ResolutionNodeList results, 
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
      // --- Namespace conflict 1:Only namespace found? This means no conflict.
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
        SetResolutionResult(typePart, winner);
        return true;
      }

      // --- Check potential type conflicts
      // --- Type conflicts can arrive only if we found the type in any assembly.
      if (tyInAsm.Count == 0) return true;

      // --- At this point tyInCode.Count equals to 1, because if the source would
      // --- have defined the same type more than once, we had also detected it.
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
        else
        {
          // --- Assembly type/source type conflict!
          // --- Do not forget: at this point tyInCode.Count equals 1!
          _Parser.Warning0436(typePart.Token, tyInCode[0].Resolver.FullName,
                              tyInAsm[0].Resolver.DeclaringUnit.Name);
          winner = tyInCode[0];
        }
        results.Clear();
        results.Add(winner);
        SetResolutionResult(typePart, winner);
        return true;
      }

      // --- If we are here, there is a conflict case we did not handle...
      throw new InvalidOperationException(
        String.Format("Name conflict not handled. tyInAsm={0}, tyInCode={1}, " +
                      "nsInAsm={2}, nsInCode={3}",
                      tyInAsm.Count, tyInCode.Count, nsInAsm.Count, nsInCode.Count));
    }

    #endregion

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
      private readonly ITypeDeclarationScope _ContextObject;

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
      /// <param name="type">Type reference representing the name to be resolved.</param>
      /// <param name="contextType">Type of resolution context.</param>
      /// <param name="contextObject">Object representing the current context.</param>
      // --------------------------------------------------------------------------------
      public NamespaceOrTypeResolverInfo(NamespaceOrTypeResolver resolver, 
        TypeReference type, ResolutionContext contextType,
        ITypeDeclarationScope contextObject)
      {
        _Resolver = resolver;
        _Type = type;
        _ContextType = contextType;
        _ContextObject = contextObject;
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
      /// Gets the object representing the current resolution context.
      /// </summary>
      // --------------------------------------------------------------------------------
      public ITypeDeclarationScope ContextObject
      {
        get { return _ContextObject; }
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

      #endregion

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
    }

    #endregion
  }
}