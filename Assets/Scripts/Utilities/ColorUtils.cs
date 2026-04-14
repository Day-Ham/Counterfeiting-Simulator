using UnityEngine;

public static class ColorUtils
{
    private const float RGB_LIMIT = 255f;

    private static int ToRGBInt(float value)
    {
        return Mathf.RoundToInt(value * RGB_LIMIT);
    }

    public static int Red(Color color) => ToRGBInt(color.r);
    public static int Green(Color color) => ToRGBInt(color.g);
    public static int Blue(Color color) => ToRGBInt(color.b);

    public static Color FromRGB(int red, int green, int blue)
    {
        return new Color(
            red / RGB_LIMIT,
            green / RGB_LIMIT,
            blue / RGB_LIMIT
        );
    }
}
