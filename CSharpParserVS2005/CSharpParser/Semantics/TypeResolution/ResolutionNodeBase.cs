using System;
using System.Collections.Generic;
using System.IO;

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
      if (string.IsNullOrEmpty(name))
      {
        throw new ArgumentNullException("name");
      }
      _ParentNode = parentNode;
      _Name = name;
      if (parentNode != null) parentNode.Children.Add(name, this);
    }

    #endregion

    #region Public properties

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
    /// Gets the child nodes belonging to this node.
    /// </summary>
    // --------------------------------------------------------------------------------
    public Dictionary<string, ResolutionNodeBase> Children
    {
      get { return _Children; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if this node has any child or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool HasChild
    {
      get { return _Children.Count > 0; }
    }

    #endregion

    #region Public methods

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
    public bool RegisterNamespace(string nameSpace, out NamespaceResolutionNode node,
      out ResolutionNodeBase conflictingNode)
    {
      string[] parts = nameSpace.Split('.');
      ResolutionNodeBase currentNode = this;
      conflictingNode = null;
      node = null;
      int partIndex = 0;

      // --- Search for existing nodes
      while (partIndex < parts.Length)
      {
        ResolutionNodeBase nextNode;
        if (!currentNode.Children.TryGetValue(parts[partIndex], out nextNode))
        {
          // --- No further part can be found.
          break;
        }

        // --- nextNode contains the part found. It can only be a NamespaceResolution
        // --- node. In any other case it is a name collision with a type.
        node = nextNode as NamespaceResolutionNode;
        if (node == null)
        {
          conflictingNode = nextNode;
          return false;
        }

        // --- OK, this is a namespace resolution node, so move to the next part.
        currentNode = nextNode;
        partIndex++;
      }

      // --- At this point currentNode contains the last part that can be found.
      // --- All the remaining namespace parts should be created.
      while (partIndex < parts.Length)
      {
        node = new NamespaceResolutionNode(currentNode, parts[partIndex++]);
        currentNode = node;
      }

      // --- Now the whole namespace is registered.
      return true;
    }

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
    public bool RegisterType(ITypeCharacteristics type, out TypeResolutionNode node)
    {
      // --- Search for the type node
      ResolutionNodeBase currentNode;
      if (Children.TryGetValue(type.SimpleName, out currentNode))
      {
        // --- currentNode contains the node found. It can only be a TypeResolution
        // --- node. In any other case it is a name collision with a namespace.
        node = currentNode as TypeResolutionNode;
        return node != null;
      }

      // --- Register the new type resolution node
      node = new TypeResolutionNode(this, type.SimpleName);
      return true;
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
      foreach (ResolutionNodeBase node in Children.Values)
      {
        node.Trace(tw, depth+1);
      }
    }

#endif

    #endregion
  }
}
