using UnityEngine;
using UnityEditor;

using LeonLaci.UI;

namespace LeonLaci.Editor
{
    [CustomEditor (typeof (AnimatedPanel))]
    public class AnimatedPanelEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI ()
        {
            var t = (AnimatedPanel) target;
            var rt = ((RectTransform) t.transform);

            EditorGUILayout.Space ();
            EditorGUILayout.Space ();
            EditorGUILayout.Space ();

            GUILayout.BeginHorizontal ();
            if (GUILayout.Button ("Store Start Values"))
            {
                t.StartPosition = rt.anchoredPosition;
                t.StartOffset = new AnimatedPanel.Offset (rt);
            }
            if (GUILayout.Button ("Store End Values"))
            {
                t.EndPosition = rt.anchoredPosition;
                t.EndOffset = new AnimatedPanel.Offset (rt);
            }
            GUILayout.EndHorizontal ();

            EditorGUILayout.Space ();

            GUILayout.BeginHorizontal ();
            if (GUILayout.Button ("Move To Start"))
            {
                t.INTERNAL_JumpToStart ();
            }
            if (GUILayout.Button ("Move To End"))
            {
                t.INTERNAL_JumpToEnd ();
            }
            GUILayout.EndHorizontal ();

            EditorGUILayout.Space ();
            EditorGUILayout.Space ();
            EditorGUILayout.Space ();

            EditorGUI.BeginChangeCheck ();

            this.serializedObject.Update ();

            SerializedProperty Iterator = this.serializedObject.GetIterator ();

            Iterator.NextVisible (true);

            while (Iterator.NextVisible (false))
            {
                EditorGUILayout.PropertyField (Iterator, true);
            }

            this.serializedObject.ApplyModifiedProperties ();
        }
    }
}
