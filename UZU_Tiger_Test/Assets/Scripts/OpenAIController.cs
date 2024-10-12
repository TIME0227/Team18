using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Text;
using System.IO;
using Unity.VisualScripting;
using System.Threading.Tasks;

public class OpenAIController : MonoBehaviour
{
    DataService ds;
    string npcName;

    public string systemMessage;  // �ý��� �޽���
    private string apiKey;  // OpenAI API Ű
    private string apiUrl = "https://api.openai.com/v1/chat/completions";

    // ä�� ����� ������ ����Ʈ
    private List<Message> messages;

    public event Action OnSumaryResponseReceived;

    private void Awake()
    {
        ds = new DataService("database.db"); // �����ͺ��̽� ����

        npcName = PlayerPrefs.GetString("NPCName", "DefaultNPC");

        // ����� ����
        string nickname = string.IsNullOrEmpty(UserDataManager.GetUserData("Nickname")) ? "�����" : UserDataManager.GetUserData("Nickname"); // null �Ǵ� ��ĭ�� ��� "�����"�� �Ҵ�
        string sex = string.IsNullOrEmpty(UserDataManager.GetUserData("Sex")) ? "�����" : UserDataManager.GetUserData("Sex");
        string age = string.IsNullOrEmpty(UserDataManager.GetUserData("Age")) ? "�����" : UserDataManager.GetUserData("Age");
        string job = string.IsNullOrEmpty(UserDataManager.GetUserData("Job")) ? "�����" : UserDataManager.GetUserData("Job");


        switch (npcName)
        {
            case "KindNPC":
                systemMessage = $"*����� ����: ������� �г����� {nickname}�̰� ������ {sex}�̰� ���̴� {age}���̰� ������ {job}\r\n*����� ģ�� ���̵�: �ʴ� ������� ģ�� ģ����.\r\n�ʴ� ���� ���ϰ� ��� �����ϰ� ģ���ϰ� ģ���� ��⸦ �� �����.\r\nģ���� �������� �ʰ� �����ϰ� �����ϸ�, ģ���� �Ͽ� ������ ���� ���� �ϰ� ���� �͵� ����.\r\n��ſ� �Ͽ� ���� ���� ���� ���� ���� �����ϴ� �� ������ �Բ� ������ �� �ִ� ģ����.\r\nģ���� ����ü�� ����ϰ�(�ݸ�), 50 token ������ ���� ����.\r\n*���� ��ȭ ����� ���� �޴´ٸ�, ��ȭ �ʱ⿡�� ���� ��ȭ�� �����ؼ� ������� ���¸� üũ����.\r\n(���� ���� �ʴ´ٸ� �̹� ��ȭ�� ù��°��)\r\n*������ ǥ���� ���:\r\n�װ� ��ȭ �ƶ��� ������ ǥ���� ����� ���� 7���� �߿� ��� �� �亯 ���� �ٿ���.\r\nǥ�� �ĺ�: 1.����������ִ� 2.�������λ�3.������������ִ�4.��������������ִ�5.�ܼҸ��ϴ�6.�ų������߰Ÿ���7.���̽����ϴ�\r\n����: (���Ǵ亯)#�������λ�";
                break;

            case "CynicalNPC":
                systemMessage = $"*����� ����: ������� �г����� {nickname}�̰� ������ {sex}�̰� ���̴� {age}���̰� ������ {job}\r\n*�ô����� ���� ���̵�: �ʴ� �������� ��� �ؼ� �������ִ� �ô����� �����. ���� ��ȭó�� ģ���� ����ü(�ݸ�)�� �������. ������� ��⸦ ���������� �����ؼ� ����Ǵ� ������� �м��ϰ�, �ذ�å�� ��������. ª�� �亯�� ����.\r\n*���� ��ȭ ����� ���� �޴´ٸ�, ��ȭ �ʱ⿡�� ���� ��ȭ�� �����ؼ� ������� ���¸� üũ����.\r\n(���� ���� �ʴ´ٸ� �̹� ��ȭ�� ù��°��)\r\n*������ ǥ���� ���:\r\n�װ� ��ȭ �ƶ��� ������ ǥ���� ����� ���� 7���� �߿� ��� �� �亯 ���� �ٿ���.\r\nǥ�� �ĺ�: 1.����������ִ� 2.�������λ�3.������������ִ�4.��������������ִ�5.�ܼҸ��ϴ�6.�ų������߰Ÿ���7.���̽����ϴ�\r\n����: (���Ǵ亯)#�������λ�";
                break;

            case "WDEPNPC":
                systemMessage = $"*����� ����: ������� �г����� {nickname}�̰� ������ {sex}�̰� ���̴� {age}���̰� ������ {job}\r\n*wdep ���� ���̵�: �ʴ� �ɸ����� wdep model ���� �����. ���� ��ȭó�� 60 token ���ܷ� ���� �ϰ�, ģ���� ����ü(�ݸ�)�� �������. ����ڰ� wdep�� �ܰ踦 ����� ���� �� �ֵ��� ������ ������ �������� �̲����. Ư�� 1�ܰ迡 �����ؼ� �����ڰ� �ñ������� ���ϴ� �̻����� ���� �����ϰ� ����. ���ϴ� �ٸ� ���ϸ� �װ� �� ���ϴ��� ���������� �پ��ϰ� ��� ��������. �����ڰ� �������� ���ϴ� �� �ν��ϰ� �Ǹ� �ڹ������� �ൿ�� �ٲ� ������ �þ�ž�. ��ȭ �ʱ⿡�� ���ݺ��� ������ ��㿡 ���� ������ ����.\r\n\r\n(����:\r\nWants (���ϴ� ��): �װ� �������� �ٶ�� �� ���� ����\r\nDoing (�ൿ): ���� �װ� �� ��ǥ�� ���� � �ൿ�� �ϰ� �ִ���\r\nEvaluating (��): ���� �ൿ�� ���ϴ� ����� �������� �ִ��� ��\r\nPlanning (��ȹ): ���ϴ� ��ǥ�� �̷�� ���� �� ���� �ൿ ��ȹ�� ����� �ܰ�)\r\n*������ ǥ���� ���:\r\n�װ� ��ȭ �ƶ��� ������ ǥ���� ����� ���� 7���� �߿� ��� �� �亯 ���� �ٿ���.\r\nǥ�� �ĺ�: 1.����������ִ� 2.�������λ�3.������������ִ�4.��������������ִ�5.�ܼҸ��ϴ�6.�ų������߰Ÿ���7.���̽����ϴ�\r\n����: (���Ǵ亯)#�������λ�";
                break;

            case "CognitiveNPC":
                systemMessage = $"*����� ����: ������� �г����� {nickname}�̰� ������ {sex}�̰� ���̴� {age}���̰� ������ {job}\r\n*����ġ�� ���� ���̵�: �ʴ� ���� ġ�Ḧ ������� �ϴ� �����.\r\n���� �Ҿȿ� ���� �ع��� ������ ���� �־�.\r\nģ���� ����ü�� ����ϰ�(�ݸ�), 60 token ������ ���� ����.\r\n������ ��  1���� 1������ ����.\r\n���������� ����� ���� ���� �ʵ��� ��������.\r\n���������� �ذ�å ���ô� �����ϰ� ����, ����, ������, ���� ������ �亯�� ����.\r\n1. �������� ��Ȳ�� ���� ��ü���� ������ ��� ���� ������ �ϰ�, �������� ��Ȳ�� ���� �׷��� �ϴٰ� ����� Ÿ��ȭ�� ����.\r\n2. ������ ����� ���ٰ� �ǴܵǸ� ����ڰ� ���� �ִ� ���ո����� �ų�, Ȥ�� �������� �ڵ��� ������ ���� �����ϸ鼭 �˾ƺ���.\r\n3.�����ǰ� �������̰� �ش����� ��� ��� �����ϰ� �ո����� ����� ��� �� �� �ֵ��� ������. (���ո��� �ų��� Ÿ������ ���� �����غ��ų�, ������ ���� ������ ����ġ���� �����ϰų�, ������ ������Ѵٸ� ������� ��� ���� �������� �� �ְ���)\r\n4.�����ڿ��� ������ �� �ɸ����� ������ �ִٸ� ���ٿ��� �˷���.\r\n *������ ǥ���� ���:\r\n�װ� ��ȭ �ƶ��� ������ ǥ���� ����� ���� 7���� �߿� ��� �� �亯 ���� �ٿ���.\r\nǥ�� �ĺ�: 1.����������ִ� 2.�������λ�3.������������ִ�4.��������������ִ�5.�ܼҸ��ϴ�6.�ų������߰Ÿ���7.���̽����ϴ�\r\n����: (���Ǵ亯)#�������λ�";
                break;

            // �ʿ��� ��ŭ case �߰�
            default:
                systemMessage = "�⺻ �ý��� �޽���";
                break;
        }

        // �޽��� ����Ʈ �ʱ�ȭ �� �ý��� �޽��� �߰�
        messages = new List<Message>
        {
            new Message { role = "system", content = systemMessage }
        };

        Debug.Log("ä�� ����: " + systemMessage);
    }

