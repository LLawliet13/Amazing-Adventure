using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoaThanChiThuat : MonoBehaviour
{
    // Start is called before the first frame update
    Transform mc;
    void Start()
    {
        mc = transform.parent;
    }
    public int ATK = 10;
    // Update is called once per frame
    void Update()
    {


    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (ATK == 0) ATK = 10;
        if (LayerMask.LayerToName(collision.gameObject.layer) == "enemy")
        {
            collision.GetComponent<EnemyStats>().getDamage(ATK);
            Vector3 ep = collision.transform.position;
            if (mc.position.x >= ep.x)
                collision.transform.position = new Vector3(ep.x - 5, ep.y, ep.z);
            else
                collision.transform.position = new Vector3(ep.x + 5, ep.y, ep.z);
        }
    }
}
