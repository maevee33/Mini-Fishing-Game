using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class FishingManager : MonoBehaviour
{
    // Singleton 
    public static FishingManager instance;

    // UI references
    public GameObject fishingUI;
    public Image indicator;             //This will change color depending on the distance between the two circles
    public Transform pulseCircle;       //This will be the moving circle
    public Transform targetCircle;      //This will be the target circle and will remain static
    public Image fishPreviewImg;
    public GameObject lineHpBarPrefab;
    public Transform lineHpBarRoot;
    public GameObject fishHpBarPrefab;
    public Transform fishHpBarRoot;

    public LevelManager levelManager;

    // Game Config
    public float pulseSpeed = 1f;
    public float maxPulseScale = 2f;
    public float minPulseScale = 1f;

    // events
    public UnityEvent OnGameStart;
    public UnityEvent OnGameEnd;

    // Attributes
    public int fishHealth;
    public int lineHealth;
    public int reelAtk;
    public float hitRadius = .3f;
    public bool isFishing = false;
    public Fish hookedFish;

    float randomOffset;             //Used as random offset for the pulse circle


    void Awake()
    {
        instance = this;
    }

    void Start()
    {
         
        TimeManager.instance.OnTimeEnd.AddListener(GameOver);
    }

    void OnDestroy()
    {
        TimeManager.instance.OnTimeEnd.RemoveListener(GameOver);
    }

    public void ResetData() {
        // Clear player prefs
        PlayerPrefs.DeleteAll();
    }

    public void ExitGame() {
        Application.Quit();
    }

    public void LoadLineHealthBars()
    {
        // delete all existing hp bars
        foreach (Transform child in lineHpBarRoot)
        {
            Destroy(child.gameObject);
        }

        // create new hp bars
        for (int i = 0; i < lineHealth; i++)
        {
            Instantiate(lineHpBarPrefab, lineHpBarRoot);
        }
    }

    public void LoadFishHealthBars()
    {
        // delete all existing hp bars
        foreach (Transform child in fishHpBarRoot)
        {
            Destroy(child.gameObject);
        }

        // create new hp bars
        for (int i = 0; i < fishHealth; i++)
        {
            Instantiate(fishHpBarPrefab, fishHpBarRoot);
        }
    }

    public void LoadItemConfigs() {
        reelAtk = InventoryManager.instance.GetLevel("REEL");
        lineHealth = InventoryManager.instance.GetLevel("LINE");
        hitRadius = .3f + InventoryManager.instance.GetLevel("ROD") * .15f;

    }

    public void InitializeGame()
    {
        levelManager.level = 0;

        LoadItemConfigs();

        TimeManager.instance.ResetTimer();
        TimeManager.instance.StartTimer();
        levelManager.ResetGame();

    
 

        // Initialize healthbars for line
        LoadLineHealthBars();

        OnGameStart?.Invoke();
    }

    // Function to load hook data
    public void OnFishHooked(FishingHook fishingHook)
    {
        hookedFish = fishingHook.fish;
        fishPreviewImg.sprite = hookedFish.icon;
        fishHealth = hookedFish.health;
        pulseSpeed = hookedFish.speed;
        LoadFishHealthBars();

        float randomScale = Random.Range(minPulseScale, maxPulseScale);
        targetCircle.localScale = Vector3.one * randomScale;

        levelManager.RemoveHook(fishingHook);

        StartCoroutine(StartFishingNextFrame());
    }

    IEnumerator StartFishingNextFrame()
    {
        yield return new WaitForEndOfFrame();
        StartFishing();
    }

    void DamageFish(int damage)
    {
        Debug.Log("Damage Dealt");

        damage = Mathf.Min(damage, fishHealth);

        fishHealth -= damage;

        // delete hp bars
        for (int i = 0; i < damage; i++)
        {
            Destroy(fishHpBarRoot.GetChild(i).gameObject);
        }
    }

    void StrikeHit()
    {
        DamageFish(reelAtk);

        if (fishHealth <= 0)
        {            
            ScoreManager.instance.score += hookedFish.score;
            StopFishing();

            hookedFish = null;

            if (levelManager.hooks.Count == 0)
            {
                levelManager.LevelPassed();
                TimeManager.instance.LevelBonus();
            }
        }
    }

    void DamageLine(int damage)
    {
        Debug.Log("Damage Taken");

        damage = Mathf.Min(damage, lineHealth);

        lineHealth -= damage;

        // delete hp bars
        for (int i = 0; i < damage; i++)
        {
            Destroy(lineHpBarRoot.GetChild(i).gameObject);
        }
    }

    public void StartFishing()
    {
        isFishing = true;
        fishingUI.SetActive(true);
        randomOffset = Random.Range(0, 2 * Mathf.PI);
        levelManager.HideHooks();
    }

    public void StopFishing()
    {
        isFishing = false;
        fishingUI.SetActive(false);
        levelManager.ShowHooks();
    }

    void GameOver()
    {
        StopFishing();

        Debug.Log("Game Over");

        // We use ?. to only call the function if it is not null
        OnGameEnd?.Invoke();
    }

    void MissedStrike()
    {
        DamageLine(1);

        if (lineHealth <= 0)
        {
            GameOver();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // only do updates if we are fishing
        if (!isFishing) return;

        // Use a sinewave along with the randomOffset to move the pulse circle


        float scale = Mathf.Lerp(minPulseScale, maxPulseScale, (Mathf.Sin((Time.time + randomOffset) * pulseSpeed) + 1) / 2);
        pulseCircle.localScale = new Vector3(scale, scale, scale);


        // Check if the pulse circle is close enough to the target circle
        float distance = Vector3.Distance(pulseCircle.localScale, targetCircle.localScale);
        bool isHit = distance
            < hitRadius;

        if (isHit)
        {
            indicator.color = Color.green;
        }
        else if (distance < hitRadius * 2)
        {
            indicator.color = Color.yellow;
        }
        else
        {
            indicator.color = Color.red;
        }

        // detect click
        if (Input.GetMouseButtonDown(0))
        {
            if (isHit)
            {
                // Hooked the fish
                StrikeHit();
            }
            else
            {
                // Missed the fish
                MissedStrike();
            }
        }
    }
}