using TMPro;
using UnityEngine;

public static class InputFieldUtility
{
    private const int MIN_RGB = 0;
    private const int MAX_RGB = 255;
    private const int MAX_DIGITS = 3;

    public static void SetupRGBInput(TMP_InputField input, RGBChannel channel, System.Action<RGBChannel, int> onValidIntValue)
    {
        input.characterValidation = TMP_InputField.CharacterValidation.Integer;
        input.characterLimit = MAX_DIGITS;

        input.onValueChanged.AddListener(value => ClampWhileTyping(input, value));
        input.onEndEdit.AddListener(value => ValidateFinal(input, channel, value, onValidIntValue));
    }

    private static void ClampWhileTyping(TMP_InputField input, string value)
    {
        if (string.IsNullOrEmpty(value)) return;

        if (int.TryParse(value, out int number) && number > MAX_RGB)
        {
            input.SetTextWithoutNotify(MAX_RGB.ToString());
        }
    }

    private static void ValidateFinal(TMP_InputField input, RGBChannel channel, string value, System.Action<RGBChannel, int> onValidIntValue)
    {
        if (!int.TryParse(value, out int number))
        {
            number = MIN_RGB;
        }

        number = Mathf.Clamp(number, MIN_RGB, MAX_RGB);

        input.SetTextWithoutNotify(number.ToString());
        onValidIntValue?.Invoke(channel, number);
    }
}
