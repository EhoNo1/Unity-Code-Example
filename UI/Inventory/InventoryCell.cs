using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class InventoryCell : MonoBehaviour
{
    public Vector2Int position;
    public Button button;
    [SerializeField] private CellUsability usability;

    public void SetCellUsability(CellUsability usability)
    {
        Debug.Log(usability);
        this.usability = usability;
        button.interactable = (usability == CellUsability.USABLE);
    }
}
