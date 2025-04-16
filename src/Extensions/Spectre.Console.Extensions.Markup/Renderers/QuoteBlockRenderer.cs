using Markdig.Syntax;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace Spectre.Console.Extensions.Markup.Renderers;

internal sealed class QuoteBlockRenderer : IRenderer<QuoteBlock>
{
    private readonly InlineRenderer _inlineRendering = new();
    private readonly BlockRenderer _blockRenderer;

    public QuoteBlockRenderer(Dictionary<string, Func<string, JustInTimeRenderable>> codeblockRenderables, MarkdownStyling styling)
    {
        _blockRenderer = new BlockRenderer(codeblockRenderables, styling);
        BorderStyle = styling.QuoteBlockBorderStyle;
        Padding = styling.QuoteBlockPadding;
        Border = styling.QuoteBlockBorder;
    }

    public Style BorderStyle { get; }
    public Padding Padding { get; }
    public BoxBorder Border { get; }

    public IRenderable Render(QuoteBlock quoteBlock)
    {
        var renderables = new CompositeRenderable(quoteBlock.Select(x =>
        {
            if (x is ParagraphBlock { Inline: not null } paragraphBlock)
            {
                return _inlineRendering.RenderInline(paragraphBlock.Inline, Style.Plain, Justify.Left);
            }
            else
            {
                return _blockRenderer.Render(x);
            }
        }));

        var panel = new Panel(renderables)
        {
            Border = Border,
            BorderStyle = BorderStyle,
            Padding = Padding
        };

        return panel;
    }
}
