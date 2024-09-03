using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainController : MonoBehaviour
{
    public GameObject HomeUI;
    public GameObject ButtonUI;
    public GameObject ChattingUI;
    public Button dialogueStorageBtn;

    // 메인 카메라 기존 pos 저장하고 채팅 종료시 저장되어있던 기존의 pos 대로 원위치 되도록
    public GameObject mainCamera;
    public Vector3 mainCameraOriginPos;
    public Quaternion mainCameraOriginRot;

    public GameObject TestNPC;
    public GameObject TestNPC2;

    public OpenAIController openAIController;

    public void Start()
    {
        // MainCamera pos & rot 값 초기화
        mainCameraOriginPos = mainCamera.transform.position;
        mainCameraOriginRot = mainCamera.transform.rotation;

        // UI 활성화값 초기화
        HomeUIOn();

        openAIController = FindObjectOfType<OpenAIController>();
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
                // 나중에 NPC 수 늘어나면 switch-case 문으로 바꾸면 될듯

                switch(hit.transform.name)
                {
                    case "TestNPC":
                        ChattingUIOn();
                        // MainCamera pos & rot 조정
                        mainCamera.transform.position = new Vector3(TestNPC.transform.position.x - 0.1f, TestNPC.transform.position.y - 0.27f, TestNPC.transform.position.z + 7.16f);
                        mainCamera.transform.rotation = Quaternion.Euler(8.334f, 180, 0);
                        openAIController.systemMessage = "너는 현실적인 사고를 해서 조언해주는 시니컬한 상담사야. 실제 대화처럼 친근한 구어체를 사용해줘. 사용자의 얘기를 현실적으로 생각해서 예상되는 어려움을 분석하고, 해결책을 제안해줘. 짧은 답변을 해줘. 답변은 150토큰 이내로 끝내줘. 모든 답변의 끝에는 그 답변에 어울리는 제스처를 덧붙여. 제스처에는 \\\"Attack, Bounce, Clicked, Death, Eat, Fear, Fly, Hit, Idle_A, Idle_B, Idle_C, Jump, Roll, Run, Sit, Spin, Swim, Walk\\\" 18가지가 있어. \\\"#\\\"를 단어 앞에 붙여서 구분할 수 있도록 답변 끝에 덧붙여. 예시로 \\\"#Attack\\\"과 같이 덧붙이면 돼. 제스처가 없으면 오류가 나기 때문에 답변마다 반드시 18개 중 하나를 붙여야 해. 답변할 때 제스처를 표시할 때 외에는 \\”#\\”을 사용해선 안 돼.";
                        Debug.Log(hit.transform.name);
                        break;

                    case "TestNPC2":
                        ChattingUIOn();
                        // MainCamera pos & rot 조정
                        mainCamera.transform.position = new Vector3(TestNPC2.transform.position.x - 0.1f, TestNPC2.transform.position.y - 0.27f, TestNPC2.transform.position.z + 7.16f);
                        mainCamera.transform.rotation = Quaternion.Euler(8.334f, 180, 0);
                        // OpenAIController의 systeMessage 변경
                        openAIController.systemMessage = "너는 사용자의 친한 친구야. 너는 아주 착하고 밝고 순수하고 친절하고 친구의 얘기를 잘 들어줘. 친구를 비판하지 않고 수용하고 존중하며, 친구의 일에 관심이 많고 같이 하고 싶은 것도 많아. 즐거운 일엔 같이 웃고 슬픈 일은 같이 슬퍼하는 등 감정을 함께 공유할 수 있는 친구야.  친근한 구어체를 사용하고(반말), 150토큰 이내로 끝내줘. 모든 답변의 끝에는 그 답변에 어울리는 제스처를 덧붙여. 제스처에는 \\\"Attack, Bounce, Clicked, Death, Eat, Fear, Fly, Hit, Idle_A, Idle_B, Idle_C, Jump, Roll, Run, Sit, Spin, Swim, Walk\\\" 18가지가 있어. \\\"#\\\"를 단어 앞에 붙여서 구분할 수 있도록 답변 끝에 덧붙여. 예시로 \\\"#Attack\\\"과 같이 덧붙이면 돼. 제스처가 없으면 오류가 나기 때문에 답변마다 반드시 18개 중 하나를 붙여야 해. 답변할 때 제스처를 표시할 때 외에는 \\”#\\”을 사용해선 안 돼.";
                        Debug.Log(hit.transform.name);
                        break;
                }

                
            }
        }

    }

    // 홈 화면 UI
    public void HomeUIOn()
    {
        HomeUI.SetActive(true);
        ButtonUI.SetActive(false);
        ChattingUI.SetActive(false);

        // MainCamera pos & rot 원위치
        mainCamera.transform.position = mainCameraOriginPos;
        mainCamera.transform.rotation = mainCameraOriginRot;
    }

    // 채팅 화면 UI
    public void ChattingUIOn()
    {
        HomeUI.SetActive(false);
        ButtonUI.SetActive(true);
        ChattingUI.SetActive(true);
    }

    // 대화 기록 보관 씬으로 이동
    public void OnClick_dialogueStorageBtn()
    {
        // 대화 기록 보관 기능 생기면 주석 해제
        /* string nextSceneName = "SavedLog";
         SceneManager.LoadScene(nextSceneName);*/
        Debug.Log("대화 기록 보관 기능 생기면 주석 해제");
    }

    public void OnClick_minimizationBtn()
    {
        // 대화창 최소화
        Debug.Log("대화창 최소화");
        HomeUIOn(); 
    }

    public void OnClick_saveBtn()
    {
        // 대화 저장
        Debug.Log("대화 저장");

        // 기능 구현 필요
    }
    public void OnClick_exitBtn()
    {
        // 대화 종료
        Debug.Log("대화 종료");
        HomeUIOn();

        // 기능 구현 필요
        openAIController.ResetConversation();
    }

}
