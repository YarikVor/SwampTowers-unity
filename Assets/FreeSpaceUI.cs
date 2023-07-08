using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeSpaceUI : MonoBehaviour
{
    public GameObject[] HiddenElements;

    public void OnClick()
    {
        foreach(var elem in HiddenElements)
        {
            elem.SetActive(false);
        }
    }
}
