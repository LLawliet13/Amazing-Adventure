using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class OptionsMenu : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject optionsMenu;
    public GameObject NPCDialogPanel;
    public GameObject controlPanel;
    main_character_2 char_script;
    public bool iscontrolPanelActive = true;
    public TextMeshProUGUI ScoreText;
    // Start is called before the first frame update
    void Start()
    {
        loadDialog();

        DisableNotActivePanel();
    }
    public void DisableNotActivePanel()
    {
        NPCDialogPanel.SetActive(false);

        optionsMenu.SetActive(false);
        NotifyTable.SetActive(false);
        guidePanel.SetActive(false);
        if (PlayerPrefs.HasKey("canTransform"))
        {
            if (PlayerPrefs.GetInt("canTransform") == 1)
            {
                transformButton.SetActive(true);
            }
            else
            {
                transformButton.SetActive(false);
            }
        }
        else
        {
            transformButton.SetActive(false);
        }
        RetryPanel.SetActive(false);
    }
    public void updateScore()
    {
        try
        {
            string score = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMasterController>().getCurrentScore() + "";
            ScoreText.SetText("Score: " + score);

        }
        catch
        {
            Debug.Log("Update Score Fail");
        }
    }
    // Update is called once per frame
    public void lockUI()
    {
        if (SystemInfo.deviceType.ToString() == "Desktop")
        {
            controlPanel.SetActive(false);
            iscontrolPanelActive = false;

        }
    }
    void Update()
    {
        lockUI();
        updateScore();

        if (Input.GetKeyDown(KeyCode.O))
        {
            if (isPaused)
            {
                Resume();
                isPaused = false;
            }
            else
            {
                Pause();
                isPaused = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            ShowGuideLine();
        }
        if (char_script == null)
            try
            {
                char_script = GameObject.FindGameObjectWithTag("Player").GetComponent<main_character_2>();
            }
            catch
            {

            }
        DetectRetry();
        updateScore();
    }

    public void Pause()
    {
        optionsMenu.SetActive(true);
        controlPanel.SetActive(false);
        isPaused = true;
        Time.timeScale = 0f;
    }
    public void Resume()
    {
        optionsMenu.SetActive(false);
        controlPanel.SetActive(true);
        isPaused = false;
        Time.timeScale = 1f;
    }
    public void Quit()
    {
        Application.Quit();
    }
    public bool moveLeft = false;
    public bool moveRight = false;
    public bool jump = false;
    public bool meleeAttack = false;
    public bool throwKunai = false;
    public bool dash = false;
    public void CancelMoveLeft()
    {
        moveLeft = false;
    }
    public void MoveLeft()
    {
        moveLeft = true;
    }
    public void CancelMoveRight()
    {
        moveRight = false;
    }

    public void MoveRight()
    {
        moveRight = true;
    }
    public void CancelJump()
    {
        jump = false;
    }
    public void Jump()
    {
        jump = true;
    }
    public void CancelSlide()
    {
        dash = false;
    }
    public void Slide()
    {
        dash = true;
    }
    public void Attack()
    {
        meleeAttack = true;
    }
    public void CancelAttack()
    {
        meleeAttack = false;
    }
    public void ThrowKunai()
    {
        throwKunai = true;
    }
    public void CancelThrowKunai()
    {
        throwKunai = false;
    }
    public void CopySpell()
    {
        try
        {
            char_script.CopySpell();
        }
        catch { }
    }
    public void Transform()
    {
        try
        {
            char_script.Transform();
        }
        catch { }
    }
    public void Rashengan()

    {
        try
        {
            char_script.useReshengan();
        }
        catch { }
    }
    public void Hoathuat()
    {
        try
        {
            char_script.useHoathanchithuat();
        }
        catch
        {
        }
    }
    public void UpScale()
    {
        try
        {
            char_script.upScale();
        }
        catch { }
    }
    [SerializeField]
    private GameObject NotifyTable;
    [SerializeField]
    private TextMeshProUGUI NotificationText;
    public void ShowNotification(string content)
    {
        Time.timeScale = 0f;
        NotifyTable.SetActive(true);
        NotificationText.text = content;
    }
    public void CloseNotification()
    {
        Debug.Log("CloseNotification");
        NotifyTable.SetActive(false);
        Time.timeScale = 1f;

    }
    public GameObject guidePanel;
    public void ShowGuideLine()
    {
        Time.timeScale = 0f;
        guidePanel.SetActive(true);
    }
    public void CloseGuideLine()
    {
        Time.timeScale = 1f;
        guidePanel.SetActive(false);
    }
    [SerializeField]
    private GameObject kunaiButton;
    public void EnableKunaiButton()
    {
        kunaiButton.SetActive(true);

    }
    [SerializeField]
    private GameObject transformButton;
    internal void EnableTransformButton()
    {
        transformButton.SetActive(true);

    }

    Dictionary<string, NPCDialog> NPCDialogList = new Dictionary<string, NPCDialog>();


    private void loadDialog()
    {
        List<String> list = new List<String>();
        list.Add("Hãy đi theo con đường phía trước và hoàn thành các nhiệm vụ sau để hoàn thành cuộc khảo sát nhập học của con\n" +
            "- Khiêu chiến thành công cửu vĩ\n" +
            "- Vượt qua được ít nhất 2 của ải\n" +
            "Chúc con hoàn thành tốt bài sát hạch!");
        NPCDialogList.Add("kakasi_say_start_game", new NPCDialog(list, "kakashi teacher"));
        list = new List<string>();
        list.Add("Thật bất ngờ con lại mạnh tới vậy <3\nChào mừng con tới trường đại học FPT");
        NPCDialogList.Add("kakasi_say_end_game", new NPCDialog(list, "kakashi teacher"));

        list = new List<string>();
        list.Add("Khá tốt con đã đánh bại được cửu vĩ,con có thể tiến tới cổng dịch chuyển để đến địa điểm khảo sát \ntiếp theo hoặc con có thể ở lại đây một lúc");
        NPCDialogList.Add("kakasi_say_end_map1", new NPCDialog(list, "kakashi teacher"));
    }
    public void DialogTrigger(string name_of_dialog, int dialogIndex)
    {
        moveLeft = false;
        moveRight = false;
        jump = false;
        meleeAttack = false;
        throwKunai = false;
        dash = false;
        runDialog(NPCDialogList[name_of_dialog].Dialogs.ElementAt(dialogIndex)
                , NPCDialogList[name_of_dialog].NPCName);
    }
    public void EndDialog()
    {
        if (!iscontrolPanelActive)
        {
            controlPanel.SetActive(true);
            iscontrolPanelActive = true;
        }
        NPCDialogPanel.SetActive(false);

    }
    [SerializeField]
    private TextMeshProUGUI CharNameText;
    [SerializeField]
    private TextMeshProUGUI DialogText;
    void DialogType(string dialog)
    {
        DialogText.text = dialog;

    }
    private void runDialog(string dialog, string characterName)
    {
        controlPanel.SetActive(false);
        iscontrolPanelActive = false;
        NPCDialogPanel.SetActive(true);
        DialogText.text = "";
        CharNameText.SetText(characterName);
        DialogType(dialog);

    }
    public GameObject RetryPanel;
    public void DetectRetry()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Time.timeScale = 0f;
            RetryPanel.SetActive(true);
        }
    }
    public void Retry()

    {
        Debug.Log("RetryClick");
        Time.timeScale = 1f;
        try
        {
            GameObject.FindGameObjectWithTag("GM").GetComponent<GameMasterController>().RespawnCharacter();
        }
        catch
        {
            Debug.Log("false to response");
        }
        RetryPanel.SetActive(false);
    }
    private class NPCDialog
    {

        public List<String> Dialogs;
        public String NPCName;
        public NPCDialog(List<String> dialogs, string NPCName)
        {
            this.Dialogs = dialogs;
            this.NPCName = NPCName;

        }


    }
}

