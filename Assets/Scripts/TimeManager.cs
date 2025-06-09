using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;
    public float remainingTime = 1f;
    public TextMeshProUGUI timeText;

    public bool isTimeRunning = false;
    public UnityEvent OnTimeEnd;
    public float defaultTime = 15f;

    void Awake()
    {
        instance = this;
    }

    public void StartTimer() {
        isTimeRunning = true;
    }

    public void StopTimer() {
        isTimeRunning = false;
    }

    public void ResetTimer() {
        remainingTime = defaultTime;
    }

    public void LevelBonus() {
        remainingTime += defaultTime;
    }

     

    // Update is called once per frame
    void Update()
    {
        if(!isTimeRunning) {
            return;
        }

        remainingTime -= Time.deltaTime;
        timeText.text = remainingTime.ToString("F2");
        if (remainingTime <= 0f) {
            remainingTime = 0f;
            isTimeRunning = false;
            OnTimeEnd?.Invoke();
        }
    }
}