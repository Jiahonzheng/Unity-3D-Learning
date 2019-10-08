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
            Vector3 startPosition = EditorGUILayout.Vector3Field("Start Position", UFOModel.startPosition);
            UFOModel.startPosition = startPosition;

            EditorGUILayout.Space();
            Vector3 startSpeed = EditorGUILayout.Vector3Field("Initial Speed", UFOModel.startSpeed);
            UFOModel.startSpeed = startSpeed;

            EditorGUILayout.Space();
            Vector3 localScale = EditorGUILayout.Vector3Field("Local Scale", UFOModel.localScale);
            UFOModel.localScale = localScale;
        }
    }
}