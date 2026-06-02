using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestTrigger2 : MonoBehaviour
{
    private bool tetiklendiMi = false;
    public GameObject Canvs;
    public GameObject finaltext;
    public GameObject panel;


    private void OnTriggerEnter(Collider other)
    {
        // Sadece ve sadece Player tag'ine sahip bir nesne girdiðinde çalýþýr
        if (other.CompareTag("Player") && !tetiklendiMi)
        {
            tetiklendiMi = true; // Ýlk saniyede kodu kilitler (çift tetiklenmeyi önler)
            Debug.Log("QuestTrigger: BAÞARILI! Oyuncu alana girdi.");

            Canvs.SetActive(true);
            finaltext.SetActive(true);
            panel.SetActive(false);


            // 2. Alt Yazýyý Tetikle
            PlayerController oyuncu = other.GetComponent<PlayerController>();
            if (oyuncu == null)
            {
                oyuncu = other.GetComponentInParent<PlayerController>();
            }

            if (oyuncu != null)
            {
               
                
            }

            // 3. Objeyi Yok Et
            //Destroy(gameObject);
        }
    }
}