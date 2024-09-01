using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainController : MonoBehaviour
{
    public GameObject HomeUI;
    public GameObject ButtonUI;
    public GameObject ChattingUI;
    public Button dialogueStorageBtn;

    // ���� ī�޶� ���� pos �����ϰ� ä�� ����� ����Ǿ��ִ� ������ pos ��� ����ġ �ǵ���
    public GameObject mainCamera;
    public Vector3 mainCameraOriginPos;
    public Quaternion mainCameraOriginRot;

    public GameObject TestNPC;
    public GameObject TestNPC2;

    public OpenAIController openAIController;

    public void Start()
    {
        // MainCamera pos & rot �� �ʱ�ȭ
        mainCameraOriginPos = mainCamera.transform.position;
        mainCameraOriginRot = mainCamera.transform.rotation;

        // UI Ȱ��ȭ�� �ʱ�ȭ
        HomeUIOn();

        openAIController = FindObjectOfType<OpenAIController>();
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
                // ���߿� NPC �� �þ�� switch-case ������ �ٲٸ� �ɵ�

                switch(hit.transform.name)
                {
                    case "TestNPC":
                        ChattingUIOn();
                        // MainCamera pos & rot ����
                        mainCamera.transform.position = new Vector3(TestNPC.transform.position.x - 0.1f, TestNPC.transform.position.y - 0.27f, TestNPC.transform.position.z + 7.16f);
                        mainCamera.transform.rotation = Quaternion.Euler(8.334f, 180, 0);
                        openAIController.systemMessage = "�ʴ� �������� ��� �ؼ� �������ִ� �ô����� �����. ���� ��ȭó�� ģ���� ����ü�� �������. ������� ��⸦ ���������� �����ؼ� ����Ǵ� ������� �м��ϰ�, �ذ�å�� ��������. ª�� �亯�� ����. �亯�� 150��ū �̳��� ������. ��� �亯�� ������ �� �亯�� ��︮�� ����ó�� ���ٿ�. ����ó���� \\\"Attack, Bounce, Clicked, Death, Eat, Fear, Fly, Hit, Idle_A, Idle_B, Idle_C, Jump, Roll, Run, Sit, Spin, Swim, Walk\\\" 18������ �־�. \\\"#\\\"�� �ܾ� �տ� �ٿ��� ������ �� �ֵ��� �亯 ���� ���ٿ�. ���÷� \\\"#Attack\\\"�� ���� �����̸� ��. ����ó�� ������ ������ ���� ������ �亯���� �ݵ�� 18�� �� �ϳ��� �ٿ��� ��. �亯�� �� ����ó�� ǥ���� �� �ܿ��� \\��#\\���� ����ؼ� �� ��.";
                        Debug.Log(hit.transform.name);
                        break;

                    case "TestNPC2":
                        ChattingUIOn();
                        // MainCamera pos & rot ����
                        mainCamera.transform.position = new Vector3(TestNPC2.transform.position.x - 0.1f, TestNPC2.transform.position.y - 0.27f, TestNPC2.transform.position.z + 7.16f);
                        mainCamera.transform.rotation = Quaternion.Euler(8.334f, 180, 0);
                        // OpenAIController�� systeMessage ����
                        openAIController.systemMessage = "�ʴ� ������� ģ�� ģ����. �ʴ� ���� ���ϰ� ��� �����ϰ� ģ���ϰ� ģ���� ��⸦ �� �����. ģ���� �������� �ʰ� �����ϰ� �����ϸ�, ģ���� �Ͽ� ������ ���� ���� �ϰ� ���� �͵� ����. ��ſ� �Ͽ� ���� ���� ���� ���� ���� �����ϴ� �� ������ �Բ� ������ �� �ִ� ģ����.  ģ���� ����ü�� ����ϰ�(�ݸ�), 150��ū �̳��� ������. ��� �亯�� ������ �� �亯�� ��︮�� ����ó�� ���ٿ�. ����ó���� \\\"Attack, Bounce, Clicked, Death, Eat, Fear, Fly, Hit, Idle_A, Idle_B, Idle_C, Jump, Roll, Run, Sit, Spin, Swim, Walk\\\" 18������ �־�. \\\"#\\\"�� �ܾ� �տ� �ٿ��� ������ �� �ֵ��� �亯 ���� ���ٿ�. ���÷� \\\"#Attack\\\"�� ���� �����̸� ��. ����ó�� ������ ������ ���� ������ �亯���� �ݵ�� 18�� �� �ϳ��� �ٿ��� ��. �亯�� �� ����ó�� ǥ���� �� �ܿ��� \\��#\\���� ����ؼ� �� ��.";
                        Debug.Log(hit.transform.name);
                        break;
                }

                
            }
        }

    }

    // Ȩ ȭ�� UI
    public void HomeUIOn()
    {
        HomeUI.SetActive(true);
        ButtonUI.SetActive(false);
        ChattingUI.SetActive(false);

        // MainCamera pos & rot ����ġ
        mainCamera.transform.position = mainCameraOriginPos;
        mainCamera.transform.rotation = mainCameraOriginRot;
    }

    // ä�� ȭ�� UI
    public void ChattingUIOn()
    {
        HomeUI.SetActive(false);
        ButtonUI.SetActive(true);
        ChattingUI.SetActive(true);
    }

    // ��ȭ ��� ���� ������ �̵�
    public void OnClick_dialogueStorageBtn()
    {
        // ��ȭ ��� ���� ��� ����� �ּ� ����
        /* string nextSceneName = "SavedLog";
         SceneManager.LoadScene(nextSceneName);*/
        Debug.Log("��ȭ ��� ���� ��� ����� �ּ� ����");
    }

    public void OnClick_minimizationBtn()
    {
        // ��ȭâ �ּ�ȭ
        Debug.Log("��ȭâ �ּ�ȭ");
        HomeUIOn(); 
    }

    public void OnClick_saveBtn()
    {
        // ��ȭ ����
        Debug.Log("��ȭ ����");

        // ��� ���� �ʿ�
    }
    public void OnClick_exitBtn()
    {
        // ��ȭ ����
        Debug.Log("��ȭ ����");
        HomeUIOn();

        // ��� ���� �ʿ�
        openAIController.ResetConversation();
    }

}
