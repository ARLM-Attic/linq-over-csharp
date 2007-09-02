using System;

namespace CSharpParser.Semantics
{
  // ==================================================================================
  /// <summary>
  /// This class describes a namespace node in the resolution tree.
  /// </summary>
  /// <remarks>
  /// Compound namespaces are represented by cascade namespace nodes. For example,
  /// "System.Collection.Generics" is represented by three nodes: 
  /// "System" --> "Collection" --> "Generics".
  /// </remarks>
  // ==================================================================================
  public sealed class NamespaceResolutionNode : ResolutionNodeBase
  {
    #region Private fields

    private bool _DefinedInSource;
    private bool _TypesAlreadyImported;
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
    public NamespaceResolutionNode(ResolutionNodeBase parentNode, string name)
      : base(parentNode, name)
    {
      _DefinedInSource = false;
      _TypesAlreadyImported = false;
    }

    #endregion

    #region public properties

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if the namespace has been declared in the source
    /// code or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool DefinedInSource
    {
      get { return _DefinedInSource; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if types in this namspace are already imported or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool TypesAlreadyImported
    {
      get { return _DefinedInSource || _TypesAlreadyImported; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the full name of the type or namespace represented by this node.
    /// </summary>
    // --------------------------------------------------------------------------------
    public override string FullName
    {
      get 
      {
        if (ParentNode == null || ParentNode is TypeResolutionTree) return Name;
        return ParentNode.FullName + "." + Name;
      }
    }

    #endregion

    #region Public methods

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Indicates that the namespace has been defined in the source code.
    /// </summary>
    /// <remarks>
    /// </remarks>
    // --------------------------------------------------------------------------------
    public void SignDefinedInSource()
    {
      _DefinedInSource = true;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Imports the types for this namespace.
    /// </summary>
    /// <remarks>
    /// If types are already imported, this method returns immediately.
    /// </remarks>
    // --------------------------------------------------------------------------------
    public void ImportTypes()
    {
      if (TypesAlreadyImported) return;

      // --- Find the parent node of this namespace resolution node
      string nameSpace = string.Empty;
      ResolutionNodeBase currentNode = this;
      while (currentNode.ParentNode != null)
      {
        nameSpace = currentNode.Name + (nameSpace.Length > 0 ? "." : "") + nameSpace;
        currentNode = currentNode.ParentNode;
      }

      // --- If the currentnode refers to a type defined in the source, the namespace
      // --- is already imported.
      if (currentNode is SourceResolutionTree) return;

      // --- At this point we have the parent node. It must be az AssemblyResolutionTree.
      AssemblyResolutionTree asmTree = currentNode as AssemblyResolutionTree;
      if (asmTree == null)
        throw new InvalidOperationException("AssemblyResolutionNode expected.");

      // --- At this point we have all information to import types from an assembly.
      foreach (Type type in asmTree.Assembly.GetTypes())
      {
        if (nameSpace == String.Empty && !String.IsNullOrEmpty(type.Namespace))
          continue;
        if (nameSpace != null && type.Namespace != nameSpace)
          continue;
        ImportTypeToHierarchy(new NetBinaryType(type));
      }

      // --- OK, sign types are already imported.
      _TypesAlreadyImported = true;
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
    public TypeResolutionNode ImportTypeToHierarchy(ITypeCharacteristics type)
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
    public TypeResolutionNode ImportType(ResolutionNodeBase resolverRoot, 
      ITypeCharacteristics type)
    {
      // --- This is a type not nested in any type. Register it for the root resolver node.
      TypeResolutionNode resolver;
      if ((resolver = resolverRoot.CreateType(type)) == null)
      {
        throw new InvalidOperationException(
          String.Format("Type ({0}) and namespace conflict within the assembly: {1}",
                        type.FullName, type.DeclaringUnit.Name));
      }
      return resolver;
    }

    #endregion
  }
}
