namespace CSharpParser.Semantics
{
  public abstract class AccessDomainElement
  {
    private ITypeCharacteristics _Type;

    protected AccessDomainElement()
    {
    }

    protected AccessDomainElement(ITypeCharacteristics _Type)
    {
      this._Type = _Type;
    }

    public ITypeCharacteristics Type
    {
      get { return _Type; }
    }

    public AccessDomainElement Intersect(AccessDomainElement other)
    {
      if (this is EmptyScope || other is EmptyScope) return this;
      if (this is Universe && other is Compilation) return other;
      if (this is Compilation && other is Universe) return this;
      return null;
    }
  }

  public sealed class EmptyScope: AccessDomainElement
  {
  }

  public sealed class Universe: AccessDomainElement
  {
  }

  public sealed class Compilation: AccessDomainElement
  {
  }

  public sealed class ConcreteType: AccessDomainElement
  {
    public ConcreteType(ITypeCharacteristics _Type) : base(_Type)
    {
    }
  }

  public sealed class InheritorsOf: AccessDomainElement
  {
    public InheritorsOf(ITypeCharacteristics _Type) : base(_Type)
    {
    }
  }
}
