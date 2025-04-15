namespace Spectre.Console.Javascript;

internal static class JavascriptTokenizer
{
    private static readonly HashSet<string> Keywords = new()
    {
        "abstract", "arguments", "async", "await", "boolean", "break", "byte",
        "case", "catch", "char", "class", "const", "continue", "constructor",
        "debugger", "declare", "default", "delete", "do", "double", "else",
        "enum", "eval", "export", "extends", "false", "final", "finally", "float",
        "for", "from", "function", "get", "global", "goto", "if", "implements",
        "import", "in", "infer", "instanceof", "interface", "is", "keyof", "let",
        "module", "namespace", "native", "new", "null", "number", "object", "of",
        "package", "private", "protected", "public", "readonly", "require", "return",
        "set", "short", "static", "string", "super", "switch", "symbol", "satisfies",
        "this", "throw", "true", "try", "type", "typeof", "undefined", "unique",
        "unknown", "var", "void", "volatile", "while", "with", "yield",

        //typescipt:
        "type", "interface", "implements", "declare", "namespace", "module",
        "readonly", "keyof", "infer", "unknown", "never", "asserts", "satisfies"
    };

    public static List<JavascriptToken> ParseText(string javascript)
    {
        var tokens = new List<JavascriptToken>();
        var span = javascript.AsSpan();
        var i = 0;

        void AddToken(int start, int len, JavascriptSyntaxKind kind)
        {
            tokens.Add(new JavascriptToken(new TextSpan(start, len), kind));
        }

        while (i < span.Length)
        {
            var c = span[i];

            // Whitespace
            if (char.IsWhiteSpace(c))
            {
                i++;
                continue;
            }

            // Multi-line Comment
            if (c == '/' && i + 1 < span.Length && span[i + 1] == '*')
            {
                var start = i;
                i += 2;
                while (i < span.Length)
                {
                    if (span[i] == '*' && i + 1 < span.Length && span[i + 1] == '/')
                    {
                        i += 2; // Include the closing */
                        break;
                    }
                    i++;
                }

                AddToken(start, i - start, JavascriptSyntaxKind.Comment);
                continue;
            }

            // Comment
            if (c == '/' && i + 1 < span.Length && span[i + 1] == '/')
            {
                var start = i;
                i += 2;
                while (i < span.Length && span[i] != '\n')
                {
                    i++;
                }
                if (span[i] == '\n')
                {
                    i++; // Include the newline character
                }

                AddToken(start, i - start, JavascriptSyntaxKind.Comment);
                continue;
            }

            // String
            if (c == '\"' || c == '\'' || c == '`')
            {
                var start = i;
                var quoteType = c;
                i++;

                while (i < span.Length)
                {
                    if (span[i] == quoteType && (i == start + 1 || span[i - 1] != '\\'))
                    {
                        i++;
                        break;
                    }
                    i++;
                }

                AddToken(start, i - start, JavascriptSyntaxKind.StringValue);
                continue;
            }

            // Number
            if (char.IsDigit(c))
            {
                var start = i;
                while (i < span.Length && (char.IsDigit(span[i]) || span[i] == '.'))
                {
                    i++;
                }

                AddToken(start, i - start, JavascriptSyntaxKind.Number);
                continue;
            }

            // Identifier or keyword
            if (char.IsLetter(c) || c == '_' || c == '$')
            {
                var start = i;
                while (i < span.Length && (char.IsLetterOrDigit(span[i]) || span[i] == '_' || span[i] == '$'))
                {
                    i++;
                }

                var token = span.Slice(start, i - start).ToString();
                var kind = Keywords.Contains(token) ? JavascriptSyntaxKind.Keyword : JavascriptSyntaxKind.Identifier;
                AddToken(start, i - start, kind);
                continue;
            }

            // Operators & symbols
            switch (c)
            {
                case '=':
                    AddToken(i, 1, JavascriptSyntaxKind.Equals);
                    i++;
                    break;
                case '*':
                case '+':
                case '-':
                case '/':
                    AddToken(i, 1, JavascriptSyntaxKind.Operator);
                    i++;
                    break;
                case '.':
                    AddToken(i, 1, JavascriptSyntaxKind.Dot);
                    i++;
                    break;
                case ',':
                    AddToken(i, 1, JavascriptSyntaxKind.Comma);
                    i++;
                    break;
                case ';':
                    AddToken(i, 1, JavascriptSyntaxKind.SequenceTerminator);
                    i++;
                    break;
                case '(':
                    AddToken(i, 1, JavascriptSyntaxKind.OpenParenthesis);
                    i++;
                    break;
                case ')':
                    AddToken(i, 1, JavascriptSyntaxKind.CloseParenthesis);
                    i++;
                    break;
                default:
                    AddToken(i, 1, JavascriptSyntaxKind.NotDefined);
                    i++;
                    break;
            }
        }

        return tokens;
    }
}