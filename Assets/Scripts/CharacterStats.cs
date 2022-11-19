using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    // Start is called before the first frame update
    public int HP = 10;
    public int DEF = 10;
    private int charka = 100;
    private int maxCharka = 100;
    [SerializeField]
    private RectTransform HpBarRect;
    [SerializeField]
    private RectTransform CharkaBarRect;


    [SerializeField]
    private TextMeshProUGUI HpText;
    private int currentHP;
    SpriteRenderer sr;
    private Color originalColor;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
        //PlayerPrefs.DeleteAll();
        currentHP = HP;
        if (transform.tag == "Player")
        {

            LoadData();
            if (HpBarRect == null)
            {
                HpBarRect = GameObject.FindGameObjectWithTag("HpBar_Character").GetComponent<RectTransform>();
            }
            if (HpText == null)
            {
                HpText = GameObject.FindGameObjectWithTag("HpText_Character").GetComponent<TextMeshProUGUI>();
            }
            if(CharkaBarRect == null)
            {
                CharkaBarRect = GameObject.FindGameObjectWithTag("CHARKA_Character").GetComponent<RectTransform>();
            }
        }
        setHp(currentHP, HP);

    }
    public int getCurrentCharka()
    {
        return charka;
    }
    public void UseCharka(int minus)
    {
        charka -= (minus+dotieuthuthemcharka);
    }

    // Update is called once per frame
    int dohoicharka = 5;// trang thai naruto se la 5, cuu vi la 10
    int dotieuthuthemcharka = 0;// trang thai naruto se la 0, cuu vi la 20
    int delayHoiCharka = 1;
    float timeToHoiCharKa = 0;
    void Update()
    {
        //if (HpBarRect == null)
        //{
        //    Debug.LogError("no barHp reference");

        //}
        //if (HpText == null)
        //{
        //    Debug.LogError("no HpText reference");

        //}
        if (charka < 100&&Time.time> timeToHoiCharKa) { 
            charka += dohoicharka;
            if (charka > 100) charka = 100;
            float value = (float)charka / maxCharka;
            CharkaBarRect.localScale = new Vector3(value, CharkaBarRect.localScale.y, CharkaBarRect.localScale.z);
            timeToHoiCharKa = Time.time + delayHoiCharka;
        }
        if (transform.tag == "Player")
        {

            isCuuViTime();
            SaveData();


        }
        if (currentHP <= 0 || transform.position.y <= -100)
        {
            setHp(0, HP);
            transform.GetComponent<main_character_2>().CancelAllCopyChar();
            StartCoroutine(runDeadAnimation());
            
        }

    }
    public IEnumerator runDeadAnimation()
    {
        transform.GetComponent<main_character_2>().ChangeAnimation("dead");
        yield return new WaitForSeconds(1);
        GameMasterController.KillAndRespawnCharacter(gameObject);
    }
    private void LoadData()
    {


        GameMasterController gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMasterController>();


        if (!gm.IsPlaying())
        {

            gm.setIsPlaying(true);
            if (!PlayerPrefs.HasKey("isEndAMap"))
            {
                if (PlayerPrefs.HasKey("CurrentHP"))
                {
                    currentHP = PlayerPrefs.GetInt("CurrentHP");
                }
                if (PlayerPrefs.HasKey("CharacterLocationX"))
                {
                    transform.position = new Vector3(PlayerPrefs.GetFloat("CharacterLocationX")
                        , PlayerPrefs.GetFloat("CharacterLocationY"), PlayerPrefs.GetFloat("CharacterLocationZ"));
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


    }
    private void SaveData()
    {
        PlayerPrefs.SetFloat("CharacterLocationX", transform.position.x);
        PlayerPrefs.SetFloat("CharacterLocationY", transform.position.y);
        PlayerPrefs.SetFloat("CharacterLocationZ", transform.position.z);
        PlayerPrefs.SetInt("CurrentHP", this.currentHP);
    }
    private void OnDisable()
    {
        SaveData();
        PlayerPrefs.Save();
    }

    public void setHp(int current, int max)
    {
        float value = (float)current / max;
        if (transform.tag == "Player")
        {
            HpBarRect.localScale = new Vector3(value, HpBarRect.localScale.y, HpBarRect.localScale.z);
            HpText.text = current + "/" + max + " HP";
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Trap")
        {
            getDamage(10);
        }
    }
    public int ATK = 10;
    private float meleeATK { get => ATK ; }

    public void GiveDamage(Collider2D collision)
    {
        collision.GetComponent<EnemyStats>().getDamage(meleeATK);

    }


    public void isCuuViTime()
    {
        // tru hp khi trong trang thai cuu vi
        if (!GameObject.FindGameObjectWithTag("Player").GetComponent<main_character_2>().IsNaruto()
            )
        {
            dohoicharka = 10;
            dotieuthuthemcharka = 20;
            ATK = 50;
        }
        else
        {
            ATK = 10;
            dohoicharka = 5;
            dotieuthuthemcharka = 0;
        }
    }

    internal void getDamage(int v)
    {
        this.currentHP -= (int)(v / DEF);
        setHp(currentHP, HP);
        isHpDecrease();
    }
    public void isHpDecrease()
    {
        //GameObject.FindGameObjectWithTag("Player").GetComponent<main_character_2>().ChangeAnimation("being_attacked");
        TakeDamageEffect();
    }
    public void TakeDamageEffect()
    {
        // Tints the sprite red and fades back to the origin color after a delay of 1 second
        StartCoroutine(DamageEffectSequence(sr, Color.red, 0.4f, 0.1f));
    }


    IEnumerator DamageEffectSequence(SpriteRenderer sr, Color dmgColor, float duration, float delay)
    {
        // save origin color
        Color originColor = originalColor;

        // tint the sprite with damage color
        sr.color = dmgColor;

        // you can delay the animation
        yield return new WaitForSeconds(delay);

        // lerp animation with given duration in seconds
        for (float t = 0; t < 1.0f; t += Time.deltaTime / duration)
        {
            sr.color = Color.Lerp(dmgColor, originColor, t);

            yield return null;
        }

        // restore origin color
        sr.color = originColor;
    }

    public void heal(bool healFull, int x)
    {
        if (healFull == false)
        {

            this.currentHP += x;
            if (currentHP > HP) currentHP = HP;
        }
        else
        {
            this.currentHP = HP;
        }
        setHp(currentHP, HP);

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Trap")
        {
            getDamage(10);
        }
    }
}
