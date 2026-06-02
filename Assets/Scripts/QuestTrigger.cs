using UnityEngine;

public class QuestTrigger : MonoBehaviour
{
    private bool tetiklendiMi = false;

    private void OnTriggerEnter(Collider other)
    {
        // Sadece ve sadece Player tag'ine sahip bir nesne girdiðinde çalýþýr
        if (other.CompareTag("Player") && !tetiklendiMi)
        {
            tetiklendiMi = true; // Ýlk saniyede kodu kilitler (çift tetiklenmeyi önler)
            Debug.Log("QuestTrigger: BAÞARILI! Oyuncu alana girdi.");

            // 1. Görevi Deðiþtir
            QuestManager questManager = FindAnyObjectByType<QuestManager>();
            if (questManager != null)
            {
                questManager.ShowQuest(2);
                Debug.Log("QuestTrigger: Görev 2 baþarýyla açýldý.");
            }

            // 2. Alt Yazýyý Tetikle
            PlayerController oyuncu = other.GetComponent<PlayerController>();
            if (oyuncu == null)
            {
                oyuncu = other.GetComponentInParent<PlayerController>();
            }

            if (oyuncu != null)
            {
                oyuncu.subtitle("Ýlginç yazýlý bir taþ bir yerlerden tanýdýk geliyor incelesem iyi olacak", 6f);
                Debug.Log("QuestTrigger: Alt yazý PlayerController'a gönderildi.");
            }

            // 3. Objeyi Yok Et
            //Destroy(gameObject);
        }
    }
}