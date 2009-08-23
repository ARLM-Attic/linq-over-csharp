// ================================================================================================
// TypeTagNodeCollection.cs
//
// Created: 2009.05.30, by Istvan Novak (DeepDiver)
// ================================================================================================
using System;
using System.Text;
namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>This class represents a collection of type tag nodes.</summary>
  /// <remarks>
  /// 	<para>
  ///         For example, if we have the following type name: "System.Text.Encoding", this
  ///         is represented by three <see cref="TypeTagNode"/> instances for "System",
  ///         "Text" and "Encoding", respectively.
  ///     </para>
  /// 	<para>Syntax:</para>
  /// 	<blockquote style="MARGIN-RIGHT: 0px" dir="ltr">
  /// 		<para>{ <em>TypeTagNode</em> }</para>
  /// 	</blockquote>
  /// 	<para>Representation:</para>
  /// 	<blockquote style="MARGIN-RIGHT: 0px" dir="ltr">
  /// 		<para><em>TypeTagNode</em>: Collection item</para>
  /// 	</blockquote>
  /// </remarks>
  // ================================================================================================
  public class TypeTagNodeCollection : SyntaxNodeCollection<TypeTagNode, NamespaceOrTypeNameNode>
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a copy of the type tag collection, but without the last type tag.
    /// </summary>
    /// <returns>A new TypeTagNodeCollection.</returns>
    /// <remarks>Throws an exception for a collection with less than 2 items.</remarks>
    // ----------------------------------------------------------------------------------------------
    public TypeTagNodeCollection GetCopyWithoutLastTag()
    {
      if (Count < 2)
      {
        throw new InvalidOperationException(string.Format("This collection has only '{0}' items.", Count));
      }

      var newTypeTagNodeCollection = new TypeTagNodeCollection();

      for (int i = 0; i < Count - 1; i++)
      {
        newTypeTagNodeCollection.Add(this[i]);
      }

      return newTypeTagNodeCollection;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a copy of the type tag collection, but without the first type tag.
    /// </summary>
    /// <returns>A new TypeTagNodeCollection.</returns>
    /// <remarks>Throws an exception for a collection with less than 2 items.</remarks>
    // ----------------------------------------------------------------------------------------------
    public TypeTagNodeCollection GetCopyWithoutFirstTag()
    {
      if (Count < 2)
      {
        throw new InvalidOperationException(string.Format("This collection has only '{0}' items.", Count));
      }

      var newTypeTagNodeCollection = new TypeTagNodeCollection();

      for (int i = 1; i < Count; i++)
      {
        newTypeTagNodeCollection.Add(this[i]);
      }

      return newTypeTagNodeCollection;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Returns the string representation of the object.
    /// </summary>
    /// <returns>The string representation of the object.</returns>
    // ----------------------------------------------------------------------------------------------
    public override string ToString()
    {
      var stringBuilder = new StringBuilder();
      
      bool firstTag = true;
      
      foreach (var typeTagNode in this)
      {
        if (firstTag)
        {
          firstTag = false;
        }
        else
        {
          stringBuilder.Append('.');
        }
      
        stringBuilder.Append(typeTagNode.ToString());
      }

      return stringBuilder.ToString();
    }
  }
}