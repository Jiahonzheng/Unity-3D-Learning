using UnityEditor;
using UnityEngine;

namespace HitUFO
{
    [CustomEditor(typeof(UFOModel))]
    public class UFOEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var target = (UFOModel)serializedObject.targetObject;

            EditorGUILayout.Space();
            Vector3 startPosition = EditorGUILayout.Vector3Field("Start Position", target.startPosition);
            target.startPosition = startPosition;

            EditorGUILayout.Space();
            Vector3 startSpeed = EditorGUILayout.Vector3Field("Initial Speed", target.startSpeed);
            target.startSpeed = startSpeed;

            EditorGUILayout.Space();
            Vector3 localScale = EditorGUILayout.Vector3Field("Local Scale", target.localScale);
            target.localScale = localScale;
        }
    }
}