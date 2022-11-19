using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStats : MonoBehaviour
{
    //
    List<string> unbreakable = new List<string>
    {
        "Boss_kakashi"
    };
    // Start is called before the first frame update
    public int HP = 10;
    public int DEF = 10;
    [SerializeField]
    private RectTransform HpBarRect;
    [SerializeField]
    private Transform StatusIndicator;
    //[SerializeField]
    //private TextMeshProUGUI HpText;
    private int currentHP;
    private Color originalColor;
    SpriteRenderer sr;
    void Start()
    {
        currentHP = HP;
        setHp(HP, HP);
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;


    }
    public int getCurrentHP() { return currentHP; }
    // Update is called once per frame
    void Update()
    {


        if (HpBarRect == null)
        {
            Debug.LogError("no barHp reference");
            Debug.LogError(gameObject.name);

        }
        //if (HpText == null)
        //{
        //    Debug.LogError("no HpText reference");

        //}
        if (currentHP <= 0)
        {
            try
            {
                GameObject.FindGameObjectWithTag("GM").GetComponent<GameMasterController>().IncreaseScore(pointPerSkill);
            }
            catch
            {
                Debug.LogError("Fail to add score");
            }
            GameMasterController.Kill(gameObject);
        }
    }
    float prevX;
    void Awake()
    {
        prevX = transform.localScale.x;
    }
    void LateUpdate()
    {
        if (transform.localScale.x * prevX < 0)
            StatusIndicator.localScale = new Vector3(StatusIndicator.localScale.x * -1, StatusIndicator.localScale.y, StatusIndicator.localScale.z);
        prevX = transform.localScale.x;
    }
    public void setHp(int current, int max)
    {
        float value = (float)current / max;
        HpBarRect.localScale = new Vector3(value, HpBarRect.localScale.y, HpBarRect.localScale.z);
        //HpText.text = current +"/"+max+" HP";
    }
    public void getDamage(float damage)
    {
        this.currentHP -= (int)(damage / DEF);
        setHp(currentHP, HP);
        TakeDamageEffect();
    }

    public int ATK = 10;
    public void GiveDamage(Collider2D collision)
    {
        collision.GetComponent<CharacterStats>().getDamage(ATK);
    }
    public void TakeDamageEffect()
    {
        // Tints the sprite red and fades back to the origin color after a delay of 1 second
        StartCoroutine(DamageEffectSequence(sr, Color.red, 0.4f, 0.1f));
    }

    public int pointPerSkill = 10;
    void OnDisable()
    {

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.tag == "Kunai")
        //{
        //    this.HP -= (int)WeaponStats.kunai / DEF;
        //    Debug.Log("Damaged");
        //}
        //if(collision.gameObject.tag == "Kunai") {
        //    if (transform.name == "Boss_kakashi") return;// kakashi mien nhiem sat thuong cua kunai
        //this.currentHP -= (int)WeaponStats.kunai / DEF;
        //setHp(currentHP, HP);
        //Debug.Log(this.currentHP);
        //}
    }
}
