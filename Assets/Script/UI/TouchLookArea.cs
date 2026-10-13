using UnityEngine;
using UnityEngine.EventSystems;

public class TouchLookArea : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    [SerializeField] MouseLook mouseLook;
    Vector2 previousPosition;

    void Awake()
    {
        if (mouseLook == null)
            Debug.LogError("Assign the camera's MouseLook component to TouchLookArea.", this);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        previousPosition = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (mouseLook == null)
            return;

        Vector2 delta = eventData.position - previousPosition;
        previousPosition = eventData.position;
        mouseLook.ApplyTouchLook(delta);
    }
}
