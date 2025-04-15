using Spectre.Console;
using Spectre.Console.Sql;

internal static class SqlTokenizer
{
    private static readonly HashSet<string> _keywords = new(StringComparer.OrdinalIgnoreCase)
    {
        "SELECT", "FROM", "WHERE", "AND", "OR", "NOT", "IN", "IS", "NULL", "LIKE", "BETWEEN",
        "ON", "GROUP", "BY", "ORDER", "ASC", "DESC", "LIMIT", "OFFSET", "TOP", "AS", "CASE", "WHEN", "THEN", "ELSE", "END",
        "INSERT", "INTO", "VALUES", "UPDATE", "SET", "DELETE", "JOIN", "INNER", "LEFT", "RIGHT", "FULL OUTER JOIN", "CROSS JOIN",
        "CREATE", "TABLE", "ALTER", "DROP", "INDEX", "VIEW", "TRIGGER", "PROCEDURE", "FUNCTION",
        "EXISTS", "UNION", "ALL", "DISTINCT", "HAVING", "EXPLAIN", "DESCRIBE", "SHOW", "USE",
        "DATABASE", "SCHEMA", "GRANT", "REVOKE", "COMMIT", "ROLLBACK", "TRANSACTION", "SAVEPOINT", "ROLLBACK TO SAVEPOINT", "SET TRANSACTION",
        "LOCK", "UNLOCK", "BEGIN", "RETURN", "DECLARE", "FETCH", "CURSOR", "OPEN",
        "CLOSE", "NEXT", "PREVIOUS", "FIRST", "LAST", "ABSOLUTE", "RELATIVE", "ROWNUM", "ROWCOUNT",
        "OVER", "PARTITION BY",
        "INT", "BIGINT", "SMALLINT", "TINYINT", "DECIMAL", "NUMERIC", "FLOAT", "REAL", "BIT", "CHAR", "VARCHAR", "TEXT", "NCHAR", "NVARCHAR", "NTEXT", "DATE", "TIME", "DATETIME", "DATETIME2", "SMALLDATETIME", "TIMESTAMP", "BINARY", "VARBINARY", "IMAGE", "UNIQUEIDENTIFIER", "XML", "JSON", "SQL_VARIANT",
        "ENUM", "SET",
        "WITH", "RECURSIVE", "CTE",
        "PRIMARY", "KEY", "FOREIGN", "REFERENCES", "CHECK", "DEFAULT", "UNIQUE", "CONSTRAINT",
        "AVG", "SUM", "COUNT", "MIN", "MAX", "ABS", "CEIL", "CEILING", "FLOOR", "ROUND", "EXP", "LOG", "LOG10", "POWER", "SQRT", "MOD", "PI", "SIN", "COS", "TAN", "ASIN", "ACOS", "ATAN", "ATAN2", "RAND", "SIGN"

    };

    public static List<SqlToken> ParseText(string sql)
    {
        var tokens = new List<SqlToken>();
        var span = sql.AsSpan(); // Convert the SQL string to a ReadOnlySpan<char>
        var i = 0;

        void AddToken(int start, int len, SqlSyntaxKind kind)
        {
            tokens.Add(new SqlToken(new TextSpan(start, len), kind));
        }

        while (i < span.Length)
        {
            var c = span[i];

            // Skip whitespace
            if (char.IsWhiteSpace(c))
            {
                i++;
                continue;
            }

            // Comment --
            if (c == '-' && i + 1 < span.Length && span[i + 1] == '-')
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
                
                AddToken(start, i - start, SqlSyntaxKind.Comment);
                continue;
            }

            // String literal
            if (c == '\'')
            {
                var start = i++;
                while (i < span.Length)
                {
                    if (span[i] == '\'' && (i + 1 >= span.Length || span[i + 1] != '\''))
                    {
                        i++;
                        break;
                    }
                    else if (span[i] == '\'' && span[i + 1] == '\'')
                    {
                        i += 2; // skip escaped ''
                    }
                    else
                    {
                        i++;
                    }
                }
                AddToken(start, i - start, SqlSyntaxKind.StringValue);
                continue;
            }

            // Number
            if (char.IsDigit(c))
            {
                var start = i;
                while (i < span.Length && (char.IsDigit(span[i]) || span[i] == '.'))
                    i++;
                AddToken(start, i - start, SqlSyntaxKind.Number);
                continue;
            }

            // Identifier or Keyword
            if (char.IsLetter(c) || c == '_')
            {
                var start = i;
                while (i < span.Length && (char.IsLetterOrDigit(span[i]) || span[i] == '_'))
                {
                    i++;
                }

                var token = span.Slice(start, i - start).ToString();
                var kind = _keywords.Contains(token) ? SqlSyntaxKind.Keyword : SqlSyntaxKind.Identifier;
                AddToken(start, i - start, kind);
                continue;
            }

            // Operators and symbols
            switch (c)
            {
                case '=':
                case '>':
                case '<':
                case '!':
                case '+':
                case '-':
                case '*':
                case '/':
                case '%':
                    AddToken(i, 1, SqlSyntaxKind.Operator);
                    i++;
                    break;
                case ',':
                    AddToken(i, 1, SqlSyntaxKind.Comma);
                    i++;
                    break;
                case '.':
                    AddToken(i, 1, SqlSyntaxKind.Dot);
                    i++;
                    break;
                case '(':
                    AddToken(i, 1, SqlSyntaxKind.OpenParenthesis);
                    i++;
                    break;
                case ')':
                    AddToken(i, 1, SqlSyntaxKind.CloseParenthesis);
                    i++;
                    break;
                case ';':
                    AddToken(i, 1, SqlSyntaxKind.SequenceTerminator);
                    i++;
                    break;
                default:
                    AddToken(i, 1, SqlSyntaxKind.NotDefined);
                    i++;
                    break;
            }
        }

        return tokens;
    }
}