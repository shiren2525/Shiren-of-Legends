using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeBarController : MonoBehaviour
{
    [SerializeField] Slider Slider = null;
    [SerializeField] Slider Slider1 = null;
    [SerializeField] GameManager GameManager = null;
    readonly int limitTime = 100;

    private IEnumerator TimeLimitCoroutin()
    {
        var count = limitTime;
        Slider.maxValue = limitTime;
        Slider1.maxValue = limitTime;
        while (true)
        {
            if (Slider.minValue < count)
            {
                --count;
                Slider.value = count;
                Slider1.value = count;

                yield return new WaitForSeconds(0.1f);
            }
            else
            {
                GameManager.IsTimeLimit = true;
                yield return false;
            }
        }
    }

    // Stop コルーチンを使うために必要
    public void StartCoroutine()
    {
        StartCoroutine(nameof(TimeLimitCoroutin));

        // TODO: コールバック関数 を使って賢い実装ができないのか
        // var coroutin = TimeLimitCoroutin();
        // StartCoroutine(coroutin);
        // Debug.Log(coroutin.Current);        
    }

    public void StopCoroutine()
    {
        StopCoroutine(nameof(TimeLimitCoroutin));
    }
}