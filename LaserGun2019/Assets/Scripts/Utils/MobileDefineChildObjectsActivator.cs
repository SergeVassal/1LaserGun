using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class MobileDefineChildObjectsActivator : MonoBehaviour
{
#if !UNITY_EDITOR
    void OnEnable()
    {
        ScriptingDefineControlsChildObjectsState();
    }
#endif

    private void Start()
    {
#if UNITY_EDITOR
        if (Application.isPlaying)
#endif
        {
            UnityEngine.EventSystems.EventSystem system = GameObject.FindObjectOfType<UnityEngine.EventSystems.EventSystem>();

            if (system == null)
            {
                GameObject eventSystemGO = new GameObject("EventSystem");
                eventSystemGO.AddComponent<UnityEngine.EventSystems.EventSystem>();
                eventSystemGO.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
            }
        }
    }

#if UNITY_EDITOR
    private void OnEnable()
    {
        EditorUserBuildSettings.activeBuildTargetChanged += ScriptingDefineControlsChildObjectsState;
        EditorApplication.update += ScriptingDefineControlsChildObjectsState;
    }

    private void OnDisable()
    {
        EditorUserBuildSettings.activeBuildTargetChanged -= ScriptingDefineControlsChildObjectsState;
        EditorApplication.update -= ScriptingDefineControlsChildObjectsState;
    }    
#endif

    private void ScriptingDefineControlsChildObjectsState()
    {
#if MOBILE_INPUT
        EnableChildObjects();
#else
        DisableChildObjects();
#endif
    }

    private void EnableChildObjects()
    {
        foreach(Transform t in transform)
        {
            t.gameObject.SetActive(true);
        }
    }

    private void DisableChildObjects()
    {
        foreach (Transform t in transform)
        {
            t.gameObject.SetActive(false);
        }
    }
}
