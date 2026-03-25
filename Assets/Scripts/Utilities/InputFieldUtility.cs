using TMPro;
using UnityEngine;

public static class InputFieldUtility
{
    private const int MinRGB = 0;
    private const int MaxRGB = 255;
    private const int MaxDigits = 3;

    public static void SetupRGBInput(TMP_InputField input, RGBChannel channel, System.Action<RGBChannel, int> onValidIntValue)
    {
        input.characterValidation = TMP_InputField.CharacterValidation.Integer;
        input.characterLimit = MaxDigits;

        input.onValueChanged.AddListener(value => ClampWhileTyping(input, value));
        input.onEndEdit.AddListener(value => ValidateFinal(input, channel, value, onValidIntValue));
    }

    private static void ClampWhileTyping(TMP_InputField input, string value)
    {
        if (string.IsNullOrEmpty(value)) return;

        if (int.TryParse(value, out int number) && number > MaxRGB)
        {
            input.SetTextWithoutNotify(MaxRGB.ToString());
        }
    }

    private static void ValidateFinal(TMP_InputField input, RGBChannel channel, string value, System.Action<RGBChannel, int> onValidIntValue)
    {
        if (!int.TryParse(value, out int number))
        {
            number = MinRGB;
        }

        number = Mathf.Clamp(number, MinRGB, MaxRGB);

        input.SetTextWithoutNotify(number.ToString());
        onValidIntValue?.Invoke(channel, number);
    }
}
