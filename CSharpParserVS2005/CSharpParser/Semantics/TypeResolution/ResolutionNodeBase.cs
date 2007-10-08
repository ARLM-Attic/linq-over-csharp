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

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the full name of the type or namespace represented by this node.
    /// </summary>
    // --------------------------------------------------------------------------------
    public virtual string FullName
    {
      get
      {
        if (_ParentNode == null) return _Name;
        return _ParentNode.FullName + "." + _Name;
      }
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
    /// <remarks>
    /// Navigates only one level from this node.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public ResolutionNodeBase FindChild(string key)
    {
      ResolutionNodeBase child;
      if (_Children.TryGetValue(key, out child)) return child;
      return null;  
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Finds the child node having the specified name using the specified compound 
    /// name.
    /// </summary>
    /// <param name="key">Name of the child to be found.</param>
    /// <returns>
    /// Child node if found; otherwise, null.
    /// </returns>
    /// <remarks>
    /// Navigates down the from this node as many levels as specified by the compound
    /// child name.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public ResolutionNodeBase FindCompoundChild(string key)
    {
      ResolutionNodeBase current = this;
      string[] nameParts = key.Split('.');
      for (int i = 0; i < nameParts.Length; i++)
      {
        if ((current = current.FindChild(nameParts[i])) == null) return null;
      }
      return current;
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
      return FindChild(type.ClrName);
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
    public virtual TypeResolutionNode CreateType(ITypeAbstraction type)
    {
      // --- Search for the type node
      ResolutionNodeBase currentNode;
      if ((currentNode = FindChild(type.Name)) != null)
      {
        // --- currentNode contains the node found. It can only be a TypeNameResolution
        // --- node. In any other case it is a name collision with a namespace.
        return currentNode as TypeResolutionNode;
      }
      return new TypeResolutionNode(this, type);
    }

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
