using Markdig.Syntax;
using Spectre.Console.Rendering;

namespace Spectre.Console.Extensions.Markup.Renderers;

internal sealed class BlockRenderer
{
    private readonly Dictionary<string, Func<string, JustInTimeRenderable>> _codeblockRenderables;
    private readonly MarkdownStyling _styling;

    public BlockRenderer(Dictionary<string, Func<string, JustInTimeRenderable>> codeblockRenderables, MarkdownStyling styling)
    {
        _codeblockRenderables = codeblockRenderables;
        _styling = styling;
    }

    public IRenderable Render(Block block)
    {
        return block switch
        {
            ParagraphBlock leafBlock => new LeafBlockRenderer().Render(leafBlock),
            HeadingBlock heading => new HeadingBlockRenderer(_styling).Render(heading),
            ListBlock list => new ListBlockRenderer(_codeblockRenderables, _styling).Render(list),
            QuoteBlock quoteBlock => new QuoteBlockRenderer(_codeblockRenderables, _styling).Render(quoteBlock),
            FencedCodeBlock codeBlock => new CodeBlockRenderer(_codeblockRenderables, _styling).Render(codeBlock),
            CodeBlock codeBlock => new CodeBlockRenderer(_codeblockRenderables, _styling).Render(codeBlock),
            ThematicBreakBlock => new Grid() { Width = 120 }.AddColumn().AddRow(new Rule()),
            _ => new Paragraph(),
        };
    }
}
