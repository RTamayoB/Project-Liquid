using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextUtils
{
    internal static TextMesh CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 40, Color? color = null, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = 5000, Vector3? colliderSize = null)
    {
        if (color == null) color = Color.white;
        return CreateWorldText(parent, text, localPosition, fontSize, color, textAnchor, textAlignment, sortingOrder, colliderSize);
    }

    //TODO Pass Cellsize for collider
    internal static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color? color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder, Vector3? colliderSize)
    {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        if(colliderSize != null)
        {
            BoxCollider collider = gameObject.AddComponent<BoxCollider>();
            collider.center = Vector3.zero;
            collider.size = (Vector3)colliderSize;
        }
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = (Color)color;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        return textMesh;
    }
}
