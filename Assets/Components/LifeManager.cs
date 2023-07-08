using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LifeManager : MonoBehaviour
{
    [Min(0)]
    public int life = 1;

    public TextMeshProUGUI lifeText;

    public static LifeManager Instance { get; private set; }

    public void Awake()
    {
        if ((Instance ??= this) != this)
            Destroy(gameObject);
    }

    public void Start()
    {
        life = GetComponent<LevelHolder>().level.life;
    }
    
    void Update()
    {
        lifeText.text = $"Life: {life}";
    }

    public void TakeLife(int count)
    {
        life -= count;
    }
}
