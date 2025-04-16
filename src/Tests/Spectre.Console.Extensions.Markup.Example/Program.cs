using System.Text;
using Spectre.Console.Json;
using Spectre.Console.Xml;

namespace Spectre.Console.Extensions.Markup.Example;

internal class Program
{
    static void Main(string[] args)
    {
        System.Console.OutputEncoding = Encoding.UTF8;
        System.Console.InputEncoding = Encoding.UTF8;

        var md = MarkupExample.GetSampleText();
        var markup = new MarkdownRenderable(md);

        //Override Heading colors
        markup.HeadingLevel1Color = Color.Yellow2;
        markup.HeadingLevel2To4Style = Color.DarkOliveGreen1_1;
        markup.HeadingLevel5AndAboveStyle = Color.DarkSeaGreen1_1;

        //use the xml renderer for html
        markup.CodeblockRenderables.Add("html", code => new XmlText(code));

        //Override styles for json
        markup.CodeblockRenderables["json"] = code =>
        {
            var jsonText = new JsonText(code)
            {
                BracesStyle = new Style(foreground: Color.DeepSkyBlue4_1),
                BracketsStyle = new Style(foreground: Color.DeepSkyBlue4_1),
                MemberStyle = new Style(foreground: Color.DeepSkyBlue3_1),
                ColonStyle = new Style(foreground: Color.Yellow),
                CommaStyle = new Style(foreground: Color.Yellow),
                StringStyle = new Style(foreground: Color.DarkSlateGray1),
                NumberStyle = new Style(foreground: Color.Blue),
                BooleanStyle = new Style(foreground: Color.Blue)
            };
            return jsonText;
        };
        AnsiConsole.Write(markup);
    }
}
