using CSharpFactory.ProjectModel;

namespace CSharpFactory.CodeExplorer.TreeNodes
{
  // ====================================================================================
  /// <summary>
  /// This tree node class represents a member of a type in the type hierarchy.
  /// </summary>
  // ====================================================================================
  internal sealed class MemberTreeNode : 
    OnDemandTreeNode,
    IPropertyPanelSupport
  {
    private readonly MemberDeclaration _Member;

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the member associated with this node.
    /// </summary>
    // --------------------------------------------------------------------------------
    public MemberDeclaration Member
    {
      get { return _Member; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new member tree node with the specified type member declaration.
    /// </summary>
    /// <param name="member">Type member declaration</param>
    // --------------------------------------------------------------------------------
    public MemberTreeNode(MemberDeclaration member)
      : base(member.Signature, false)
    {
      _Member = member;
      if (member is ConstDeclaration) ImageKey = "Constant";
      else if (member is ConstructorDeclaration) ImageKey = "Method";
      else if (member is FieldDeclaration) ImageKey = "Field";
      else if (member is ConstructorDeclaration) ImageKey = "Method";
      else if (member is FinalizerDeclaration) ImageKey = "Method";
      else if (member is EventPropertyDeclaration) ImageKey = "Event";
      else if (member is PropertyDeclaration) ImageKey = "Property";
      else if (member is OperatorDeclaration) ImageKey = "Operator";
      else if (member is CastOperatorDeclaration) ImageKey = "Operator";
      else ImageKey = "Method";
      SelectedImageKey = ImageKey;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the object providing information about this node.
    /// </summary>
    /// <returns>
    /// Information object
    /// </returns>
    // --------------------------------------------------------------------------------
    public object GetSelectedObject()
    {
      return Member;
    }
  }
}
