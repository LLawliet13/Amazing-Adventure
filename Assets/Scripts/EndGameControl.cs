using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static MenuGameControl;

public class EndGameControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("currentScore"))
        {
            YourScore.SetText("Your Score: " + PlayerPrefs.GetInt("currentScore"));
        }
    }

    // Update is called once per frame
    void Update()
    {
        PlayerPrefs.SetInt("isPlaying", 0);
    }
    public TextMeshProUGUI YourScore;
    private void OnDisable()
    {
        if (PlayerPrefs.HasKey("currentScore"))
        {
            PlayerPrefs.DeleteKey("ScoreRecords");
            int currentScore = PlayerPrefs.GetInt("currentScore");
            List<ScorePoint> scores;
            if (PlayerPrefs.HasKey("ScoreRecords"))
            {

                string scoreList1 = PlayerPrefs.GetString("ScoreRecords");
                if (scoreList1 == "{}")
                    scores = new List<ScorePoint>();
                else
                    scores = JsonConvert.DeserializeObject<List<ScorePoint>>(scoreList1);

            }
            else
            {
                scores = new List<ScorePoint>();

            }
            scores.Add(new ScorePoint(DateTime.Now, currentScore));

           
            string scoreList = JsonConvert.SerializeObject(scores);
            PlayerPrefs.SetString("ScoreRecords", scoreList);
            PlayerPrefs.DeleteKey("currentScore");
        }
        PlayerPrefs.Save();
    }
}
