using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class MobileDefineChildObjectsActivator : MonoBehaviour 
{
#if UNITY_EDITOR
    private void OnEnable()
    {
        EditorUserBuildSettings.activeBuildTargetChanged += SetChildObjectsState;
        EditorApplication.update += SetChildObjectsState;
    }

    private void OnDisable()
    {
        EditorUserBuildSettings.activeBuildTargetChanged -= SetChildObjectsState;
        EditorApplication.update -= SetChildObjectsState;
    }
#endif

#if !UNITY_EDITOR
    private void OnEnable()
    {
        SetChildObjectsState();
    }
#endif

    private void SetChildObjectsState()
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
