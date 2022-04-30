using System;

public static class EnumExtensions
{
    public static T Next<T>(this T src) where T : Enum
    {
        var values = (T[]) Enum.GetValues(src.GetType());
        var nextIndex = Array.IndexOf(values, src) + 1;
        return values.Length == nextIndex ? values[0] : values[nextIndex];
    }
}