namespace Spectre.Console.Extensions.Markup.Example;

internal class Program
{
    static void Main(string[] args)
    {
        var md = MarkupExample.GetSampleText();
        var markup = new MarkdownRenderable(md);
        AnsiConsole.Write(markup);
    }
}
