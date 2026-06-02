using System.Collections.Generic;
using UnityEngine;
using TMPro; // TextMeshPro için þart

public class QuestManager : MonoBehaviour
{
    [System.Serializable]
    public class Quest
    {
        public string questDescription; // Görev metni
        public TextMeshProUGUI questTextObject; // Editörden baðlayacaðýn kendi TextMeshPro kutusu
        public bool isVisible = false;  // Baþta görünüyor mu?
        public bool isCompleted = false; // Bitti mi?
    }

    [Header("4 Adet Göreviniz")]
    public List<Quest> allQuests = new List<Quest>();

    void Start()
    {
        // Oyun baþladýðýnda ilk 2 görevi görünür yap, diðerlerini gizle
        if (allQuests.Count >= 2)
        {
            allQuests[0].isVisible = true;
            allQuests[1].isVisible = true;
        }

        UpdateQuestUI();
    }

    // Görevlerin ekrandaki açýk/kapalý durumunu günceller
    public void UpdateQuestUI()
    {
        for (int i = 0; i < allQuests.Count; i++)
        {
            if (allQuests[i].questTextObject != null)
            {
                // Görev görünür mü? Görünürse Text kutusunu AKTÝF et, deðilse GÝZLE
                allQuests[i].questTextObject.gameObject.SetActive(allQuests[i].isVisible);

                // Eðer görev görünürse ve tamamlanmýþsa yazýyý yeþil yap ve [BÝTTÝ] ekle
                if (allQuests[i].isVisible)
                {
                    if (allQuests[i].isCompleted)
                    {
                        allQuests[i].questTextObject.text = "<color=green>[BÝTTÝ]</color> " + allQuests[i].questDescription;
                    }
                    else
                    {
                        allQuests[i].questTextObject.text = allQuests[i].questDescription;
                    }
                }
            }
        }
    }

    // Yeni görevi açmak için çaðrýlacak fonksiyon (Örn: ShowQuest(2) -> 3. görevi açar)
    public void ShowQuest(int questIndex)
    {
        if (questIndex >= 0 && questIndex < allQuests.Count)
        {
            allQuests[questIndex].isVisible = true;
            UpdateQuestUI();
        }
    }

    // Görevi tamamlamak için çaðrýlacak fonksiyon
    public void CompleteQuest(int questIndex)
    {
        if (questIndex >= 0 && questIndex < allQuests.Count)
        {
            allQuests[questIndex].isCompleted = true;
            UpdateQuestUI();
        }
    }
}