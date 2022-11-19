using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class MenuGameControl : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject MainMenu;
    public GameObject MainMenuForContinue;
    public GameObject ScorePanel;
    public GameObject ScorePanelButton;
    public TextMeshProUGUI ScoreText;
    GameObject globalMenu;

    void Start()
    {
        if (PlayerPrefs.HasKey("isPlaying"))
        {
            if (PlayerPrefs.GetInt("isPlaying") == 1)
            {
                MainMenu.SetActive(false);
                MainMenuForContinue.SetActive(true);
                globalMenu = MainMenuForContinue;
                MainMenuForContinue.GetComponent<SceneSwitcher>().nextScene = 1;
                MainMenuForContinue.GetComponent<SceneSwitcher>().currentScene = PlayerPrefs.GetInt("currentScenePlaying");
            }
            else
            {
                MainMenu.SetActive(true);
                MainMenuForContinue.SetActive(false);
                globalMenu = MainMenu;
                MainMenu.GetComponent<SceneSwitcher>().nextScene = 1;
            }
        }
        else
        {
            MainMenu.SetActive(true);
            MainMenuForContinue.SetActive(false);
            globalMenu = MainMenu;
            MainMenu.GetComponent<SceneSwitcher>().nextScene = 1;
        }

        if (PlayerPrefs.HasKey("ScoreRecords"))
        {
            ScorePanelButton.SetActive(true);

        }
        else
        {
            ScorePanelButton.SetActive(false);
        }
        ScorePanel.SetActive(false);
    }
    public void DeleteAllPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
    public void Play()
    {
        String saveScoreRecords = "";
        if (PlayerPrefs.HasKey("ScoreRecords"))
        {
            saveScoreRecords = PlayerPrefs.GetString("ScoreRecords");
            DeleteAllPrefs();
            PlayerPrefs.SetString("ScoreRecords", saveScoreRecords);


        }else
            DeleteAllPrefs();
        PlayerPrefs.Save();
        globalMenu.GetComponent<SceneSwitcher>().LoadNextScene();
    }
    private void OnDisable()
    {
        PlayerPrefs.Save();
    }
    public void Continue()
    {
        globalMenu.GetComponent<SceneSwitcher>().Restart();
    }
    public void showScoreList()
    {
        List<ScorePoint> scores = JsonConvert.DeserializeObject<List<ScorePoint>>(PlayerPrefs.GetString("ScoreRecords"));
        String content = "";
        int i = 1;
        scores = scores.OrderByDescending(s => s.score).Take(5).ToList();
        foreach (ScorePoint score in scores)
        {
            content += "Top " +i+": "+ score.score + " Điểm\n";
        }
        ScorePanel.SetActive(true);
        ScoreText.SetText(content);

    }
    public void closeScoreList()
    {
        ScorePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    [Serializable]
    public class ScorePoint
    {
        public DateTime time;
        public int score;

        public ScorePoint(DateTime time, int score)
        {
            this.time = time;
            this.score = score;
        }

    }
}
