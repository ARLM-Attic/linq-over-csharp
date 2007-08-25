using System;
using System.Collections.Generic;
using System.IO;
using CSharpParser.ProjectModel;

namespace CSharpParser.Semantics
{
  // ==================================================================================
  /// <summary>
  /// This abstract class describes a node of the resolution tree.
  /// </summary>
  // ==================================================================================
  public abstract class ResolutionNodeBase
  {
    #region Private fields

    private readonly ResolutionNodeBase _ParentNode;
    private readonly string _Name;
    private readonly Dictionary<string, ResolutionNodeBase> _Children = 
      new Dictionary<string, ResolutionNodeBase>();
    private readonly int _Depth;

    #endregion

    #region Lifecycle methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new node with the specified parent and name.
    /// </summary>
    /// <param name="parentNode">Parent node.</param>
    /// <param name="name">Node name.</param>
    /// <remarks>
    /// The parent can be null (the node has no parent), name cannot be null or empty.
    /// </remarks>
    // --------------------------------------------------------------------------------
    protected ResolutionNodeBase(ResolutionNodeBase parentNode, string name)
    {
      if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("name");
      _Name = name;
      _ParentNode = parentNode;
      if (parentNode == null)
      {
        _Depth = 0;
      }
      else
      {
        _Depth = parentNode._Depth + 1;
        parentNode._Children.Add(name, this);
      }
    }

    #endregion

