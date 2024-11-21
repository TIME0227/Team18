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

    public GameObject NewReportAlert;
    DataService ds;
    bool canMakeReport_Kind = false;
    bool canMakeReport_Cynical = false;
    bool canMakeReport_Cognitive = false;

    public void Awake()
    {
        ds = new DataService("database.db"); // 데이터베이스 연결
    }


    public void Start()
    {
        disableNPCInfo();


        // 대화 내용 요약본이 5개 이상인지 확인
        canMakeReport_Kind = ds.HasFiveNotReportedSessionLogs(ds.GetCounselorIdByName("KindNPC"));
        canMakeReport_Cynical = ds.HasFiveNotReportedSessionLogs(ds.GetCounselorIdByName("CynicalNPC"));
        canMakeReport_Cognitive = ds.HasFiveNotReportedSessionLogs(ds.GetCounselorIdByName("CognitiveNPC"));

        UpdateNewReportAlert();
    }

    public void Update()
    {
        // 해당 NPC 오브젝트 클릭 시
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
                        PlayerPrefs.SetString("NPCName", "StrengthNPC");
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

            case "StrengthNPC":
                SceneManager.LoadScene("WDEPNPC_Chat");
                break;

            case "CognitiveNPC":
                SceneManager.LoadScene("CognitiveNPC_Chat");
                break;
        }

    }

    public void OnClick_CloseNPCInfo()
    {
        // Debug.Log("닫기 버튼 클릭됨");

        disableNPCInfo();
    }

    public void disableNPCInfo()
    {
        KindNPCInfo.SetActive(false);
        CynicalNPCInfo.SetActive(false);
        WDEPNPCInfo.SetActive(false);
        CognitiveNPCInfo.SetActive(false);
        // Debug.Log("NPC 정보창 내리기");
    }


    // 대화 기록 보관 씬으로 이동
    public void OnClick_reportStorageBtn()
    {
        PlayerPrefs.SetString("NPCName", "ReporterNPC"); // 기록보관 씬에서는 채팅기능 실행되지 않게...

        SceneManager.LoadScene("ReportStorage");
        // Debug.Log("대화 기록 보관 기능 생기면 주석 해제");
    }

    public void UpdateNewReportAlert()
    {
        if (canMakeReport_Kind || canMakeReport_Cynical || canMakeReport_Cognitive)
        {
            NewReportAlert.SetActive(true);
            //Debug.Log("새로운 리포트 알림 활성화");
        }
        else
        {
            NewReportAlert.SetActive(false);
            //Debug.Log("새로운 리포트 알림 비활성화");
        }
    }

    // 유저정보 수정(튜토리얼) 씬으로 이동
    public void OnClick_userSettingsBtn()
    {
        SceneManager.LoadScene("Settings_Tutorial");
    }
}
