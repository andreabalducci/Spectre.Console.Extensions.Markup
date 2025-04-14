using Markdig.Syntax;
using Spectre.Console.Rendering;

namespace Spectre.Console.Extensions.Markup.Renderers;

internal sealed class BlockRenderer
{
    public IRenderable Render(Block block)
    {
        return block switch
        {
            ParagraphBlock leafBlock => new LeafBlockRenderer().Render(leafBlock, this),
            HeadingBlock heading => new HeadingBlockRenderer().Render(heading, this),
            ListBlock list => new ListBlockRenderer().Render(list, this),
            QuoteBlock quoteBlock => new QuoteBlockRenderer().Render(quoteBlock, this),
            FencedCodeBlock codeBlock => new CodeBlockRenderer().Render(codeBlock, this),
            CodeBlock codeBlock => new CodeBlockRenderer().Render(codeBlock, this),
            ThematicBreakBlock => new Grid() { Width = 120 }.AddColumn().AddRow(new Rule()),
            _ => new Paragraph(),
        };
    }
}
