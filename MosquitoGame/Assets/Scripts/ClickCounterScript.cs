using UnityEngine;
using TMPro;
using System.Threading;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;
using Unity.VisualScripting;

public class ClickCounterScript : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI TextBeaterCounter;
    [SerializeField]
    private int BeatingCounter = 0;
    [SerializeField]
    private float CooldownTime = 0.3f;
    [SerializeField]
    private float ResetDelay = 1;

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
            HandleClick("Left");
        if (Input.GetMouseButtonDown(1))
            HandleClick("Right");

        UpdateUI();
    }


    void HandleClick(string button)
    {
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

    void UpdateUI()
    {
        TextBeaterCounter.text = "Beatings: " + BeatingCounter.ToString();
    }
}
