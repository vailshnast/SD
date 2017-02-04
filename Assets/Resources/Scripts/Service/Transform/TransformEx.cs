using UnityEngine;
public static class TransformEx
{
    public static Transform DestroyChildren(this Transform transform)
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        return transform;
    }
    public static Transform DestroyChildren(this Transform transform, GameObject parentObject)
    {
        foreach (Transform child in parentObject.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        return transform;
    }
    public static GameObject getChild(this Transform transform, GameObject objectToLook, string name)
    {
        Transform[] allChildren = objectToLook.GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            if (child.name == name)
                return child.gameObject;
        }
        return null;
    }
    public static GameObject getChild(this Transform transform, string name)
    {
        Transform[] allChildren = transform.gameObject.GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            if (child.name == name)
                return child.gameObject;
        }
        return null;
    }
    public static Transform FindDeepChild(this Transform aParent, string aName)
    {
        var result = aParent.Find(aName);
        if (result != null)
            return result;
        foreach (Transform child in aParent)
        {
            result = child.FindDeepChild(aName);
            if (result != null)
                return result;
        }
        return null;
    }
    public static Color HexToColor(this MonoBehaviour mono, string hex)
    {
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        return new Color32(r, g, b, 255);
    }
}