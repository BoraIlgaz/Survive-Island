using UnityEngine;

public class Interactable : MonoBehaviour
{
    public string itemName = "Eþya"; // Editörden taþ, odun vb. yazabilirsin

    // Karakter bu nesneyle etkileþime girdiðinde ne olacaðýný bu fonksiyon belirler
    public void Interact()
    {
        Debug.Log(itemName + " toplandý!");

        // Þimdilik hayatta kalma mekaniði olarak yerden yok edelim.
        // Ýleride buraya "Envantere Ekle" kodu yazacaksýn.
        Destroy(gameObject);
    }
}