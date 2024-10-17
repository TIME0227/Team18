using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReportScript : MonoBehaviour
{
    public Image Bg;
    public Image NPC_portrait;
    public TMP_Text Report_Text;
    public Button reportCloseBtn;

    public GameObject Report;

    public void OnClickCloseReportBtn()
    {
        Report.SetActive(false);
    }

}
