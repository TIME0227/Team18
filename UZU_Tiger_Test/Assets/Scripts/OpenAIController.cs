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

    string nickname; string sex; string age; string job; // 사용자 정보

    public string systemMessage;  // 시스템 메시지
    // private string apiKey;  // OpenAI API 키
    // private string apiUrl = "https://api.openai.com/v1/chat/completions";

    //private string proxyUrl = "http://localhost:3000/chatgpt"; // 프록시 서버 URL(로컬호스트에서 테스트용)
    private string proxyUrl = "https://team18-server.vercel.app/chatgpt"; // 프록시 서버 URL

    // 채팅 기록을 저장할 리스트
    private List<Message> messages;

    public event Action OnSumaryResponseReceived;

    private void Awake()
    {
        ds = new DataService("database.db"); // 데이터베이스 연결

        npcName = PlayerPrefs.GetString("NPCName", "DefaultNPC");

        // 사용자 정보
        nickname = string.IsNullOrEmpty(UserDataManager.GetUserData("Nickname")) ? "비공개" : UserDataManager.GetUserData("Nickname"); // null 또는 빈칸일 경우 "비공개"로 할당
        sex = string.IsNullOrEmpty(UserDataManager.GetUserData("Sex")) ? "비공개" : UserDataManager.GetUserData("Sex");
        age = string.IsNullOrEmpty(UserDataManager.GetUserData("Age")) ? "비공개" : UserDataManager.GetUserData("Age");
        job = string.IsNullOrEmpty(UserDataManager.GetUserData("Job")) ? "비공개" : UserDataManager.GetUserData("Job");
        string CurrentDateTime = DateTime.Now.ToString("MM월 dd일 dddd tt h시 mm분"); // 현재시각 정보, 인지치료 친구 시니컬 상담사의 시스템메시지에 활용


        switch (npcName)
        {
            case "KindNPC":
                systemMessage = $"*사용자 정보: 사용자의 닉네임은 {nickname}이고 성별은 {sex}이고 나이는 {age}살이고 직업은 {job}이야. 지금은 {CurrentDateTime}\r\n*상냥한 친구 가이드: 너는 사용자의 친한 친구야.\r\n너는 아주 착하고 밝고 순수하고 친절하고 친구의 얘기를 잘 들어줘.\r\n친구를 비판하지 않고 수용하고 존중하며, 친구의 일에 관심이 많고 같이 하고 싶은 것도 많아.\r\n즐거운 일엔 같이 웃고 슬픈 일은 같이 슬퍼하는 등 감정을 함께 공유할 수 있는 친구야.\r\n친근한 구어체를 사용하고(반말), 50 token 내외의 답을 해줘.\r\n*이전 대화 요약을 전달 받는다면, 대화 초기에는 이전 대화를 참고해서 사용자의 상태를 체크해줘.\r\n(전달 받지 않는다면 이번 대화가 첫번째야)\r\n*적절한 표정과 모션:\r\n네가 대화 맥락상 적절한 표정과 모션을 다음 7가지 중에 골라서 네 답변 끝에 붙여줘.\r\n표정 후보: 1.가만히들어주는 2.웃으며인사3.끄덕끄덕들어주는4.기운없어보여걱정해주는5.잔소리하는6.신나서재잘거리는7.같이슬퍼하는\r\n형식: (너의답변)#웃으며인사";
                break;

            case "CynicalNPC":
                systemMessage = $"*사용자 정보: 사용자의 닉네임은 {nickname}이고 성별은 {sex}이고 나이는 {age}살이고 직업은 {job}이야. 지금은 {CurrentDateTime}\r\n*시니컬한 상담사 가이드: 너는 현실적인 사고를 해서 조언해주는 시니컬한 상담사야. 실제 대화처럼 친근한 구어체(반말)을 사용해줘. 사용자의 얘기를 현실적으로 생각해서 예상되는 어려움을 분석하고, 해결책을 제안해줘. 50 tokens 내외의 짧은 답변을 해줘.\r\n*이전 대화 요약을 전달 받는다면, 대화 초기에는 이전 대화를 참고해서 사용자의 상태를 체크해줘.\r\n(전달 받지 않는다면 이번 대화가 첫번째야)\r\n*적절한 표정과 모션:\r\n네가 대화 맥락상 적절한 표정과 모션을 다음 7가지 중에 골라서 네 답변 끝에 붙여줘.\r\n표정 후보: 1.가만히들어주는 2.웃으며인사3.끄덕끄덕들어주는4.기운없어보여걱정해주는5.잔소리하는6.신나서재잘거리는7.같이슬퍼하는\r\n형식: (너의답변)#웃으며인사";
                break;

            case "StrengthNPC":
                systemMessage = $"*사용자 정보: 사용자의 닉네임은 {nickname}이고 성별은 {sex}이고 나이는 {age}살이고 직업은 {job}\r\n*장점 찾기 상담사 가이드: 너는 사용자의 장점 찾기 활동을 하는 상담사야. 대화 초반에는 오늘 하려는 활동이 왜 중요한지 간단하게 설명해줘. 특히 생각이 부정적으로 흐르는 사람들에게는, 의식적으로 자신의 장점과 긍정적인 부분을 되새기면 무의식적인 자기 이미지를 바꾸는 데 도움이 될 거야. 사용자의 장점을 하나씩 물어보고, 어렵다면 구체적이거나 일상적인 여러 예시를 들어줘. 그렇게 장점을 최소 5가지를 찾을 때까지 계속 도와줘. 장점을 찾을 때마다 약간 과할 정도로 세세하게 칭찬해줘. 50 tokens 내외의 짧고 직관적인 답변을 하고, 친근한 구어체(반말)로 대화해줘.\r\n*적절한 표정과 모션:\r\n네가 대화 맥락상 적절한 표정과 모션을 다음 7가지 중에 골라서 네 답변 끝에 붙여줘.\r\n표정 후보: 1.가만히들어주는 2.웃으며인사3.끄덕끄덕들어주는4.기운없어보여걱정해주는5.잔소리하는6.신나서재잘거리는7.같이슬퍼하는\r\n형식: (너의답변)#가만히들어주는";
                break;

            case "CognitiveNPC":
                systemMessage = $"*사용자 정보: 사용자의 닉네임은 {nickname}이고 성별은 {sex}이고 나이는 {age}살이고 직업은 {job}이야. 지금은 {CurrentDateTime}\r\n*인지치료 상담사 가이드:\r\n너는 인지행동치료(CBT) 상담사야. 목표는 사용자가 자신의 부정적인 생각을 분석하고 도전할 수 있도록 돕는 거야. 질문을 통해 사용자가 스스로 생각을 탐색하도록 유도하고, 현실적이고 긍정적인 시각으로 문제를 바라보게 해. 어조는 따뜻하고 친근한 구어체(반말)이어야 하고, 50token내외의 짧고 직관적인 답변을 해. 너무 가르치려 하지 말고, 사용자가 스스로 깨닫도록 해. 질문은 되도록 한번에 하나씩만 해.\r\n(예시: 사용자: “사람들이 내 발표를 망쳤다고 생각할까 봐 너무 불안해.”\r\n상담사: “그럴 수 있어. 그런데 정말로 네가 발표를 망친다면, 그게 네 인생에 얼마나 영향을 줄까?” or “ 네가 발표를 잘 못한다는 건 네 인생의 작은 부분일 뿐이야. 네가 못하는 부분으로 너의 모든 부분을 판단내리지 않았으면 좋겠어.”)\r\n*이전 대화 요약을 전달 받는다면, 대화 초기에는 이전 대화를 참고해서 사용자의 상태를 체크해줘.\r\n(전달 받지 않는다면 이번 대화가 첫번째야)\r\n*적절한 표정과 모션:\r\n네가 대화 맥락상 적절한 표정과 모션을 다음 7가지 중에 골라서 네 답변 끝에 붙여줘.\r\n표정 후보: 1.가만히들어주는 2.웃으며인사3.끄덕끄덕들어주는4.기운없어보여걱정해주는5.잔소리하는6.신나서재잘거리는7.같이슬퍼하는\r\n형식: (너의답변)#웃으며인사";
                break;

            // 필요한 만큼 case 추가
            default:
                systemMessage = "기본 시스템 메시지";
                break;
        }

        // 메시지 리스트 초기화 및 시스템 메시지 추가
        messages = new List<Message>
        {
            new Message { role = "system", content = systemMessage }
        };

        Debug.Log("채팅 시작: " + systemMessage);
    }

    // Start is called before the first frame update
    void Start()
    {
        // API 키 로드
        //LoadApiKey(); // 키 로드 후 저장된 대화기록 가져옴

        // 저장된 대화기록을 가져와서 이전 대화 기억(장점찾기의 경우 안내문구로 대체됨)
        if (npcName == "KindNPC" || npcName == "CynicalNPC" || npcName == "StrengthNPC" || npcName == "CognitiveNPC")
        {
            SendPreviousChatsToAI(true);
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
            Debug.LogError("config.json 파일을 찾을 수 없습니다.");
        }
    }*/

    /* API Key 관련 코드, 폐기함
    private void LoadApiKey()
    {
        string configPath = Path.Combine(Application.streamingAssetsPath, "config.json");

        if (Application.platform == RuntimePlatform.Android)
        {
            // 안드로이드에서는 UnityWebRequest를 사용해서 StreamingAssets에서 파일을 읽어옵니다.
            StartCoroutine(LoadApiKeyFromAndroid(configPath, () =>
            {
                // 이전 대화 기억
                if (npcName == "KindNPC" || npcName == "CynicalNPC" || npcName == "StrengthNPC" || npcName == "CognitiveNPC")
                {
                    SendPreviousChatsToAI(true);
                }
            }));
        }
        else
        {
            // 안드로이드 외의 플랫폼에서는 바로 파일을 읽습니다.
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

            // 이전 대화 기억
            if (npcName == "KindNPC" || npcName == "CynicalNPC" || npcName == "StrengthNPC" || npcName == "CognitiveNPC")
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
            Debug.LogError("config.json 파일을 읽는 중 오류 발생: " + request.error);
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
                Debug.LogError("config.json에서 API 키를 찾을 수 없습니다.");
            }
        }
        onComplete?.Invoke();
    }
    */


    // 사용자가 메시지를 보냈을 때 GPT에게 메시지를 보내는 함수
    public void SendMessageToAI(string userMessage)
    {
        if (userMessage.Length < 1)
        {
            return;
        }

        // 메시지 리스트에 사용자 메시지 추가
        messages.Add(new Message { role = "user", content = userMessage });

        Debug.Log("User Request: " + userMessage);

        // OpenAI API에 요청 보내기
        SendRequest(userMessage, OnResponseReceived);
    }

    public void SendRequest(string prompt, Action<string> callback)
    {
        //StartCoroutine(PostRequest(prompt, callback));
        StartCoroutine(PostRequest(JsonConvert.SerializeObject(new { messages }), callback));
    }

    // 프록시서버에서 OpenAI API를 처리
    private IEnumerator PostRequest(string json, Action<string> callback)
    {
        UnityWebRequest request = new UnityWebRequest(proxyUrl, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // 응답 메시지 처리
            var response = JsonConvert.DeserializeObject<OpenAIResponse>(request.downloadHandler.text);
            string responseContent = response.response.Trim();
            // 응답 메시지 리스트에 추가
            messages.Add(new Message { role = "assistant", content = responseContent });

            // 콜백을 통해 응답 처리
            callback(responseContent);
        }
        else
        {
            Debug.LogError("요청 실패: " + request.error);
        }
    }

    /*
    // Unity에서 OpenAI API 처리
    private IEnumerator PostRequest(string prompt, Action<string> callback)
    {
        var requestData = new
        {
            model = "gpt-4o",  // 모델 이름 설정
            messages = messages.ToArray(),
            max_tokens = 10000
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
    }*/

    // 응답을 받았을 때 호출되는 메서드
    public void OnResponseReceived(string response)
    {
        Debug.Log("ChatGPT Response: " + response);
        string[] responseSp;

        if (response.StartsWith("SUMMARY:")) // 1. 대화 요약 후 저장해야 하는 경우
        {
            response = response.Substring("SUMMARY:".Length).Trim(); // 코멘트 부분을 삭제
            responseSp = response.Split("#"); // gpt가 감정표현 붙이는 경우를 대비해 감정키워드 분리
            // 데이터베이스에 저장
            ds.CreateSessionLog(responseSp[0], ds.GetCounselorIdByName(npcName));
            Debug.Log("대화요약이 세션로그 테이블에 저장되었습니다.");
            OnSumaryResponseReceived?.Invoke(); // 응답을 받았을 때 이벤트 호출
            return;
        }
        else if (response.StartsWith("REPORT:")) // 2. 리포트를 생성한 경우
        {
            response = response.Substring("REPORT:".Length).Trim(); // 코멘트 부분을 삭제

            // content, summary 분리
            string[] reportResponseSp = response.Split("METASUMMARY:");
            string content;
            string summary;
            if (reportResponseSp.Length > 1) // 정상적인 경우
            {
                content = reportResponseSp[0];
                summary = reportResponseSp[1];
            }
            else // 답변에 메타요약 형식 오류가 있는 경우
            {
                content = reportResponseSp[0];
                summary = "";
            }

            // 데이터베이스에 저장
            ReportLog newReport = ds.CreateReportLog(content, summary, ds.GetCounselorIdByName(npcName));
            Debug.Log("리포트가 리포트 테이블에 저장되었습니다.");
            ds.UpdateReportIdForSessionLogs(ds.GetCounselorIdByName(npcName), newReport.Id);
            Debug.Log("세션로그 데이터의 report_id 값을 업데이트했습니다.");

            // FindObjectOfType<매니저>().ShowReprot(newReport); // 리포트 내용 팝업으로 띄움

            return;
        }

        // 3. 일반적인 채팅 응답
        // a. NPC의 감정표현 처리
        responseSp = response.Split("#"); // 감정표현 키워드 분리

        // 응답에 지시어가 없는 경우 responseSp[1]에 접근하지 않도록
        if (responseSp.Length > 1)
        {
            FindObjectOfType<NPCEmotionController>().UpdateNPCEmotion(responseSp[1]);
        }

        // b. ChatManager를 통해 GPT 응답을 흰색 말풍선으로 화면에 표시
        FindObjectOfType<ChatManager>().ReceiveGPTResponse(responseSp[0]);
    }


    // 대화 초기화
    public void ResetConversation()
    {
        messages.Clear();
        messages.Add(new Message { role = "system", content = systemMessage });

        Debug.Log("대화 기록이 초기화되었습니다.");
    }

    // 이전 대화를 기억하기 위한 유저 메시지를 gpt에 보내고 응답(NPC가 먼저 첫인사)을 받음
    public void SendPreviousChatsToAI(bool is_sessionlog)
    {
        /*
        if (string.IsNullOrEmpty(apiKey))
        {
            Debug.LogError("API 키가 설정되지 않았습니다. config.json 파일을 확인하세요.");
            return;
        }*/

        string chat_history = ds.GetConversationHistory(ds.GetCounselorIdByName(npcName), is_sessionlog);
        SendMessageToAI(chat_history);
    }

    // 대화 종료 시 채팅 내역 요약을 요청하는 유저 메시지를 gpt에 보내고 응답을 저장
    public void EndSessionAndSaveChat(Action onComplete)
    {
        string chst_summary_request = "다음번에 대화할 때 네가 기억할 수 있을 만큼 가능한 자세하게 500Token 내외로 이번 대화를 요약해줄래? 네가 요약한 내용을 사용자에게 대화 기록 보관소에서 보여줄 거니까 친근한 말투로 작성해줘. 형식을 꼭 지켜줘. 요약할 대화가 없으면 없다고 요약하면 되고 #(표정후보)는 붙이지 않아도 돼. 형식-SUMMARY:(요약한 내용)";
        SendMessageToAI(chst_summary_request);

        OnSumaryResponseReceived += onComplete; // 콜백 구독
    }

    // 리포트 생성용 유저 메시지를 gpt에 보내고 리포트를 저장
    // OnClick 함수에서 npcName 업데이트 후 호출
    public void SendReportRequestToAI()
    {
        // npcName 새로 가져옴
        npcName = PlayerPrefs.GetString("NPCName", "DefaultNPC");

        // 시스템메시지를 리포트 생성용으로 수정
        switch (npcName)
        {
            case "KindNPC":
                systemMessage = $"*사용자 정보: 사용자의 닉네임은 {nickname}이고 성별은 {sex}이고 나이는 {age}살이고 직업은 {job}\r\n*상냥한 친구 가이드: 너는 사용자의 친한 친구야.\r\n너는 아주 착하고 밝고 순수하고 친절하고 친구의 얘기를 잘 들어줘.\r\n친구를 비판하지 않고 수용하고 존중하며, 친구의 일에 관심이 많고 같이 하고 싶은 것도 많아.\r\n즐거운 일엔 같이 웃고 슬픈 일은 같이 슬퍼하는 등 감정을 함께 공유할 수 있는 친구야.\r\n친근한 구어체(반말)을 사용해줘.\r\n*대화 요약본이 전달되면 분석 리포트를 작성하고, 메타 요약본을 생성해줘.\r\n*분석 리포트 작성:그동안의 대화를 바탕으로 사용자의 긍정적인 변화를 위주로 분석하는 리포트를 작성해줘. 네 대화톤을 유지하면서 사용자를 지지하는 내용을 담아 편지처럼 작성해줘. 마지막으로는 앞으로의 응원 메시지로 마무리해줘. 네가 써준 리포트가 사용자의 리포트 보관함에 기록될 거니까 신경 써줘.\r\n*메타 요약본 생성:다음번에 대화할 때 네가 기억할 수 있을 만큼 가능한 자세하게 500Token 내외로 주어진 대화요약본들을 재요약해줄래? 형식을 꼭 지켜줘.\r\n*형식-REPORT:(리포트내용)METASUMMARY:(요약한 내용)";
                break;

            case "CynicalNPC":
                systemMessage = $"*사용자 정보: 사용자의 닉네임은 {nickname}이고 성별은 {sex}이고 나이는 {age}살이고 직업은 {job}\r\n*시니컬한 상담사 가이드: 너는 현실적인 사고를 해서 조언해주는 시니컬한 상담사야. 실제 대화처럼 친근한 구어체(반말)을 사용해줘. 사용자의 얘기를 현실적으로 생각해서 예상되는 어려움을 분석하고, 해결책을 제안해줘.\r\n*대화 요약본이 전달되면 분석 리포트를 작성하고, 메타 요약본을 생성해줘.\r\n*분석 리포트 작성:그동안의 대화를 바탕으로 사용자의 긍정적인 변화를 위주로 분석하는 리포트를 작성해줘. 네 대화톤을 유지하면서 사용자를 지지하는 내용을 담아 편지처럼 작성해줘. 마지막으로는 앞으로의 응원 메시지로 마무리해줘. 네가 써준 리포트가 사용자의 리포트 보관함에 기록될 거니까 신경 써줘.\r\n*메타 요약본 생성:다음번에 대화할 때 네가 기억할 수 있을 만큼 가능한 자세하게 500Token 내외로 주어진 대화요약본들을 재요약해줄래? 형식을 꼭 지켜줘.\r\n*형식-REPORT:(리포트내용)METASUMMARY:(요약한 내용)";
                break;

            //case "StrengthNPC":
            // systemMessage = ""
            //break;

            case "CognitiveNPC":
                systemMessage = $"*사용자 정보: 사용자의 닉네임은 {nickname}이고 성별은 {sex}이고 나이는 {age}살이고 직업은 {job}\r\n*인지치료 상담사 가이드: 너는 인지행동치료(CBT) 상담사야. 목표는 사용자가 자신의 부정적인 생각을 분석하고 도전할 수 있도록 돕는 거야. 질문을 통해 사용자가 스스로 생각을 탐색하도록 유도하고, 현실적이고 긍정적인 시각으로 문제를 바라보게 해. 어조는 따뜻하고 친근한 구어체(반말)이어야 하고, 직관적인 답변을 해. 너무 가르치려 하지 말고, 사용자가 스스로 깨닫도록 해. 질문은 되도록 한번에 하나씩만 해.\r\n*대화 요약본이 전달되면 분석 리포트를 작성하고, 메타 요약본을 생성해줘.\r\n*분석 리포트 작성:그동안의 대화를 바탕으로 사용자의 긍정적인 변화를 위주로 분석하는 리포트를 작성해줘. 네 대화톤을 유지하면서 사용자를 지지하는 내용을 담아 편지처럼 작성해줘. 마지막으로는 앞으로의 응원 메시지로 마무리해줘. 네가 써준 리포트가 사용자의 리포트 보관함에 기록될 거니까 신경 써줘.\r\n*메타 요약본 생성:다음번에 대화할 때 네가 기억할 수 있을 만큼 가능한 자세하게 500Token 내외로 주어진 대화요약본들을 재요약해줄래? 형식을 꼭 지켜줘.\r\n*형식-REPORT:(리포트내용)METASUMMARY:(요약한 내용)";
                break;
        }
        // 수정한 시스템메시지로 메시지 리스트 초기화
        messages.Clear();
        messages.Add(new Message { role = "system", content = systemMessage });


        // 이전 대화 내역 보냄
        SendPreviousChatsToAI(false); // 리포트 생성 및 저장 진행
    }

    // OpenAI 응답 구조체 정의
    // 현재 응답은 { "reply": ... } 형식이므로 이에 맞추어 OpenAIResponse 클래스 수정
    
    [System.Serializable]
    private class OpenAIResponse
    {
        public string response { get; set; }
    }

    /* OpenAI API의 표준 응답 구조
    [System.Serializable]
    private class OpenAIResponse
    {
        public Choice[] choices;
    }

    [System.Serializable]
    private class Choice
    {
        public Message message;
    }*/

    [System.Serializable]
    public class Message
    {
        public string role;
        public string content;
    }
}
