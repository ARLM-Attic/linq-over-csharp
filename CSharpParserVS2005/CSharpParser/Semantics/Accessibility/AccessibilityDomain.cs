using System.Collections.Generic;

namespace CSharpParser.Semantics
{
  // ==================================================================================
  /// <summary>
  /// This type represents an accessibility value of a type or a type member.
  /// </summary>
  /// <remarks>
  /// Accessibility domains can be compared to determine if one equals with the other,
  /// or one is a superset of an other or they are disjoint.
  /// </remarks>
  // ==================================================================================
  public sealed class AccessibilityDomain
  {
    private readonly List<AccessDomainElement> _Elements = new List<AccessDomainElement>();

    public void Add(AccessDomainElement element)
    {
      if (_Elements.Count == 0) 
      {
        if (!(element is EmptyScope)) _Elements.Add(element);
      }
      else if (element is EmptyScope || _Elements[0] is Universe)
      {
        return;
      }
      else if (element is Universe) 
      {
        _Elements.Clear();
        _Elements.Add(element);
      }
      _Elements.Add(element);
    }

    public List<AccessDomainElement> Elements
    {
      get { return _Elements; }
    }

    public void Add(AccessibilityDomain domain)
    {
      foreach (AccessDomainElement element in domain._Elements) Add(element);
    }

    public void Intersect(AccessibilityDomain other)
    {
      AccessibilityDomain result = new AccessibilityDomain();
      foreach(AccessDomainElement element in _Elements)
        foreach (AccessDomainElement otherElement in other.Elements)
          result.Add(element.Intersect(otherElement));
    }
  }
}
