namespace CSharpFactory.ProjectModel
{
  // ==================================================================================
  /// <summary>
  /// This enumeration defines the target of a successful type resolution.
  /// </summary>
  // ==================================================================================
  public enum ResolutionTarget
  {
    /// <summary>The name has not been resolved.</summary>
    Unresolved = 0,
    /// <summary>The name has been resolved as a namespace hierarchy.</summary>
    NamespaceHierarchy,
    /// <summary>The name has been resolved as a namespace.</summary>
    Namespace,
    /// <summary>The name has been resolved as a type.</summary>
    Type,
    /// <summary>The name has been resolved to a type parameter of a type declaration.</summary>
    TypeParameter,
    /// <summary>The name has been resolved to a member of a type declaration.</summary>
    TypeMember,
    /// <summary>The name has been resolved to a type parameter of a member declaration.</summary>
    MethodTypeParameter,
    /// <summary>The name has been resolved to a formal parameter of a member declaration.</summary>
    MemberFormalParameter,
    /// <summary>The name has been resolved to a local constant declaration.</summary>
    LocalConstant,
    /// <summary>The name has been resolved to a local variable declaration.</summary>
    LocalDeclaration,
    /// <summary>The name has been resolved to a simple name.</summary>
    Name
  }
}
