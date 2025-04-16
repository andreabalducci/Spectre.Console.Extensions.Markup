using System.Text;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using Spectre.Console.Rendering;

namespace Spectre.Console.Extensions.Markup.Renderers;

internal sealed class HeadingBlockRenderer : IRenderer<HeadingBlock>
{
    private readonly InlineRenderer _inlineRendering = new();

    public HeadingBlockRenderer(MarkdownStyling styling)
    {
        HeadingLevel1Color = styling.HeadingLevel1Color;
        HeadingLevel2To4Style = styling.HeadingLevel2To4Style;
        HeadingLevel5AndAboveStyle = styling.HeadingLevel5AndAboveStyle;
    }

    public Color HeadingLevel1Color { get; }
    public Style HeadingLevel2To4Style { get; }
    public Style HeadingLevel5AndAboveStyle { get; }

    public IRenderable Render(HeadingBlock heading)
    {
        if (heading.Inline is null)
        {
            return Text.Empty;
        }

        if (heading.Level == 1)
        {
            var bob = new StringBuilder();
            foreach (var inline in heading.Inline)
            {
                if (inline is LiteralInline literal)
                {
                    bob.Append(literal.Content.ToString().EscapeMarkup());
                }
            }
            return new FigletText(bob.ToString()).Color(HeadingLevel1Color);
        }

        if (heading.Level > 4)
        {
            return _inlineRendering.RenderContainerInline(heading.Inline, HeadingLevel5AndAboveStyle);
        }

        var content = _inlineRendering.RenderContainerInline(heading.Inline, HeadingLevel2To4Style);
        var rowChar = heading.Level == 2 ? '=' : '-';

        var rowRow = new Text(new string(rowChar, heading.Inline.Span.Length), HeadingLevel2To4Style);

        return new Rows(content, rowRow);
    }
}
