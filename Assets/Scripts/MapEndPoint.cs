using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapEndPoint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    bool isEndAMap = false;
    // Update is called once per frame
    void Update()
    {
    }

    public int nextMap;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerPrefs.SetInt("isEndAMap", 1);
        if (collision.gameObject.tag=="Player")
            SceneManager.LoadScene(nextMap);
    }
    private void OnDisable()
    {
        PlayerPrefs.Save();
    }
}
