using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitController : MonoBehaviour {

    Animator animator;
   
    // anim hashes
    int speedHash = Animator.StringToHash("speed");
    int foodHash = Animator.StringToHash("food");
    int nervosityHash = Animator.StringToHash("nervosity");
    int watchHash = Animator.StringToHash("watch");
    int deathHash = Animator.StringToHash("death");
    int jumpHash = Animator.StringToHash("jump");
    int danceHash = Animator.StringToHash("dance");

    [Header("Carrot")]
    public GameObject carrotPrefab;
    GameObject boneForCarrot;

    [Header("Rabbit Display")]
    public GameObject rabbitSmooth;
    public GameObject rabbitFlat;

    SkinnedMeshRenderer rabbitSmoothMesh, rabbitFlatMesh;

    public Material whiteFur;
    public Material greyFur;
    public Material brownFur;
    public Material whiteLowPoly;
    public Material greyLowPoly;
    public Material brownLowPoly;

    bool smoothActive = true;

    //TextScript
    //public TextMesh textObject;
    int stage = 0;

    //initial and last positions
    Vector3 lp, fp;
    public GameObject Apple, Peanut, Pear, Peach;
    bool correct = false;
    Vector3 touchLoc;


    void Start () {
        animator = GetComponentInChildren<Animator>();
        // create nicer solution
        boneForCarrot = GameObject.FindGameObjectWithTag("Carrot");

        rabbitSmoothMesh = rabbitSmooth.GetComponentInChildren<SkinnedMeshRenderer>();
        rabbitFlatMesh = rabbitFlat.GetComponentInChildren<SkinnedMeshRenderer>();
    }

    void Update() {

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
                stage++;
                //Jump();
                animator.SetTrigger(jumpHash);
            }
        }
        

        switch (stage)
        {
            case 1:
                //textObject.text = "An ____ a day keeps the doctor away";

                if (Vector3.Distance(touchLoc, Apple.transform.position) <
                    Vector3.Distance(touchLoc, Peanut.transform.position))
                {
                    //interpolate here
                }
                else {  }
                break;
            default:
                break;
        }
    }
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
