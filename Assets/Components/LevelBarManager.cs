using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelBarManager : MonoBehaviour
{
    public Image progressBar;
    public TextMeshProUGUI titleBar;
    private Spawner spawner;
    
    void Start()
    {
        spawner = GetComponent<Spawner>();
    }

    void Update()
    {
        progressBar.fillAmount = spawner.MonsterFillPercentage;
        titleBar.text = $"{spawner.CurrentPositionWave} / {spawner.CountWaves}";
    }
}
