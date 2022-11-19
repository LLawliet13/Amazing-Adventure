using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMasterController : MonoBehaviour
{
    public static GameMasterController gm;
    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("currentScore"))
        {
            PlayerPrefs.SetInt("currentScore", 0);
        }
        PlayerPrefs.SetInt("currentScenePlaying", SceneManager.GetActiveScene().buildIndex);
        if (gm == null)
        {
            gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMasterController>();
        }
        LoadData();
    }
    bool isPlaying = false;
    public bool IsPlaying()
    {
        Debug.Log("isPlaying" + isPlaying);
        return isPlaying;
    }
    public void setIsPlaying(bool i)
    {
        isPlaying = i;
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt("isPlaying", 1);
        PlayerPrefs.Save();
    }
    int score = 0;
    public int getCurrentScore()
    {
        return score;
    }
    public void IncreaseScore(int score)
    {
        this.score = this.score + score;
        PlayerPrefs.SetInt("currentScore", this.score);
    }
    public static void Respawn(Transform character, Vector3 location, Quaternion quaternion)
    {
        Instantiate(character, location, quaternion);
    }
    public static void Kill(GameObject gameObject)
    {
        Destroy(gameObject);
    }
    public void saveData()
    {
        PlayerPrefs.SetFloat("RespawnPointX", RespawnPoint.position.x);
        PlayerPrefs.SetFloat("RespawnPointY", RespawnPoint.position.y);
        PlayerPrefs.SetFloat("RespawnPointZ", RespawnPoint.position.z);
    }

    public void LoadData()
    {
        if (PlayerPrefs.HasKey("currentScore"))
        {
            score = PlayerPrefs.GetInt("currentScore");

        }
        else
        {
            score = 0;
        }
        if (!PlayerPrefs.HasKey("isEndAMap"))
        {
            if (PlayerPrefs.HasKey("RespawnPointX"))
            {
                RespawnPoint.position = new Vector3(PlayerPrefs.GetFloat("RespawnPointX", RespawnPoint.position.x),
                PlayerPrefs.GetFloat("RespawnPointY", RespawnPoint.position.y),
                PlayerPrefs.GetFloat("RespawnPointZ", RespawnPoint.position.z));
            }
        }
        else
        {
            if (PlayerPrefs.GetInt("isEndAMap") == 0)
            {
                PlayerPrefs.DeleteKey("isEndAMap");
            }
            else
            {
                PlayerPrefs.SetInt("isEndAMap", 0);//tien trinh sau dung xong thi xoa di
            }
            PlayerPrefs.Save();
        }
    }
    // Update is called once per frame
    void Update()
    {
        ObtainKunai();
        ObtainTransformSkill();
        saveData();
    }
    [SerializeField]
    public GameObject MainCharacter;
    [SerializeField]
    public Transform RespawnPoint;
    public void RespawnCharacter()
    {
        //yield return new WaitForSeconds(2);
        Instantiate(MainCharacter, RespawnPoint.position, Quaternion.identity);
        pendingAction();// khoi phuc cac skill cho player

    }
    public static void KillAndRespawnCharacter(GameObject player)
    {
        Destroy(player);
        //if (player.tag == "Player")
        //    gm.StartCoroutine(gm.RespawnCharacter());
    }

    public GameObject UI;
    bool isObtainTransform = false;
    bool isObtainKunai = false;
    public void ObtainKunai()
    {
        //if(GameObject.Find("SomeBoss") == null&& isObtainKunai == false)
        //{
        //    OptionsMenu om = UI.GetComponent<OptionsMenu>();
        //    om.ShowNotification("Bạn Nhận Được Kunai");
        //    om.EnableKunaiButton();
        //    isObtainKunai = true;
        //try
        //{
        //    //GameObject.FindGameObjectWithTag("Player").GetComponent<main_character_2>().canTransform = true;
        //}
        //catch
        //{
        //    Debug.LogError("No Character Found");
        //    isEnableKunaiSkill = true;
        //}
        //}
    }
    public void ObtainTransformSkill()
    {
        if (GameObject.Find("Boss_CuuVi") == null && isObtainTransform == false && SceneManager.GetActiveScene().buildIndex == 1 &&
            GameObject.FindGameObjectWithTag("Player").GetComponent<main_character_2>().canTransform == false)
        {
            OptionsMenu om = UI.GetComponent<OptionsMenu>();
            om.ShowNotification("Bạn Nhận Được Kĩ Năng Biến Hình");
            om.EnableTransformButton();
            isObtainTransform = true;
            try
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<main_character_2>().canTransform = true;
            }
            catch
            {
                Debug.LogError("No Character Found");
                isEnableTransformSkill = true;
            }
        }
    }
    bool isEnableTransformSkill = false;
    bool isEnableKunaiSkill = false;
    public void pendingAction()
    {
        //xu ly truong hop nhan vat chet sau do hoi sinh khong nhan dc skill sau khi giet quai
        if (isEnableTransformSkill)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<main_character_2>().canTransform = true;
        }
        if (isEnableKunaiSkill)
        {
            //GameObject.FindGameObjectWithTag("Player").GetComponent<main_character_2>().canTransform = true;
        }
    }
    public void ChangeSavePoint(Transform t)
    {
        RespawnPoint = t;
    }


}
