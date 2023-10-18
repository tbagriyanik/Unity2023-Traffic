using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class yayaSonrakiHedef : MonoBehaviour
{
    public GameObject[] sonrakiler;
    public Color renk;

    public GameObject trafikLambasiKirmizi; //arabalara kirmizi bize yeþil

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
