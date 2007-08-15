using System;
using System.Collections.Generic;
using System.Reflection;

namespace CSharpParser.Semantics
{
  // ==================================================================================
  /// <summary>
  /// This class represents a namespace hierarchy that ise used when resolving type
  /// names.
  /// </summary>
  /// <remarks>
  /// During the compilation there is a global namespece hierarchy including the 
  /// types and namespaced declared in the source code plus types in assemblies not 
  /// aliased with the "extern alias" directive.
  /// </remarks>
  // ==================================================================================
  public sealed class NamespaceHierarchy: ResolutionNodeBase
  {
    #region Private fields

    private readonly Dictionary<string, NamespaceResolutionNode> _Cache = 
      new Dictionary<string, NamespaceResolutionNode>();

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new namespace hierarchy with the specified name.
    /// </summary>
    /// <param name="name">Name of the namespace hierarchy.</param>
    // --------------------------------------------------------------------------------
    public NamespaceHierarchy(string name): base(null, name)
    {
    }

    #endregion

    #region Public properties

    #endregion

    #region Public methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Clear all nodes of this hierarchy.
    /// </summary>
    // --------------------------------------------------------------------------------
    public void Clear()
    {
      _Cache.Clear();
      Children.Clear();
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the resolver node belonging to the specified namespace key.
    /// </summary>
    /// <param name="nsKey">Namespace key</param>
    /// <returns>
    /// The resolver node belonging to the namespace if found; otherwise, null.
    /// </returns>
    // --------------------------------------------------------------------------------
    public NamespaceResolutionNode this[string nsKey]
    {
      get
      {
        NamespaceResolutionNode result;
        if (_Cache.TryGetValue(nsKey, out result)) return result;
        return null;
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Imports all types from the specified assembly.
    /// </summary>
    /// <param name="asm">Assembly to import types from</param>
    /// <param name="importNs">Namespace to import</param>
    /// <remarks>
    /// If importNs is null, imports all types. If importNs is an empty string, imports
    /// types within the global namespace; otherwise imports only types in the 
    /// specified namespace.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public void ImportTypes(Assembly asm, string importNs)
    {
      // --- Go through all types an import them one-by-one
      foreach (Type type in asm.GetTypes())
      {
        if (importNs == String.Empty && !String.IsNullOrEmpty(type.Namespace))
          continue;
        if (importNs != null && type.Namespace != importNs)
          continue;
        ResolutionNodeBase nsResolver = ObtainNamespaceResolver(type);
        ImportTypeToHierarchy(nsResolver, type);
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Obtains the namespace resolver for the specified type.
    /// </summary>
    /// <param name="type">Type to obtain the namespace resolver for.</param>
    /// <returns>The namespace resolver of the specified type.</returns>
    // --------------------------------------------------------------------------------
    public ResolutionNodeBase ObtainNamespaceResolver(Type type)
    {
      if (String.IsNullOrEmpty(type.Namespace))
      {
        // --- These types belong to the global namespace hierarchy
        return this;
      }
      // --- Type has an explicit namspace, register it.
      NamespaceResolutionNode nsResolver;
      ResolutionNodeBase conflictingNode;
      if (!RegisterNamespace(type.Namespace, out nsResolver, out conflictingNode))
      {
        throw new InvalidOperationException(
          String.Format("Type and namespace conflict within the assembly: {0}",
                        type.Assembly.FullName));
      }
      return nsResolver;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Imports the type to the specified resolver node and creates the hierarchy
    /// according to type nesting.
    /// </summary>
    /// <param name="resolverRoot">Resolver node</param>
    /// <param name="type">Type to import.</param>
    /// <returns>Type resolution node of the specified type.</returns>
    /// <remarks>
    /// If the type is a nested type, first imports its declaring type.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public TypeResolutionNode ImportTypeToHierarchy(ResolutionNodeBase resolverRoot, 
      Type type)
    {
      if (type.DeclaringType == null)
      {
        // --- This is a simple type
        return ImportType(resolverRoot, type);
      }
      else
      {
        // --- This is a type nested in an other type. We must provide that the declaring 
        // --- type is imported.
        TypeResolutionNode typeResolver = 
          ImportTypeToHierarchy(resolverRoot, type.DeclaringType);
        return ImportType(typeResolver, type);
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Imports the type to the specified resolver node.
    /// </summary>
    /// <param name="resolverRoot">Resolver node</param>
    /// <param name="type">Type to import.</param>
    /// <returns>Type resolution node of the specified type.</returns>
    // --------------------------------------------------------------------------------
    public TypeResolutionNode ImportType(ResolutionNodeBase resolverRoot, Type type)
    {
      // --- Wrap the type
      NetBinaryType binType = new NetBinaryType(type);
      TypeResolutionNode typeResolver;
      // --- This is a type not nested in any type. Register it for the root resolver node.
      ResolutionNodeBase resolver;
      if (resolverRoot.Children.TryGetValue(binType.SimpleName, out resolver))
      {
        // --- There is a node with this type name
        typeResolver = resolver as TypeResolutionNode;
        if (typeResolver == null)
        {
          throw new InvalidOperationException(
            String.Format("Type and namespace conflict within the assembly: {0}",
            type.Assembly.FullName));
        }
        foreach (ITypeCharacteristics resolvedType in typeResolver.Resolvers)
        {
          if (resolvedType.Compilation.Name.Equals(type.Assembly.GetName().Name))
          {
            // --- This type has already been resolved by the assembly of this type.
            return typeResolver;
          }
        }
      }
      else
      {
        // --- There is no node for this type
        typeResolver = new TypeResolutionNode(resolverRoot, binType.SimpleName);
      }

      // --- At this point we add a resolver to this type node
      typeResolver.AddTypeResolver(binType);
      return typeResolver;
    }

    #endregion
  }
}
