using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 720f;
    public float gravity = 20f;
    [SerializeField] TextMeshProUGUI altyazi;
    [SerializeField] GameObject panel;

    [Header("Etkileþim Ayarlarý")]
    public float interactDistance = 3f;
    public float displayDuration = 2.5f;
    public float fadeInDuration = 1f;
    public float fadeOutDuration = 1f;

    private CharacterController characterController;
    private Animator animator;
    private Transform cameraTransform;
    private Coroutine altyaziCoroutine;

    private Vector3 moveDirection;
    private Vector3 velocity;

    private bool isInteracting = false;
    private bool havefirework = false;
    private bool questfirework = false;
    private bool havebook = false;
    public TMPro.TMP_Text gorevYazisi1Text;
    public TMPro.TMP_Text gorevYazisi2Text;
    public TMPro.TMP_Text gorevYazisi3Text;
    public TMPro.TMP_Text gorevYazisi4Text;
    public GameObject isin1;
    public GameObject isin2;

    // ================================================================
    // EKLEME: Bavulun durumunu akýlda tutacak olan deðiþken
    // ================================================================
    private bool isChestOpen = false;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        cameraTransform = Camera.main.transform;
        
       
    }

    void Update()
    {
        if (isInteracting) return;

        HandleMovement();
        ApplyGravity();
        HandleInteraction();
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        moveDirection = (forward * vertical + right * horizontal).normalized;

        if (moveDirection.magnitude >= 0.1f)
        {
            animator.Play("Run");

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
        }
        else
        {
            animator.Play("Idle");
        }
    }

    void ApplyGravity()
    {
        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y -= gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }
    
    public void subtitle(string msg, float customDuration = -1f)
    {
        
        if (altyaziCoroutine != null)
        {
            StopCoroutine(altyaziCoroutine);
        }

        altyazi.text = msg;
        Color textDynamicColor = altyazi.color;
        textDynamicColor.a = 0f;
        altyazi.color = textDynamicColor;

        float finalDuration = (customDuration > 0f) ? customDuration : displayDuration;

        
        altyaziCoroutine = StartCoroutine(SubtitleRoutine(finalDuration));
    }

    
    private System.Collections.IEnumerator SubtitleRoutine(float activeDisplayDuration)
    {
        Color startColor = altyazi.color;
        float counter = 0f;

      
        while (counter < fadeInDuration)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, counter / fadeInDuration);
            altyazi.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }

        altyazi.color = new Color(startColor.r, startColor.g, startColor.b, 1f);

        
        yield return new WaitForSeconds(activeDisplayDuration);

        
        counter = 0f;
        while (counter < fadeOutDuration)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, counter / fadeOutDuration);
            altyazi.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }

        altyazi.color = new Color(startColor.r, startColor.g, startColor.b, 0f);
        altyazi.text = "";

        altyaziCoroutine = null; 
    }
    private System.Collections.IEnumerator KehanetMesajlariniOynat()
    {
        // 1. MESAJI BAÞLAT
        if (havebook == false)
        {
            subtitle("Bu iþaretler bir yerden tandýk geliyo dur bi dakika!", 5f);

        
            yield return null;

           
            while (altyaziCoroutine != null)
            {
                yield return null;
            }

           
            yield return new WaitForSeconds(1f);

       
            subtitle("Uçakta yanýmda oturan sarý bavullu adamýn okuduðu ansiklopedide de bu iþaretlerden vardý ", 5f);

      
            QuestManager questManager = FindAnyObjectByType<QuestManager>();
            if (questManager != null)
            {
                questManager.ShowQuest(3);
                Debug.Log("Görev 3 baþarýyla açýldý!");
            }
        }
        else
        {
            subtitle("Bu ada 33 yýlda bir 3 günlüðüne yeryüzüne çýkar bu Tanrýlardan bir armaðan.", 5f);
            gorevYazisi3Text.text ="<s>3-)Kehaneti Öðren</s>";

        }
    }

    void HandleInteraction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;

            Vector3 rayOrigin = transform.position + Vector3.up * 1.6f;
            Vector3 rayDirection = cameraTransform.forward;

            float laserThickness = 0.4f;

            if (Physics.SphereCast(rayOrigin, laserThickness, rayDirection, out hit, interactDistance))
            {

                if (hit.collider.CompareTag("Bavul"))
                {
                    // Karakteri pürüzsüzce bavula döndür
                    Vector3 targetPos = hit.collider.transform.position;
                    targetPos.y = transform.position.y;
                    transform.LookAt(targetPos);

                    if (!isChestOpen)
                    {
                        Transform bavulKapak = hit.collider.transform;
                        BoxCollider bavulCollider = hit.collider.GetComponent<BoxCollider>();

                        

                        bavulKapak.DORotate(new Vector3(120, 0, 0), 3f);

                        isChestOpen = true;
                        Debug.Log("Deðiþken hemen true yapýldý. 2. basýþa hazýr!");
                    }
                    else
                    {
                        

                        Transform esyaChild = null;
                        foreach (Transform child in hit.collider.transform)
                        {
                            if (child.CompareTag("Esya") || child.CompareTag("fisek") || child.CompareTag("ansiklepodi"))
                            {
                                esyaChild = child;
                                break;
                            }
                        }

                        if (esyaChild != null)
                        {
                            Interactable interactable = esyaChild.GetComponent<Interactable>();
                            StartCoroutine(InteractRoutineWithChild(esyaChild.gameObject, interactable));
                            isChestOpen = false;
                            if (esyaChild.tag == "fisek")
                            {
                                subtitle("Bir iþaret fiþeði buldun bu kaçmana yardýmcý olabilir");
                                havefirework = true;
                                gorevYazisi2Text.text = "<s>2-)Ýþaret Fiþeðini Bul</s>";
                                isin1.SetActive(true);
                            }
                            else if (esyaChild.tag == "ansiklepodi")
                            {
                                subtitle("Bir iþaret ansiklepodi buldun bu biþileri çevirmende yardýmcý olabilir");
                                havebook = true;
                                gorevYazisi4Text.text = "<s>4-)Ansiklepodiyi Bul</s>";
                            }
                            else if (esyaChild.tag == "esya")
                            {
                                subtitle("Biraz su ve atýþtýrmalýk buldun karnýn atrtýk tok");
                                havebook = true;
                            }

                        }
                        else
                        {
                            subtitle("Bi insan bavula hiçmi birþey koymaz.");

                        }
                    }
                }
                // ================================================================
                // YERDEKÝ DÜZ EÞYALAR ÝÇÝN (Orijinal Sistem)
                // ================================================================
                else if (hit.collider.CompareTag("Esya"))
                {
                    Interactable interactable = hit.collider.GetComponent<Interactable>();

                    if (interactable != null)
                    {
                        Vector3 targetPos = hit.collider.transform.position;
                        targetPos.y = transform.position.y;
                        transform.LookAt(targetPos);

                        StartCoroutine(InteractRoutine(interactable));
                    }
                }
                // ================================================================
                // YENÝ EKLEME: KEHANET TAÞI ETKÝLEÞÝMÝ
                // ================================================================
                else if (hit.collider.CompareTag("Kehanet"))
                {
                    // Karakteri pürüzsüzce taþa doðru döndür
                    Vector3 targetPos = hit.collider.transform.position;
                    targetPos.y = transform.position.y;
                    transform.LookAt(targetPos);

                    // Buraya týrnak içine yazýlmasýný istediðin gizemli kehanet mesajýný yazabilirsin
                    // Arkasýndaki 6f deðeri yazýnýn ekranda kaç saniye asýlý kalacaðýný belirler
                    StartCoroutine(KehanetMesajlariniOynat());

                    Debug.Log("Kehanet taþýyla etkileþime girildi!");
                }
            }
        }
    }

    // Orijinal Coroutine yapýn (Yerdeki tekil eþyalar için çalýþýr)
    System.Collections.IEnumerator InteractRoutine(Interactable targetItem)
    {
        isInteracting = true;
        animator.Play("Interact");

        yield return new WaitForSeconds(1.0f);

        if (targetItem != null)
        {
            targetItem.Interact();
            Debug.Log("Eþya baþarýyla toplandý ve yok edildi.");
        }

        yield return new WaitForSeconds(1.0f);

        isInteracting = false;
    }

    // EKLEME COROUTINE: Bavulun içindeki child (çocuk) olan küpü silen özel rutin
    System.Collections.IEnumerator InteractRoutineWithChild(GameObject childObject, Interactable targetItem)
    {
        isInteracting = true;
        animator.Play("Interact"); // Karakter toplama animasyonuna girer

        yield return new WaitForSeconds(1.0f); // Eðilme payý

        if (targetItem != null)
        {
            targetItem.Interact(); // Varsa kendi üzerindeki fonksiyonu tetikler
        }

        if (childObject != null)
        {
            Destroy(childObject); // Küpü sahneden tamamen siler
            Debug.Log("Bavulun içindeki eþya silindi!");
        }

        yield return new WaitForSeconds(1.0f); // Doðrulma payý
        isInteracting = false;
    }
}