using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharpTreeBuilder.CSharpSemanticGraph;
using CSharpTreeBuilder.ProjectContent;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class implements the member lookup algorithm described in the spec (§7.3).
  /// A member lookup is the process whereby the meaning of a name in the context of a type is determined. 
  /// </summary>
  /// <remarks>
  /// Member lookup considers not only the name of a member but also 
  /// the number of type parameters the member has and whether the member is accessible. 
  /// For the purposes of member lookup, generic methods and nested generic types 
  /// have the number of type parameters indicated in their respective declarations 
  /// and all other members have zero type parameters.
  /// </remarks>
  // ================================================================================================
  public sealed class MemberLookup
  {
    /// <summary>Error handler object for error and warning reporting.</summary>
    private readonly ICompilationErrorHandler _ErrorHandler;

    /// <summary>The semantic graph.</summary>
    private readonly SemanticGraph _SemanticGraph;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="MemberLookup"/> class.
    /// </summary>
    /// <param name="errorHandler">Error handler object for error and warning reporting.</param>
    /// <param name="semanticGraph">The semantic graph.</param>
    // ----------------------------------------------------------------------------------------------
    public MemberLookup(ICompilationErrorHandler errorHandler, SemanticGraph semanticGraph)
    {
      _ErrorHandler = errorHandler;
      _SemanticGraph = semanticGraph;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a collection of type members in the context of a type based on name and type parameter count.
    /// </summary>
    /// <param name="name">A name.</param>
    /// <param name="typeParameterCount">The number of type parameters.</param>
    /// <param name="contextEntity">A type entity.</param>
    /// <param name="accessingEntity">An entity for accessibility checking.</param>
    /// <param name="isInvocation">True, if the member should be in the context of an invocation.</param>
    /// <returns>A collection of type members. Can be empty.</returns>
    /// <remarks>If the simple-name or member-access occurs as the simple-expression 
    /// of an invocation-expression (§7.5.5.1), the member is said to be invoked.</remarks>
    // ----------------------------------------------------------------------------------------------
    public MemberLookupResult Lookup(
      string name, int typeParameterCount, TypeEntity contextEntity, ISemanticEntity accessingEntity, bool isInvocation)
    {
      // A member lookup of a name N with K type parameters in a type T is processed as follows:
      string N = name;
      int K = typeParameterCount;
      TypeEntity T = contextEntity;

      // First, a set of accessible members named N is determined:
      var members = GetAccessibleMembersByName(N, T, accessingEntity);

      // Next, if K is zero, all nested types whose declarations include type parameters are removed. 
      // If K is not zero, all members with a different number of type parameters are removed. 
      // Note that when K is zero, methods having type parameters are not removed, 
      // since the type inference process (§7.4.2) might be able to infer the type arguments.

      // --> Translated to: remove type member with a type parameter count other than K, ...
      members.RemoveWhere(x => (x is GenericCapableTypeEntity) && ((GenericCapableTypeEntity)x).OwnTypeParameterCount != K);
      // --> ... and remove methods with a type parameter count other than K, but only if K is not zero.
      members.RemoveWhere(x => (K != 0) && (x is MethodEntity) && ((MethodEntity)x).OwnTypeParameterCount != K);

      // Next, if the member is invoked, all non-invocable members are removed from the set.
      if (isInvocation)
      {
        members.RemoveWhere(x => !x.IsInvocable);
      }

      // Next, members that are hidden by other members are removed from the set. 
      // For every member S.M in the set, where S is the type in which the member M is declared, 
      // the following rules are applied:

      // If M is a constant, field, property, event, or enumeration member, 
      // then all members declared in a base type of S are removed from the set.
      // --> Translated to: remove all members declared in a base class of another member of the listed member-types.
      members.RemoveWhere(removableMember =>
        members.Any(descendantMember =>
          (removableMember.Parent is TypeEntity)
          && (removableMember.Parent as TypeEntity).IsBaseOf(descendantMember.Parent as TypeEntity)
          && (descendantMember is ConstantMemberEntity || descendantMember is FieldEntity || descendantMember is PropertyEntity)
        ));

      // If M is a type declaration, then all non-types declared in a base type of S are removed from the set, 
      // and all type declarations with the same number of type parameters as M declared in a base type of S are removed from the set.
      // --> Translated to: remove all non-type members or type members with the same number of type params
      // --> that are declared in a base class of a nested type member.
      members.RemoveWhere(removableMember =>
        (
          !(removableMember is TypeEntity)
        ||
          (K == 0 && !(removableMember is GenericCapableTypeEntity))
        ||
          ((removableMember as GenericCapableTypeEntity).OwnTypeParameterCount == K)
        )
        &&
        members.Any(descendantMember =>
          (removableMember.Parent is TypeEntity)
          && (removableMember.Parent as TypeEntity).IsBaseOf(descendantMember.Parent as TypeEntity)
          && (descendantMember is TypeEntity)
        ));
      
      // If M is a method, then all non-method members declared in a base type of S are removed from the set.
      // --> Translated to: remove all non-methods that are declared in a base class of a method.
      members.RemoveWhere(removableMember =>
        !(removableMember is MethodEntity)
        && members.Any(descendantMember =>
            (removableMember.Parent is TypeEntity)
            && (removableMember.Parent as TypeEntity).IsBaseOf(descendantMember.Parent as TypeEntity)
            && (descendantMember is MethodEntity)
      ));

      // Next, interface members that are hidden by class members are removed from the set. 
      // This step only has an effect if T is a type parameter and T has both an effective base class other than object 
      // and a non-empty effective interface set (§10.1.5). 
      if (T is TypeParameterEntity
        && T.BaseClass.BuiltInTypeValue != BuiltInType.Object
        && T.BaseInterfaces.Count > 0)
      {
        // Comparer for method signature comparison
        var signatureComparer = new SignatureEqualityComparerForCompleteMatching();

        // Creating a copy of the member-set for iteration. Iterating on members would make it un
        var iteratorSet = members.ToList();

        // Spec: For every member S.M in the set, where S is the type in which the member M is declared, ...
        foreach (var M in iteratorSet)
        {
          // If M is not yet removed from the set then let's see if it hides other members.
          if (members.Contains(M))
          {
            var S = M.Parent as ClassEntity;

            // Spec: ... the following rules are applied if S is a class declaration other than object:
            if (S != null && S.BuiltInTypeValue != BuiltInType.Object)
            {
              // If M is a constant, field, property, event, enumeration member, or type declaration, 
              if (M is ConstantMemberEntity || M is FieldEntity || M is PropertyEntity // TODO: || M is EventEntity
                || M is TypeEntity) // Enum member is also a ConstantMemberEntity so no need to check it separately
              {
                // then all members declared in an interface declaration are removed from the set.
                members.RemoveWhere(removableMember =>
                                    removableMember.Parent is InterfaceEntity
                                    && removableMember.Name == M.Name);
              }

              // If M is a method, 
              if (M is MethodEntity)
              {
                // then all non-method members declared in an interface declaration are removed from the set, 
                members.RemoveWhere(removableMember =>
                                    !(removableMember is MethodEntity)
                                    && removableMember.Parent is InterfaceEntity
                                    && removableMember.Name == M.Name);

                // and all methods with the same signature as M declared in an interface declaration are removed from the set.
                members.RemoveWhere(removableMember =>
                                    removableMember is MethodEntity
                                    && removableMember.Parent is InterfaceEntity
                                    && signatureComparer.Equals((removableMember as MethodEntity).Signature, (M as MethodEntity).Signature));
              }
            }
          }
        }
      }

      // Finally, having removed hidden members, the result of the lookup is determined:
      // - If the set consists of a single member that is not a method, then this member is the result of the lookup.
      // - Otherwise, if the set contains only methods, then this group of methods is the result of the lookup.
      // - Otherwise, the lookup is ambiguous, and a compile-time error occurs.
      // TODO: detect ambiguity and signal error 
      // (csc.exe signals warning CS0467, see test case MemberLookupTest.).

      return new MemberLookupResult(members);
      
      // Note:
      // Spec: For member lookups in types other than type parameters and interfaces, 
      // and member lookups in interfaces that are strictly single-inheritance 
      // (each interface in the inheritance chain has exactly zero or one direct base interface), 
      // the effect of the lookup rules is simply that derived members hide base members with the same name or signature. 
      // Such single-inheritance lookups are never ambiguous. 
      // The ambiguities that can possibly arise from member lookups in multiple-inheritance interfaces are described in §13.2.5.

      // TODO:
      // Methods hidden by a "new" method are not excluded from the resulting method group. Is this the correct behaviour?
    }

    #region Private methods

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets accessible members by name in the context of a type.
    /// </summary>
    /// <param name="N">A name.</param>
    /// <param name="T">A type entity.</param>
    /// <param name="accessingEntity">An entity for accessibility checking.</param>
    /// <returns>The collection of accessible members named N in T.</returns>
    // ----------------------------------------------------------------------------------------------
    private HashSet<IMemberEntity> GetAccessibleMembersByName(string N, TypeEntity T, ISemanticEntity accessingEntity)
    {
      var members = new HashSet<IMemberEntity>();

      // If T is a type parameter, ...
      if (T is TypeParameterEntity)
      {
        // ... then the set is the union of the sets of accessible members named N 
        // in each of the types specified as a primary constraint or secondary constraint (§10.1.5) for T, ...

        var typeParameter = T as TypeParameterEntity;

        if (typeParameter.ClassTypeConstraint != null)
        {
          members.UnionWith(typeParameter.ClassTypeConstraint.GetAccessibleMembers<IMemberEntity>(N, accessingEntity));
          members.UnionWith(typeParameter.ClassTypeConstraint.GetAccessibleInheritedMembers<IMemberEntity>(N, accessingEntity));
        }

        foreach (var typeEntity in typeParameter.InterfaceTypeConstraints)
        {
          members.UnionWith(typeEntity.GetAccessibleMembers<IMemberEntity>(N, accessingEntity));
        }

        // ... along with the set of accessible members named N in object.
        members.UnionWith(GetAccessibleMembersOfObject(N, accessingEntity));
      }
      else
      {
        // Otherwise, the set consists of all accessible (§3.5) members named N in T, including inherited members ... 
        members.UnionWith(T.GetAccessibleMembers<IMemberEntity>(N, accessingEntity));
        members.UnionWith(T.GetAccessibleInheritedMembers<IMemberEntity>(N, accessingEntity));

        // ... and the accessible members named N in object. 
        members.UnionWith(GetAccessibleMembersOfObject(N, accessingEntity));

        // If T is a constructed type, the set of members is obtained 
        // by substituting type arguments as described in §10.3.2.
        // --> This is handled by the entity objects.

        // Members that include an override modifier are excluded from the set.
        members.RemoveWhere(x => (x is FunctionMemberEntity) && (x as FunctionMemberEntity).IsOverride);
      }

      return members;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the accessible members of object named N.
    /// </summary>
    /// <param name="N">A name.</param>
    /// <param name="accessingEntity">An entity for accessibility checking.</param>
    /// <returns>The collection of accessible members named N in object.</returns>
    // ----------------------------------------------------------------------------------------------
    private IEnumerable<IMemberEntity> GetAccessibleMembersOfObject(string N, ISemanticEntity accessingEntity)
    {
      var objectEntity = _SemanticGraph.GetTypeEntityByBuiltInType(BuiltInType.Object);
      return objectEntity.GetAccessibleMembers<NonTypeMemberEntity>(N, accessingEntity).Cast<IMemberEntity>();
    }

    #endregion
  }
}
