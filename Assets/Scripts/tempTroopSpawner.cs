using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class tempTroopSpawner : MonoBehaviour, IPointerDownHandler{

    public TroopMovement troopMovement;
    public Image spawnButton;
    

    public virtual void OnPointerDown(PointerEventData ped)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(spawnButton.rectTransform, ped.position, ped.pressEventCamera, out pos))
        {
            troopMovement.spawnATroop();
        }
    }


}
