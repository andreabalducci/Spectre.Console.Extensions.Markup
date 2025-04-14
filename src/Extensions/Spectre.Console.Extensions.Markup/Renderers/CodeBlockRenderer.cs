using System.Text;
using Markdig.Syntax;
using Spectre.Console.CSharp;
using Spectre.Console.Json;
using Spectre.Console.Rendering;
using Spectre.Console.Xml;

namespace Spectre.Console.Extensions.Markup.Renderers;

internal sealed class CodeBlockRenderer : IRenderer<CodeBlock>
{
    public IRenderable Render(CodeBlock codeBlock)
    {
        var code = GetLinesAsString(codeBlock);

        Panel? panel = null;

        if (codeBlock is FencedCodeBlock fencedCodeBlock && !string.IsNullOrEmpty(fencedCodeBlock.Info))
        {
            panel = fencedCodeBlock.Info!.ToLowerInvariant() switch
            {
                "json" => new Panel(new JsonText(code)),
                "csharp" => new Panel(new CSharpText(code)),
                "xml" => new Panel(new XmlText(code)),
                _ => null
            };

            if (panel is not null)
            {
                panel.Header = new PanelHeader(fencedCodeBlock.Info.EscapeMarkup());
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