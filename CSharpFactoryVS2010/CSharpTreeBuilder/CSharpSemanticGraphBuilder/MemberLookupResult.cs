using System;
using System.Collections.Generic;
using System.Linq;
using CSharpTreeBuilder.CSharpSemanticGraph;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class represents the result of a member lookup. 
  /// The result is either a method group or a single non-method member.
  /// </summary>
  // ================================================================================================
  public sealed class MemberLookupResult
  {
    #region State

    /// <summary>
    /// The single member result of the member lookup.
    /// </summary>
    public IMemberEntity SingleMember { get; private set; }

    /// <summary>
    /// The method group result of the member lookup.
    /// </summary>
    public MethodGroup MethodGroup { get; private set; }

    #endregion

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="MemberLookupResult"/> class.
    /// </summary>
    /// <param name="members">A collection of member entities.</param>
    // ----------------------------------------------------------------------------------------------
    public MemberLookupResult(IEnumerable<IMemberEntity> members)
    {
      var memberList = members.ToList();

      // If the set consists of a single member that is not a method, then this member is the result of the lookup.
      if (memberList.Count == 1 && !(memberList[0] is MethodEntity))
      {
        SingleMember = memberList[0];
      }
      // Otherwise, if the set contains only methods, then this group of methods is the result of the lookup.
      else if (memberList.Count > 0 && memberList.All(member => member is MethodEntity))
      {
        MethodGroup = new MethodGroup(members.Cast<MethodEntity>());
      }

      // Otherwise, the lookup is ambiguous
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this result is empty.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsEmpty
    {
      get { return SingleMember == null && MethodGroup == null; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this result is a single member.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsSingleMember
    {
      get { return SingleMember != null; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this result is a method group.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsMethodGroup
    {
      get { return MethodGroup != null; }
    }
  }
}
