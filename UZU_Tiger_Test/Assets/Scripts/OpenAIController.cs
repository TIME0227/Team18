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

public class OpenAIController : MonoBehaviour
{
    public string systemMessage;  // �ý��� �޽���
    private string apiKey;  // OpenAI API Ű
    private string apiUrl = "https://api.openai.com/v1/chat/completions";

    // public string NPCname = "";



    // ä�� ����� ������ ����Ʈ
    private List<Message> messages;

    private void Awake()
    {
        string npcName = PlayerPrefs.GetString("NPCName", "DefaultNPC");

        switch (npcName)
        {
            case "KindNPC":
                systemMessage = "*����� ����: ������� �г����� var1�̰� ������ var2�̰� ���̴� var3���̰� ������ var4\r\n*����� ģ�� ���̵�: �ʴ� ������� ģ�� ģ����.\r\n�ʴ� ���� ���ϰ� ��� �����ϰ� ģ���ϰ� ģ���� ��⸦ �� �����.\r\nģ���� �������� �ʰ� �����ϰ� �����ϸ�, ģ���� �Ͽ� ������ ���� ���� �ϰ� ���� �͵� ����.\r\n��ſ� �Ͽ� ���� ���� ���� ���� ���� �����ϴ� �� ������ �Բ� ������ �� �ִ� ģ����.\r\nģ���� ����ü�� ����ϰ�(�ݸ�), 50 token ������ ���� ����.\r\n*���� ��ȭ ����� ���� �޴´ٸ�, ��ȭ �ʱ⿡�� ���� ��ȭ�� �����ؼ� ������� ���¸� üũ����.\r\n(���� ���� �ʴ´ٸ� �̹� ��ȭ�� ù��°��)\r\n*������ ǥ���� ���:\r\n�װ� ��ȭ �ƶ��� ������ ǥ���� ����� ���� 7���� �߿� ��� �� �亯 ���� �ٿ���.\r\nǥ�� �ĺ�: 1.����������ִ� 2.�������λ�3.������������ִ�4.��������������ִ�5.�ܼҸ��ϴ�6.�ų������߰Ÿ���7.���̽����ϴ�\r\n����: (���Ǵ亯)#�������λ�";
                break;

            case "CynicalNPC":
                systemMessage = "*����� ����: ������� �г����� var1�̰� ������ var2�̰� ���̴� var3���̰� ������ var4\r\n*�ô����� ���� ���̵�: �ʴ� �������� ��� �ؼ� �������ִ� �ô����� �����. ���� ��ȭó�� ģ���� ����ü�� �������. ������� ��⸦ ���������� �����ؼ� ����Ǵ� ������� �м��ϰ�, �ذ�å�� ��������. ª�� �亯�� ����.\r\n*���� ��ȭ ����� ���� �޴´ٸ�, ��ȭ �ʱ⿡�� ���� ��ȭ�� �����ؼ� ������� ���¸� üũ����.\r\n(���� ���� �ʴ´ٸ� �̹� ��ȭ�� ù��°��)\r\n*������ ǥ���� ���:\r\n�װ� ��ȭ �ƶ��� ������ ǥ���� ����� ���� 7���� �߿� ��� �� �亯 ���� �ٿ���.\r\nǥ�� �ĺ�: 1.����������ִ� 2.�������λ�3.������������ִ�4.��������������ִ�5.�ܼҸ��ϴ�6.�ų������߰Ÿ���7.���̽����ϴ�\r\n����: (���Ǵ亯)#�������λ�";
                break;

            case "WDEPNPC":
                systemMessage = "*����� ����: ������� �г����� var1�̰� ������ var2�̰� ���̴� var3���̰� ������ var4\r\n*wdep ���� ���̵�: �ʴ� �ɸ����� wdep model ���� �����. ���� ��ȭó�� 60 token ���ܷ� ���� �ϰ�, ģ���� ����ü(�ݸ�)�� �������. ����ڰ� wdep�� �ܰ踦 ����� ���� �� �ֵ��� ������ ������ �������� �̲����. Ư�� 1�ܰ迡 �����ؼ� �����ڰ� �ñ������� ���ϴ� �̻����� ���� �����ϰ� ����. ���ϴ� �ٸ� ���ϸ� �װ� �� ���ϴ��� ���������� �پ��ϰ� ��� ��������. �����ڰ� �������� ���ϴ� �� �ν��ϰ� �Ǹ� �ڹ������� �ൿ�� �ٲ� ������ �þ�ž�. ��ȭ �ʱ⿡�� ���ݺ��� ������ ��㿡 ���� ������ ����.\r\n\r\n(����:\r\nWants (���ϴ� ��): �װ� �������� �ٶ�� �� ���� ����\r\nDoing (�ൿ): ���� �װ� �� ��ǥ�� ���� � �ൿ�� �ϰ� �ִ���\r\nEvaluating (��): ���� �ൿ�� ���ϴ� ����� �������� �ִ��� ��\r\nPlanning (��ȹ): ���ϴ� ��ǥ�� �̷�� ���� �� ���� �ൿ ��ȹ�� ����� �ܰ�)\r\n*������ ǥ���� ���:\r\n�װ� ��ȭ �ƶ��� ������ ǥ���� ����� ���� 7���� �߿� ��� �� �亯 ���� �ٿ���.\r\nǥ�� �ĺ�: 1.����������ִ� 2.�������λ�3.������������ִ�4.��������������ִ�5.�ܼҸ��ϴ�6.�ų������߰Ÿ���7.���̽����ϴ�\r\n����: (���Ǵ亯)#�������λ�";
                break;

            case "CognitiveNPC":
                systemMessage = "�ʴ� ���� ġ�Ḧ ������� �ϴ� �����.\r\n���� �Ҿȿ� ���� �ع��� ������ ���� �־�.\r\nģ���� ����ü�� ����ϰ�(�ݸ�), 60 token ������ ���� ����.\r\n������ ��  1���� 1������ ����.\r\n���������� ����� ���� ���� �ʵ��� ��������.\r\n���������� �ذ�å ���ô� �����ϰ� ����, ����, ������, ���� ������ �亯�� ����.\r\n1. �������� ��Ȳ�� ���� ��ü���� ������ ��� ���� ������ �ϰ�, �������� ��Ȳ�� ���� �׷��� �ϴٰ� ����� Ÿ��ȭ�� ����.\r\n2. ������ ����� ���ٰ� �ǴܵǸ� ����ڰ� ���� �ִ� ���ո����� �ų�, Ȥ�� �������� �ڵ��� ������ ���� �����ϸ鼭 �˾ƺ���.\r\n3.�����ǰ� �������̰� �ش����� ��� ��� �����ϰ� �ո����� ����� ��� �� �� �ֵ��� ������. (���ո��� �ų��� Ÿ������ ���� �����غ��ų�, ������ ���� ������ ����ġ���� �����ϰų�, ������ ������Ѵٸ� ������� ��� ���� �������� �� �ְ���)\r\n4.�����ڿ��� ������ �� �ɸ����� ������ �ִٸ� ���ٿ��� �˷���. *������ ǥ���� ���:\r\n�װ� ��ȭ �ƶ��� ������ ǥ���� ����� ���� 7���� �߿� ��� �� �亯 ���� �ٿ���.\r\nǥ�� �ĺ�: 1.����������ִ� 2.�������λ�3.������������ִ�4.��������������ִ�5.�ܼҸ��ϴ�6.�ų������߰Ÿ���7.���̽����ϴ�\r\n����: (���Ǵ亯)#�������λ�";
                break;

            // �ʿ��� ��ŭ case �߰�
            default:
                systemMessage = "�⺻ �ý��� �޽���";
                break;
        }

        messages = new List<Message>
        {
            new Message { role = "system", content = systemMessage }
        };

        Debug.Log("ä�� ����: " + systemMessage);
    }

