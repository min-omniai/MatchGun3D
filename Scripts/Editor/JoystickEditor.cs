using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(JoystickController))]
public class JoystickEditor : Editor
{
    [MenuItem("Create/Joystick")]
    public static void Create()
    {
        GameObject go = GameObject.Instantiate(Resources.Load("JoystickCanvas")) as GameObject;
        go.name = "JoystickCanvas";
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
    }
}
