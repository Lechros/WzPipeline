namespace WzJson.Shared;

public sealed class NaturalStringComparer : IComparer<string>
{
    // Logic taken from: https://github.com/nwoltman/string-natural-compare/blob/master/natural-compare.js
    public int Compare(string? a, string? b)
    {
        if (ReferenceEquals(a, b)) return 0;
        if (a == null) return -1;
        if (b == null) return 1;

        a = a.ToLower();
        b = b.ToLower();

        var ai = 0;
        var bi = 0;
        var firstDiffInLeadingZeros = 0;

        while (ai < a.Length && bi < b.Length)
        {
            var charA = a[ai];
            var charB = b[bi];

            if (char.IsDigit(charA))
            {
                if (!char.IsDigit(charB)) return charA - charB;

                var numStartA = ai;
                var numStartB = bi;

                while (charA == '0' && ++numStartA < a.Length)
                    charA = a[numStartA];
                while (charB == '0' && ++numStartB < b.Length)
                    charB = b[numStartB];

                if (numStartA != numStartB && firstDiffInLeadingZeros == 0)
                    firstDiffInLeadingZeros = numStartA - numStartB;

                var numEndA = numStartA;
                var numEndB = numStartB;

                while (numEndA < a.Length && char.IsDigit(a[numEndA]))
                    ++numEndA;
                while (numEndB < b.Length && char.IsDigit(b[numEndB]))
                    ++numEndB;

                var diff = numEndA - numStartA - numEndB + numStartB; // numA length - numB length
                if (diff != 0) return diff;

                while (numStartA < numEndA)
                {
                    diff = a[numStartA++] - b[numStartB++];
                    if (diff != 0) return diff;
                }

                ai = numEndA;
                bi = numEndB;
                continue;
            }

            if (charA != charB) return charA - charB;

            ++ai;
            ++bi;
        }

        if (ai < a.Length) return 1;
        if (bi < b.Length) return -1;

        return firstDiffInLeadingZeros;
    }
}