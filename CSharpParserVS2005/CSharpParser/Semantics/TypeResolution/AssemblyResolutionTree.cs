using System;
using System.Collections.Generic;
using System.Reflection;

namespace CSharpParser.Semantics
{
  // ==================================================================================
  /// <summary>
  /// This class represents a resolving unit (a referenced assembly) that contains a
  /// hierarchy of namespaces and types.
  /// </summary>
  /// <remarks>
  /// The parser uses this resolution unit to resolve namespaces and type names.
  /// A namespace hierarchy may contain one or more resolution unit.
  /// </remarks>
  // ==================================================================================
  public sealed class AssemblyResolutionTree : TypeResolutionTree
  {
    #region Private fields

    private readonly Assembly _Assembly;
    // --- Contains a list of imported namespaces to check before duplicate import
    private readonly List<string> _ImportedNamespaces =
      new List<string>();

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new resolution unit for the specified assembly.
    /// </summary>
    /// <param name="assembly">
    /// Assembly that contains types used by this resolution unit
    /// </param>
    // --------------------------------------------------------------------------------
    public AssemblyResolutionTree(Assembly assembly)
      : base(null, assembly.FullName)
    {
      _Assembly = assembly;
      CollectNamespaces();
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the list of imported namespaces
    /// </summary>
    // --------------------------------------------------------------------------------
    public List<string> ImportedNamespaces
    {
      get { return _ImportedNamespaces; }
    }

    #endregion

    #region Public methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks if the types in the specified namespace are imported in this hierarchy 
    /// or not.
    /// </summary>
    /// <param name="nsKey">Full namespace name</param>
    /// <returns>
    /// True, if the namespace has already been imported; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    public bool IsImported(string nsKey)
    {
      return _ImportedNamespaces.Contains(nsKey);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks if the types in the specified namespace are imported in this hierarchy 
    /// or not.
    /// </summary>
    /// <param name="nsKey">Full namespace name</param>
    /// <returns>
    /// True, if the namespace has already been imported; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    public void SignNamespaceIsImported(string nsKey)
    {
      _ImportedNamespaces.Add(nsKey);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Collects namespace information from this resolution unit
    /// </summary>
    // --------------------------------------------------------------------------------
    public void CollectNamespaces()
    {
      // --- Store namespaces already imported to avoid repeated processing
      Dictionary<string, int> nsCache = new Dictionary<string, int>();

      // --- This is the first time we import namespaces from this assembly. We go
      // --- through its types to obtain namespace names.
      foreach (Type type in _Assembly.GetTypes())
      {
        if (!String.IsNullOrEmpty(type.Namespace) && !nsCache.ContainsKey(type.Namespace))
        {
          // --- This namespace is not collected yet from this assembly instance
          AddNamespace(type.Namespace, _Assembly.FullName);
          nsCache.Add(type.Namespace, 0);
        }
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Add a namespace to the resolution unit.
    /// </summary>
    /// <param name="ns">Namespace information</param>
    /// <param name="resolverName">Name of resolver registering the namespace.</param>
    // --------------------------------------------------------------------------------
    private void AddNamespace(string ns, string resolverName)
    {
      NamespaceResolutionNode nsResolver;
      ResolutionNodeBase conflictingNode;
      if (!RegisterNamespace(ns, out nsResolver, out conflictingNode))
      {
        throw new InvalidOperationException(
          String.Format("Conflict when resolving namespace '{0}' in '{1}'",
                        ns, resolverName));
      }
      return;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Imports the types for the specified namespace.
    /// </summary>
    /// <param name="nameSpace">Namespace</param>
    // --------------------------------------------------------------------------------
    public void ImportNamespace(string nameSpace)
    {
      // --- Check if this tree has the specified namespace
      if (this[nameSpace] == null) return;

      // --- Check if this namespace has already been imported by the hierarchy
      if (IsImported(nameSpace)) return;

      // --- This tree has this namespace but types has not been imported yet.
      foreach (Type type in _Assembly.GetTypes())
      {
        if (nameSpace == String.Empty && !String.IsNullOrEmpty(type.Namespace))
          continue;
        if (nameSpace != null && type.Namespace != nameSpace)
          continue;
        ImportTypeToHierarchy(type);
        SignNamespaceIsImported(nameSpace);
      }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Imports the type to the specified resolver node and creates the hierarchy
    /// according to type nesting.
    /// </summary>
    /// <param name="type">Type to import.</param>
    /// <returns>Type resolution node of the specified type.</returns>
    /// <remarks>
    /// If the type is a nested type, first imports its declaring type.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public TypeResolutionNode ImportTypeToHierarchy(Type type)
    {
      if (type.DeclaringType == null)
      {
        // --- This is a simple type
        return ImportType(this, type);
      }
      else
      {
        // --- This is a type nested in an other type. We must provide that the declaring 
        // --- type is imported.
        TypeResolutionNode typeResolver =
          ImportTypeToHierarchy(type.DeclaringType);
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
            String.Format("Type ({0}) and namespace conflict within the assembly: {1}",
            type.FullName, type.Assembly.FullName));
        }
        foreach (ITypeCharacteristics resolvedType in typeResolver.Resolvers)
        {
          if (resolvedType.DeclaringUnit.Name.Equals(type.Assembly.GetName().Name))
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
