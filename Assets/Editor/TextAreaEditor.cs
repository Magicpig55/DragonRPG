using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(TouchMessage)), CanEditMultipleObjects]
public class TextAreaEditor : Editor {

    public SerializedProperty longStringProp;
    void OnEnable() {
        longStringProp = serializedObject.FindProperty("Message");
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();
        longStringProp.stringValue = EditorGUILayout.TextArea(longStringProp.stringValue, GUILayout.MaxHeight(75));
        serializedObject.ApplyModifiedProperties();
    }
}
