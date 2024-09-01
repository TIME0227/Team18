using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Tutorial_Text : MonoBehaviour
{

    public Button nextBtn;

    public void OnClick_nextBtn()
    {
        string nextSceneName = "Main";
        SceneManager.LoadScene(nextSceneName);
    }

}
