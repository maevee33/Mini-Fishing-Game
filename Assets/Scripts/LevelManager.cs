using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public List<FishData> fishData;
    public int level;
    public int minFishCount;
    public int fishRarity;
    public int fishRarityInc;
    public FishingHook hookPrefab;

    public BoxCollider2D fishingArea;
    public float radius = 1f;
    public List<FishingHook> hooks;

    public void ClearHooks()
    {
        foreach (FishingHook hook in hooks)
        {
            Destroy(hook.gameObject);
        }
        hooks.Clear();
    }

    public void LevelPassed()
    {
        level++;

        //Check if we are about to increase rarity
        if (level % fishRarityInc == 0)
        {
            // Make sure we don't go over the fish data count
            if (fishRarity < fishData.Count)
                fishRarity++;
        }

        GenerateLevel();
    }

    public void RemoveHook(FishingHook hook)
    {
        hooks.Remove(hook);
        Destroy(hook.gameObject);
    }

    public void HideHooks() {
        foreach (FishingHook hook in hooks)
        {
            hook.gameObject.SetActive(false);
        }
    }

    public void ShowHooks() {
        foreach (FishingHook hook in hooks)
        {
            hook.gameObject.SetActive(true);
        }
    }


    public void GenerateLevel()
    {
        // 1. Remove all hooks
        ClearHooks();

        // 2. Get fish count 
        int fishCount = level % fishRarityInc == 0 ? minFishCount : minFishCount + (level % fishRarityInc);

        // 3. Generate points and spawn hooks

        for (int i = 0; i < fishCount; i++)
        {


            // 3.1 get random fish index
            int fishIndex = Random.Range(0, fishRarity);
            fishIndex = Mathf.Min(fishData.Count, fishIndex);

            // 3.2 get fish data
            FishData randomFishData = fishData[fishIndex];

            // 3.3 instantiate hook
            FishingHook hook = Instantiate(hookPrefab, transform);

            // 3.4 get random hook position 
            Vector2 randomHookPos = GetRandomPointInFishingArea();

            // 3.5 set hook position
            hook.transform.position = randomHookPos;

            // 3.6 set fish data
            hook.fish = randomFishData.FishDataToFish();

            // 3.7 add hook to hooks list
            hooks.Add(hook);
        }


    }

    public void ResetGame()
    {
        
        ClearHooks();
        level = 0;
        fishRarity = 0;
        GenerateLevel();
    }
 


    Vector2 GetRandomPointInFishingArea()
    {
        Bounds bounds = fishingArea.bounds;

        return new Vector2(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y)
        );
    }



}