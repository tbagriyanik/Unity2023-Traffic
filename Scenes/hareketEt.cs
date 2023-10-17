using UnityEngine;

public class hareketEt : MonoBehaviour
{
    public Transform hedef;
    public float aracHizi = 10f;
    public float donusHizi = 3.5f;

    public GameObject frenLambasi;
    public float brakeForce = 10f;
    public float rayLength = 5f;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        frenLambasi.SetActive(false); //normalde fren lambasý kapalý

        

        Vector3 direction = (hedef.position - transform.position).normalized;
        rb.velocity = direction * aracHizi;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * donusHizi);
        if (Vector3.Distance(transform.position, hedef.position) < 2f)
        {   //Hedefe ulaþýldý ise sonraki seçilir. trafik ýþýk kontrolü yapýlýr
            if (kirmiziIsikKontrol()) { dur(); return; }
            int hedefSayisi = hedef.gameObject.GetComponent<sonrakiHedef>().sonrakiler.Length;
            if (hedefSayisi > 0)
                hedef = hedef.gameObject.GetComponent<sonrakiHedef>().sonrakiler[Random.Range(0, hedefSayisi)].transform;
            else
            {
                dur(); //gidecek yer yok, dur
            }
        }

        Ray carray = new(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), transform.forward);
        if (Physics.Raycast(carray, out RaycastHit carhit, rayLength) && carhit.transform.gameObject.CompareTag("vehicle"))
        {
            dur(); //önde araç var, dur
        }
    }

    private bool kirmiziIsikKontrol()
    {
        if (hedef.gameObject.GetComponent<sonrakiHedef>().trafikLambalari.Length == 0) return false;

        return hedef.gameObject.GetComponent<sonrakiHedef>().trafikLambalari[0].activeSelf ||
            hedef.gameObject.GetComponent<sonrakiHedef>().trafikLambalari[1].activeSelf;        
    }

    private void dur()
    {
        rb.velocity = Vector3.zero; //Sonraki hedef yok ise durur.
        rb.angularVelocity = Vector3.zero;
        frenLambasi.SetActive(true);
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (hedef != null)
            Gizmos.DrawLine(transform.position, hedef.position);
    }
}
