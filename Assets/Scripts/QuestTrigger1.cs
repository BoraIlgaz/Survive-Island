using UnityEngine;

public class QuestTrigger1 : MonoBehaviour
{
    private bool tetiklendiMi = false;
    public GameObject isin1copy;
    public GameObject isin2copy;
    public GameObject helicopter;

    private void OnTriggerEnter(Collider other)
    {
        // Sadece ve sadece Player tag'ine sahip bir nesne girdiðinde çalýþýr
        if (other.CompareTag("Player") && !tetiklendiMi)
        {
            tetiklendiMi = true; // Ýlk saniyede kodu kilitler (çift tetiklenmeyi önler)
            Debug.Log("QuestTrigger: BAÞARILI! Oyuncu alana girdi.");

            isin1copy.SetActive(false);
            isin2copy.SetActive(true);
            helicopter.SetActive(true);


            // 2. Alt Yazýyý Tetikle
            PlayerController oyuncu = other.GetComponent<PlayerController>();
            if (oyuncu == null)
            {
                oyuncu = other.GetComponentInParent<PlayerController>();
            }

            if (oyuncu != null)
            {
                oyuncu.subtitle("Ýþaret fiþeðini attýn bir helikopter yaklaþýyor hemen ineceði noktaya git", 3f);
                
            }

            // 3. Objeyi Yok Et
            //Destroy(gameObject);
        }
    }
}