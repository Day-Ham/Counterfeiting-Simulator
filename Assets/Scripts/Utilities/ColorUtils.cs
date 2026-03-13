using UnityEngine;

public static class ColorUtils
{
    private const float RGB_SCALE = 255f;

    private static int ToRGBInt(float value)
    {
        return Mathf.RoundToInt(value * RGB_SCALE);
    }

    public static int Red(Color color) => ToRGBInt(color.r);
    public static int Green(Color color) => ToRGBInt(color.g);
    public static int Blue(Color color) => ToRGBInt(color.b);

    public static Vector3Int ToRGB(Color color)
    {
        return new Vector3Int(
            ToRGBInt(color.r),
            ToRGBInt(color.g),
            ToRGBInt(color.b)
        );
    }

    public static Color FromRGB(int red, int green, int blue)
    {
        return new Color(
            red / RGB_SCALE,
            green / RGB_SCALE,
            blue / RGB_SCALE
        );
    }
}
