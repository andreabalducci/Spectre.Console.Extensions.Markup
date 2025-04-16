using Markdig.Syntax;
using Spectre.Console.Rendering;

namespace Spectre.Console.Extensions.Markup.Renderers;

internal sealed class BlockRenderer
{
    private readonly Dictionary<string, Func<string, JustInTimeRenderable>> _codeblockRenderables;

    public BlockRenderer(Dictionary<string, Func<string, JustInTimeRenderable>> codeblockRenderables)
    {
        _codeblockRenderables = codeblockRenderables;
    }

    public IRenderable Render(Block block)
    {
        return block switch
        {
            ParagraphBlock leafBlock => new LeafBlockRenderer().Render(leafBlock),
            HeadingBlock heading => new HeadingBlockRenderer().Render(heading),
            ListBlock list => new ListBlockRenderer(_codeblockRenderables).Render(list),
            QuoteBlock quoteBlock => new QuoteBlockRenderer(_codeblockRenderables).Render(quoteBlock),
            FencedCodeBlock codeBlock => new CodeBlockRenderer(_codeblockRenderables).Render(codeBlock),
            CodeBlock codeBlock => new CodeBlockRenderer(_codeblockRenderables).Render(codeBlock),
            ThematicBreakBlock => new Grid() { Width = 120 }.AddColumn().AddRow(new Rule()),
            _ => new Paragraph(),
        };
    }
}
