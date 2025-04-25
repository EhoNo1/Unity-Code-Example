using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace Riders.Objects
{
    /// <summary>
    /// Invisible box that marks the path of the racetrack.
    /// </summary>
    [RequireComponent(typeof(BoxCollider))]
    public class Gate : InteractibleObject
    {
        public List<Gate> nextGates = new List<Gate>();

        //Helps indentify how far down the course a gate is, propogates from the startline
        [SerializeField] private int index = -1;

        void Start()
        {
            while (nextGates.Contains(null)) nextGates.Remove(null);
            if (nextGates.Count == 0) Debug.LogError("Gate " + gameObject.name + " does not point to anything else, this should not be!");
            while (nextGates.Contains(this))
            {
                Debug.LogError("Gate " + gameObject.name + " points to itself!");
                nextGates.Remove(this);
            }
        }

        public void SetIndex(int newIndex)
        {
            if (index == -1 || newIndex < index)
            {
                index = newIndex;
                for (int i = 0; i < nextGates.Count; i++)
                {
                    if (nextGates[i] != null)
                    {
                        nextGates[i].SetIndex(newIndex + 1);
                    }
                }
            }
        }
        public int GetIndex() { return index; }


#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            if (nextGates.Count == 0) { return; }
            for (int i = 0; i < nextGates.Count; i++)
            {
                if (nextGates.ElementAt(i) == null || nextGates.ElementAt(i) == this)
                {
                    if (i < nextGates.Count) continue;
                    return;
                }
                Gate nextGate = nextGates.ElementAt(i);

                Debug.DrawLine(transform.position, nextGate.transform.position, Color.white);
                Vector3 direction = (nextGate.transform.position - transform.position).normalized;
                Vector3 leftWing = Quaternion.AngleAxis(135f, Vector3.up) * direction * 3f;
                leftWing.y = 0f;
                Debug.DrawLine(nextGate.transform.position, nextGate.transform.position + leftWing * 5f, Color.white);
                Vector3 rightWing = Quaternion.AngleAxis(-135f, Vector3.up) * direction * 3f;
                rightWing.y = 0f;
                Debug.DrawLine(nextGate.transform.position, nextGate.transform.position + rightWing * 5f, Color.white);
            }
        }

        public Color GetAdvantageColor()
        {
            return Color.white;
        }
#endif
    }
}
