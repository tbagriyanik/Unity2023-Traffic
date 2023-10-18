using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraSwitch : MonoBehaviour
{
    public GameObject[] kameralar;
    int kameralarIndex = 0;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            kameralarIndex++;
            if (kameralarIndex == kameralar.Length) kameralarIndex = 0;

            for (int i = 0; i < kameralar.Length; i++)
            {
                kameralar[i].SetActive(kameralarIndex == i);
            }
        }
    }
}
