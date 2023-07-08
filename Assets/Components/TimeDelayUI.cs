using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;


public class TimeDelayUI : MonoBehaviour
{
    public Image ImageBar;
    public Image Background;

    public float precentageFill
    {
        get => ImageBar.fillAmount;
        set => ImageBar.fillAmount = value;
    }

    public bool IsActive
    {
        set => ImageBar.enabled = Background.enabled = value;
    }

    public float maxFill = 1;
    public float fill;

    void Start()
    {
        Spawner.Instance.OnStartWave += OnStartWave;
    }

    private void OnStartWave(float seconds)
    {
        maxFill = seconds;
        fill = seconds;
        precentageFill = 1;
    }

    void Update()
    {
        if (fill > 0)
        {
            IsActive = true;
            fill -= Time.deltaTime;
            precentageFill = fill / maxFill;
        }
        else
        {
            IsActive = false;
        }
    }
}