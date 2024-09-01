using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Text;
using System.IO;

public class OpenAIController : MonoBehaviour
{
    public TMP_Text textField;
    public TMP_InputField inputField;
    public ScrollRect scrollRect;  // ScrollRect�� ����

    public string systemMessage;
    public Animator animator;

    private string apiKey;  // OpenAI API key
    private string apiUrl = "https://api.openai.com/v1/chat/completions";

    // Save the chat history
    private List<Message> messages;

    // Start is called before the first frame update
    void Start()
    {
        inputField.text = "";

        // Initialize the messages list and add the system message
        messages = new List<Message>
        {
            new Message { role = "system", content = systemMessage }
        };

        // Load the API key from config.json
        LoadApiKey();

        if (string.IsNullOrEmpty(apiKey))
        {
            Debug.LogError("API key is not set. Please check the config.json file.");
            return;
        }

        // Listen for Enter key in the input field
        inputField.onSubmit.AddListener(delegate { OnEnterKeyPressed(); });
    }

    private void LoadApiKey()
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
            Debug.LogError("config.json file not found.");
        }
    }

    private void OnEnterKeyPressed()
    {
        string prompt = inputField.text;

        if (prompt.Length < 1)
        {
            return;
        }

        if (prompt.Length > 100)
        {
            // Limit messages to 100 characters
            prompt = prompt.Substring(0, 100);
        }

        Debug.Log(string.Format("Your Prompt: {0}", prompt));

        // Add a newline before the user message for better readability
        textField.text += string.Format("\n\n���: {0}", prompt);

        // Clear the input field
        inputField.text = "";

        // Add the user message to the messages list
        messages.Add(new Message { role = "user", content = prompt });

        SendRequest(prompt, OnResponseReceived);
    }

    public void SendRequest(string prompt, System.Action<string> callback)
    {
        StartCoroutine(PostRequest(prompt, callback));
    }

    private IEnumerator PostRequest(string prompt, System.Action<string> callback)
    {
        var requestData = new
        {
            model = "gpt-4o",  // �� �̸��� "gpt-4o"
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

            // Add the assistant response to the messages list
            messages.Add(new Message { role = "assistant", content = responseContent });

            callback(responseContent);
        }
    }

    public void ResetConversation()
    {
        // �޽��� ����Ʈ �ʱ�ȭ �� �ý��� �޽��� �ٽ� �߰�
        messages.Clear();
        messages.Add(new Message { role = "system", content = systemMessage });

        // �ؽ�Ʈ �ʵ� �ʱ�ȭ
        textField.text = "";

        Debug.Log("��ȭ ����� �ʱ�ȭ�Ǿ����ϴ�.");
    }

    private void OnResponseReceived(string response)
    {
        Debug.Log("ChatGPT Response: " + response);

        // �ִϸ��̼� ������Ʈ
        string[] responseSp = response.Split("#"); // �ִϸ��̼� ���þ� ����

        Debug.Log("����ó: " + responseSp[1]);

        UpdateAnimation(responseSp[1]);

        // Add a newline before the assistant message for better readability
        textField.text += string.Format("\n\nAI����: {0}", responseSp[0]);

        // �ڵ� ��ũ��
        Canvas.ForceUpdateCanvases();
        textField.GetComponentInParent<ScrollRect>().verticalNormalizedPosition = 0f;

        // Focus the input field to allow for quick subsequent inputs
        inputField.Select();
        inputField.ActivateInputField();
    }

    private void UpdateAnimation(string state)
    {
        // ���¿� ���� �ִϸ��̼� ��ȯ
        if (state == "Attack")
        {
            animator.Play("Attack");
        }
        else if (state == "Bounce")
        {
            animator.Play("Bounce");
        }
        else if (state == "Clicked")
        {
            animator.Play("Clicked");
        }
        else if (state == "Death")
        {
            animator.Play("Death");
        }
        else if (state == "Eat")
        {
            animator.Play("Eat");
        }
        else if (state == "Fear")
        {
            animator.Play("Fear");
        }
        else if (state == "Fly")
        {
            animator.Play("Fly");
        }
        else if (state == "Hit")
        {
            animator.Play("Hit");
        }
        else if (state == "Idle_A")
        {
            animator.Play("Idle_A");
        }
        else if (state == "Idle_B")
        {
            animator.Play("Idle_B");
        }
        else if (state == "Idle_C")
        {
            animator.Play("Idle_C");
        }
        else if (state == "Jump")
        {
            animator.Play("Jump");
        }
        else if (state == "Roll")
        {
            animator.Play("Roll");
        }
        else if (state == "Run")
        {
            animator.Play("Run");
        }
        else if (state == "Sit")
        {
            animator.Play("Sit");
        }
        else if (state == "Spin")
        {
            animator.Play("Spin");
        }
        else if (state == "Swim")
        {
            animator.Play("Swim");
        }
        else if (state == "Walk")
        {
            animator.Play("Walk");
        }
    }

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
