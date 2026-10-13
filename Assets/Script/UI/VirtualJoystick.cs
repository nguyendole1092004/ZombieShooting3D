using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] RectTransform background;
    [SerializeField] RectTransform handle;
    [SerializeField, Range(0f, 1f)] float deadZone = 0.1f;

    public Vector2 Direction { get; private set; }

    void Awake()
    {
        if (background == null)
            background = transform as RectTransform;

        if (background == null || handle == null)
            Debug.LogError("VirtualJoystick requires Background and Handle RectTransforms.", this);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (background == null || handle == null)
            return;

        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                background, eventData.position, eventData.pressEventCamera, out Vector2 localPoint))
            return;

        Vector2 halfSize = background.rect.size * 0.5f;
        if (halfSize.x <= 0f || halfSize.y <= 0f)
            return;

        Vector2 radius = Vector2.Max(halfSize - handle.rect.size * 0.5f, Vector2.one);
        Vector2 normalized = new Vector2(localPoint.x / halfSize.x, localPoint.y / halfSize.y);
        Direction = Vector2.ClampMagnitude(normalized, 1f);
        if (Direction.magnitude < deadZone)
            Direction = Vector2.zero;

        handle.anchoredPosition = Vector2.Scale(Direction, radius);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Direction = Vector2.zero;
        if (handle != null)
            handle.anchoredPosition = Vector2.zero;
    }
}
