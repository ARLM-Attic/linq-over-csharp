// ================================================================================================
// NamedCompilationEntity.cs
//
// Created: 2009.05.10, by Istvan Novak (DeepDiver)
// ================================================================================================
using System;

namespace CSharpTreeBuilder.Cst
{
  // ================================================================================================
  /// <summary>
  /// This class represents compilation entities having a name.
  /// </summary>
  /// <remarks>
  /// Each named compilation entity has a <see cref="Name"/> that is a simple C# identifier in the 
  /// form used in the declaration. 
  /// </remarks>
  // ================================================================================================
  public abstract class NamedCompilationEntity : CompilationEntity
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="NamedCompilationEntity"/> class with the 
    /// specified name.
    /// </summary>
    /// <param name="name">The name of the entity.</param>
    // ----------------------------------------------------------------------------------------------
    protected NamedCompilationEntity(string name)
    {
      if (name == null) throw new ArgumentNullException("name");
      Name = name;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the name of the entity.
    /// </summary>
    /// <remarks>
    /// This name can be set only through the instance constructor of the class and represents the 
    /// name used in the declaration of the compilation entity.
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    public string Name { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the full name of this entity in the context where the entity is defined.
    /// </summary>
    /// <remarks>
    /// This property can be overridden in derived classes to set up the full name to the context
    /// of the entity. By default the <see cref="Name"/> is retrieved.
    /// For example, for type declarations this name is composed from the namespace, the declraing 
    /// type and type name like "System.Int32".
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    public virtual string FullName { get { return Name; } }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the name of this entity as the CLR uses it for its own representation.
    /// </summary>
    /// <remarks>
    /// This property can be overridden in derived classes to set up the CLR name to the context
    /// of the entity. By default the <see cref="Name"/> is retrieved.
    /// For example, for generic type declarations this name is composed from the name, and the 
    /// number of type parameters.
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    public virtual string ClrName { get { return Name; } }
  }
}