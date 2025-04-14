using System.Text;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using Spectre.Console.Rendering;

namespace Spectre.Console.Extensions.Markup.Renderers;

internal sealed class HeadingBlockRenderer : IRenderer<HeadingBlock>
{
    private readonly InlineRenderer _inlineRendering = new();

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
            return new FigletText(bob.ToString());
        }

        if (heading.Level > 4)
        {
            return _inlineRendering.RenderContainerInline(heading.Inline, new Style(foreground: Color.Yellow));
        }

        var content = _inlineRendering.RenderContainerInline(heading.Inline, new Style(foreground: Color.Yellow));
        var rowChar = heading.Level == 2 ? '=' : '-';

        var rowRow = new Text(new string(rowChar, heading.Inline.Span.Length), Color.Yellow);

        return new Rows(content, rowRow);
    }
}
