using Markdig.Syntax;
using Spectre.Console.Rendering;

namespace Spectre.Console.Extensions.Markup;

internal sealed class ListBlockRenderer : IRenderer<ListBlock>
{
    public IRenderable Render(ListBlock listBlock, BlockRenderer blockRenderer)
    {
        var table = new Table().HideHeaders().Border(TableBorder.None);

        table.AddColumn(new TableColumn("Marker").Width(3));
        table.AddColumn(new TableColumn("Content"));

        var index = 1;

        foreach (var item in listBlock)
        {
            if (item is ListItemBlock listItem)
            {
                var marker = listBlock.IsOrdered ? $"{index}." : "\u25cb";
                index++;

                // Create a list of renderables for all blocks in the list item
                var contentRenderables = new List<IRenderable>();

                foreach (var block in listItem)
                {
                    // Recursively render each block and add it to the list
                    var blockRenderable = blockRenderer.Render(block);
                    contentRenderables.Add(blockRenderable);
                }

                // Use Rows to combine all the renderables
                var contentRows = new Rows(contentRenderables);

                // Add the marker and content to the table
                table.AddRow(new Text(marker, Color.Green), contentRows);
            }
        }

        return table;
    }
}
