using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Gun Stats")]
    public string gunName = "Pistol";
    public int damage = 25;
    public float fireRate = 0.2f; // Thời gian giữa mỗi phát bắn
    public int maxAmmo = 30;
    public int currentAmmo;
    public float reloadTime = 1.5f;
    public float range = 100f;
    [SerializeField] bool infiniteAmmo;

    [Header("Effects")]
    public ParticleSystem muzzleFlash;
    public Light muzzleFlashLight;
    public float muzzleFlashLightDuration = 0.04f;
    public GameObject impactEffect;
    public AudioClip shootSound;
    public AudioClip reloadSound;

    [Header("Recoil")]
    public float recoilAmount = 2f;
    [SerializeField] Transform recoilTransform;
    [SerializeField] Vector3 recoilLocalPosition = new Vector3(0f, -0.025f, -0.06f);
    [SerializeField] Vector3 recoilLocalRotation = new Vector3(-3f, 0f, 0f);
    [SerializeField, Min(0.01f)] float recoilRecoverySpeed = 18f;

    float nextFireTime;
    float reloadCompleteTime;
    int pendingReloadAmmo;
    float muzzleFlashLightEndTime;
    float recoilEndTime;
    bool isReloading;
    bool mobileFireHeld;
    Vector3 recoilRestPosition;
    Quaternion recoilRestRotation;
    AudioSource audioSource;
    Camera fpsCam;
    Animator animator;

    void Start()
    {
        if (currentAmmo <= 0)
            currentAmmo = maxAmmo;
        audioSource = GetComponent<AudioSource>();
        fpsCam = Camera.main;
        animator = GetComponent<Animator>();
        if (recoilTransform == null)
            recoilTransform = transform;
        recoilRestPosition = recoilTransform.localPosition;
        recoilRestRotation = recoilTransform.localRotation;

        if (muzzleFlashLight != null)
            muzzleFlashLight.enabled = false;
    }

    void LateUpdate()
    {
        if (recoilTransform != null)
        {
            bool recoilActive = Time.time < recoilEndTime;
            float recoilScale = Mathf.Max(0f, recoilAmount) * 0.25f;
            Vector3 targetPosition = recoilActive
                ? recoilRestPosition + recoilLocalPosition * recoilScale
                : recoilRestPosition;
            Quaternion targetRotation = recoilActive
                ? recoilRestRotation * Quaternion.Euler(recoilLocalRotation * recoilScale)
                : recoilRestRotation;

            float blend = 1f - Mathf.Exp(-recoilRecoverySpeed * Time.deltaTime);
            recoilTransform.localPosition = Vector3.Lerp(recoilTransform.localPosition, targetPosition, blend);
            recoilTransform.localRotation = Quaternion.Slerp(recoilTransform.localRotation, targetRotation, blend);
        }

        if (muzzleFlashLight != null && muzzleFlashLight.enabled && Time.time >= muzzleFlashLightEndTime)
            muzzleFlashLight.enabled = false;
    }

    void Update()
    {
        if (Time.timeScale <= 0f)
            return;

        if (isReloading)
        {
            if (Time.time >= reloadCompleteTime)
                FinishReload();
            return;
        }

        if (currentAmmo <= 0 && (infiniteAmmo || GameManager.GetReserveAmmo() > 0))
        {
            TryReload();
            return;
        }

        bool mouseFireHeld =
            !Application.isMobilePlatform &&
            Input.touchCount == 0 &&
            Input.GetButton("Fire1");
        if ((mobileFireHeld || mouseFireHeld) && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo)
            TryReload();
    }

    void Shoot()
    {
        if (currentAmmo <= 0 || fpsCam == null)
            return;

        if (!infiniteAmmo)
            currentAmmo--;

        if (muzzleFlash != null)
            muzzleFlash.Play();

        if (muzzleFlashLight != null)
        {
            muzzleFlashLight.enabled = true;
            muzzleFlashLightEndTime = Time.time + muzzleFlashLightDuration;
        }

        recoilEndTime = Time.time + 0.05f;

        if (audioSource != null && shootSound != null)
            audioSource.PlayOneShot(shootSound);

        // Raycast để bắn
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log("Hit: " + hit.transform.name);

            // Kiểm tra xem bắn trúng zombie không
            ZombieHealth zombie = hit.transform.GetComponent<ZombieHealth>();
            if (zombie != null)
            {
                zombie.TakeDamage(damage);
            }

            // Tạo impact effect
            if (impactEffect != null)
            {
                GameObject impact = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impact, 2f);
            }
        }

        // Apply recoil
        if (animator != null)
            animator.SetTrigger("Shoot");
    }

    public bool TryReload()
    {
        if (isReloading || currentAmmo >= maxAmmo)
            return false;

        pendingReloadAmmo = infiniteAmmo
            ? maxAmmo - currentAmmo
            : GameManager.TakeReserveAmmo(maxAmmo - currentAmmo);
        if (pendingReloadAmmo <= 0)
            return false;

        isReloading = true;
        reloadCompleteTime = Time.time + reloadTime;
        if (animator != null)
            animator.SetTrigger("Reload");
        if (audioSource && reloadSound)
            audioSource.PlayOneShot(reloadSound);

        return true;
    }

    void FinishReload()
    {
        currentAmmo = Mathf.Min(maxAmmo, currentAmmo + pendingReloadAmmo);
        pendingReloadAmmo = 0;
        isReloading = false;
    }

    void OnDisable()
    {
        if (isReloading)
            FinishReload();
        mobileFireHeld = false;
    }

    public void SetMobileFireHeld(bool isHeld)
    {
        mobileFireHeld = isHeld;
    }

    public bool IsReloading()
    {
        return isReloading;
    }

    public bool HasInfiniteAmmo => infiniteAmmo;
}
