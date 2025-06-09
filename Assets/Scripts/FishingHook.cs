using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingHook : MonoBehaviour
{
    public Fish fish;
    
    private void OnMouseDown() {
        Debug.Log("Hook clicked");
        FishingManager.instance.OnFishHooked(this);
        Destroy(gameObject);
    }
}
