using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

namespace Spectre.Console.CSharp;

internal sealed class CSharpSyntax : CSharpSyntaxWalker
{
    private readonly string _sourceText;
    private readonly List<(TextSpan Span, Style Style)> _styledSegments;
    private readonly CSharpBuilderContext _context;

    internal CSharpSyntax(string sourceText, CSharpBuilderContext context)
        : base(SyntaxWalkerDepth.Trivia)
    {
        _sourceText = sourceText;
        _context = context;
        _styledSegments = [];
    }

    public override void VisitTrivia(SyntaxTrivia trivia)
    {
        if (
            trivia.IsKind(SyntaxKind.SingleLineCommentTrivia)
            || trivia.IsKind(SyntaxKind.MultiLineCommentTrivia)
        )
        {
            _styledSegments.Add((trivia.Span, _context.Styling.CommentStyle));
        }

        base.VisitTrivia(trivia);
    }

    public override void VisitToken(SyntaxToken token)
    {
        if (token.IsMissing)
        {
            return;
        }

        var style = token.Kind() switch
        {
            // Braces: { or }
            SyntaxKind.OpenBraceToken
            or SyntaxKind.CloseBraceToken
                => _context.Styling.BracesStyle,

            // Brackets: [ or ]
            SyntaxKind.OpenBracketToken
            or SyntaxKind.CloseBracketToken
                => _context.Styling.BracketsStyle,

            // Identifiers: Variable or member names
            SyntaxKind.IdentifierToken
                => _context.Styling.MemberStyle,

            // Keywords: Reserved words in C#
            SyntaxKind.AbstractKeyword
            or SyntaxKind.AsKeyword
            or SyntaxKind.BaseKeyword
            or SyntaxKind.BoolKeyword
            or SyntaxKind.BreakKeyword
            or SyntaxKind.ByteKeyword
            or SyntaxKind.CaseKeyword
            or SyntaxKind.CatchKeyword
            or SyntaxKind.CharKeyword
            or SyntaxKind.CheckedKeyword
            or SyntaxKind.ClassKeyword
            or SyntaxKind.ConstKeyword
            or SyntaxKind.ContinueKeyword
            or SyntaxKind.DecimalKeyword
            or SyntaxKind.DefaultKeyword
            or SyntaxKind.DelegateKeyword
            or SyntaxKind.DoKeyword
            or SyntaxKind.DoubleKeyword
            or SyntaxKind.ElseKeyword
            or SyntaxKind.EnumKeyword
            or SyntaxKind.EventKeyword
            or SyntaxKind.ExplicitKeyword
            or SyntaxKind.ExternKeyword
            or SyntaxKind.FalseKeyword
            or SyntaxKind.FinallyKeyword
            or SyntaxKind.FixedKeyword
            or SyntaxKind.FloatKeyword
            or SyntaxKind.ForKeyword
            or SyntaxKind.ForEachKeyword
            or SyntaxKind.GotoKeyword
            or SyntaxKind.IfKeyword
            or SyntaxKind.ImplicitKeyword
            or SyntaxKind.InKeyword
            or SyntaxKind.IntKeyword
            or SyntaxKind.InterfaceKeyword
            or SyntaxKind.InternalKeyword
            or SyntaxKind.IsKeyword
            or SyntaxKind.LockKeyword
            or SyntaxKind.LongKeyword
            or SyntaxKind.NamespaceKeyword
            or SyntaxKind.NewKeyword
            or SyntaxKind.NullKeyword
            or SyntaxKind.ObjectKeyword
            or SyntaxKind.OperatorKeyword
            or SyntaxKind.OutKeyword
            or SyntaxKind.OverrideKeyword
            or SyntaxKind.OrKeyword
            or SyntaxKind.ParamsKeyword
            or SyntaxKind.PrivateKeyword
            or SyntaxKind.ProtectedKeyword
            or SyntaxKind.PublicKeyword
            or SyntaxKind.ReadOnlyKeyword
            or SyntaxKind.RefKeyword
            or SyntaxKind.ReturnKeyword
            or SyntaxKind.RecordKeyword
            or SyntaxKind.SByteKeyword
            or SyntaxKind.SealedKeyword
            or SyntaxKind.ShortKeyword
            or SyntaxKind.SizeOfKeyword
            or SyntaxKind.StackAllocKeyword
            or SyntaxKind.StaticKeyword
            or SyntaxKind.StringKeyword
            or SyntaxKind.StructKeyword
            or SyntaxKind.SwitchKeyword
            or SyntaxKind.ThisKeyword
            or SyntaxKind.ThrowKeyword
            or SyntaxKind.TrueKeyword
            or SyntaxKind.TryKeyword
            or SyntaxKind.TypeOfKeyword
            or SyntaxKind.UIntKeyword
            or SyntaxKind.ULongKeyword
            or SyntaxKind.UncheckedKeyword
            or SyntaxKind.UnsafeKeyword
            or SyntaxKind.UShortKeyword
            or SyntaxKind.UsingKeyword
            or SyntaxKind.VirtualKeyword
            or SyntaxKind.VoidKeyword
            or SyntaxKind.VolatileKeyword
            or SyntaxKind.WhileKeyword
            or SyntaxKind.YieldKeyword
                => _context.Styling.KeywordStyle,

            // String literals: "text"
            SyntaxKind.StringLiteralToken
                => _context.Styling.StringStyle,
            SyntaxKind.StringLiteralExpression => _context.Styling.StringStyle,

            // Default style for unhandled tokens
            _ => Color.White
        };

        _styledSegments.Add((token.Span, style));
        base.VisitToken(token);
    }

    internal void Accept(SyntaxNode rootNode)
    {
        Visit(rootNode);

        var sortedSegments = _styledSegments.OrderBy(s => s.Span.Start).ToList();
        var currentPos = 0;

        foreach (var (span, style) in sortedSegments)
        {
            if (currentPos < span.Start)
            {
                _context.Paragraph.Append(
                    _sourceText.Substring(currentPos, span.Start - currentPos),
                    style
                );
            }

            var tokenText = _sourceText.Substring(span.Start, span.Length);
            _context.Paragraph.Append($"{Markup.Escape(tokenText)}", style);
            currentPos = span.End;
        }

        if (currentPos < _sourceText.Length)
        {
            _context.Paragraph.Append(_sourceText.Substring(currentPos), Color.DarkSlateGray1);
        }
    }
}
