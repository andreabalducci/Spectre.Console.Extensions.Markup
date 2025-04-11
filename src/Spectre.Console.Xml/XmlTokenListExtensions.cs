namespace Spectre.Console.Xml;
internal static class XmlTokenListExtensions
{
    public static void Add(this List<XmlToken> tokens, int start, int length, XmlSyntaxKind kind)
    {
        tokens.Add(new XmlToken(new TextSpan(start, length), kind));
    }
}