    #region Public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the 
    /// </summary>
    // --------------------------------------------------------------------------------
    public int Depth
    {
      get { return _Depth; }
    } 
    
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the parent of this node.
    /// </summary>
    /// <value>
    /// Null, if the node has no parent, otherwise the parent node.
    /// </value>
    // --------------------------------------------------------------------------------
    public ResolutionNodeBase ParentNode
    {
      get { return _ParentNode; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the root of this node (TypeResolutionTree)
    /// </summary>
    // --------------------------------------------------------------------------------
    public ResolutionNodeBase Root
    {
      get { return _ParentNode == null ? this : _ParentNode.Root; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this node has a parent or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool HasParent
    {
      get { return _ParentNode != null; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the name of this node.
    /// </summary>
    // --------------------------------------------------------------------------------
    public string Name
    {
      get { return _Name; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this node has any child or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool HasAnyChild
    {
      get { return _Children.Count > 0; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this node represents a namespace.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsNamespace
    {
      get { return this is NamespaceResolutionNode; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this node represents a type.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool IsType
    {
      get { return this is TypeResolutionNode; }
    }

    #endregion

    #region Public methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Clears the nodes within this resolution node.
    /// </summary>
    // --------------------------------------------------------------------------------
    public virtual void Clear()
    {
      _Children.Clear();
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Finds the child node having the specified name.
    /// </summary>
    /// <param name="key">Name of the child to be found.</param>
    /// <returns>
    /// Child node if found; otherwise, null.
    /// </returns>
    // --------------------------------------------------------------------------------
    public ResolutionNodeBase FindChild(string key)
    {
      ResolutionNodeBase child;
      if (_Children.TryGetValue(key, out child)) return child;
      return null;  
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Finds a simple namespace or type within this node.
    /// </summary>
    /// <param name="type">Represents the name to be found.</param>
    /// <returns>
    /// Child node if found; otherwise, null.
    /// </returns>
    // --------------------------------------------------------------------------------
    public ResolutionNodeBase FindSimpleNamespaceOrType(TypeReference type)
    {
      // --- Check, if the next part of the name can be resolved
      ResolutionNodeBase node = FindChild(type.Name);
      if (node == null) return null;

      // --- If the current node is a TypeNameResolutionNode, we must look for the 
      // --- next TypeResolutionNode.
      TypeNameResolutionNode nameNode = node as TypeNameResolutionNode;
      if (nameNode != null)
      {
        // --- We are dealing with a type.
        node = nameNode.FindChild(type.Arguments.Count.ToString());
        if (node == null)
        {
          // --- We found the part name but not the one with correct number of
          // --- type parameters.
          return null;
        }
      }
      return node;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Checks if this node contains a child node having the specified name.
    /// </summary>
    /// <param name="key">Name of the child to be found.</param>
    /// <returns>
    /// True, if child node found; otherwise, false.
    /// </returns>
    // --------------------------------------------------------------------------------
    public bool HasChild(string key)
    {
      return _Children.ContainsKey(key);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a node within the type resolution tree representing a namespace.
    /// </summary>
    /// <param name="nameSpace">Full namespace to create.</param>
    /// <returns>
    /// Node representing the namespace node created.
    /// </returns>
    /// <remarks>
    /// Creates a separate node for each partof the namespace. For example, 
    /// "System.Collections.Generic" gereates three nodes as 
    /// System --> Collections --> Generic, and the "Generic" node is retreived.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public virtual NamespaceResolutionNode CreateNamespace(string nameSpace)
    {
      string[] parts = nameSpace.Split('.');
      ResolutionNodeBase currentNode = this;
      int partIndex = 0;

      // --- Search for existing nodes
      while (partIndex < parts.Length)
      {
        ResolutionNodeBase nextNode;
        if ((nextNode = currentNode.FindChild(parts[partIndex])) == null)
        {
          // --- No further part can be found.
          break;
        }

        // --- nextNode contains the part found. It can only be a NamespaceResolution
        // --- node. In any other case it is a name collision with a type and this may
        // --- bot occur when calling this method.
        if (nextNode is NamespaceResolutionNode)
        {
          // --- OK, this is a namespace resolution node, so move to the next part.
          currentNode = nextNode;
          partIndex++;
        }
        else
          throw new InvalidOperationException("NamespaceResolutionNode expected.");
      }

      // --- At this point currentNode contains the last part that can be found.
      // --- All the remaining namespace parts should be created.
      while (partIndex < parts.Length)
      {
        currentNode = new NamespaceResolutionNode(currentNode, parts[partIndex++]);
      }

      // --- Now the whole namespace is registered.
      return currentNode as NamespaceResolutionNode;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a node within the type resolution tree representing a type.
    /// </summary>
    /// <param name="type">Type to create the resolution node for.</param>
    /// <returns>
    /// Node representing the type node created.
    /// </returns>
    /// Name of the node representing the type if creation is OK. Null, if the node
    /// with the type name is already reserved for a namespace.
    /// <remarks>
    /// <para>
    /// For each type creates two chained node. The first node is a TypeNameResolutionNode,
    /// the second is a TypeResolutionNode. The first node names the type, the second is
    /// named by the number of generic parameters (0 for non-generic types).
    /// </para>
    /// <para>
    /// For example, for List{T}: 
    /// TypeNameResolutionNode("List") --> TypeResolutionNode("1").
    /// </para>
    /// </remarks>
    // --------------------------------------------------------------------------------
    public virtual TypeResolutionNode CreateType(ITypeCharacteristics type)
    {
      // --- Search for the type node
      ResolutionNodeBase currentNode;
      TypeNameResolutionNode namingNode;
      if ((currentNode = FindChild(type.SimpleName)) != null)
      {
        // --- currentNode contains the node found. It can only be a TypeNameResolution
        // --- node. In any other case it is a name collision with a namespace.
        namingNode = currentNode as TypeNameResolutionNode;
        if (namingNode == null) return null;
      }
      else 
      {
        namingNode = new TypeNameResolutionNode(this, type.SimpleName);
        return new TypeResolutionNode(namingNode, type);
      }

      // --- At this point we have the naming node, lets check for the type node
      if ((currentNode = currentNode.FindChild(type.TypeParameterCount.ToString())) != null)
      {
        TypeResolutionNode resultNode = currentNode as TypeResolutionNode;

        // --- Node at this position must be a TypeResolutionNode
        if (resultNode == null)
        {
          throw new InvalidOperationException("Typeresolution node missing.");
        }
        return resultNode;
      }

      // --- Create the type resolution node for this type
      return new TypeResolutionNode(namingNode, type);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Registers a namespace node.
    /// </summary>
    /// <param name="nameSpace">Namespace to register.</param>
    /// <param name="node">The node representing the namespace.</param>
    /// <param name="conflictingNode">Node causing collision.</param>
    /// <returns>
    /// True, if the node successfully registered; otherwise, false. False actually 
    /// means that there is a name collision with a type name.
    /// </returns>
    /// <remarks>
    /// If the namespace node is already registered, does not create any new node. If
    /// any namespace node is missing, this methods creates that node.
    /// </remarks>
    // --------------------------------------------------------------------------------
    //public virtual bool RegisterNamespace(string nameSpace, out NamespaceResolutionNode node,
    //  out ResolutionNodeBase conflictingNode)
    //{
    //  string[] parts = nameSpace.Split('.');
    //  ResolutionNodeBase currentNode = this;
    //  conflictingNode = null;
    //  node = null;
    //  int partIndex = 0;

    //  // --- Search for existing nodes
    //  while (partIndex < parts.Length)
    //  {
    //    ResolutionNodeBase nextNode;
    //    if ((nextNode = currentNode.FindChild(parts[partIndex])) == null)
    //    {
    //      // --- No further part can be found.
    //      break;
    //    }

    //    // --- nextNode contains the part found. It can only be a NamespaceResolution
    //    // --- node. In any other case it is a name collision with a type.
    //    node = nextNode as NamespaceResolutionNode;
    //    if (node == null)
    //    {
    //      conflictingNode = nextNode;
    //      return false;
    //    }

    //    // --- OK, this is a namespace resolution node, so move to the next part.
    //    currentNode = nextNode;
    //    partIndex++;
    //  }

    //  // --- At this point currentNode contains the last part that can be found.
    //  // --- All the remaining namespace parts should be created.
    //  while (partIndex < parts.Length)
    //  {
    //    node = new NamespaceResolutionNode(currentNode, parts[partIndex++]);
    //    currentNode = node;
    //  }

    //  // --- Now the whole namespace is registered.
    //  return true;
    //}

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Finds the name described by the specified type reference instance.
    /// </summary>
    /// <param name="type">TypeReference representing the name to find.</param>
    /// <param name="resolvedNode">
    /// The node that fully or partially resolved the name.</param>
    /// <param name="nextPart">Next part of the name that cannot be resolved.</param>
    /// <returns>
    /// Number of name fragments successfully resolved.
    /// </returns>
    /// <remarks>
    /// If returns 0, it means that no part of the name could be resolved. If 
    /// 'nextPart' is null, it means the whole name has been susseccfully resolved.
    /// </remarks>
    // --------------------------------------------------------------------------------
    //public int FindName(TypeReference type, out ResolutionNodeBase resolvedNode,
    //  out TypeReference nextPart)
    //{
    //  int depth = 0;
    //  resolvedNode = null;
    //  ResolutionNodeBase node = this;
    //  nextPart = type;
    //  while (nextPart != null && node.Children.TryGetValue(nextPart.Name, out node))
    //  {
    //    // --- We resolved the current part
    //    resolvedNode = node;
    //    nextPart = nextPart.SubType;
    //    depth++;
    //  }
    //  return depth;
    //}

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Registers a type node.
    /// </summary>
    /// <param name="type">Type to register.</param>
    /// <param name="node">The node representing the type.</param>
    /// <returns>
    /// True, if the node successfully registered; otherwise, false. False actually 
    /// means that there is a name collision with a namspace.
    /// </returns>
    /// <remarks>
    /// If the type node is already registered, does not create any new node. Otherwise 
    /// this methods creates the node.
    /// </remarks>
    // --------------------------------------------------------------------------------
    //public virtual bool RegisterType(ITypeCharacteristics type, out TypeResolutionNode node)
    //{
    //  // --- Search for the type node
    //  ResolutionNodeBase currentNode;
    //  if (Children.TryGetValue(type.SimpleName, out currentNode))
    //  {
    //    // --- currentNode contains the node found. It can only be a TypeResolution
    //    // --- node. In any other case it is a name collision with a namespace.
    //    node = currentNode as TypeResolutionNode;
    //    return node != null;
    //  }

    //  // --- Register the new type resolution node
    //  node = new TypeResolutionNode(this, type);
    //  return true;
    //}

    #endregion

    #region Diagnostics

#if DIAGNOSTICS

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Displays the content of this node.
    /// </summary>
    /// <param name="tw">Output</param>
    /// <param name="depth">Padding depth</param>
    // --------------------------------------------------------------------------------
    public void Trace(TextWriter tw, int depth)
    {
      tw.WriteLine("{0}{1}: {2}", "".PadLeft(2*depth, ' '), GetType().Name, Name);
      foreach (ResolutionNodeBase node in _Children.Values)
      {
        node.Trace(tw, depth+1);
      }
    }

#endif

    #endregion
  }
}
