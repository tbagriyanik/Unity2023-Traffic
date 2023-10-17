using NUnit;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class sonrakiHedef : MonoBehaviour
{
    public GameObject[] sonrakiler;
    public Color renk;

    public GameObject[] trafikLambalari;
    public float kirmiziIsikZamani = 10;
    public float yesilIsikZamani = 10;
    public bool tersTaraf = false;
    int aktifAn = 0;

    private void Start()
    {
        if (trafikLambalari.Length > 0)
        {
            InvokeRepeating(nameof(TrafikLambaYak), 0, 1f);
        }
    }

    void TrafikLambaYak()
    {
        aktifAn++;
        if (!tersTaraf)
        {
            trafikLambalari[0].SetActive(aktifAn < kirmiziIsikZamani + 2); //0-11
            trafikLambalari[1].SetActive((aktifAn > kirmiziIsikZamani - 2 && aktifAn < kirmiziIsikZamani + 2) ||
                  (aktifAn >= kirmiziIsikZamani + yesilIsikZamani + 2 && aktifAn <= kirmiziIsikZamani + 2 + yesilIsikZamani + 2)); //9-11 22-23
            trafikLambalari[2].SetActive(aktifAn >= kirmiziIsikZamani + 2 && aktifAn < kirmiziIsikZamani + 2 + yesilIsikZamani); //12-21
        }
        else
        {
            trafikLambalari[0].SetActive(aktifAn >= yesilIsikZamani + 2 && aktifAn < kirmiziIsikZamani + 2 + yesilIsikZamani + 2); //12-23
            trafikLambalari[1].SetActive((aktifAn >= yesilIsikZamani && aktifAn < yesilIsikZamani + 2) ||
                  (aktifAn > kirmiziIsikZamani + yesilIsikZamani + 2 && aktifAn <= kirmiziIsikZamani + 2 + yesilIsikZamani + 2)); //10-11 23-24
            trafikLambalari[2].SetActive(aktifAn < yesilIsikZamani); //0-9
        }

        if (aktifAn >= kirmiziIsikZamani + 2 + yesilIsikZamani + 2)
        {
            aktifAn = 0;
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = renk;
        foreach (GameObject go in sonrakiler)
            if (go != null)
            {
                Vector3 start = transform.position;
                Vector3 end = go.transform.position;
                Vector3 dir = (end - start).normalized;
                float length = 2; //(end - start).magnitude;
                Gizmos.DrawLine(start, end);
                Vector3 left = Quaternion.AngleAxis(45, Vector3.up) * (-dir);
                Gizmos.DrawLine(end, end + 0.5f * length * left);
                Vector3 right = Quaternion.AngleAxis(-45, Vector3.up) * (-dir);
                Gizmos.DrawLine(end, end + 0.5f * length * right);
            }
    }
}