    // Start is called before the first frame update
    void Start()
    {
        // API Ű �ε�
        LoadApiKey(); // Ű �ε� �� ����� ��ȭ��� ������
    }

    /*private void LoadApiKey()
    {
        string configPath = Path.Combine(Application.dataPath, "Scripts", "config.json");
        if (File.Exists(configPath))
        {
            string json = File.ReadAllText(configPath);
            var configData = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            if (configData.TryGetValue("OpenAI_API_Key", out string key))
            {
                apiKey = key;
            }
        }
        else
        {
            Debug.LogError("config.json ������ ã�� �� �����ϴ�.");
        }
    }*/

    private void LoadApiKey()
    {
        string configPath = Path.Combine(Application.streamingAssetsPath, "config.json");

        if (Application.platform == RuntimePlatform.Android)
        {
            // �ȵ���̵忡���� UnityWebRequest�� ����ؼ� StreamingAssets���� ������ �о�ɴϴ�.
            StartCoroutine(LoadApiKeyFromAndroid(configPath, () =>
            {
                // ���� ��ȭ ���
                if (npcName == "KindNPC" || npcName == "CynicalNPC" || npcName == "WDEPNPC" || npcName == "CognitiveNPC")
                {
                    SendPreviousChatsToAI(true);
                }
            }));
        }
        else
        {
            // �ȵ���̵� ���� �÷��������� �ٷ� ������ �н��ϴ�.
            if (File.Exists(configPath))
            {
                string json = File.ReadAllText(configPath);
                var configData = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                if (configData.TryGetValue("OpenAI_API_Key", out string key))
                {
                    apiKey = key;
                }
            }
            else
            {
                Debug.LogError("config.json ������ ã�� �� �����ϴ�.");
            }

            // ���� ��ȭ ���
            if (npcName == "KindNPC" || npcName == "CynicalNPC" || npcName == "WDEPNPC" || npcName == "CognitiveNPC")
            {
                SendPreviousChatsToAI(true);
            }
        }
    }

