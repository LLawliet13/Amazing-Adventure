using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sasuke_weapon : MonoBehaviour
{
    // Start is called before the first frame update
    Transform mc;
    Sasuke sasuke;
    int ATK = 10;
    void Start()
    {
        mc = transform.parent;
        sasuke = mc.GetComponent<Sasuke>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        string currentSkill = sasuke.skill_selected();
        if (currentSkill != null)
        {
            if (currentSkill == "skill_lighting")
                ATK = 60;
            else
            if (currentSkill == "skill_fire")
                ATK = 20;
            else
            if (currentSkill == "skill_fire_dragon")
                ATK = 40;
            else
                ATK = 10;
        }

        if (LayerMask.LayerToName(collision.gameObject.layer) == "Player")
        {
            collision.GetComponent<CharacterStats>().getDamage(ATK);
            Vector3 ep = collision.transform.position;
            if (currentSkill == "skill_fire_dragon")
            {
                if (mc.position.x >= ep.x)
                    collision.transform.position = new Vector3(ep.x - 5, ep.y, ep.z);
                else
                    collision.transform.position = new Vector3(ep.x + 5, ep.y, ep.z);
            }
        }
    }
}
