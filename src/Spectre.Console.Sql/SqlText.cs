using Spectre.Console.Rendering;

namespace Spectre.Console.Sql;

/// <summary>
/// A renderable piece of SQL text.
/// </summary>
public sealed class SqlText : JustInTimeRenderable
{
    private readonly string _sql;

    /// <summary>
    /// Gets or sets the style for string values in SQL.
    /// </summary>
    public Style? StringValueStyle { get; set; }

    /// <summary>
    /// Gets or sets the style for numbers in SQL.
    /// </summary>
    public Style? NumberStyle { get; set; }

    /// <summary>
    /// Gets or sets the style for SQL keywords.
    /// </summary>
    public Style? KeywordStyle { get; set; }

    /// <summary>
    /// Gets or sets the style for SQL identifiers.
    /// </summary>
    public Style? IdentifierStyle { get; set; }

    /// <summary>
    /// Gets or sets the style for SQL operators.
    /// </summary>
    public Style? OperatorStyle { get; set; }

    /// <summary>
    /// Gets or sets the style for commas in SQL.
    /// </summary>
    public Style? CommaStyle { get; set; }

    /// <summary>
    /// Gets or sets the style for dots in SQL.
    /// </summary>
    public Style? DotStyle { get; set; }

    /// <summary>
    /// Gets or sets the style for open parentheses in SQL.
    /// </summary>
    public Style? OpenParenthesisStyle { get; set; }

    /// <summary>
    /// Gets or sets the style for close parentheses in SQL.
    /// </summary>
    public Style? CloseParenthesisStyle { get; set; }

    /// <summary>
    /// Gets or sets the style for sequence terminators in SQL.
    /// </summary>
    public Style? SequenceTerminatorStyle { get; set; }

    /// <summary>
    /// Gets or sets the style for undefined SQL elements.
    /// </summary>
    public Style? NotDefinedStyle { get; set; }

    /// <summary>
    /// Gets or sets the style for SQL comments.
    /// </summary>
    public Style? CommentStyle { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SqlText"/> class.
    /// </summary>
    /// <param name="sql">The SQL to render.</param>
    public SqlText(string? sql)
    {
        _sql = sql?.Trim() ?? throw new ArgumentNullException(nameof(sql));
    }

    /// <inheritdoc/>
    protected override IRenderable Build()
    {
        var styles =
            new SqlTextStyles
            {
                NumberStyle = NumberStyle ?? Color.DeepSkyBlue3_1,
                StringValueStyle = StringValueStyle ?? Color.DarkSlateGray1,
                CommentStyle = CommentStyle ?? new Style(foreground: Color.FromHex("006400")),
                KeywordStyle = KeywordStyle ?? Color.Turquoise2,
                IdentifierStyle = IdentifierStyle ?? Color.White,
                OperatorStyle = OperatorStyle ?? Color.DeepSkyBlue4_2,
                CommaStyle = CommaStyle ?? Color.DeepSkyBlue4_2,
                DotStyle = DotStyle ?? Color.DeepSkyBlue4_2,
                OpenParenthesisStyle = OpenParenthesisStyle ?? Color.DeepSkyBlue4_2,
                CloseParenthesisStyle = CloseParenthesisStyle ?? Color.DeepSkyBlue4_2,
                SequenceTerminatorStyle = SequenceTerminatorStyle ?? Color.DeepSkyBlue4_2,
                NotDefinedStyle = NotDefinedStyle ?? Color.DeepSkyBlue4_2,
            };

        var syntax = new SqlSyntax(_sql);
        var paragraph = syntax.BuildStyledSqlParagraph(styles);
        return paragraph;
    }
}
