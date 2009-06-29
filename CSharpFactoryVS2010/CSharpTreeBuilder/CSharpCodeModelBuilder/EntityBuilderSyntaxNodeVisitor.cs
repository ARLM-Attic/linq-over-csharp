using CSharpTreeBuilder.Ast;
using CSharpTreeBuilder.Cst;

namespace CSharpTreeBuilder.CSharpCodeModelBuilder
{
  public class EntityBuilderSyntaxNodeVisitor : BlankSyntaxNodeVisitor
  {
    private ICSharpSemanticsTree _CodeModelGraph;
    private CompilationEntity _CurrentScopeEntity;

    public EntityBuilderSyntaxNodeVisitor(ICSharpSemanticsTree codeModelGraph)
    {
      _CodeModelGraph = codeModelGraph;
      _CurrentScopeEntity = new GlobalNamespaceItem();
    }

    public override void Visit(NamespaceDeclarationNode node)
    {
      // For every tag in the namespace name
      foreach (var nameTag in node.NameTags)
      {
        // Build a NamespaceEntity
        var namespaceEntity = new NamespaceItem(nameTag.Identifier) {IsExplicit = true};

        // Set its parent namespace


        // Add it to the CodeModel graph
        _CodeModelGraph.AddEntity(namespaceEntity);
      }
    }
  }
}
