using System;
using System.Globalization;
using System.Text;
using UnityEngine;

public static class NumberFormat
{
    private static int ALPHABET_LETTERS_SIZE = 26;

    private static CultureInfo CultureInfo => CultureInfo.CurrentCulture;

    private static string[] _shortNames = new string[]
    {
        "k",
        "M",
        "B",
        "T",
    };

    public static string ToShortFormatAsPercentage(this double value)
    {
        return value.ToShortFormat()+"%";
    }
    
    private static StringBuilder _builder = new StringBuilder();

    public static string ToShortFormat(this int value) => ToShortFormat((double)value);
    public static string ToShortFormat(this float value) => ToShortFormat((double)value); 
    public static string ToShortFormat(this double value)
    {
        bool negative = value < 0;
        double zerosNumber = Math.Log10(Math.Abs(value));

        if (zerosNumber < 3) // If under the thousands, no need to convert
        {
            return value.ToString($"#0.##", CultureInfo);
        }

        int prefix = (int)(zerosNumber / 3);
        double number = value / (Math.Pow(10, ((prefix) * 3)));

        _builder.Clear();
        if (negative)
        {
            _builder.Append("-");
        }
        _builder.Append(number.ToString($"#0.##", CultureInfo));
        string prefixText = GetPrefix(prefix);

        _builder.Append(prefixText);
        return _builder.ToString();
    }

    private static string GetPrefix(int suffix)
    {
        if (suffix <= _shortNames.Length)
            return _shortNames[Mathf.Max(0, suffix - 1)];

        suffix -= _shortNames.Length;
        // the prefix begin with aa, then ab, until infinite
        int tens = 'a'+suffix / ALPHABET_LETTERS_SIZE;
        int units = 'a'+suffix % ALPHABET_LETTERS_SIZE;
        return $"{(char)tens}{(char)(units-1)}";
}
}
