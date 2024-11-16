using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System;
using System.Collections;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement;

public class ReportManager : MonoBehaviour
{
    public GameObject Report;
    public Image NPC_portrait;
    public TMP_Text Report_Text;
    public TMP_Text Report_Date;
    public TMP_Text Report_NPCName;
    public Sprite[] npcPortraits;        // �� ���� �ʻ�ȭ �̹���
    public Button showReportBtn;

    private int NPCflag = 0;
    // cog = 1 , kind = 3, cynical = 4

    DataService ds;
    DialogueManager dm;

    private void Awake()
    {
        ds = new DataService("database.db");
        dm = GetComponent<DialogueManager>();
        Report.SetActive(false);
    }

    public void Onclick_ShowCynicalReportBtn()
    {
        OnReportGenerated();
        showReportBtn.gameObject.SetActive(false);
    }

    #region CognitiveNPC
    public void OnClick_CreateReport_CognitiveNPC()
    {
        if (dm.canMakeReport_Cognitive)
        {
            NPCflag = 1;

            // NPC �̸� ����
            Report_NPCName.text = "����ġ�� ����";

            // NPC �ʻ�ȭ �̹��� ����
            NPC_portrait.sprite = npcPortraits[0];

            // ����Ʈ ���� ȭ�鿡 ���
            ShowReport();

            // NPC �̸� ����
            PlayerPrefs.SetString("NPCName", "CognitiveNPC");

            // ����Ʈ ���� ��û
            OpenAIController openAIController = FindObjectOfType<OpenAIController>();
            openAIController.SendReportRequestToAI();
        }
        else
        {
            Debug.Log("����Ʈ ���� �Ұ�");
        }

    }
    #endregion

    #region KindNPC
    public void OnClick_CreateReport_KindNPC()
    {
        if (dm.canMakeReport_Kind)
        {
            NPCflag = 3;

            // NPC �̸� ����
            Report_NPCName.text = "����� ����";

            // NPC �ʻ�ȭ �̹��� ����
            NPC_portrait.sprite = npcPortraits[2];

            // ����Ʈ ���� ȭ�鿡 ���
            ShowReport();

            // NPC �̸� ����
            PlayerPrefs.SetString("NPCName", "KindNPC");

            // ����Ʈ ���� ��û
            OpenAIController openAIController = FindObjectOfType<OpenAIController>();
            openAIController.SendReportRequestToAI();
        }
        else
        {
            Debug.Log("����Ʈ ���� �Ұ�");
        }
        
    }
    #endregion

    #region Cynical
    public void OnClick_CreateReport_CynicalNPC()
    {
        if (dm.canMakeReport_Cynical)
        {
            NPCflag = 4;

            // NPC �̸� ����
            Report_NPCName.text = "�ô����� ����";

            // NPC �ʻ�ȭ �̹��� ����
            NPC_portrait.sprite = npcPortraits[3];

            // ����Ʈ ���� ȭ�鿡 ���
            ShowReport();

            // NPC �̸� ����
            PlayerPrefs.SetString("NPCName", "CynicalNPC");

            // ����Ʈ ���� ��û
            OpenAIController openAIController = FindObjectOfType<OpenAIController>();
            openAIController.SendReportRequestToAI();
        }
        else
        {
            Debug.Log("����Ʈ ���� �Ұ�");
        }
    }

    private void OnReportGenerated()
    {
        var reportLogs = ds.GetReportLog(NPCflag);
        ReportLog reportLog = reportLogs.FirstOrDefault();

        if (reportLog != null)
        {
            // ����Ʈ �ؽ�Ʈ ����
            Report_Text.text = reportLog.Content;

            // ����Ʈ ���� ��¥ ����
            Report_Date.text = reportLog.Created_at.ToString("yyyy-MM-dd HH:mm:ss");

            // ����Ʈ ���� ȭ�鿡 ���
            ShowReport();
        }

    }
    #endregion

    // ����Ʈ �˾� ���
    public void ShowReport()
    {
        Report.SetActive(true);
    }

    // �ݱ� ��ư ���� ����Ʈ �˾� ������
    public void OnClickCloseReportBtn()
    {
        Report.SetActive(false);
        SceneManager.LoadScene("ReportStorage");
    }

}
