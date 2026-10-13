using UnityEngine;

public class ImpactEffect : MonoBehaviour
{
    [Header("Effect Settings")]
    public float lifetime = 2f;

    void Start()
    {
        // Tự động destroy sau một thời gian
        Destroy(gameObject, lifetime);
    }
}
