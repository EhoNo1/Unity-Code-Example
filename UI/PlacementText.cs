using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Riders.Player;
using Riders.Managers;

namespace Riders.UI
{
    public class PlacementText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private PlayerController controller;
        void FixedUpdate()
        {
            text.text = (RaceController.GetPlacement(controller) + 1) + "";
        }
    }
}