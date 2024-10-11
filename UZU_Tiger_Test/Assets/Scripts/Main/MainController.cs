using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainController : MonoBehaviour
{
    public Button dialogueStorageBtn;

    public  GameObject KindNPCInfo;
    public GameObject CynicalNPCInfo;
    public GameObject WDEPNPCInfo;
    public GameObject CognitiveNPCInfo;

    public void Start()
    {
        disableNPCInfo();
    }

    public void Update()
    {
        // �ش� NPC ������Ʈ Ŭ�� ��
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            { 

                switch(hit.transform.name)
                {
                    case "KindNPC":
                        PlayerPrefs.SetString("NPCName", "KindNPC");
                        showNPCInfo(hit.transform.name);
                        // Debug.Log(hit.transform.name);
                        break;

                    case "CynicalNPC":
                        PlayerPrefs.SetString("NPCName", "CynicalNPC");
                        showNPCInfo(hit.transform.name);
                        // Debug.Log(hit.transform.name);
                        break;

                    case "WDEPNPC":
                        PlayerPrefs.SetString("NPCName", "WDEPNPC");
                        showNPCInfo(hit.transform.name);
                        // Debug.Log(hit.transform.name);
                        break;

                    case "CognitiveNPC":
                        PlayerPrefs.SetString("NPCName", "CognitiveNPC");
                        showNPCInfo(hit.transform.name);
                        // Debug.Log(hit.transform.name);
                        break;
                }

                
            }
        }

    }

    public void showNPCInfo(string NPCName)
    {

        // Debug.Log("showNPCInfo : "+ NPCName);

        switch (NPCName)
        {
            case "KindNPC":
                KindNPCInfo.SetActive(true);
                break;

            case "CynicalNPC":
                CynicalNPCInfo.SetActive(true);
                break;

            case "WDEPNPC":
                WDEPNPCInfo.SetActive(true);
                break;

            case "CognitiveNPC":
                CognitiveNPCInfo.SetActive(true);
                break;
        }

    }

    public void OnClick_StartChatBtn()
    {

        // Debug.Log("startchat : " + PlayerPrefs.GetString("NPCName"));

        switch (PlayerPrefs.GetString("NPCName"))
        {
            case "KindNPC":
                SceneManager.LoadScene("KindNPC_Chat");
                break;

            case "CynicalNPC":
                SceneManager.LoadScene("CynicalNPC_Chat");
                break;

            case "WDEPNPC":
                SceneManager.LoadScene("WDEPNPC_Chat");
                break;

            case "CognitiveNPC":
                SceneManager.LoadScene("CognitiveNPC_Chat");
                break;
        }

    }

    public void OnClick_CloseNPCInfo()
    {
        // Debug.Log("�ݱ� ��ư Ŭ����");

        disableNPCInfo();
    }

    public void disableNPCInfo()
    {
        KindNPCInfo.SetActive(false);
        CynicalNPCInfo.SetActive(false);
        WDEPNPCInfo.SetActive(false);
        CognitiveNPCInfo.SetActive(false);
        // Debug.Log("NPC ����â ������");
    }


    // ��ȭ ��� ���� ������ �̵�
    public void OnClick_reportStorageBtn()
    {
        SceneManager.LoadScene("ReportStorage");
        // Debug.Log("��ȭ ��� ���� ��� ����� �ּ� ����");
    }

}
