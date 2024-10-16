using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using OpenCover.Framework.Model;
using System;
using System.Collections;

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


    #region CognitiveNPC
    public void OnClick_CreateReport_CognitiveNPC()
    {
        // NPC �̸� ����
        PlayerPrefs.SetString("NPCName", "CognitiveNPC");

        // ����Ʈ ���� ��û
        OpenAIController openAIController = FindObjectOfType<OpenAIController>();
        openAIController.SendReportRequestToAI();

        // ����Ʈ ���� �� ó�� ���
        StartCoroutine(WaitForReportResponse_Cognitive());

    }

    private IEnumerator WaitForReportResponse_Cognitive()
    {
        // ���� ��� (�ʿ��� �ð���ŭ ���)
        yield return new WaitForSeconds(1.0f); // �ʿ��� �ð����� ����

        // ����Ʈ ���� �� ó���� �޼��� ȣ��
        OnReportGenerated_Cognitive();
    }

    private void OnReportGenerated_Cognitive()
    {
        // ����Ʈ ������ �Ϸ�� �� �ҷ�����
        var reportLogs = ds.GetReportLog(1);
        ReportLog reportLog = reportLogs.FirstOrDefault();

        if (reportLog != null)
        {
            // ����Ʈ �ؽ�Ʈ ����
            Report_Text.text = reportLog.Content;

            // ����Ʈ ���� ��¥ ����
            Report_Date.text = reportLog.Created_at.ToString("yyyy-MM-dd HH:mm:ss");

            // NPC �̸� ����
            Report_NPCName.text = "����ġ�� ����";

            // NPC �ʻ�ȭ �̹��� ����
            NPC_portrait.sprite = npcPortraits[0];

            // ����Ʈ ���� ȭ�鿡 ���
            ShowReport();
        }

        // �̺�Ʈ ���� ����
        FindObjectOfType<OpenAIController>().OnSumaryResponseReceived -= OnReportGenerated_Cognitive;
    }
    #endregion

    #region KindNPC
    public void OnClick_CreateReport_KindNPC()
    {
        // NPC �̸� ����
        PlayerPrefs.SetString("NPCName", "KindNPC");

        // ����Ʈ ���� ��û
        OpenAIController openAIController = FindObjectOfType<OpenAIController>();
        openAIController.SendReportRequestToAI();

        // ����Ʈ ���� �� ó�� ���
        StartCoroutine(WaitForReportResponse());
    }

    private IEnumerator WaitForReportResponse()
    {
        // ���� ��� (�ʿ��� �ð���ŭ ���)
        yield return new WaitForSeconds(2.0f); // �ʿ��� �ð����� ����

        // ����Ʈ ���� �� ó���� �޼��� ȣ��
        OnReportGenerated_Kind();
    }

    private void OnReportGenerated_Kind()
    {
        // ����Ʈ ������ �Ϸ�� �� �ҷ�����
        var reportLogs = ds.GetReportLog(3);
        ReportLog reportLog = reportLogs.FirstOrDefault();

        if (reportLog != null)
        {
            // ����Ʈ �ؽ�Ʈ ����
            Report_Text.text = reportLog.Content;

            // ����Ʈ ���� ��¥ ����
            Report_Date.text = reportLog.Created_at.ToString("yyyy-MM-dd HH:mm:ss");

            // NPC �̸� ����
            Report_NPCName.text = "����� ����";

            // NPC �ʻ�ȭ �̹��� ����
            NPC_portrait.sprite = npcPortraits[2];

            // ����Ʈ ���� ȭ�鿡 ���
            ShowReport();
        }

        // �̺�Ʈ ���� ����
        FindObjectOfType<OpenAIController>().OnSumaryResponseReceived -= OnReportGenerated_Kind;
    }
    #endregion


    #region Cynical
    public void OnClick_CreateReport_CynicalNPC()
    {
        // NPC �̸� ����
        PlayerPrefs.SetString("NPCName", "CynicalNPC");

        // ����Ʈ ���� ��û
        OpenAIController openAIController = FindObjectOfType<OpenAIController>();
        openAIController.SendReportRequestToAI();

        // ����Ʈ ���� �� ó�� ���
        StartCoroutine(WaitForReportResponse_Cynical());
    }

    private IEnumerator WaitForReportResponse_Cynical()
    {
        // ���� ��� (�ʿ��� �ð���ŭ ���)
        yield return new WaitForSeconds(1.0f); // �ʿ��� �ð����� ����

        // ����Ʈ ���� �� ó���� �޼��� ȣ��
        OnReportGenerated_Cynical();
    }

    private void OnReportGenerated_Cynical()
    {
        // ����Ʈ ������ �Ϸ�� �� �ҷ�����
        var reportLogs = ds.GetReportLog(4);
        ReportLog reportLog = reportLogs.FirstOrDefault();

        if (reportLog != null)
        {
            // ����Ʈ �ؽ�Ʈ ����
            Report_Text.text = reportLog.Content;

            // ����Ʈ ���� ��¥ ����
            Report_Date.text = reportLog.Created_at.ToString("yyyy-MM-dd HH:mm:ss");

            // NPC �̸� ����
            Report_NPCName.text = "�ô����� ����";

            // NPC �ʻ�ȭ �̹��� ����
            NPC_portrait.sprite = npcPortraits[3];

            // ����Ʈ ���� ȭ�鿡 ���
            ShowReport();
        }

        // �̺�Ʈ ���� ����
        FindObjectOfType<OpenAIController>().OnSumaryResponseReceived -= OnReportGenerated_Cynical;
    }
    #endregion

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
