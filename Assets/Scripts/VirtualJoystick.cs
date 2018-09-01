using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler {

    private Image bgImg;
    private Image joystickImg;
    private Vector2 inputVector;

    private void Start() {
        bgImg = GetComponent<Image>();
        joystickImg = transform.GetChild(0).GetComponent<Image>();
    }
    public virtual void OnDrag(PointerEventData ped)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(bgImg.rectTransform, ped.position, ped.pressEventCamera, out pos)) {
            pos.x = (pos.x / bgImg.rectTransform.sizeDelta.x);
            pos.y = (pos.y / bgImg.rectTransform.sizeDelta.y);
            

            //inputVector = new Vector2(pos.x * 2 + 1, pos.y * 2 - 1);

            inputVector = new Vector2((pos.x - 0.5f) * 2, (pos.y - 0.5f) * 2);

            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

            joystickImg.rectTransform.anchoredPosition = new Vector2(inputVector.x * (bgImg.rectTransform.sizeDelta.x/2), inputVector.y * (bgImg.rectTransform.sizeDelta.y/2));
        }
    }
    public virtual void OnPointerDown(PointerEventData ped) {
        OnDrag(ped);
    }
    public virtual void OnPointerUp(PointerEventData ped)
    {
        inputVector = Vector2.zero;
        joystickImg.rectTransform.anchoredPosition = Vector2.zero;
    }
    public float Horizontal() {
        if (inputVector.x != 0)
        {
            return inputVector.x;
        }
        else {
            return Input.GetAxis("Horizontal");
        }
    }
    public float Vertical() {
        if (inputVector.y != 0)
        {
            return inputVector.y;
        }
        else
        {
            return Input.GetAxis("Vertical");
        }
    }
}
