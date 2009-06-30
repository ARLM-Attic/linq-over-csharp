using CSharpTreeBuilder.CSharpAstBuilder;

namespace CSharpTreeBuilder.Ast
{
  // ================================================================================================
  /// <summary>
  /// This class represents an object creation or a delegate creation expression.
  /// Unfortunately these two cannot be distinguished by pure syntax.
  /// </summary>
  /// <remarks>
  /// <para>object-creation-expression:</para>
  /// <para><code>  new type ( argument-list-opt ) object-or-collection-initilaizer-opt</code></para>
  /// <para><code>  new type object-or-collection-initilaizer</code></para>
  /// <para>delegate-creation-expression:</para>
  /// <para><code>  new delegate-type ( expression )</code></para>
  /// </remarks>
  // ================================================================================================
  public class ObjectCreationExpressionNode : NewOperatorNode, IParentheses
  {
    // --- Backing fields
    private TypeOrNamespaceNode _TypeName;

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Initializes a new instance of the <see cref="ObjectCreationExpressionNode"/> class.
    /// </summary>
    /// <param name="start">Token providing information about the element.</param>
    // ----------------------------------------------------------------------------------------------
    public ObjectCreationExpressionNode(Token start)
      : base(start)
    {
      Arguments = new ArgumentNodeCollection() {ParentNode = this};
    } 

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the name of the type.
    /// </summary>
    /// <value>The name of the type.</value>
    // ----------------------------------------------------------------------------------------------
    public TypeOrNamespaceNode TypeName
    {
      get { return _TypeName; }
      internal set
      {
        _TypeName = value;
        if (_TypeName != null) _TypeName.ParentNode = this;
      }
    }
    
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the opening parenthesis token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token OpenParenthesis { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the constructor arguments.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ArgumentNodeCollection Arguments { get; private set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether the object creation expression has constructor arguments.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool HasConstructorArguments
    {
      get { return Arguments != null && Arguments.Count > 0; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets the closing parenthesis token.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public Token CloseParenthesis { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets or sets the optional initializer.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public ObjectOrCollectionInitializerNode ObjectOrCollectionInitializer { get; internal set; }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance is an implicit constructor call.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool IsImplicitCall
    {
      get { return OpenParenthesis == null; }
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Gets a value indicating whether this instance has an initializer.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public bool HasObjectInitializer
    {
      get { return ObjectOrCollectionInitializer != null; }
    }
  }
}