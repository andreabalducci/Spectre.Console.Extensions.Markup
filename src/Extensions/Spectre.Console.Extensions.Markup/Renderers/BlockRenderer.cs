using Markdig.Syntax;
using Spectre.Console.Rendering;

namespace Spectre.Console.Extensions.Markup.Renderers;

internal sealed class BlockRenderer
{
    public IRenderable Render(Block block)
    {
        return block switch
        {
            ParagraphBlock leafBlock => new LeafBlockRenderer().Render(leafBlock),
            HeadingBlock heading => new HeadingBlockRenderer().Render(heading),
            ListBlock list => new ListBlockRenderer().Render(list),
            QuoteBlock quoteBlock => new QuoteBlockRenderer().Render(quoteBlock),
            FencedCodeBlock codeBlock => new CodeBlockRenderer().Render(codeBlock),
            CodeBlock codeBlock => new CodeBlockRenderer().Render(codeBlock),
            ThematicBreakBlock => new Grid() { Width = 120 }.AddColumn().AddRow(new Rule()),
            _ => new Paragraph(),
        };
    }
}
