using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// UI component that scoots accross the screen to broadcast a player has finished the race.
/// </summary>
[RequireComponent(typeof(RectTransform))]
public class FinishScroller : MonoBehaviour
{
    [SerializeField] private float ScrollSpeed;
    private RectTransform rectTransform;
    private Rect rect;


    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        rect = rectTransform.rect;
    }

    void Update()
    {
        if (rectTransform.transform.position.x < -rect.width) { Destroy(gameObject); }
        rectTransform.Translate(Vector3.left * Time.deltaTime * ScrollSpeed * Screen.width/rect.width);
    }
}
