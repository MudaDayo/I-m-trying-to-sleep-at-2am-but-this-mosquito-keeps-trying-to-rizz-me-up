using UnityEngine;
using TMPro;
using System.Threading;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;
using Unity.VisualScripting;
using System.Timers;

public class ClickCounterScript : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI TextBeaterCounter;

    [Header("Counter & Timing")]
    [SerializeField] private int BeatingCounter = 0;
    [SerializeField] private float CooldownTime = 0.3f;
    [SerializeField] private float ResetDelay = 1;
    [SerializeField] private float UndoDelay = 0.5f;

    [Header("Hands")]
    [SerializeField] private GameObject HandLeft01;
    [SerializeField] private GameObject HandLeft02;
    [SerializeField] private GameObject HandRight01;
    [SerializeField] private GameObject HandRight02;
    [SerializeField] private float HandDistance = 1;

    [Header("GameJuice")]
    [SerializeField] public GameJuiceEffectScript HandJuice01_1;
    [SerializeField] public GameJuiceEffectScript HandJuice01_2;
    [SerializeField] public GameJuiceEffectScript HandJuice02_1;
    [SerializeField] public GameJuiceEffectScript HandJuice02_2;
    [SerializeField] public GameJuiceEffectScript MosquitoJuice;
    [SerializeField] public GameJuiceEffectScript CounterJuice;
    [SerializeField] public GameJuiceEffectScript CounterNumberJuice;

    [Header("Animation")]
    [SerializeField] public Animator MosquitoGetHitAnim;
    [SerializeField] public GameObject Mosquito;
    [SerializeField] private float AnimationDuration = 1f;

    private float animTimer = 0f;
    private bool isAnimActive = false;

    private float cooldownTimer = 0;
    private bool onCooldown = false;
    private float inactivityTimer = 0f;
    private string lastButton = "";

    void Update()
    {
        inactivityTimer += Time.deltaTime;

        if (inactivityTimer >= ResetDelay)
        {
            lastButton = "";
        }

        if (onCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                onCooldown = false;
                cooldownTimer = 0f;
                lastButton = "";
                Debug.Log("Cooldown expired");
            }
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            HandleClick("Left");
            HandJuice01_2.TriggerJuice();
            MosquitoJuice.TriggerJuice();
            CounterJuice.TriggerJuice();
            CounterNumberJuice.TriggerJuice();

            //TriggerGetHit();
        }
        if (Input.GetMouseButtonDown(1))
        {
            HandleClick("Right");
            HandJuice02_2.TriggerJuice();
            MosquitoJuice.TriggerJuice();
            CounterJuice.TriggerJuice();
            CounterNumberJuice.TriggerJuice();
            
            //TriggerGetHit();
            
        }

        if (isAnimActive)
        {
            animTimer -= Time.deltaTime;
            if (animTimer <= 0f)
            {
                MosquitoGetHitAnim.SetBool("GetHit", false);
                isAnimActive = false;
            }
        }


        UpdateFollowObjects();
        UpdateUI();
    }


    void HandleClick(string button)
    {
        if (button == "Left")
            StartCoroutine(FlashObject(HandLeft01, HandLeft02));
        else if (button == "Right")
            StartCoroutine(FlashObject(HandRight01, HandRight02));

        if (!IsMouseOverMosquito())
            return;

        //Juice
        HandJuice01_1.TriggerJuice();
        HandJuice02_1.TriggerJuice();

        inactivityTimer = 0f;

        if (onCooldown)
        {
            if (button != lastButton)
            {
                onCooldown = false;
                cooldownTimer = 0f;
                BeatingCounter++;
                lastButton = button;
                Debug.Log("Recovered from cooldown early");
            }
            else
            {
                Debug.Log("Still on cooldown");
            }

            return;
        }

        if (button == lastButton && lastButton != "")
        {
            onCooldown = true;
            cooldownTimer = CooldownTime;
            Debug.Log("Cooldown triggered for " + CooldownTime + "s!");
        }

        if (!onCooldown)
            BeatingCounter++;

        lastButton = button;
    }

    void UpdateFollowObjects()
    {
        if (HandLeft02 != null && HandLeft02.activeSelf)
            HandLeft02.transform.position = GetMouseWorldPosition();

        if (HandRight02 != null && HandRight02.activeSelf)
            HandRight02.transform.position = GetMouseWorldPosition();
    }
    Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = HandDistance;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }

    bool IsMouseOverMosquito()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Mosquito"))
                return true;
        }
        return false;
    }

    void UpdateUI()
    {
        TextBeaterCounter.text = "Beatings: " + BeatingCounter.ToString();
    }

    public void TriggerGetHitRight()
    {
        animTimer = AnimationDuration;
        
        //Mathf.Abs(Mosquito.transform.localScale);

        if (!isAnimActive)
        {
            isAnimActive = true;
            MosquitoGetHitAnim.SetBool("GetHit", true);
        }

    }

    public void TriggerGetHitLeft()
    {
        animTimer = AnimationDuration;
        Mosquito.transform.localScale = new Vector3(-1 * Mosquito.transform.localScale.x, Mosquito.transform.localScale.y, Mosquito.transform.localScale.z);
        //Mathf.Abs()

        if (!isAnimActive)
        {
            isAnimActive = true;
            MosquitoGetHitAnim.SetBool("GetHit", true);
        }
    }

    private System.Collections.IEnumerator FlashObject(GameObject toHide, GameObject toShow)
    {
        SpriteRenderer hideRenderer = toHide != null ? toHide.GetComponent<SpriteRenderer>() : null;
        if (hideRenderer != null) hideRenderer.enabled = false;

        SpriteRenderer showRenderer = toShow != null ? toShow.GetComponent<SpriteRenderer>() : null;
        if (showRenderer != null) showRenderer.enabled = true;

        float timer = 0f;
        while (timer < UndoDelay)
        {
            timer += Time.deltaTime;
            if (showRenderer != null)
                toShow.transform.position = GetMouseWorldPosition();
            yield return null;
        }

        if (hideRenderer != null) hideRenderer.enabled = true;
        if (showRenderer != null) showRenderer.enabled = false;
    }
    
}
