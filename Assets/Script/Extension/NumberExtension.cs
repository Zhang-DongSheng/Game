using System;

public static partial class NumberExtension
{
    public static bool EqualOrMoreThan(this int self, int value)
    {
        return self >= value;
    }

    public static bool EqualOrLessThan(this int self, int value)
    {
        return self <= value;
    }

    public static bool Between(this int self, int min, int max)
    {
        return self >= min && self <= max;
    }

    public static bool Near(this int self, int other, int distance)
    {
        return self.Between(other - distance, other + distance);
    }

    public static bool EqualOrMoreThan(this float self, float value)
    {
        return self >= value;
    }

    public static bool EqualOrLessThan(this float self, float value)
    {
        return self <= value;
    }

    public static bool Between(this float self, float min, float max)
    {
        return self >= min && self <= max;
    }

    public static bool Near(this float self, float other, float distance)
    {
        return self.Between(other - distance, other + distance);
    }

    public static string ToSimple(this int self, int digit)
    {
        float number;

        string unit = string.Empty;

        if (self < 1000)
        {
            number = self;
        }
        else if (self < 1000000)
        {
            number = self / 1000; unit = "k+";
        }
        else
        {
            number = self / 1000000; unit = "m+";
        }
        return string.Format("{0}{1}", Math.Round(number, digit), unit);
    }
}
