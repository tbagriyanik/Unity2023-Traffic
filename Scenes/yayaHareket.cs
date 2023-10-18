using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class yayaHareket : MonoBehaviour
{
    public Transform hedef;
    public float animasyonHizi = 0.5f;
    public float yayaHizi = 1f;
    public float donusHizi = 3.5f;
    public float rayLength = 5f;

    private Animator playerAnim;
    private Rigidbody rb;
    bool dursunMu = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        playerAnim.SetFloat("Speed_f", animasyonHizi);
        InvokeRepeating(nameof(karaktedIdleCesidi), 0, 3f);
    }

    private void karaktedIdleCesidi()
    {
        int rasgele = Random.Range(0, 7);
        playerAnim.SetInteger("Animation_int", rasgele);
    }

    IEnumerator dur()
    {
        rb.velocity = Vector3.zero; //Sonraki hedef yok ise durur.
        rb.angularVelocity = Vector3.zero;
        dursunMu = true;
        if (rb.velocity.magnitude > 0.01f)
            playerAnim.SetFloat("Speed_f", 0.3f);
        else
            playerAnim.SetFloat("Speed_f", 0);
        yield return new WaitForSeconds(0.3f);
    }

    private void yuru()
    {
        if (dursunMu) return;

        Vector3 direction = (hedef.position + randomOffset() - transform.position).normalized;
        rb.velocity = direction * yayaHizi;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * donusHizi);

        if (rb.velocity.magnitude > 0.01f && rb.velocity.magnitude < 0.5f)
            playerAnim.SetFloat("Speed_f", 0.3f);
        else
            playerAnim.SetFloat("Speed_f", animasyonHizi);
    }

    void FixedUpdate()
    {
        Ray carray = new(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), transform.forward);
        if (Physics.Raycast(carray, out RaycastHit carhit, rayLength) && carhit.transform.gameObject.CompareTag("vehicle"))
        {
            StartCoroutine(dur()); //önde araç var, dur
        }
        else
        {
            if (Vector3.Distance(transform.position, hedef.position) < 0.5f)
            {   //Hedefe ulaþýldý ise sonraki seçilir
                if (kirmiziIsikKontrol()) StartCoroutine(dur());
                int hedefSayisi = hedef.gameObject.GetComponent<yayaSonrakiHedef>().sonrakiler.Length;
                if (hedefSayisi > 0)
                {
                    hedef = hedef.gameObject.GetComponent<yayaSonrakiHedef>().sonrakiler[Random.Range(0, hedefSayisi)].transform;
                }
                else
                    StartCoroutine(dur()); //gidecek yer yok, dur
            }
            else dursunMu = false;
        }

        yuru();
    }

    private bool kirmiziIsikKontrol()
    {
        if (!hedef.gameObject.GetComponent<yayaSonrakiHedef>().trafikLambasiKirmizi) 
            return false;

        return !hedef.gameObject.GetComponent<yayaSonrakiHedef>().trafikLambasiKirmizi.activeSelf;
    }

    private Vector3 randomOffset()
    {
        return new Vector3(Random.Range(0, 0.2f), 0, Random.Range(0, 0.2f)); //ayný yolda giderken birbirine takýlmasýnlar
    }
}
