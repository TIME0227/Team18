using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using OpenCover.Framework.Model;
using System;

public class ReportManager : MonoBehaviour
{
    public GameObject Report;
    public Image NPC_portrait;
    public TMP_Text Report_Text;
    public TMP_Text Report_Date;
    public TMP_Text Report_NPCName;
    public Sprite[] npcPortraits;        // �� ���� �ʻ�ȭ �̹���

    DataService ds;

    private void Awake()
    {
        ds = new DataService("database.db");
        Report.SetActive(false);
    }

    // ����Ʈ ���� & ���� ��û �޼���

    public void OnClick_CreateReport_CognitiveNPC()
    {
        // �� NPCname�� Ȥ�� counsler_id�� ���� ���ô޸��ϱ�
        // ������ �׽�Ʈ���̶� CognitiveNPC�θ�
        PlayerPrefs.SetString("NPCName", "CognitiveNPC");

        // ����Ʈ ���� �� ���� ��û
        FindObjectOfType<OpenAIController>().SendReportRequestToAI();

        // ������ ����Ʈ �ҷ�����
        var reportLogs = ds.GetReportLog(1);


        // �� ù��° ����Ʈ�ε�?
        ReportLog reportLog = reportLogs.FirstOrDefault();


        // ����Ʈ �ؽ�Ʈ ����
        Report_Text.text = reportLog.Content;

        // NPC �̸� ����
        Report_NPCName.text = "����ġ�� ����";

        // NPC �ʻ�ȭ �̹��� ����
        Image npcPortrait = NPC_portrait;
        npcPortrait.sprite = npcPortraits[0];


        // ����Ʈ ���� ȭ�鿡 ���
        ShowReport();

    }

    public void OnClick_CreateReport_KindNPC()
    {
        // �� NPCname ���� ���� �޸��ϱ�
        PlayerPrefs.SetString("NPCName", "KindNPC");

        // ����Ʈ ���� �� ���� ��û
        FindObjectOfType<OpenAIController>().SendReportRequestToAI();

        // ������ ����Ʈ �ҷ�����
        var reportLogs = ds.GetReportLog(3);

        // �� ù��° ����Ʈ�ε�?
        ReportLog reportLog = reportLogs.FirstOrDefault();

        foreach (var log in ds.GetReportLog(3))
        {
            Debug.Log(log.ToString());
            int id = log.Id;
            string report_content = log.Content;
            string summary = log.Summary;
            DateTime created_at = log.Created_at;
        }

        // ����Ʈ �ؽ�Ʈ ����
        Report_Text.text = reportLog.Content;

        // ����Ʈ ���� ��¥ ����
        Report_Date.text = reportLog.Created_at.ToString("yyyy-MM-dd HH:mm:ss");

        // NPC �̸� ����
        Report_NPCName.text = "����� ����";


        // NPC �ʻ�ȭ �̹��� ����
        Image npcPortrait = NPC_portrait;
        npcPortrait.sprite = npcPortraits[2];


        // ����Ʈ ���� ȭ�鿡 ���
        ShowReport();

    }

    public void OnClick_CreateReport_CynicalNPC()
    {
        // �� NPCname ���� ���� �޸��ϱ�
        PlayerPrefs.SetString("NPCName", "CynicalNPC");

        // ����Ʈ ���� �� ���� ��û
        FindObjectOfType<OpenAIController>().SendReportRequestToAI();

        // ������ ����Ʈ �ҷ�����
        var reportLogs = ds.GetReportLog(4);

/*        // ����Ʈ�� ���� ��쿡 ���� ���� ó��
        if (reportLogs == null || !reportLogs.Any())
        {
            Debug.LogError("Report logs are empty or null.");
            return;  // ����Ʈ�� ������ ���� �ߴ�
        }*/

        // �� ù��° ����Ʈ�ε�?
        ReportLog reportLog = reportLogs.FirstOrDefault();

        /*        // ����Ʈ�� null�� ��� ó��
                if (reportLog == null)
                {
                    Debug.LogError("No report log found.");
                    return;  // ����Ʈ�� ������ ���� �ߴ�
                }*/

        // ����Ʈ �ؽ�Ʈ ����
        Report_Text.text = reportLog.Content;

        // NPC �̸� ����
        Report_NPCName.text = "�ô����� ����";

        // NPC �ʻ�ȭ �̹��� ����
        Image npcPortrait = NPC_portrait;
        npcPortrait.sprite = npcPortraits[3];


        // ����Ʈ ���� ȭ�鿡 ���
        ShowReport();

    }

    public void OnClick_TestReport()
    {
        ShowReport();
    }

    // ����Ʈ �˾� ���
    public void ShowReport()
    {
        Report.SetActive(true);
    }

    // �ݱ� ��ư ���� ����Ʈ �˾� ������
    public void OnClickCloseReportBtn()
    {
        Report.SetActive(false);
    }

}
