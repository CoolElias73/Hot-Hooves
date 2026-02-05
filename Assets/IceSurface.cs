using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class IceSurface : MonoBehaviour
{
    [Range(0f, 1f)] public float friction = 0f;
    [Range(0f, 1f)] public float bounciness = 0f;

    [SerializeField, HideInInspector] private PhysicsMaterial2D runtimeMaterial;

    private void Reset()
    {
        Apply();
    }

    private void Awake()
    {
        Apply();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        Apply();
    }
#endif

    private void Apply()
    {
        var collider = GetComponent<Collider2D>();
        if (collider == null)
            return;

        if (runtimeMaterial == null)
        {
            runtimeMaterial = new PhysicsMaterial2D("IceSurface (Runtime)");
            runtimeMaterial.hideFlags = HideFlags.HideAndDontSave;
        }

        runtimeMaterial.friction = friction;
        runtimeMaterial.bounciness = bounciness;

        collider.sharedMaterial = runtimeMaterial;
    }
}
