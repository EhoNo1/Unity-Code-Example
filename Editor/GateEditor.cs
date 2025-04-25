using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Riders.Objects;

namespace Riders
{
    [CustomEditor(typeof(Gate))]
    [CanEditMultipleObjects]
    public class GateEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();
            DrawDefaultInspector();

            
            if (GUILayout.Button("Create Next")) CreateNext();

            bool multipleSelected = targets.Length > 1;
            if (multipleSelected && GUILayout.Button("Connect Two Selected Gates")) ConnectGates();
        }

        public void CreateNext()
        {
            Gate gate = (Gate)target;
            Object gatePrefab = Resources.Load("Gate");
            GameObject newGateObject = (GameObject)PrefabUtility.InstantiatePrefab(gatePrefab);
            newGateObject.transform.position = gate.transform.position;
            newGateObject.transform.rotation = gate.transform.rotation;
            newGateObject.transform.localScale = gate.transform.localScale;

            //move the newly created gate forward a bit
            newGateObject.transform.position += newGateObject.transform.forward * 30f;

            //clear the newly created gate's gate list
            Gate nextGate = newGateObject.GetComponent<Gate>();
            nextGate.nextGates = new List<Gate>();

            //and add the newly created gate to our list
            gate.nextGates.Add(nextGate);

            //Report the changes to the editor so that it knows to save them
            EditorUtility.SetDirty(gate);
            PrefabUtility.RecordPrefabInstancePropertyModifications(gate);
            EditorUtility.SetDirty(nextGate);
            PrefabUtility.RecordPrefabInstancePropertyModifications(nextGate);
        }

        public void ConnectGates()
        {
            GameObject[] targetObjects = Selection.gameObjects;

            if (targetObjects.Length > 2)
            {
                Debug.LogWarning("There were more than 2 objects selected.");
                return;
            }
            if (targetObjects.Length < 2)
            {
                Debug.LogWarning("There needs to be 2 objects selected.");
            }

            Gate lastGate = targetObjects[0].GetComponent<Gate>();
            Gate currentGate = targetObjects[1].GetComponent<Gate>();

            if (currentGate != null && currentGate != null)
            {
                lastGate.nextGates.Add(currentGate);
                EditorUtility.SetDirty(lastGate);
                PrefabUtility.RecordPrefabInstancePropertyModifications(targetObjects[0]);
            } else
            {
                Debug.LogError("One of the object's selected wasn't a gate!");
            }

        }
    }
}
