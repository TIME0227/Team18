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
    public ScrollRect scrollRect;  // ScrollRect를 참조

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
        textField.text += string.Format("\n\n당신: {0}", prompt);

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
            model = "gpt-4o",  // 모델 이름을 "gpt-4o"
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
        // 메시지 리스트 초기화 및 시스템 메시지 다시 추가
        messages.Clear();
        messages.Add(new Message { role = "system", content = systemMessage });

        // 텍스트 필드 초기화
        textField.text = "";

        Debug.Log("대화 기록이 초기화되었습니다.");
    }

    private void OnResponseReceived(string response)
    {
        Debug.Log("ChatGPT Response: " + response);

        // 애니메이션 업데이트
        string[] responseSp = response.Split("#"); // 애니메이션 지시어 저장

        Debug.Log("제스처: " + responseSp[1]);

        UpdateAnimation(responseSp[1]);

        // Add a newline before the assistant message for better readability
        textField.text += string.Format("\n\nAI상담사: {0}", responseSp[0]);

        // 자동 스크롤
        Canvas.ForceUpdateCanvases();
        textField.GetComponentInParent<ScrollRect>().verticalNormalizedPosition = 0f;

        // Focus the input field to allow for quick subsequent inputs
        inputField.Select();
        inputField.ActivateInputField();
    }

    private void UpdateAnimation(string state)
    {
        // 상태에 따른 애니메이션 전환
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
