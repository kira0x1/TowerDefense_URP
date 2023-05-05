using UnityEngine;

namespace Kira.GridGen
{
    public static class GridUtils
    {
        public static Vector3 GetMouseWorldPosition(Camera camera)
        {
            Vector3 pos = GetMouseWorldPositionWithZ(Input.mousePosition, camera);
            pos.z = 0f;
            return pos;
        }

        public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
        {
            Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
            return worldPosition;
        }

        public static TextMesh CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default, int fontSize = 40, Color? color = null)
        {
            color ??= Color.white;

            GameObject go = new GameObject("World_Text", typeof(TextMesh));
            Transform textObj = go.transform;
            textObj.SetParent(parent, false);
            textObj.localPosition = localPosition;
            TextMesh textMesh = go.GetComponent<TextMesh>();
            textMesh.fontSize = fontSize;
            textMesh.text = text;
            textMesh.color = (Color)color;
            textMesh.alignment = TextAlignment.Center;
            textMesh.anchor = TextAnchor.MiddleCenter;
            return textMesh;
        }
    }
}