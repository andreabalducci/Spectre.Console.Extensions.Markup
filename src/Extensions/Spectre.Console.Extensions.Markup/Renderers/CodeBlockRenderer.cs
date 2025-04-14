using System.Text;
using Markdig.Syntax;
using Spectre.Console.CSharp;
using Spectre.Console.Json;
using Spectre.Console.Rendering;
using Spectre.Console.Xml;

namespace Spectre.Console.Extensions.Markup.Renderers;

internal sealed class CodeBlockRenderer : IRenderer<CodeBlock>
{
    public IRenderable Render(CodeBlock codeBlock, BlockRenderer blockRenderer)
    {
        var sb = new StringBuilder();
        var lines = codeBlock.Lines.Lines;

        foreach (var line in lines)
        {
            sb.AppendLine(line.ToString());
        }


        Panel? panel = null;

        if (codeBlock is FencedCodeBlock fencedCodeBlock && !string.IsNullOrEmpty(fencedCodeBlock.Info))
        {
            if (string.Equals(fencedCodeBlock.Info, "json", StringComparison.OrdinalIgnoreCase))
            {
                var jsonText = new JsonText(sb.ToString().Trim());
                panel = new Panel(jsonText)
                {
                    Header = new PanelHeader(fencedCodeBlock.Info.EscapeMarkup())
                };
            }
            else if (string.Equals(fencedCodeBlock.Info, "csharp", StringComparison.OrdinalIgnoreCase))
            {
                var csharpText = new CSharpText(sb.ToString().Trim());
                panel = new Panel(csharpText)
                {
                    Header = new PanelHeader(fencedCodeBlock.Info.EscapeMarkup())
                };
            }
            else if (string.Equals(fencedCodeBlock.Info, "xml", StringComparison.OrdinalIgnoreCase))
            {
                var xmlText = new XmlText(sb.ToString().Trim());
                panel = new Panel(xmlText)
                {
                    Header = new PanelHeader(fencedCodeBlock.Info.EscapeMarkup())
                };
            }
        }

        panel ??= new Panel(sb.ToString().EscapeMarkup().Trim());

        panel.BorderStyle = Color.Blue;
        panel.Padding = new Padding(1, 0, 0, 0);
        panel.Border = new LeftBorder();

        return panel;
    }
}