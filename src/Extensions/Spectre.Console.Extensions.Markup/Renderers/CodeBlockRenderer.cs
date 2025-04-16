using System.Text;
using Markdig.Syntax;
using Spectre.Console.Rendering;

namespace Spectre.Console.Extensions.Markup.Renderers;

internal sealed class CodeBlockRenderer : IRenderer<CodeBlock>
{
    private readonly Dictionary<string, Func<string, JustInTimeRenderable>> _codeblockRenderables;

    public CodeBlockRenderer(Dictionary<string, Func<string, JustInTimeRenderable>> codeblockRenderables)
    {
        _codeblockRenderables = codeblockRenderables;
    }

    public IRenderable Render(CodeBlock codeBlock)
    {
        var code = GetLinesAsString(codeBlock);

        Panel? panel = null;

        if (codeBlock is FencedCodeBlock fencedCodeBlock && !string.IsNullOrEmpty(fencedCodeBlock.Info))
        {
            if (_codeblockRenderables.TryGetValue(fencedCodeBlock.Info!, out var renderableFactory))
            {
                var renderable = renderableFactory(code);
                panel = new Panel(renderable)
                {
                    Header = new PanelHeader(fencedCodeBlock.Info.EscapeMarkup())
                };
            }
        }

        panel ??= new Panel(code.EscapeMarkup());
        panel.BorderStyle = Color.Blue;
        panel.Padding = new Padding(1, 0, 0, 0);
        panel.Border = new LeftBoxBorder();

        return panel;
    }

    private static string GetLinesAsString(CodeBlock codeBlock)
    {
        var sb = new StringBuilder();
        var lines = codeBlock.Lines.Lines;

        foreach (var line in lines)
        {
            sb.AppendLine(line.ToString());
        }

        return sb.ToString().Trim();
    }
}
