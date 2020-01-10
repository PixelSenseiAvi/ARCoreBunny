using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//using UnityOSC;
using GoogleARCore.Examples.HelloAR;


public class scoreUpdater : MonoBehaviour
{
    private GameObject controller, andyPrefab, bunny;
  //  private HelloARController controller;
    private TextMeshProUGUI textMeshProUGUI;

    private GameObject newGameObj;

    void Start()
    {
        textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        //textMeshProUGUI.SetText("Score: 0");

        controller = GameObject.Find("Example Controller");
        andyPrefab = controller.GetComponent<HelloARController>().AndyPlanePrefab;
      
    }

    // Update is called once per frame
    void Update()
    {
        textMeshProUGUI.text = "Score: "+ andyPrefab.GetComponentInChildren<RabbitController>().score;
    }
}
