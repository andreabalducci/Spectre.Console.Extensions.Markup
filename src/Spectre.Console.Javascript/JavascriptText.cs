using Spectre.Console.Rendering;

namespace Spectre.Console.Javascript;

/// <summary>
/// A renderable piece of JAVASCRIPT text.
/// </summary>
public sealed class JavascriptText : JustInTimeRenderable
{
    private readonly string _javascript;

    /// <summary>
    /// Gets or sets the style for undefined elements.
    /// </summary>
    public Style? NotDefinedStyle { get; set; }

    /// <summary>
    /// Gets or sets the style for the "and" operator.
    /// </summary>
    public Style? AndStyle { get; set; }

    /// <summary>
    /// Gets or sets the style for application elements.
    /// </summary>
    public Style? ApplicationStyle { get; set; }

    /// <summary>
    /// Gets or sets the style for the "between" operator.
    /// </summary>
    public Style? BetweenStyle { get; set; }

    /// <summary>
    /// Gets or sets the style for closing parentheses.
    /// </summary>
    public Style? CloseParenthesisStyle { get; set; }

    /// <summary>
    /// Gets or sets the style for commas.
    /// </summary>
    public Style? CommaStyle { get; set; }

    /// <summary>
    /// Gets or sets the style for date-time values.
    /// </summary>
    public Style? DateTimeValueStyle { get; set; }

    /// <summary>
    /// Gets or sets the style for the equals operator.
    /// </summary>
    public Style? EqualsStyle { get; set; }

    /// <summary>
    /// Gets or sets the style for exception types.
    /// </summary>
    public Style? ExceptionTypeStyle { get; set; }

    /// <summary>
    /// Gets or sets the style for fingerprints.
    /// </summary>
    public Style? FingerprintStyle { get; set; }

    /// <summary>
    /// Gets or sets the style for the "in" operator.
    /// </summary>
    public Style? InStyle { get; set; }

    /// <summary>
    /// Gets or sets the style for invalid elements.
    /// </summary>
    public Style? InvalidStyle { get; set; }

    /// <summary>
    /// Gets or sets the style for the "like" operator.
    /// </summary>
    public Style? LikeStyle { get; set; }

    /// <summary>
    /// Gets or sets the style for the "limit" operator.
    /// </summary>
    public Style? LimitStyle { get; set; }

    /// <summary>
    /// Gets or sets the style for match elements.
    /// </summary>
    public Style? MatchStyle { get; set; }

    /// <summary>
    /// Gets or sets the style for messages.
    /// </summary>
    public Style? MessageStyle { get; set; }

    /// <summary>
    /// Gets or sets the style for the "not equals" operator.
    /// </summary>
    public Style? NotEqualsStyle { get; set; }

    /// <summary>
    /// Gets or sets the style for the "not in" operator.
    /// </summary>
    public Style? NotInStyle { get; set; }

    /// <summary>
    /// Gets or sets the style for the "not like" operator.
    /// </summary>
    public Style? NotLikeStyle { get; set; }

    /// <summary>
    /// Gets or sets the style for numbers.
    /// </summary>
    public Style? NumberStyle { get; set; }

    /// <summary>
    /// Gets or sets the style for the "or" operator.
    /// </summary>
    public Style? OrStyle { get; set; }

    /// <summary>
    /// Gets or sets the style for opening parentheses.
    /// </summary>
    public Style? OpenParenthesisStyle { get; set; }

    /// <summary>
    /// Gets or sets the style for stack frames.
    /// </summary>
    public Style? StackFrameStyle { get; set; }

    /// <summary>
    /// Gets or sets the style for string values.
    /// </summary>
    public Style? StringValueStyle { get; set; }

    /// <summary>
    /// Gets or sets the style for sequence terminators.
    /// </summary>
    public Style? SequenceTerminatorStyle { get; set; }

    /// <summary>
    /// Gets or sets the style for identifiers.
    /// </summary>
    public Style? IdentifierStyle { get; set; }

    /// <summary>
    /// Gets or sets the style for keywords.
    /// </summary>
    public Style? KeywordStyle { get; set; }

    /// <summary>
    /// Gets or sets the style for JavaScript comments.
    /// </summary>
    public Style? CommentStyle { get; set; }

    /// <summary>
    /// Gets or sets the style for operators.
    /// </summary>
    public Style? OperatorStyle { get; set; }

    /// <summary>
    /// Gets or sets the style for dots.
    /// </summary>
    public Style? DotStyle { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="JavascriptText"/> class.
    /// </summary>
    /// <param name="javascript">The JavaScript code to render.</param>
    public JavascriptText(string? javascript)
    {
        _javascript = javascript?.Trim() ?? throw new ArgumentNullException(nameof(javascript));
    }

    /// <inheritdoc/>
    protected override IRenderable Build()
    {
        var styles =
            new JavascriptTextStyles
            {
                NotDefinedStyle = NotDefinedStyle ?? Color.Turquoise2,
                AndStyle = AndStyle ?? Color.Turquoise2,
                ApplicationStyle = ApplicationStyle ?? Color.Turquoise2,
                BetweenStyle = BetweenStyle ?? Color.DeepSkyBlue3_1,
                CommaStyle = CommaStyle ?? Color.White,
                DateTimeValueStyle = DateTimeValueStyle ?? Color.Blue,
                EqualsStyle = EqualsStyle ?? Color.Yellow,
                ExceptionTypeStyle = ExceptionTypeStyle ?? Color.Red,
                FingerprintStyle = FingerprintStyle ?? Color.DarkSlateGray1,
                InStyle = InStyle ?? Color.Green,
                InvalidStyle = InvalidStyle ?? Color.Red,
                LikeStyle = LikeStyle ?? Color.DeepSkyBlue3_1,
                LimitStyle = LimitStyle ?? Color.DarkSlateGray1,
                MatchStyle = MatchStyle ?? Color.Green,
                MessageStyle = MessageStyle ?? Color.White,
                NotEqualsStyle = NotEqualsStyle ?? Color.Yellow,
                NotInStyle = NotInStyle ?? Color.Green,
                NotLikeStyle = NotLikeStyle ?? Color.DeepSkyBlue3_1,
                NumberStyle = NumberStyle ?? Color.Blue,
                OrStyle = OrStyle ?? Color.Green,
                OpenParenthesisStyle = OpenParenthesisStyle ?? Color.DeepSkyBlue4_1,
                CloseParenthesisStyle = CloseParenthesisStyle ?? Color.DeepSkyBlue4_1,
                StackFrameStyle = StackFrameStyle ?? Color.DarkSlateGray1,
                StringValueStyle = StringValueStyle ?? Color.DarkSlateGray1,
                SequenceTerminatorStyle = SequenceTerminatorStyle ?? Color.White,
                IdentifierStyle = IdentifierStyle ?? Color.White,
                KeywordStyle = KeywordStyle ?? Color.DeepSkyBlue3_1,
                CommentStyle = CommentStyle ?? new Style(foreground: Color.FromHex("006400")),
                OperatorStyle = OperatorStyle ?? Color.DeepSkyBlue4_2,
                DotStyle = DotStyle ?? Color.DeepSkyBlue4_2
            };

        var syntax = new JavascriptSyntax(_javascript);
        var paragraph = syntax.BuildStyledJavascriptParagraph(styles);
        return paragraph;
    }
}
