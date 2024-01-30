using Mew.Core.Extensions;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SceneManagement))]
public class SceneManagementEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var sceneManagement = (SceneManagement)target;

        if (GUILayout.Button("Title"))
            sceneManagement.LoadTitle().Forget();
        if (GUILayout.Button("Stage Select"))
            sceneManagement.LoadStageSelect().Forget();
    }
}