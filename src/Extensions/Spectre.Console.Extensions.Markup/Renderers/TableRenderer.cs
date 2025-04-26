using Markdig.Extensions.Tables;
using Markdig.Syntax;
using Spectre.Console.Rendering;

namespace Spectre.Console.Extensions.Markup.Renderers;

internal sealed class TableRenderer : IRenderer<Markdig.Extensions.Tables.Table>
{
    private readonly InlineRenderer _inlineRendering = new();
    private readonly BlockRenderer _blockRenderer;

    public TableRenderer(Dictionary<string, Func<string, JustInTimeRenderable>> codeblockRenderables, MarkdownStyling styling)
    {
        _blockRenderer = new BlockRenderer(codeblockRenderables, styling);
        BorderStyle = styling.TableBorderStyle;
        Border = styling.TableBorder;
    }

    public Style BorderStyle { get; }
    public TableBorder Border { get; }

    public IRenderable Render(Markdig.Extensions.Tables.Table tableBlock)
    {
        var table = new Table
        {
            BorderStyle = BorderStyle,
            Border = Border
        };

        foreach (var row in tableBlock)
        {
            if (row is Markdig.Extensions.Tables.TableRow tableRow)
            {
                var cells = new List<IRenderable>();
                for (var i = 0; i < tableRow.Count; i++)
                {
                    var justify = GetJustification(tableBlock, i);

                    var cell = tableRow[i];
                    if (cell is TableCell tableCell)
                    {
                        var renderable = GetRenderable(justify, tableCell);
                        if (tableRow.IsHeader)
                        {
                            var tableColumn = new TableColumn(renderable) { Alignment = justify };
                            table.AddColumn(tableColumn);
                        }
                        else
                        {
                            cells.Add(renderable);
                        }
                    }
                }
                if (!tableRow.IsHeader)
                {
                    table.AddRow(cells.ToArray());
                }
            }
        }
        return table;
    }

    private static Justify GetJustification(Markdig.Extensions.Tables.Table tableBlock, int i)
    {
        if (i < 0 || i >= tableBlock.ColumnDefinitions.Count)
        {
            return Justify.Left;
        }

        return tableBlock.ColumnDefinitions[i].Alignment switch
        {
            TableColumnAlign.Left => Justify.Left,
            TableColumnAlign.Center => Justify.Center,
            TableColumnAlign.Right => Justify.Right,
            _ => Justify.Left
        };
    }

    private IRenderable GetRenderable(Justify justify, TableCell tableCell)
    {
        var renderables = new List<IRenderable>();
        foreach (var block in tableCell)
        {
            if (block is ParagraphBlock paragraph && paragraph.Inline is not null)
            {
                // Render the paragraph block
                var renderedParagraph = _inlineRendering.RenderInline(paragraph.Inline, Style.Plain, justify);
                renderables.Add(renderedParagraph);
            }
            else
            {
                var renderedBlock = _blockRenderer.Render(block);
                renderables.Add(renderedBlock);
            }
        }
        return new CompositeRenderable(renderables);
    }
}
