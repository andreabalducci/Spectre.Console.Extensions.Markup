using Microsoft.CodeAnalysis.CSharp;
using Spectre.Console.Rendering;

namespace Spectre.Console.CSharp;

/// <summary>
/// A renderable piece of CSHARP text.
/// </summary>
public sealed class CSharpText : JustInTimeRenderable
{
    private readonly string _csharp;

    /// <summary>
    /// Gets or sets the style used for braces.
    /// </summary>
    public Style? BracesStyle { get; set; }

    /// <summary>
    /// Gets or sets the style used for brackets.
    /// </summary>
    public Style? BracketsStyle { get; set; }

    /// <summary>
    /// Gets or sets the style used for member names.
    /// </summary>
    public Style? MemberStyle { get; set; }

    /// <summary>
    /// Gets or sets the style used for string literals.
    /// </summary>
    public Style? StringStyle { get; set; }

    /// <summary>
    /// Gets or sets the style used for keywords.
    /// </summary>
    public Style? KeywordStyle { get; set; }

    /// <summary>
    /// Gets or sets the style used for keywords.
    /// </summary>
    public Style? CommentStyle { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CSharpText"/> class.
    /// </summary>
    /// <param name="csharp">The C# to render.</param>
    public CSharpText(string csharp)
    {
        _csharp = csharp ?? throw new ArgumentNullException(nameof(csharp));
    }

    /// <inheritdoc/>
    protected override IRenderable Build()
    {
        var context = new CSharpBuilderContext(
            new CSharpTextStyles
            {
                BracesStyle = BracesStyle ?? Color.Yellow,
                BracketsStyle = BracketsStyle ?? Color.Grey,
                MemberStyle = MemberStyle ?? Color.Blue,
                StringStyle = StringStyle ?? Color.Red,
                KeywordStyle = KeywordStyle ?? new Style(foreground: Color.FromHex("F4206B")),
                CommentStyle = CommentStyle ?? new Style(foreground: Color.FromHex("006400"))
            }
        );

        var syntax = new CSharpSyntax(_csharp, context);
        var tree = CSharpSyntaxTree.ParseText(_csharp);
        syntax.Accept(tree.GetRoot());
        return context.Paragraph;
    }
}
