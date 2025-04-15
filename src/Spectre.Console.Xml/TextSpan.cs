namespace Spectre.Console;

internal readonly struct TextSpan
{
    //
    // Summary:
    //     Start point of the span.
    public int Start { get; }

    //
    // Summary:
    //     End of the span.
    public int End => Start + Length;

    //
    // Summary:
    //     Length of the span.
    public int Length { get; }

    //
    // Summary:
    //     Creates a TextSpan instance beginning with the position Start and having the
    //     Length specified with length.
    public TextSpan(int start, int length)
    {
        if (start < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(start));
        }

        if (start + length < start)
        {
            throw new ArgumentOutOfRangeException(nameof(length));
        }

        Start = start;
        Length = length;
    }

    public override string ToString()
    {
        return $"[{Start}..{End})";
    }
}
