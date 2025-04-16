using Markdig.Extensions.Abbreviations;
using Markdig.Extensions.CustomContainers;
using Markdig.Extensions.Emoji;
using Markdig.Extensions.Footnotes;
using Markdig.Extensions.JiraLinks;
using Markdig.Extensions.Mathematics;
using Markdig.Extensions.SmartyPants;
using Markdig.Extensions.Tables;
using Markdig.Extensions.TaskLists;
using Markdig.Syntax.Inlines;
using Spectre.Console;
using Spectre.Console.Extensions.Markup.Renderers;
using Spectre.Console.Rendering;

namespace Spectre.Console.Extensions.Markup;

internal sealed class InlineRenderer
{
    //private readonly HttpClient _httpClient;

    //public MarkdownInlineRendering(HttpClient httpClient)
    //{
    //    _httpClient = httpClient;
    //}

    internal IRenderable RenderInline(Inline inline, Style style, Justify alignment)
    {
        switch (inline)
        {
            // TODO: These features are less adopted in practice and the MarkdownPipeline isn't configured to generate them - feel free to add support!
            case JiraLink jiraLink:
            case SmartyPant smartyPant:
            case TaskList taskList:
            case MathInline mathInline:
            case HtmlEntityInline htmlEntityInline:
            case HtmlInline htmlInline:
            case FootnoteLink footnoteLink:
            case CustomContainerInline customContainerInline:
            case AbbreviationInline abbreviationInline:
            case AutolinkInline autolinkInline:
                break;

            case EmojiInline emojiInline:
                return new Text(Emoji.Replace(emojiInline.Content.ToString()), style) { Justification = alignment };
            case CodeInline codeInline:
                return new Text(codeInline.Content ?? string.Empty, new Style(background: Color.Grey));

            case EmphasisInline emphasisInline:
                var styleDecoration =
                    emphasisInline.DelimiterChar == '~'
                        ? Decoration.Strikethrough
                        : emphasisInline.DelimiterCount switch
                        {
                            1 => Decoration.Italic,
                            2 => Decoration.Bold,
                            _ => Decoration.None,
                        };

                var emphasisChildStyle = new Style(decoration: styleDecoration, foreground: styleDecoration == Decoration.Bold ? Color.White : Color.Default);
                return RenderContainerInline(emphasisInline, emphasisChildStyle);
            case LinkInline linkInline:
                if (linkInline.IsImage)
                {
                    //if (this.TryGetCanvasImageForUrl(linkInline.Url, out var canvasImage))
                    //{
                    //    return canvasImage;
                    //}

                    return new Text($"- image ({linkInline.Url}) here - ");
                }

                var linkInlineChildStyle = new Style(link: linkInline.Url);
                return RenderContainerInline(linkInline, linkInlineChildStyle);

            // We don't care what delimiters were used to compose a particular document structure
            case PipeTableDelimiterInline:
                break;
            case EmphasisDelimiterInline:
                break;
            case LinkDelimiterInline:
                break;
            case LineBreakInline:
                return Text.NewLine;
            case ContainerInline containerInline:
                return RenderContainerInline(containerInline);
            case LiteralInline literalInline:
                return new Text(literalInline.Content.ToString(), style) { Justification = alignment };
            default:
                throw new ArgumentOutOfRangeException(nameof(inline));
        }

        return Text.Empty;
    }

    public IRenderable RenderContainerInline(ContainerInline inline, Style? style = null, Justify alignment = Justify.Left)
    {
        return new CompositeRenderable(inline.Select(x => RenderInline(x, style ?? Style.Plain, alignment)));
    }

    //private bool TryGetCanvasImageForUrl(string url, out CanvasImage canvasImage)
    //{
    //    // TODO: Refactor this for easier unit testing - could do an initial "get image" pass to parallelise the IO too?
    //    try
    //    {
    //        var imageStream = _httpClient.GetStreamAsync(url).Result;
    //        using var memoryStream = new MemoryStream();
    //        imageStream.CopyTo(memoryStream);

    //        // TODO: Make the canvas size a little more dynamic
    //        canvasImage = new CanvasImage(memoryStream.ToArray());
    //        return true;
    //    }
    //    catch (Exception)
    //    {
    //        canvasImage = null!;
    //        return false;
    //    }
    //}
}