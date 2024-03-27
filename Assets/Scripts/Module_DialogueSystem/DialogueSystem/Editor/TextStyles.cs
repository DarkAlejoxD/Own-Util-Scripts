using UnityEngine;

public static class TextStyles
{
    public static GUIStyle HeaderStyle { get; private set; }
    static TextStyles()
    {
        HeaderStyle = new();
        HeaderStyle.fontStyle = FontStyle.Bold;
        HeaderStyle.normal.textColor = Color.white;
        HeaderStyle.alignment = TextAnchor.MiddleLeft;
    }
}