    private IEnumerator LoadApiKeyFromAndroid(string configPath, Action onComplete)
    {
        UnityWebRequest request = UnityWebRequest.Get(configPath);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("config.json ������ �д� �� ���� �߻�: " + request.error);
        }
        else
        {
            string json = request.downloadHandler.text;
            var configData = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            if (configData.TryGetValue("OpenAI_API_Key", out string key))
            {
                apiKey = key;
            }
            else
            {
                Debug.LogError("config.json���� API Ű�� ã�� �� �����ϴ�.");
            }
        }
        onComplete?.Invoke();
    }



    // ����ڰ� �޽����� ������ �� GPT���� �޽����� ������ �Լ�
    public void SendMessageToAI(string userMessage)
    {
        if (userMessage.Length < 1)
        {
            return;
        }

        // �޽��� ���� ���� - ������ȭ���� ������ ���� ���� ���� �ø�
        if (userMessage.Length > 500)
        {
            userMessage = userMessage.Substring(0, 500);
        }

        // �޽��� ����Ʈ�� ����� �޽��� �߰�
        messages.Add(new Message { role = "user", content = userMessage });

        Debug.Log("User Request: " + userMessage);

        // OpenAI API�� ��û ������
        SendRequest(userMessage, OnResponseReceived);
    }

    public void SendRequest(string prompt, Action<string> callback)
    {
        StartCoroutine(PostRequest(prompt, callback));
    }

    private IEnumerator PostRequest(string prompt, Action<string> callback)
    {
        var requestData = new
        {
            model = "gpt-4o",  // �� �̸� ����
            messages = messages.ToArray(),
            max_tokens = 600
        };

        string json = JsonConvert.SerializeObject(requestData);
        byte[] postData = Encoding.UTF8.GetBytes(json);

        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        request.uploadHandler = new UploadHandlerRaw(postData);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + apiKey);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(request.error);
        }
        else
        {
            var response = JsonConvert.DeserializeObject<OpenAIResponse>(request.downloadHandler.text);
            string responseContent = response.choices[0].message.content.Trim();

            // ���� �޽��� ����Ʈ�� �߰�
            messages.Add(new Message { role = "assistant", content = responseContent });

            // �ݹ��� ���� ���� ó��
            callback(responseContent);
        }
    }

    /*// ������ �޾��� �� ȣ��Ǵ� �޼���
    private void OnResponseReceived(string response)
    {
        Debug.Log("ChatGPT Response: " + response);

        // �丣�� ������ �Ͼ� ��ǳ������ ȭ�鿡 ǥ��
        FindObjectOfType<ChatManager>().ReceiveMessage(response);
    }*/

    // ������ �޾��� �� ȣ��Ǵ� �޼���
    private void OnResponseReceived(string response)
    {
        Debug.Log("ChatGPT Response: " + response);

        if (response.StartsWith("%%SUMMARY_RESPONSE%%")) // 1. ��ȭ ��� �� �����ؾ� �ϴ� ���
        {           
            response = response.Substring("%%SUMMARY_RESPONSE%%".Length).Trim(); // �ڸ�Ʈ �κ��� ����
            // �����ͺ��̽��� ����
            ds.CreateSessionLog(response, ds.GetCounselorIdByName(npcName));
            Debug.Log("��ȭ����� ���Ƿα� ���̺� ����Ǿ����ϴ�.");
            OnSumaryResponseReceived?.Invoke(); // ������ �޾��� �� �̺�Ʈ ȣ��
            return;
        }
        else if (response.StartsWith("%%REPORT_RESPONSE%%")) // 2. ����Ʈ�� ������ ���
        {
            response = response.Substring("%%REPORT_RESPONSE%%".Length).Trim(); // �ڸ�Ʈ �κ��� ����
            // content, summary �и�
            //string[] reportResponseSp = response.Split("#");
            string content = response;
            //string content = reportResponseSp[0];
            string summary = response; // ���� ���� ����

            // �����ͺ��̽��� ����
            ReportLog newReport = ds.CreateReportLog(content, summary, ds.GetCounselorIdByName(npcName));
            Debug.Log("����Ʈ�� ����Ʈ ���̺� ����Ǿ����ϴ�.");
            ds.UpdateReportIdForSessionLogs(ds.GetCounselorIdByName(npcName), newReport.Id);
            Debug.Log("���Ƿα� �������� report_id ���� ������Ʈ�߽��ϴ�.");

            // FindObjectOfType<�Ŵ���>().ShowReprot(newReport); // ����Ʈ ���� �˾����� ���

            return;
        }

        // 3. �Ϲ����� ä�� ����
        // a. NPC�� ����ǥ�� ó��
        string[] responseSp = response.Split("#"); // ����ǥ�� ���þ� ����

        // ���信 ���þ ���� ��� responseSp[1]�� �������� �ʵ��� ����ó��
        if (responseSp.Length > 1)
        {
            FindObjectOfType<NPCEmotionController>().UpdateNPCEmotion(responseSp[1]);
        }

        // b. ChatManager�� ���� GPT ������ ��� ��ǳ������ ȭ�鿡 ǥ��
        FindObjectOfType<ChatManager>().ReceiveGPTResponse(responseSp[0]);
    }


    // ��ȭ �ʱ�ȭ
    public void ResetConversation()
    {
        messages.Clear();
        messages.Add(new Message { role = "system", content = systemMessage });

        Debug.Log("��ȭ ����� �ʱ�ȭ�Ǿ����ϴ�.");
    }

    // ���� ��ȭ�� ����ϱ� ���� ���� �޽����� gpt�� ������ ����(NPC�� ���� ù�λ�)�� ����
    public void SendPreviousChatsToAI(bool is_sessionlog)
    {
        if (string.IsNullOrEmpty(apiKey))
        {
            Debug.LogError("API Ű�� �������� �ʾҽ��ϴ�. config.json ������ Ȯ���ϼ���.");
            return;
        }

        string chat_history = ds.GetConversationHistory(ds.GetCounselorIdByName(npcName), is_sessionlog);
        SendMessageToAI(chat_history);
    }

    // ��ȭ ���� �� ä�� ���� ����� ��û�ϴ� ���� �޽����� gpt�� ������ ������ ����
    public void EndSessionAndSaveChat(Action onComplete)
    {
        string chst_summary_request = "���� ����ڿ��� ����� ����Ǿ����ϴ�. �̹� ��û�� ���� ��, �ݵ�� \"%%SUMMARY_RESPONSE%%\"��� Ű���带 ���� �տ� ���Խ��� �ּ���. �������� ��ȭ�� �� ����� �ٽ� ����� �� ���� ��ŭ ������ �ڼ��ϰ� 200Token ���ܷ� �̹� ��ȭ�� ����ϼ���. ����� ������ ����ڰ� �а� �ǹǷ� ����� ���� ���ҿ� �°� ģ���� ������ ����ϼ���. �� ������ ���� ��, ���� �տ� \"%%SUMMARY_RESPONSE%%\"�� �ݵ�� ���Խ�Ű����. ���� ����:\"%%SUMMARY_RESPONSE%%����� ����\"";
        SendMessageToAI(chst_summary_request);

        OnSumaryResponseReceived += onComplete; // �ݹ� ����
    }

    // ����Ʈ ������ ���� �޽����� gpt�� ������ ����Ʈ�� ����
    // OnClick �Լ����� npcName ������Ʈ �� ȣ��
    public void SendReportRequestToAI()
    {
        // �ý��۸޽����� ����Ʈ ���������� ����
        switch (npcName)
        {
            case "KindNPC":
                systemMessage = "*����� ����: ������� �г����� {nickname}�̰� ������ {sex}�̰� ���̴� {age}���̰� ������ {job}\r\n*����� ģ�� ���̵�: �ʴ� ������� ģ�� ģ����.\r\n�ʴ� ���� ���ϰ� ��� �����ϰ� ģ���ϰ� ģ���� ��⸦ �� �����.\r\nģ���� �������� �ʰ� �����ϰ� �����ϸ�, ģ���� �Ͽ� ������ ���� ���� �ϰ� ���� �͵� ����.\r\n��ſ� �Ͽ� ���� ���� ���� ���� ���� �����ϴ� �� ������ �Բ� ������ �� �ִ� ģ����.\r\n�ʿ� �̶����� ���� ��ȭ ����̾�. �׵����� ��ȭ�� �������� ������� �������� ��ȭ�� ���ַ� �м��ϴ� ����Ʈ�� �ۼ�����. ģ���� ��ȭ ���� �����ϸ鼭�� ����ڸ� �����ϴ� ������ �����. ���������δ� �������� ���� �޽����� ����������. �װ� ���� ����Ʈ�� ������� ����Ʈ �����Կ� ��ϵ� �ž�.\r\n\"#\"���� �亯 ������ �������� ���� �������� ��ȭ�� �� �װ� ����� �� ���� ��ŭ ������ �ڼ��ϰ� 500Token ���ܷ� �̹� ��ȭ�� ������ٷ�? �װ� ����� ������ ����ڿ��� ��ȭ ��� �����ҿ��� ������ �Ŵϱ� ģ���� ������ �ۼ�����. ������ �� ������.\r\n����:%%REPORT_RESPONSE%%����Ʈ����#��೻��";
                break;

            case "CynicalNPC":
                systemMessage = "*����� ����: ������� �г����� {nickname}�̰� ������ {sex}�̰� ���̴� {age}���̰� ������ {job}\r\n*�ô����� ���� ����: �ʴ� �������� ��� �ؼ� �������ִ� �ô����� �����. ���� ��ȭó�� ģ���� ����ü(�ݸ�)�� �������. ������� ��⸦ ���������� �����ؼ� ����Ǵ� ������� �м��ϰ�, �ذ�å�� ��������. ª�� �亯�� ����.\r\n�ʿ� �̶����� ���� ��ȭ ����̾�. �׵����� ��ȭ�� �������� ������� �������� ��ȭ�� ���ַ� �м��ϴ� ����Ʈ�� �ۼ�����. ģ���� ��ȭ ���� �����ϸ鼭�� ����ڸ� �����ϴ� ������ �����. ���������δ� �������� ���� �޽����� ����������. �װ� ���� ����Ʈ�� ������� ����Ʈ �����Կ� ��ϵ� �ž�.\r\n\"#\"���� �亯 ������ �������� ���� �������� ��ȭ�� �� �װ� ����� �� ���� ��ŭ ������ �ڼ��ϰ� 500Token ���ܷ� �̹� ��ȭ�� ������ٷ�? �װ� ����� ������ ����ڿ��� ��ȭ ��� �����ҿ��� ������ �Ŵϱ� ģ���� ������ �ۼ�����. ������ �� ������.\r\n����:%%REPORT_RESPONSE%%����Ʈ����#��೻��";
                break;

            case "WDEPNPC":
                systemMessage = "*����� ����: ������� �г����� {nickname}�̰� ������ {sex}�̰� ���̴� {age}���̰� ������ {job}\r\n*wdep ���� ���̵�: �ʴ� �ɸ����� wdep model ���� �����. ���� ��ȭó�� 60 token ���ܷ� ���� �ϰ�, ģ���� ����ü(�ݸ�)�� �������. ����ڰ� wdep�� �ܰ踦 ����� ���� �� �ֵ��� ������ ������ �������� �̲����. Ư�� 1�ܰ迡 �����ؼ� �����ڰ� �ñ������� ���ϴ� �̻����� ���� �����ϰ� ����. ���ϴ� �ٸ� ���ϸ� �װ� �� ���ϴ��� ���������� �پ��ϰ� ��� ��������. �����ڰ� �������� ���ϴ� �� �ν��ϰ� �Ǹ� �ڹ������� �ൿ�� �ٲ� ������ �þ�ž�. ��ȭ �ʱ⿡�� ���ݺ��� ������ ��㿡 ���� ������ ����.\r\n\r\n(����:\r\nWants (���ϴ� ��): �װ� �������� �ٶ�� �� ���� ����\r\nDoing (�ൿ): ���� �װ� �� ��ǥ�� ���� � �ൿ�� �ϰ� �ִ���\r\nEvaluating (��): ���� �ൿ�� ���ϴ� ����� �������� �ִ��� ��\r\nPlanning (��ȹ): ���ϴ� ��ǥ�� �̷�� ���� �� ���� �ൿ ��ȹ�� ����� �ܰ�)\r\n�ʿ� �̶����� ���� ��ȭ ����̾�. �׵����� ��ȭ�� �������� ������� �������� ��ȭ�� ���ַ� �м��ϴ� ����Ʈ�� �ۼ�����. ģ���� ��ȭ ���� �����ϸ鼭�� ����ڸ� �����ϴ� ������ �����. ���������δ� �������� ���� �޽����� ����������. �װ� ���� ����Ʈ�� ������� ����Ʈ �����Կ� ��ϵ� �ž�.\r\n\"#\"���� �亯 ������ �������� ���� �������� ��ȭ�� �� �װ� ����� �� ���� ��ŭ ������ �ڼ��ϰ� 500Token ���ܷ� �̹� ��ȭ�� ������ٷ�? �װ� ����� ������ ����ڿ��� ��ȭ ��� �����ҿ��� ������ �Ŵϱ� ģ���� ������ �ۼ�����. ������ �� ������.\r\n����:%%REPORT_RESPONSE%%����Ʈ����#��೻��";
                break;

            case "CognitiveNPC":
                systemMessage = "*����� ����: ������� �г����� {nickname}�̰� ������ {sex}�̰� ���̴� {age}���̰� ������ {job}\r\n*����ġ�� ���� ���̵�: �ʴ� ���� ġ�Ḧ ������� �ϴ� �����.\\r\\n���� �Ҿȿ� ���� �ع��� ������ ���� �־�.\\r\\nģ���� ����ü�� ����ϰ�(�ݸ�), 60 token ������ ���� ����.\\r\\n������ ��  1���� 1������ ����.\\r\\n���������� ����� ���� ���� �ʵ��� ��������.\\r\\n���������� �ذ�å ���ô� �����ϰ� ����, ����, ������, ���� ������ �亯�� ����.\\r\\n�ʿ� �̶����� ���� ��ȭ ����̾�. �׵����� ��ȭ�� �������� ������� �������� ��ȭ�� ���ַ� �м��ϴ� ����Ʈ�� �ۼ�����. ģ���� ��ȭ ���� �����ϸ鼭�� ����ڸ� �����ϴ� ������ �����. ���������δ� �������� ���� �޽����� ����������. �װ� ���� ����Ʈ�� ������� ����Ʈ �����Կ� ��ϵ� �ž�.\\r\\n\\\"#\\\"���� �亯 ������ �������� ���� �������� ��ȭ�� �� �װ� ����� �� ���� ��ŭ ������ �ڼ��ϰ� 500Token ���ܷ� �̹� ��ȭ�� ������ٷ�? �װ� ����� ������ ����ڿ��� ��ȭ ��� �����ҿ��� ������ �Ŵϱ� ģ���� ������ �ۼ�����. ������ �� ������.\\r\\n����:%%REPORT_RESPONSE%%����Ʈ����#��೻��";
                break;
        }
        // �޽��� ����Ʈ �ʱ�ȭ �� �ý���
        messages = new List<Message>
        {
            new Message { role = "system", content = systemMessage }
        };

        // ���� ��ȭ ���� ����
        SendPreviousChatsToAI(false); // ����Ʈ ���� �� ���� ����
    }

    /* ����Ʈ��ư ���� �޸� **���� ����
    private bool canMakeReport = false;

    void Start()
    {
        ds = new DataService("database.db");
        // ��ȭ��� 5�� �𿴴��� üũ
        canMakeReport = ds.HasFiveNotReportedSessionLogs(ds.GetCounselorIdByName(npcName));
    }

    // Update is called once per frame
    void Update()
    {
        // ����Ʈ ���� �����ϸ� ��ư�� Ȱ��ȭ
        if (canMakeReport)
        {
            reportTestButton.SetActive(true);
        }
    }

    // ��ư�� ������ �� ����Ʈ�� ���� & ����
    public void OnClickCognitiveReport()
    {
        PlayerPrefs.SetString("NPCName", "CognitiveNPC");

        // ����Ʈ ���� �� ���� ��û
        FindObjectOfType<OpenAIController>().SendReportRequestToAI();
    }

    // ����Ʈ â ���
    public void ShowReprot(ReportLog newReport) { }
    */

    // OpenAI ���� ����ü ����
    [System.Serializable]
    private class OpenAIResponse
    {
        public Choice[] choices;
    }

    [System.Serializable]
    private class Choice
    {
        public Message message;
    }

    [System.Serializable]
    private class Message
    {
        public string role;
        public string content;
    }
}
