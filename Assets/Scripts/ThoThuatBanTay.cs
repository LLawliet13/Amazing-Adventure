using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThoThuatBanTay : MonoBehaviour
{
    // Start is called before the first frame update
    public int ATK = 10;
    public LayerMask userLayer;
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.Play("bantaytroilen");
        Destroy(gameObject, 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (LayerMask.LayerToName(collision.gameObject.layer) != LayerMask.LayerToName(findLayerNumber(userLayer)))
        {
            if (LayerMask.LayerToName(collision.gameObject.layer) == "Player")
            {
                collision.GetComponent<CharacterStats>().getDamage(ATK);
            }
            if (LayerMask.LayerToName(collision.gameObject.layer) == "enemy")
            {
                collision.GetComponent<EnemyStats>().getDamage(ATK);
            }
        }
    }
    int findLayerNumber(int x)
    {
        for (int i = 0; i < 100; i++)
        {
            if ((1 << i) == x)
            {
                return i;
            }
        }
        return -1;
    }
}
