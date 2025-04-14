using System.Text;

namespace Spectre.Console.Extensions.Markup.Example;

internal class Program
{
    static void Main(string[] args)
    {
        System.Console.OutputEncoding = Encoding.UTF8;
        System.Console.InputEncoding = Encoding.UTF8;

        var md = MarkupExample.GetSampleText();
        var markup = new MarkdownRenderable(md);
        AnsiConsole.Write(markup);
    }
}
