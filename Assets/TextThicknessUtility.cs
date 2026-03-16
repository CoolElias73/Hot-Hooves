using UnityEngine;
using TMPro;

public static class TextThicknessUtility
{
    public static void ApplyBoldToAllTexts(bool includeInactive = true)
    {
        var texts = Object.FindObjectsOfType<TMP_Text>(includeInactive);
        for (int i = 0; i < texts.Length; i++)
        {
            var text = texts[i];
            if (text == null)
                continue;

            text.fontStyle |= FontStyles.Bold;
        }
    }
}
