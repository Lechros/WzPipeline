using System.Runtime.InteropServices;

namespace WzJson;

public sealed class NaturalStringComparer : IComparer<string>
{
    public int Compare(string? a, string? b)
    {
        if (ReferenceEquals(a, b)) return 0;
        if (a == null) return -1;
        if (b == null) return 1;
        return NativeMethods.StrCmpLogicalW(a, b);
    }

    private static class NativeMethods
    {
        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
        public static extern int StrCmpLogicalW(string psz1, string psz2);
    }
}