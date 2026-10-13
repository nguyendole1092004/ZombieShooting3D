using UnityEngine;
using UnityEngine.EventSystems;

public class MobileFireButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    [SerializeField] MobileCombatControls controls;

    void Awake()
    {
        if (controls == null)
            Debug.LogError("Assign MobileCombatControls to MobileFireButton.", this);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (controls != null)
            controls.SetFireHeld(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StopFiring();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopFiring();
    }

    void OnDisable()
    {
        StopFiring();
    }

    void StopFiring()
    {
        if (controls != null)
            controls.SetFireHeld(false);
    }
}