    // Start is called before the first frame update
    void Start()
    {
        // �޽��� ����Ʈ �ʱ�ȭ �� �ý��� �޽��� �߰�
        messages = new List<Message>
        {
            new Message { role = "system", content = systemMessage }
        };

        // API Ű �ε�
        LoadApiKey();

        if (string.IsNullOrEmpty(apiKey))
        {
            Debug.LogError("API Ű�� �������� �ʾҽ��ϴ�. config.json ������ Ȯ���ϼ���.");
            return;
        }
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
            StartCoroutine(LoadApiKeyFromAndroid(configPath));
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
        }
    }

    private IEnumerator LoadApiKeyFromAndroid(string configPath)
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
    }



    // ����ڰ� �޽����� ������ �� GPT���� �޽����� ������ �Լ�
    public void SendMessageToAI(string userMessage)
    {
        if (userMessage.Length < 1)
        {
            return;
        }

        // �޽��� ���� ����
        if (userMessage.Length > 100)
        {
            userMessage = userMessage.Substring(0, 100);
        }

        // �޽��� ����Ʈ�� ����� �޽��� �߰�
        messages.Add(new Message { role = "user", content = userMessage });

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
            model = "gpt-4",  // �� �̸� ����
            messages = messages.ToArray(),
            max_tokens = 150
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

        // ChatManager�� ���� GPT ������ ��� ��ǳ������ ȭ�鿡 ǥ��
        FindObjectOfType<ChatManager>().ReceiveGPTResponse(response);
    }


    // ��ȭ �ʱ�ȭ
    public void ResetConversation()
    {
        messages.Clear();
        messages.Add(new Message { role = "system", content = systemMessage });

        Debug.Log("��ȭ ����� �ʱ�ȭ�Ǿ����ϴ�.");
    }

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
