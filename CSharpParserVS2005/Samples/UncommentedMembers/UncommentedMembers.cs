using System.Collections.Generic;
using CSharpParser.ProjectModel;

namespace CSharpParser.Samples.UncommentedMembers
{
  // ==================================================================================
  /// <summary>
  /// Utility class demonstrating CSharpParser features
  /// </summary>
  // ==================================================================================
  public static partial class CSharpUtility
  {
    // --------------------------------------------------------------------------------
    /// <summary>
    /// Collects types and members not having XML comments declared.
    /// </summary>
    /// <param name="unit">Compilation unit.</param>
    /// <returns>A list of uncommented members</returns>
    // --------------------------------------------------------------------------------
    public static List<TypeCommentInfo> GetUncommentedMembers(CompilationUnit unit)
    {
      List<TypeCommentInfo> results = new List<TypeCommentInfo>();
      
      // --- Collect uncommented types
      foreach (TypeDeclaration type in unit.DeclaredTypes)
      {
        TypeCommentInfo newInfo = null;
        if (type.IsVisible && GetXmlComment(type) == null) 
          newInfo = new TypeCommentInfo(type, true);

        // --- Collect uncommented members
        foreach (MemberDeclaration member in type.Members)
        {
          if (member.IsValid && member.Visibility != Visibility.Private && 
            GetXmlComment(member) == null)
          {
            if (newInfo == null) newInfo = new TypeCommentInfo(type, false);
            newInfo.AddMember(member);
          }
        }
        if (newInfo != null) results.Add(newInfo);
      }
      return results;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Obtains the XML comment belonging to an element.
    /// </summary>
    /// <param name="element">
    /// Attributed languauge element representing the type or member.</param>
    /// <returns>
    /// XmlCommentLine intance representing the comment found; or null, if no XML comment
    /// found.
    /// </returns>
    // --------------------------------------------------------------------------------
    private static XmlCommentLine GetXmlComment(AttributedElement element)
    {
      // --- The comment may belong to the element itself or to its
      // --- first attribute.
      CommentInfo comment = element.Comment;
      if (comment == null)
      {
        if (element.Attributes.Count > 0) comment = element.Attributes[0].Comment;
      }

      // --- Return, if no comment found
      if (comment == null) return null;

      // --- Return if XML comment found.
      XmlCommentLine xmlComment = comment as XmlCommentLine;
      if (xmlComment != null) return xmlComment;

      // --- Search for an XML comment in a multi comment block
      MultiCommentBlock commentBlock = comment as MultiCommentBlock;
      if (commentBlock == null) return null;
      foreach (CommentInfo commentInfo in commentBlock.Comments)
      {
        // --- Retrieve the first XML comment in the block
        XmlCommentLine xmlCommenntInBlock = commentInfo as XmlCommentLine;
        if (xmlCommenntInBlock != null) return xmlCommenntInBlock;
      }
      return null;
    }
  }

  // ==================================================================================
  /// <summary>
  /// This type represents information about missing XML commennts.
  /// </summary>
  // ==================================================================================
  public sealed class TypeCommentInfo
  {
    private readonly TypeDeclaration _Type;
    private readonly bool _TypeCommentMissing;
    private readonly List<MemberDeclaration> _Members = new List<MemberDeclaration>();

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new instance of information structure.
    /// </summary>
    /// <param name="type">Type havin no comment, or member with no comment</param>
    /// <param name="commentMissing">Has the type XML comment?</param>
    // --------------------------------------------------------------------------------
    public TypeCommentInfo(TypeDeclaration type, bool commentMissing)
    {
      _Type = type;
      _TypeCommentMissing = commentMissing;
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Adds a new member with no comment tho this instance.
    /// </summary>
    /// <param name="member">Member with no comment.</param>
    // --------------------------------------------------------------------------------
    public void AddMember(MemberDeclaration member)
    {
      _Members.Add(member);
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the type declaration part.
    /// </summary>
    // --------------------------------------------------------------------------------
    public TypeDeclaration Type
    {
      get { return _Type; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the flag indicating if type has XML comment or not.
    /// </summary>
    // --------------------------------------------------------------------------------
    public bool TypeCommentMissing
    {
      get { return _TypeCommentMissing; }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// Gets the list of members with no comments.
    /// </summary>
    // --------------------------------------------------------------------------------
    public List<MemberDeclaration> Members
    {
      get { return _Members; }
    }
  }
}