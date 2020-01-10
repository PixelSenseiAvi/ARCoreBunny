using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
//using GoogleARCore.Examples.Common;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class RabbitController : MonoBehaviour {

    Animator animator;

    private Transform startPos;

    // anim hashes
    int speedHash = Animator.StringToHash("speed");
    int foodHash = Animator.StringToHash("food");
    int nervosityHash = Animator.StringToHash("nervosity");
    int watchHash = Animator.StringToHash("watch");
    int deathHash = Animator.StringToHash("death");
    int jumpHash = Animator.StringToHash("jump");
    int danceHash = Animator.StringToHash("dance");

    [Header("Rabbit")]
    public GameObject carrotPrefab;
    GameObject boneForCarrot;
    
    public GameObject rabbitSmooth;
    public GameObject rabbitFlat;
    GameObject startPlacement;

    SkinnedMeshRenderer rabbitSmoothMesh, rabbitFlatMesh;

    public Material whiteFur;
    public Material greyFur;
    public Material brownFur;
    public Material whiteLowPoly;
    public Material greyLowPoly;
    public Material brownLowPoly;

    bool smoothActive = true;
    bool textTrigger;

    [Header("Text")]
    public TextMeshPro textMesh;

    //TextScript
    private float startTime;
    string stage;
    float speed;

    //initial and last positions
    Vector3 lp, fp;
    [Header("Fruits")]
    public GameObject Apple;
    public GameObject Peanut;
    public GameObject Pear;
    public GameObject Peach;
    public int score;
    Vector3 touchLoc;
    float mTimer;

    float dist1, dist2;
    [Header("Other Variables")]
    public GameObject finalPlacement;
    Vector3 adjustedPos;

    [Range(0.0f, 1.0f)]
    public float LightThreshold = 0.5f;
    bool flag;
    private GameObject textGameObject;

    public GameObject ScoreVar;

    void Start () {

        ScoreVar = GameObject.Find("ScoreVar");

        startTime = Time.deltaTime;
        speed = 0.5f;
        mTimer = 0.0f;
        animator = GetComponentInChildren<Animator>();
        // create nicer solution
        boneForCarrot = GameObject.FindGameObjectWithTag("Carrot");

        rabbitSmoothMesh = rabbitSmooth.GetComponentInChildren<SkinnedMeshRenderer>();
        rabbitFlatMesh = rabbitFlat.GetComponentInChildren<SkinnedMeshRenderer>();

        //startPos = rabbitSmooth.transform.position;
        startPlacement = GameObject.FindGameObjectWithTag("startQuad");
        stage = "1";

        adjustedPos = new Vector3(0.0f, 0.0f, 0.0f);
        flag = false;

        //textGameObject = GameObject.FindWithTag("TMP");
        //textMesh = GameObject.FindWithTag("TMP").GetComponent<TextMeshProUGUI>();

        textMesh.text = "An ______ a day keeps the doctor away";

        score = 0;
    }

    void Update() {

        ScoreVar.GetComponent<Text>().text = score.ToString();

        mTimer += Time.deltaTime;

        float MaxTimeWait = 1;
        float VariancePosition = 1;

        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            float DeltaTime = Input.GetTouch(0).deltaTime;
            float DeltaPositionLength = Input.GetTouch(0).deltaPosition.magnitude;

            if (DeltaTime > 0 && DeltaTime < MaxTimeWait && DeltaPositionLength < VariancePosition)
            {
                Touch touch = Input.GetTouch(0);
                //get touch location
                touchLoc = touch.position;

                Ray ray = Camera.main.ScreenPointToRay(touchLoc);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    
                    adjustedPos = new Vector3(hit.transform.position.x, hit.transform.position.y - 0.25f,
                        hit.transform.position.z);
                    //interpolate here
                    float distance = Vector3.Distance(rabbitSmooth.transform.position,
                        adjustedPos); // hit.transform.position

                    float distCovered = (Time.deltaTime - startTime) * speed;
                    //float fracJourney = distCovered / distance;
                    float fracJourney = distCovered / distance;

                    Jump();
                    transform.position = Vector3.Lerp(
                            rabbitSmooth.transform.position, adjustedPos,
                            fracJourney); //hit.transform.position

                    //Destroy
                    //Destroy(hit.collider.gameObject);
                    
                    if (hit.collider.name == "apple")// && stage == "1"
                    {
                        score++;
                        stage = "2";
                        animator.SetTrigger(danceHash);

                        this.transform.position = hit.transform.position;

                        StartCoroutine(scoreCoroutine("I am a symbol of mortality"));
                        //text.text="I am a symbol of mortality";
                        Destroy(hit.collider.gameObject);

                    }
                    else if (hit.collider.name == "peanut" && stage != "1")
                    {
                        stage = "2";
                        this.transform.position = hit.transform.position;

                        //text.text = "I am a symbol of mortality";
                        SetText("I am a symbol of mortality");
                        Destroy(hit.collider.gameObject);
                    }

                    if (hit.collider.name == "pear")//&& stage == "2"
                    {
                        score++;
                        stage = "3";
                        this.transform.position = hit.transform.position;

                        animator.SetTrigger(danceHash);
                        Destroy(hit.collider.gameObject);

                        // text.text = "rich in Vitamin C";
                        StartCoroutine(scoreCoroutine("rich in Vitamin C"));
                    }
                    else if (hit.collider.name == "peach" && stage != "2")
                    {
                        stage = "3";
                        this.transform.position = hit.transform.position;

                        Destroy(hit.collider.gameObject);

                        // text.text = "rich in Vitamin C";
                        SetText("rich in Vitamin C");
                    }

                    if (hit.collider.name == "lemon") //&& stage == "3"
                    {
                        score++;
                        stage = "4";
                        this.transform.position = hit.transform.position;

                        animator.SetTrigger(danceHash);
                        Destroy(hit.collider.gameObject);

                        //text.text = "I am yellow, peel me I am white";
                        StartCoroutine(scoreCoroutine("I am yellow, peel me I am white"));

                    }
                    else if (hit.collider.name == "strawberry" && stage !="3")
                    {
                        stage = "4";
                        Destroy(hit.collider.gameObject);
                        this.transform.position = hit.transform.position;

                        //text.text = "I am yellow, peel me I am white";
                        SetText("I am yellow, peel me I am white");
                    }

                    if (hit.collider.name == "banana") // && stage == "4"
                    {
                        score++;
                        stage = "Complete";
                        this.transform.position = hit.transform.position;

                        animator.SetTrigger(danceHash);
                        Destroy(hit.collider.gameObject);

                        StartCoroutine(CoroutineOne());

                        // text.text = "Completed";
                        if (score == 4)
                            SetText("Winner");
                        else
                            SetText("Score: " + score);
                    }
                }

            }
        }


        if (flag)
        {
            // try Light estimation
            if (Frame.LightEstimate.PixelIntensity < LightThreshold)
            {
                //Dark
                DeathInSit();
                //NervousIdle();
            }
            else
            {
                //Light
                //this.GetComponent<Text>().text = "LIGHT " + Frame.LightEstimate.PixelIntensity.ToString();
                Watch();
            }
        }

    }

    private IEnumerator CoroutineOne()
    {
        yield return new WaitForSeconds(4);

        
        //interpolate here
        float distance = Vector3.Distance(rabbitSmooth.transform.position,
            finalPlacement.transform.position);

        float distCover = (Time.time - startTime) * speed;
        float fracJourn = distCover / distance;

        Jump();
        transform.position = Vector3.Lerp(
           rabbitSmooth.transform.position, finalPlacement.transform.position,
           fracJourn);
        

        yield return StartCoroutine(CoroutineTwo());
    }

    private IEnumerator CoroutineTwo()
    {
        animator.SetTrigger(danceHash);
        yield return new WaitForSeconds(2);
        //EatGrass();
        //yield return new WaitForSeconds(2);
        EatCarrot();

        //light estimation
        yield return new WaitForSeconds(2);
        flag = true;
    }


    private IEnumerator scoreCoroutine(string newText)
    {
        SetText("+1");
        yield return new WaitForSeconds(2);
        SetText(newText); 
    }

    private void SetText(string riddle)
    {
        textMesh.text = riddle;
    }

    public int GetScore()
    {
        return score;
    }
    //void FixedUpdate()
    //{
    //    if (textTrigger)
    //        text.text = riddle;
    //}


    /* For testing only
     * private void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Keypad1))
        {
            SetRabbit("smooth");
            SetMaterial("white");
        }
        else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Keypad2))
        {
            SetRabbit("smooth");
            SetMaterial("grey");
        }
        else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Keypad3))
        {
            SetRabbit("smooth");
            SetMaterial("brown");
        }
        else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Keypad4))
        {
            SetRabbit("lowpoly");
            SetMaterial("white");
        }
        else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Keypad5))
        {
            SetRabbit("lowpoly");
            SetMaterial("grey");
        }
        else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Keypad6))
        {
            SetRabbit("lowpoly");
            SetMaterial("brown");
        }
        else if (Input.GetKeyDown(KeyCode.Keypad1))
            CalmIdle();
        else if (Input.GetKeyDown(KeyCode.Keypad2))
            NervousIdle();
        else if (Input.GetKeyDown(KeyCode.Keypad3))
            Watch();
        else if (Input.GetKeyDown(KeyCode.Keypad4))
            EatCarrot();
        else if (Input.GetKeyDown(KeyCode.Keypad5))
            EatGrass();
        else if (Input.GetKeyDown(KeyCode.Keypad6))
            Hop();
        else if (Input.GetKeyDown(KeyCode.Keypad7))
            Run();
        else if (Input.GetKeyDown(KeyCode.Keypad8))
            Jump();
        else if (Input.GetKeyDown(KeyCode.KeypadMinus))
            DeathInSit();
        else if (Input.GetKeyDown(KeyCode.KeypadPlus))
            DeathInRun();
        else if (Input.GetKeyDown(KeyCode.Keypad0))
            Dance();
    }*/

    #region Actions

    public void CalmIdle()
    {
        SetSpeed(0);
        SetFood(0);
        SetNervosity(0);
    }

    public void NervousIdle()
    {
        SetSpeed(0);
        SetFood(0);
        SetNervosity(1);
    }

    public void Watch()
    {
        if(animator.GetFloat(speedHash) > 0 || animator.GetFloat(foodHash) > 0)
        {
            SetSpeed(0);
            SetFood(0);
            StartCoroutine(SetLateTrigger(watchHash));
        }
        else
        {
            animator.SetTrigger(watchHash);
        }
    }

    public void EatCarrot()
    {
        SetSpeed(0);
        SetFood(1);
    }

    public void EatGrass()
    {
        SetSpeed(0);
        SetFood(0.5f);
    }

    public void Hop()
    {
        SetSpeed(0.5f);
        SetFood(0);
    }

    public void Run()
    {
        SetSpeed(1);
        SetFood(0);
    }

    public void Jump()
    {
        if(animator.GetFloat(foodHash) > 0)
        {
            SetFood(0);
            StartCoroutine(SetLateTrigger(jumpHash));
        }
        else
        {
            animator.SetTrigger(jumpHash);
        }
    }

    public void DeathInSit()
    {
        if(animator.GetFloat(speedHash) > 0 || animator.GetFloat(foodHash) > 0)
        {
            SetSpeed(0);
            SetFood(0);
            StartCoroutine(SetLateTrigger(deathHash));
        }
        else
        {
            animator.SetTrigger(deathHash);
        }
    }

    public void DeathInRun()
    {
        if (animator.GetFloat(speedHash) < 1 || animator.GetFloat(foodHash) > 0)
        {
            SetSpeed(1);
            SetFood(0);
            StartCoroutine(SetLateTrigger(deathHash));
        }
        else
        {
            animator.SetTrigger(deathHash);
        }
    }

    public void Dance()
    {
        if (animator.GetFloat(speedHash) > 0 || animator.GetFloat(foodHash) > 0)
        {
            SetSpeed(0);
            SetFood(0);
            StartCoroutine(SetLateTrigger(danceHash));
        }
        else
        {
            animator.SetTrigger(danceHash);
        }
    }

    #endregion

    #region SetValues

    // animations

    private void SetSpeed(float speed)
    {
        float startValue = animator.GetFloat(speedHash);
        bool ascending = true;
        if (startValue > speed)
            ascending = false;

        if (startValue != speed)
            StartCoroutine(SetSmoothFloat(speedHash, startValue, speed, ascending));
    }

    private void SetFood(float food)
    {
        float startValue = animator.GetFloat(foodHash);
        bool ascending = true;
        if (startValue > food)
            ascending = false;

        if (startValue != food)
            StartCoroutine(SetSmoothFloat(foodHash, startValue, food, ascending));

        if (food > 0.5)
        {
            GameObject carrot = Instantiate(carrotPrefab, boneForCarrot.transform);
            carrot.transform.parent = boneForCarrot.transform;
        }
        else
        {
            if (boneForCarrot.transform.childCount > 0)
            {
                foreach (Transform child in boneForCarrot.transform)
                {
                    if (child.gameObject)
                        Destroy(child.gameObject);
                }
            }
        }
    }

    private void SetNervosity(float nervosity)
    {
        float startValue = animator.GetFloat(nervosityHash);
        bool ascending = true;
        if (startValue > nervosity)
            ascending = false;

        if (startValue != nervosity)
            StartCoroutine(SetSmoothFloat(nervosityHash, startValue, nervosity, ascending));
    }

    IEnumerator SetSmoothFloat(int hash, float startValue, float endValue, bool ascending)
    {
        if ((startValue < endValue && ascending) || (startValue > endValue && !ascending))
        {
            animator.SetFloat(hash, startValue);
            yield return new WaitForSeconds(0.025f);
            StartCoroutine(SetSmoothFloat(hash, (startValue + ((ascending) ? 0.04f : -0.04f)), endValue, ascending));
        }
        else
        {
            animator.SetFloat(hash, endValue);
        }
    }

    /// <summary>
    /// Waits 1 second and then sets trigger.
    /// Used for waiting for transitions.
    /// </summary>
    /// <param name="hash">Trigger name hash.</param>
    /// <returns>IEnumerator</returns>
    IEnumerator SetLateTrigger(int hash)
    {
        yield return new WaitForSeconds(1);
        animator.SetTrigger(hash);
    }

    // materials

    public void SetRabbit(string rabbit)
    {
        if(rabbit.Equals("smooth"))
        {
            rabbitSmooth.SetActive(true);
            rabbitFlat.SetActive(false);
            smoothActive = true;
        }
        else
        {
            rabbitSmooth.SetActive(false);
            rabbitFlat.SetActive(true);
            smoothActive = false;
        }

        // set animator and bones
        Start();
    }

    public void SetMaterial(string material)
    {
        switch (material)
        {
            case "white":
                if(smoothActive)
                    rabbitSmoothMesh.material = whiteFur;
                else
                    rabbitFlatMesh.material = whiteLowPoly;
                break;
            case "grey":
                if (smoothActive)
                    rabbitSmoothMesh.material = greyFur;
                else
                    rabbitFlatMesh.material = greyLowPoly;
                break;
            case "brown":
                if (smoothActive)
                    rabbitSmoothMesh.material = brownFur;
                else
                    rabbitFlatMesh.material = brownLowPoly;
                break;
        }
    }

    #endregion
}