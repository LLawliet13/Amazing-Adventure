using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rashengan : MonoBehaviour
{
    // Start is called before the first frame update
    int ATK = 50;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {


        if (LayerMask.LayerToName(collision.gameObject.layer) == "enemy")
        {
            collision.GetComponent<EnemyStats>().getDamage(ATK);
        }


    }
}
