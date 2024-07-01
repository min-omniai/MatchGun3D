using System.Collections.Generic;
using UnityEngine;

public class Util : MonoBehaviour
{
    private static Dictionary<float, WaitForSeconds> s_waitCache = new Dictionary<float, WaitForSeconds>();
    public static WaitForSeconds GetWaitForSeconds(float seconds)
    {
        if (s_waitCache.TryGetValue(seconds, out WaitForSeconds cached)) return cached;
        return s_waitCache[seconds] = new WaitForSeconds(seconds);
    }
    public static WaitForFixedUpdate WaitFixed = new WaitForFixedUpdate();


    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();
        if (component == null)
            component = go.AddComponent<T>();

        return component;
    }

    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform tf = FindChild<Transform>(go, name, recursive);

        if (tf == null)
            return null;

        return tf.gameObject;
    }

    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null)
            return null;

        if (recursive == false)
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform tf = go.transform.GetChild(i);

                if (string.IsNullOrEmpty(name) || tf.name == name)
                {
                    T component = tf.GetComponent<T>();
                    if (component != null)
                        return component;
                }
            }
        }
        else
        {
            foreach (T component in go.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }

        return null;
    }
}
