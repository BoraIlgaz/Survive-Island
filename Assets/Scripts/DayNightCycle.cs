using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Header("Zaman Ayarlarý")]
    [Tooltip("Bir tam günün gerçek hayatta kaç saniye süreceðini belirler.")]
    public float fullDayInSeconds = 120f;

    [Range(0, 1)]
    public float currentTimeOfDay = 0.25f; // 0.25 sabah, 0.50 öðle, 0.75 akþam üstü, 0.0 veya 1.0 gece yarýsý

    [Header("Iþýk Yoðunluðu Ayarlarý")]
    public float maxSunIntensity = 1.2f;
    public float minSunIntensity = 0f;

    private Light sunLight;

    void Start()
    {
        // Bu script'in atandýðý nesnedeki Iþýk (Light) bileþenini alýyoruz
        sunLight = GetComponent<Light>();
    }

    void Update()
    {
        UpdateResultingTime();
        RotateSun();
        UpdateLightIntensity();
    }

    // Zamanýn sürekli akmasýný saðlayan fonksiyon
    void UpdateResultingTime()
    {
        currentTimeOfDay += Time.deltaTime / fullDayInSeconds;

        // Zaman 1'e ulaþtýðýnda (gün bittiðinde) tekrar 0'a sýfýrla
        if (currentTimeOfDay >= 1f)
        {
            currentTimeOfDay = 0f;
        }
    }

    // Güneþ'i zamanýn deðerine göre döndüren fonksiyon
    void RotateSun()
    {
        // Zaman deðerini (0-1 arasýný) 360 derecelik açýya çeviriyoruz
        // -90 yapma sebebimiz sabah saatlerinde güneþin tam ufuktan doðmasýný saðlamak
        float sunRotationX = (currentTimeOfDay * 360f) - 90f;

        // Güneþ'i X ekseninde döndür (Doðu-Batý yönü gibi)
        transform.localRotation = Quaternion.Euler(sunRotationX, 170f, 0f);
    }

    // Gece olduðunda ýþýðý tamamen kapatan, gündüz açan fonksiyon
    void UpdateLightIntensity()
    {
        // Güneþ ufkun altýndaysa (Gece ise)
        if (currentTimeOfDay <= 0.23f || currentTimeOfDay >= 0.75f)
        {
            sunLight.intensity = minSunIntensity; // Iþýðý kapat
        }
        // Güneþ ufkun üstündeyse (Gündüz ise)
        else
        {
            // Güneþ tam tepedeyken (0.50) en parlak, doðarken ve batarken daha loþ olmasý için matematiksel geçiþ
            float intensityMultiplier = 1f;

            if (currentTimeOfDay <= 0.50f)
            {
                intensityMultiplier = Mathf.InverseLerp(0.23f, 0.50f, currentTimeOfDay);
            }
            else
            {
                intensityMultiplier = Mathf.InverseLerp(0.75f, 0.50f, currentTimeOfDay);
            }

            sunLight.intensity = Mathf.Lerp(minSunIntensity, maxSunIntensity, intensityMultiplier);
        }
    }
}