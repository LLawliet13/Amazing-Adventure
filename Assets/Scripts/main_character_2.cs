using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class main_character_2 : MonoBehaviour
{
    // Start is called before the first frame update
    Animator animator;
    Rigidbody2D rb;
    Joystick joystick;
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        //start up
        LoadAnimation();
        setUpPriorityAnimation();
        currentAnimation = "stand";
        ChangeAnimation(currentAnimation);
        rb.freezeRotation = true;
        cam = Camera.main;
        startCameraScale = cam.orthographicSize;
        normalCharScale = transform.localScale;
        LoadData();

    }
    private void OnDisable()
    {
        PlayerPrefs.Save();
    }
    private void LoadData()
    {
        if (PlayerPrefs.HasKey("canTransform"))
        {
            if (PlayerPrefs.GetInt("canTransform") == 1)
                canTransform = true;
            else
                canTransform = false;
        }
    }
    private void SaveData()
    {

        PlayerPrefs.SetInt("canTransform", canTransform == true ? 1 : 0);

    }
    Dictionary<String, AnimationCustom> animationPriority = new Dictionary<string, AnimationCustom>();
    // animation co priority cao hon khi chay se khong bi animation co priority thap hon ngat
    // cac animation co priority thap hon se dc chay khi AnimationTime cua animation co priority cao hon ket thuc
    //animation name:
    //- stand
    //- run
    //- slide
    //- throw
    //- jump
    //- normal_attack
    //- jump_throw
    //- jump_attack
    //- dead
    void setUpPriorityAnimation()
    {
        animationPriority.Clear();
        animationPriority.Add("dead", new AnimationCustom("dead", true, 6, 999));
        animationPriority.Add("rashengan", new AnimationCustom("rashengan", true, 6, getTimeOfAAnimation("rashengan")));
        animationPriority.Add("hoathanchithuat", new AnimationCustom("hoathanchithuat", true, 6, getTimeOfAAnimation("hoathanchithuat")));
        animationPriority.Add("transform", new AnimationCustom("transform", true, 5, getTimeOfAAnimation("transform")));
        animationPriority.Add("being_attacked", new AnimationCustom("being_attacked", false, 5, getTimeOfAAnimation("being_attacked")));
        animationPriority.Add("stand", new AnimationCustom("stand", false, 2, getTimeOfAAnimation("stand")));
        animationPriority.Add("throw", new AnimationCustom("throw", true, 5, getTimeOfAAnimation("throw")));
        animationPriority.Add("jump_throw", new AnimationCustom("jump_throw", true, 5, getTimeOfAAnimation("jump_throw")));
        animationPriority.Add("jump_attack", new AnimationCustom("jump_attack", true, 4, getTimeOfAAnimation("jump_attack")));
        animationPriority.Add("normal_attack", new AnimationCustom("normal_attack", true, 4, getTimeOfAAnimation("normal_attack")));
        animationPriority.Add("slide", new AnimationCustom("slide", true, 3, getTimeOfAAnimation("slide")));
        animationPriority.Add("jump", new AnimationCustom("jump", true, 2, getTimeOfAAnimation("jump")));
        animationPriority.Add("run", new AnimationCustom("run", true, 1, getTimeOfAAnimation("run")));
    }

    Dictionary<string, string> animationNaruto = new Dictionary<string, string>();
    Dictionary<string, string> animationNarutoTransform = new Dictionary<string, string>();
    Dictionary<string, string> animationNameManager;

    bool isNaruto = true;
    public bool IsNaruto()
    {
        return isNaruto;
    }
    public void LoadAnimation()
    {
        animationNaruto.Add("dead", "dead");
        animationNaruto.Add("rashengan", "rashengan");
        animationNaruto.Add("being_attacked", "being_attacked");
        animationNaruto.Add("stand", "stand");
        animationNaruto.Add("throw", "throw");
        animationNaruto.Add("jump_throw", "jump_throw");
        animationNaruto.Add("jump_attack", "jump_attack");
        animationNaruto.Add("normal_attack", "normal_attack");
        animationNaruto.Add("jump", "jump");
        animationNaruto.Add("slide", "slide");
        animationNaruto.Add("run", "run");
        animationNaruto.Add("transform", "transform");
        animationNaruto.Add("hoathanchithuat", "hoathanchithuat");

        animationNarutoTransform.Add("hoathanchithuat", "hoathanchithuat_transform");
        animationNarutoTransform.Add("transform", "transform_revert");
        animationNarutoTransform.Add("rashengan", "rashengan_transform");
        animationNarutoTransform.Add("dead", "dead_transform");
        animationNarutoTransform.Add("being_attacked", "being_attacked_transform");
        animationNarutoTransform.Add("stand", "stand_transform");
        animationNarutoTransform.Add("throw", "throw_transform");
        animationNarutoTransform.Add("jump_throw", "jump_throw_transform");
        animationNarutoTransform.Add("jump_attack", "jump_attack_transform");
        animationNarutoTransform.Add("normal_attack", "normal_attack_transform");
        animationNarutoTransform.Add("jump", "jump_transform");
        animationNarutoTransform.Add("slide", "slide_transform");
        animationNarutoTransform.Add("run", "run_transform");
        animationNameManager = animationNaruto;
    }
    public bool canTransform = false;
    public void Transform()
    {
        if (canTransform && gameObject.tag == "Player")
        {
            if (isNaruto)
            {
                isNaruto = false;
                upScale();// tro ve kich thuoc ban dau
                CopySpell();// xoa cac ban phan than khi bien hinh
                animationNameManager = animationNarutoTransform;
                runVelocity = 10f;

            }
            else
            {
                animationNameManager = animationNaruto;
                isNaruto = true;
                runVelocity = 8f;
            }


            Debug.Log("Change Transform");
            setUpPriorityAnimation();
            ChangeAnimation("transform");
        }
    }
    // Update is called once per frame
    void Update()
    {

        MovingProcess();
        SaveData();
        try
        {
            joystick = GameObject.FindGameObjectWithTag("joystick").GetComponent<Joystick>();
            if (joystick != null)
                MoveUsingJoyStick();
        }
        catch
        {

        }

    }
    private void MoveUsingJoyStick()
    {
        float currentRangeX = joystick.Horizontal;
        float currentRangeY = joystick.Vertical;


        if (currentRangeY > 0)
        {
            if (!isFloating)
            {
                //if (MathF.Abs(currentRangeY) > 0.5)
                rb.velocity = new Vector3(rb.velocity.x, 1 * jumpVelocity, 0);
                //else
                //    rb.velocity = new Vector3(rb.velocity.x, 0.5f * jumpVelocity, 0);
                ChangeAnimation("jump");
                isFloating = true;

            }

        }

        if (currentRangeX < 0)
        {
            if (directionRight)
            {
                transform.localScale = new Vector3(gameObject.transform.localScale.x * -1.0f,
                    gameObject.transform.localScale.y,
                    gameObject.transform.localScale.z);
                directionRight = false;
            }
            rb.velocity = new Vector3(currentRangeX * runVelocity, rb.velocity.y, 0);
            ChangeAnimation("run");
        }
        if (currentRangeX > 0)
        {
            if (!directionRight)
            {
                transform.localScale = new Vector3(gameObject.transform.localScale.x * -1.0f,
                    gameObject.transform.localScale.y,
                    gameObject.transform.localScale.z);
                directionRight = true;
            }
            rb.velocity = new Vector3(currentRangeX * runVelocity, rb.velocity.y, 0);
            ChangeAnimation("run");
        }
    }
    private void FixedUpdate()
    {
        groundCheck();
        wallCheck();
    }
    public LayerMask grounds;
    public Transform GroundCheckPoint;
    public Transform WallCheckPoint1;
    public Transform WallCheckPoint2;
    public Transform WallCheckPoint3;
    public Transform WallCheckPoint4;


    private void wallCheck()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(WallCheckPoint1.position, 0.2f, grounds);
        Collider2D[] colliders2 = Physics2D.OverlapCircleAll(WallCheckPoint2.position, 0.2f, grounds);
        Collider2D[] colliders3 = Physics2D.OverlapCircleAll(WallCheckPoint3.position, 0.2f, grounds);
        Collider2D[] colliders4 = Physics2D.OverlapCircleAll(WallCheckPoint4.position, 0.2f, grounds);
        if (colliders.Length > 0 || colliders2.Length > 0 || colliders3.Length > 0 || colliders4.Length > 0)
        {

            isFloating = false;
        }

    }
    private void groundCheck()
    {

        Collider2D[] colliders = Physics2D.OverlapCircleAll(GroundCheckPoint.position, 0.2f, grounds);
        if (colliders.Length > 0)
        {
            if (currentAnimation.Contains("jump"))
                ChangeAnimation("stand");
            isFloating = false;
        }
        else
        {
            isFloating = true;
            if(Time.time-LastTimeInteract>=0.2)
            ChangeAnimation("jump");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
    }
    bool isFloating = true;
    bool directionRight = true;
    Vector3 lastVelocity;
    float LastTimeInteract;

    void OnlineCheck()
    {
        if (Input.anyKey)
        {
            LastTimeInteract = Time.time;
        }

        if (Time.time - LastTimeInteract >= 0.2)
        {
            if (currentAnimation != "jump")
                ChangeAnimation("stand");
        }
    }
    public void Jump()
    {
        if (!isFloating)
        {
            ChangeAnimation("jump");
            Action("jump", null);
            isFloating = true;
        }
    }
    public void MoveRight()
    {
        if (!directionRight)
            transform.localScale = new Vector3(gameObject.transform.localScale.x * -1.0f,
                gameObject.transform.localScale.y,
                gameObject.transform.localScale.z);

        Action("run", "right");
        ChangeAnimation("run");
        directionRight = true;
    }
    public void MoveLeft()
    {
        if (directionRight)
            transform.localScale = new Vector3(gameObject.transform.localScale.x * -1.0f,
                gameObject.transform.localScale.y,
                gameObject.transform.localScale.z);
        ChangeAnimation("run");
        Action("run", "left");
        directionRight = false;
    }
    public void Slide()
    {
        if (directionRight)
            Action("slide", "right");
        else Action("slide", "left");
        ChangeAnimation("slide");
    }
    private float kunaiUseTime = 0;
    public void ThrowKunai()
    {
        if (kunaiUseTime <= Time.time)
        {

            if (isFloating)
                ChangeAnimation("jump_throw");
            else ChangeAnimation("throw");
            if (isNaruto)
            {
                getChildByName("Kunai", getChildByName("FirePoint",
                    getChildByName("Hand", transform))).GetComponent<KunaiFireController>().shoot("kunai");
                kunaiUseTime = Time.time + 1f;
            }
            else
            {
                getChildByName("Kunai", getChildByName("FirePoint",
                getChildByName("Hand", transform))).GetComponent<KunaiFireController>().shoot("cuuviFire");
                kunaiUseTime = Time.time + 1f;
            }
        }
    }
    public void ThrowKunaiAuto()
    {
        if (kunaiUseTime <= Time.time)
        {

            if (isFloating)
                ChangeAnimation("jump_throw");
            else ChangeAnimation("throw");
            if (isNaruto)
            {
                getChildByName("Kunai", getChildByName("FirePoint",
                    getChildByName("Hand", transform))).GetComponent<KunaiFireController>().shootAuto("kunai");
                kunaiUseTime = Time.time + 1f;
            }
            else
            {
                getChildByName("Kunai", getChildByName("FirePoint",
                getChildByName("Hand", transform))).GetComponent<KunaiFireController>().shootAuto("cuuviFire");
                kunaiUseTime = Time.time + 1f;
            }
        }
    }
    Transform getChildByName(string name, Transform transform)
    {
        foreach (Transform child in transform)
        {
            if (child.name == name)
            {
                return child;
            }
        }
        return null;
    }
    float stopTime;
    public void MovingProcess()
    {
        OnlineCheck();

        if (Input.GetKeyDown(KeyCode.R))
        {
            useReshengan();
        }
        if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)))
        {
            Jump();
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            MoveRight();

        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            MoveLeft();

        }

        if ((Input.GetMouseButton(1) && SystemInfo.deviceType.ToString() == "Desktop") || Input.GetKey(KeyCode.K))
        {
            meleeAttack();

        }
        if (Input.GetKey(KeyCode.Space))
        {

            Slide();

        }

        if (Input.GetMouseButton(0) && SystemInfo.deviceType.ToString() == "Desktop")
        {
            ThrowKunai();
        }
        if (Input.GetKeyDown(KeyCode.B))
            upScale();
        if (Input.GetKeyDown(KeyCode.C))
        {
            CopySpell();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {

            Transform();
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            useHoathanchithuat();
        }
        MoveProcessForButtonDownPhone();
        lastVelocity = rb.velocity;

    }
    public void MoveProcessForButtonDownPhone()
    {
        OptionsMenu om = GameObject.FindGameObjectWithTag("UI").GetComponent<OptionsMenu>();
        if (om != null)
        {
            if (om.moveLeft)
            {
                MoveLeft();
            }
            if (om.moveRight)
                MoveRight();
            if (om.jump)
                Jump();
            if (om.meleeAttack)
                meleeAttack();
            if (om.dash)
                Slide();
            if (om.throwKunai)
                ThrowKunaiAuto();
        }
    }
    float jumpVelocity = 20f;
    float runVelocity = 5f;
    float slideVelocity = 13f;
    void Action(string actionName, string direction)
    {
        if (actionName == "jump")
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpVelocity, 0);
        }
        if (actionName == "run")
        {
            if (direction == "right")
                rb.velocity = new Vector3(runVelocity, rb.velocity.y, 0);
            else
                rb.velocity = new Vector3(-runVelocity, rb.velocity.y, 0);
        }
        if (actionName == "slide")
        {
            if (direction == "right")
            {
                rb.velocity = new Vector3(slideVelocity, rb.velocity.y, 0);
                //Debug.Log(rb.velocity.y);
            }
            else
                rb.velocity = new Vector3(-slideVelocity, rb.velocity.y, 0);
        }
    }

    //Status: building
    string currentAnimation;

    float endTimeAnimation = 0;

    void caculateTimeAnimation(string animationName)
    {
        //Debug.Log(animationNameManager[animationName] + animationPriority[animationName].animationTime);

        endTimeAnimation = Time.time + animationPriority[animationName].animationTime;

    }

    public bool ChangeAnimation(string animationName)

    {

        bool isChangeAnimation = false;
        if (animationPriority[currentAnimation].priority < animationPriority[animationName].priority)
        {
            currentAnimation = animationName;
            animator.Play(animationNameManager[currentAnimation]);
            if (animationPriority[currentAnimation].isNeedCheckTimeAnimation)
            {
                caculateTimeAnimation(currentAnimation);
            }
            isChangeAnimation =  true;
        }
        else
        {
            if (Time.time >= endTimeAnimation)
            {
                currentAnimation = animationName;
                animator.Play(animationNameManager[currentAnimation]);
                if (animationPriority[currentAnimation].isNeedCheckTimeAnimation)
                {
                    caculateTimeAnimation(currentAnimation);
                }
                isChangeAnimation =  true;
            }
        }
        if (currentAnimation == "rashengan")
            transform.Find("Reshengan").gameObject.SetActive(true);
        else
            transform.Find("Reshengan").gameObject.SetActive(false);
        if (currentAnimation == "hoathanchithuat")
            transform.Find("hoathanchithuat_nuyeu").gameObject.SetActive(true);
        else
            transform.Find("hoathanchithuat_nuyeu").gameObject.SetActive(false);

        return isChangeAnimation;
    }

    public Transform attack_point;
    public float MeleeAttackRange = 4f;
    public LayerMask enemies;
    private float delayAttackTime = 1f;
    private float attackTime = 0;
    public void meleeAttack()
    {
        if (Time.time > attackTime)
        {
            if (isFloating)
                ChangeAnimation("jump_attack");
            else
            {
                ChangeAnimation("normal_attack");
            }

            Collider2D[] collisions = Physics2D.OverlapCircleAll(attack_point.position, MeleeAttackRange, enemies);
            CharacterStats cs = transform.GetComponent<CharacterStats>();
            if (cs != null)
            {
                foreach (Collider2D e in collisions)
                {
                    Debug.Log(e.name);
                    cs.GiveDamage(e);

                }
            }
            else
            {
                Debug.LogError("Missing EnemyStatsSCript");
            }

            attackTime = Time.time + delayAttackTime;
        }
    }
    float delayRashengan = 3f;
    float timeToUseRashengan = 0;
    public void useReshengan()
    {
        if (Time.time > timeToUseRashengan)
        {

            CharacterStats cs = transform.GetComponent<CharacterStats>();
            if (cs.getCurrentCharka() >= 10)
            {
                if (ChangeAnimation("rashengan")) { 
                cs.UseCharka(10);
                timeToUseRashengan = Time.time+ delayRashengan;
                }
            }
        }
    }
    float delayHoathanchithuat = 3f;
    float timeToUseHoathanchithuat = 0;
    public void useHoathanchithuat()
    {
        if (Time.time > timeToUseHoathanchithuat)
        {

            CharacterStats cs = transform.GetComponent<CharacterStats>();
            if (cs.getCurrentCharka() >= 20)
            {
                if (ChangeAnimation("hoathanchithuat")) { 
                cs.UseCharka(20);

                timeToUseHoathanchithuat = Time.time + delayHoathanchithuat;
                }
            }
        }
    }
    float getTimeOfAAnimation(string animationName)
    {
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        animationName = animationNameManager[animationName];
        foreach (AnimationClip clip in clips)
        {
            if (animationName == clip.name)
            {

                return clip.length;
            }
        }
        return 0;

    }
    ////////////////

    //Skills
    public GameObject copyspell_character;
    public int sizeXCopySpell = 5;
    List<GameObject> copy_char = new List<GameObject>();
    public void CopySpell()
    {

        if (transform.tag == "Player")
        {
            List<GameObject> copy_charExist = new List<GameObject>();
            foreach (var c in copy_char)
            {
                if (c != null)
                {
                    if (!isNaruto) Destroy(c);
                    else
                        copy_charExist.Add(c);
                }

            }
            if (!isNaruto) return;
            copy_char = copy_charExist;
            if (copy_char.Count <= sizeXCopySpell)
            {
                GameObject copyA = Instantiate(copyspell_character,
                    new Vector3(transform.position.x - sizeXCopySpell / 5, transform.position.y + 1, 0), Quaternion.identity);
                float scaleUP = MathF.Abs(transform.localScale.x / copyA.transform.localScale.x);
                copyA.transform.localScale = new Vector3(copyA.transform.localScale.x * scaleUP, copyA.transform.localScale.x * scaleUP, copyA.transform.localScale.x * scaleUP);
                copy_char.Add(copyA);
                ;
            }
        }
    }
    public void CancelAllCopyChar()
    {
        foreach (var c in copy_char)
        {
            if (c != null)
            {
                Destroy(c);
            }

        }
    }
    public float camEdgePosition(String edge)
    {
        if (cam)
        {
            if (edge == "bottom")
                return cam.ViewportToWorldPoint(new Vector3(0f, 0f, 0f)).y;
            if (edge == "right")
                return cam.ViewportToWorldPoint(new Vector3(1.0f, 0f, 0f)).x;
            if (edge == "top")
                return cam.ViewportToWorldPoint(new Vector3(0f, 1.0f, 0f)).y;
            if (edge == "left")
                return cam.ViewportToWorldPoint(new Vector3(0f, 0f, 0f)).x;
        }
        return 0.0f;

    }


    public Camera cam;
    public float startCameraScale;
    public float speedFollowCam = 10;
    bool scaleUp = false;
    Vector3 normalCharScale;
    public void upScale()
    {
        if (isNaruto)
        {
            if (scaleUp == false)
            {
                gameObject.transform.localScale = gameObject.transform.localScale * 1.5f;
                gameObject.transform.Translate(new Vector3(0, 5, 0));
                jumpVelocity *= 1.5f;
                speedFollowCam += 5;
                cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, cam.orthographicSize + 4, Time.deltaTime * 10);
                scaleUp = true;
                return;
            }

            if (scaleUp == true)
            {
                gameObject.transform.localScale = gameObject.transform.localScale / 1.5f;
                jumpVelocity /= 1.5f;
                speedFollowCam -= 5;
                cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, cam.orthographicSize - 4, Time.deltaTime * 10);
                scaleUp = false;
            }
        }
        else
        {
            jumpVelocity = 25f;
            scaleUp = false;
            cam.orthographicSize = startCameraScale;
            speedFollowCam = 15;
            transform.localScale = transform.localScale.x < 0 ? new Vector3(normalCharScale.x * -1, normalCharScale.y, normalCharScale.z) : normalCharScale;
        }
    }
    public GameObject AutoDetect()
    {
        float left = camEdgePosition("left");
        float right = camEdgePosition("right");
        float bottom = camEdgePosition("bottom");
        float top = camEdgePosition("top");
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            float x = enemy.transform.position.x;
            float y = enemy.transform.position.y;
            if (x >= left && x <= right &&
                y >= bottom && y <= top)
            {
                return enemy;
            }
        }
        return null;
    }
}

public class AnimationCustom
{
    public String animationName;
    public bool isNeedCheckTimeAnimation;
    public int priority;
    public float animationTime;

    public AnimationCustom(string animationName, bool isNeedCheckTimeAnimation, int priority, float animationTime)
    {
        this.animationName = animationName;
        this.isNeedCheckTimeAnimation = isNeedCheckTimeAnimation;
        this.priority = priority;
        this.animationTime = animationTime;
    }
}
