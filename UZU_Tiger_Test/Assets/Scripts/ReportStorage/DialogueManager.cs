using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using UnityEditor.AssetImporters;

public class DialogueManager : MonoBehaviour
{
    DialogueScript Dialogue;
    DataService ds;

    public GameObject dialoguePrefab; // Dialogue ������
    public Transform dialogueParent;  // ��ȭ �������� �߰��� �θ� ������Ʈ (UI)

    // �� ���翡 ���� NPC �ʻ�ȭ �̹���
    public Sprite[] npcPortraits; // Sprite �迭�� �ν����Ϳ��� ���� ����

    public bool canMakeReport_Kind = false;
    public bool canMakeReport_Cynical = false;
    public bool canMakeReport_Cognitive = false;

    string npcName;

    private GameObject dialogueInstance; // �߰�: dialogueInstance �ʵ�
    private SessionLog sessionLog; // �߰�: sessionLog �ʵ�

    public Button CRB_Kind;
    public Button CRB_Cynical;
    public Button CRB_Cognitive;

    public Sprite unActiveSprite;

    public void Awake()
    {
        Dialogue = GetComponent<DialogueScript>();
        ds = new DataService("database.db"); // �����ͺ��̽� ����

        Image CRB_KindBtnImg = CRB_Kind.GetComponent<Image>();
        Image CRB_CynicalBtnImg = CRB_Cynical.GetComponent<Image>();
        Image CRB_CognitiveBtnImg = CRB_Cognitive.GetComponent<Image>();

        PlayerPrefs.SetString("NPCName", "ReporterNPC");
        npcName = PlayerPrefs.GetString("NPCName");
        Debug.Log("DialogueManager : " + npcName);

        // DB�� �ִ� ��� ���� ��ȭ ��ົ ������ ��������
        for (int counselorId = 1; counselorId <= 4; counselorId++)
        {
            var sessionLogs = ds.GetSessionLog(counselorId); // �� ���翡 ���� �α� ��������
            Debug.Log($"SessionLogs for Counselor {counselorId}: Count = {sessionLogs.Count()}");

            // ��� SessionLog���� Summary�� ����� ������ ��ȭ ������ ����
            foreach (var log in sessionLogs)
            {
                if (log != null)
                {
                    Debug.Log($"Checking SessionLog Id: {log.Id}, Summary: {log.Summary}");

                    if (!string.IsNullOrWhiteSpace(log.Summary))
                    {
                        createDialogueContent(log); // ������ ��ȭ ��ົ�� ��ȭ ���������� ���
                    }
                    else
                    {
                        Debug.Log($"SessionLog Id {log.Id} has an empty Summary.");
                    }
                }
                else
                {
                    Debug.Log("SessionLog is null");
                }
            }
        }

        // ��ȭ ���� ��ົ�� 5�� �̻����� Ȯ��
        canMakeReport_Kind = ds.HasFiveNotReportedSessionLogs(ds.GetCounselorIdByName("KindNPC"));
        canMakeReport_Cynical = ds.HasFiveNotReportedSessionLogs(ds.GetCounselorIdByName("CynicalNPC"));
        canMakeReport_Cognitive = ds.HasFiveNotReportedSessionLogs(ds.GetCounselorIdByName("CognitiveNPC"));

        Debug.Log("canMakeReport_Kind : " + canMakeReport_Kind);
        Debug.Log("canMakeReport_Cynical : " + canMakeReport_Cynical);
        Debug.Log("canMakeReport_Cognitive : " + canMakeReport_Cognitive);

        if (!canMakeReport_Kind)
            CRB_KindBtnImg.sprite = unActiveSprite;
        else
            CRB_KindBtnImg.sprite = Resources.Load<Sprite>("NPC_P/Circle Button - Pink Rose");

        if (!canMakeReport_Cynical)
            CRB_CynicalBtnImg.sprite = unActiveSprite;
        else
            CRB_CynicalBtnImg.sprite = Resources.Load<Sprite>("NPC_P/Circle Button - Green");

        if (!canMakeReport_Cognitive)
            CRB_CognitiveBtnImg.sprite = unActiveSprite;
        else
            CRB_CognitiveBtnImg.sprite = Resources.Load<Sprite>("NPC_P/Circle Button - Yellow");
    }

    private void Update()
    {
        if (dialogueInstance == null)
        {
            // dialogueInstance�� null�� ��� �α׸� ����ϰ� return
            Debug.Log("dialogueInstance�� null�Դϴ�.");
            return;
        }
    }

    public void createDialogueContent(SessionLog log)
    {


        // ��ȭ ������ ������ ����
        if (string.IsNullOrWhiteSpace(log.Summary)) return;

        // ��ȭ ������ �ν��Ͻ� ����
        dialogueInstance = Instantiate(dialoguePrefab, dialogueParent);
        TMP_Text dialogueText = dialogueInstance.GetComponentInChildren<TMP_Text>();

        if (dialogueText != null)
        {
            dialogueText.text = log.Summary;
        }

        // NPC �ʻ�ȭ �̹��� ����
        Image npcPortrait = dialogueInstance.GetComponentInChildren<DialogueScript>().NPC_portrait;
        if (npcPortrait != null && log.Counselor_id > 0 && log.Counselor_id <= npcPortraits.Length)
        {
            npcPortrait.sprite = npcPortraits[log.Counselor_id - 1];
        }

        // ��ົ ���� ��¥ ����
        dialogueInstance.GetComponentInChildren<DialogueScript>().Dialogue_Date.text = log.Created_at.ToString("yyyy-MM-dd HH:mm:ss");

        // ���� ��ȭ �α� ����
        sessionLog = log;

        // ����Ʈ ��ư ���� (��ȭ ���� 5�� �̻��Ͻ� true, �ƴϸ� false)
        Image CRB_BtnImg = CRB_Kind.GetComponent<Image>();
        CRB_BtnImg.sprite = unActiveSprite;
    }




    public void OnClick_backtoMain()
    {
        SceneManager.LoadScene("Main");
    }
}