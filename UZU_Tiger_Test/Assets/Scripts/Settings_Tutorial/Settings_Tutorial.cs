using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Tutorial_Text : MonoBehaviour
{

    public GameObject text1;
    public GameObject text2;
    public GameObject text3;

    public GameObject btn1;
    public GameObject btn2;
    public GameObject btn3;


    public void Start()
    {
        text1.SetActive(true);
        text2.SetActive(false);
        text3.SetActive(false);

        btn1.SetActive(true);
        btn2.SetActive(false);
        btn3.SetActive(false);

    }



    public void OnClick_nextBtn1()
    {
        text1.SetActive(false);
        text2.SetActive(true);
        text3.SetActive(false);

        btn1.SetActive(false);
        btn2.SetActive(true);
        btn3.SetActive(false);
    }

    public void OnClick_nextBtn2()
    {
        text1.SetActive(false);
        text2.SetActive(false);
        text3.SetActive(true);

        btn1.SetActive(false);
        btn2.SetActive(false);
        btn3.SetActive(true);
    }

    public void OnClick_nextBtn3()
    {
        string nextSceneName = "Main";
        SceneManager.LoadScene(nextSceneName);
    }

}
