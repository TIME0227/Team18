using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Text;
using System.IO;

public class OpenAIController : MonoBehaviour
{
    public string systemMessage;  // 시스템 메시지
    private string apiKey;  // OpenAI API 키
    private string apiUrl = "https://api.openai.com/v1/chat/completions";

    // 채팅 기록을 저장할 리스트
    private List<Message> messages;

    // Start is called before the first frame update
    void Start()
    {
        // 메시지 리스트 초기화 및 시스템 메시지 추가
        messages = new List<Message>
        {
            new Message { role = "system", content = systemMessage }
        };

        // API 키 로드
        LoadApiKey();

        if (string.IsNullOrEmpty(apiKey))
        {
            Debug.LogError("API 키가 설정되지 않았습니다. config.json 파일을 확인하세요.");
            return;
        }
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
            Debug.LogError("config.json 파일을 찾을 수 없습니다.");
        }
    }

    // 사용자가 메시지를 보냈을 때 GPT에게 메시지를 보내는 함수
    public void SendMessageToAI(string userMessage)
    {
        if (userMessage.Length < 1)
        {
            return;
        }

        // 메시지 길이 제한
        if (userMessage.Length > 100)
        {
            userMessage = userMessage.Substring(0, 100);
        }

        // 메시지 리스트에 사용자 메시지 추가
        messages.Add(new Message { role = "user", content = userMessage });

        // OpenAI API에 요청 보내기
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
            model = "gpt-4",  // 모델 이름 설정
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

            // 응답 메시지 리스트에 추가
            messages.Add(new Message { role = "assistant", content = responseContent });

            // 콜백을 통해 응답 처리
            callback(responseContent);
        }
    }

    /*// 응답을 받았을 때 호출되는 메서드
    private void OnResponseReceived(string response)
    {
        Debug.Log("ChatGPT Response: " + response);

        // 토르의 응답을 하얀 말풍선으로 화면에 표시
        FindObjectOfType<ChatManager>().ReceiveMessage(response);
    }*/

    // 응답을 받았을 때 호출되는 메서드
    private void OnResponseReceived(string response)
    {
        Debug.Log("ChatGPT Response: " + response);

        // ChatManager를 통해 GPT 응답을 흰색 말풍선으로 화면에 표시
        FindObjectOfType<ChatManager>().ReceiveGPTResponse(response);
    }


    // 대화 초기화
    public void ResetConversation()
    {
        messages.Clear();
        messages.Add(new Message { role = "system", content = systemMessage });

        Debug.Log("대화 기록이 초기화되었습니다.");
    }

    // OpenAI 응답 구조체 정의
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
