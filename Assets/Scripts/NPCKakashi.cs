using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCKakashi : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
      
    }
    int index = 0;
    public void setDialogName(string name)
    {
        dialogName = name;
    }
    private string dialogName = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            OptionsMenu UI = GameObject.FindGameObjectWithTag("UI").GetComponent<OptionsMenu>();
            if (dialogName != null)
                UI.DialogTrigger(dialogName, index);
            else
            UI.DialogTrigger("kakasi_say_start_game", index);
        }
    }
}
