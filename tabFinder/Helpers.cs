using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

class CompareByArtistName : IComparer<string> {
    public int Compare(string x, string y) {

        if(x == null || y == null) {
            return 0;
        }

        string artistX = x.Split("-")[1];
        string artistY = y.Split("-")[1];

        return artistX.CompareTo(artistY);
    }
}

static class BetterContains {
    public static bool Contains(string a, string b) {

        string pattern = $@"\b{Regex.Escape(a)}\b";
        return Regex.IsMatch(b, pattern);
    }
}