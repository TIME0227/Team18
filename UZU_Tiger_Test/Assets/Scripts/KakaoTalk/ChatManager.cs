using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;
using UnityEngine.SceneManagement;
using TMPro;



public class ChatManager : MonoBehaviour
{
    public GameObject YellowArea, WhiteArea, DateArea;
    public RectTransform ContentRect;
    public Scrollbar scrollBar;
    public Toggle MineToggle;
    AreaScript LastArea;

    string text;
    public TMP_InputField InputField;
    public Button sendBtn;

    public GameObject checkClose;
    public GameObject checkRS;

    private void Awake()
    {
        checkClose.SetActive(false);
        checkRS.SetActive(false);
    }

    public void OnClick_sendBtn()
    {
        text = InputField.text;
        // 사용자의 메시지를 보내고 화면에 표시
        SendUserMessage(text);
        text = "";
        GUI.FocusControl(null);
        InputField.text = "";
    }


    /*public void ReceiveMessage(string text)
    {
        if (MineToggle.isOn) Chat(true, text, "나", null);
        else Chat(false, text, "타인", null);
    }*/

    /*public void ReceiveMessage(string text)
    {
        Chat(false, text, "토르", Resources.Load<Texture2D>("ETC/자른톨"));  // GPT의 응답은 하얀색으로
    }*/

    public void SendMessage(string text)
    {
        Chat(true, text, "나", null);  // 내가 보낸 메시지는 노란색으로
    }

    public void SendUserMessage(string userMessage)
    {
        // 사용자가 보낸 메시지를 노란색 말풍선으로 표시
        Chat(true, userMessage, "나", null);

        // OpenAIController를 통해 GPT 응답 요청
        FindObjectOfType<OpenAIController>().SendMessageToAI(userMessage);
    }

    public void ReceiveGPTResponse(string gptResponse)
    {
        // GPT의 응답을 흰색 말풍선으로 표시
        // Chat(false, gptResponse, "상담사", null);

        switch (PlayerPrefs.GetString("NPCName"))
        {
            case "KindNPC":
                Chat(false, gptResponse, "상냥한 상담새", Resources.Load<Texture2D>("ETC/Kindport"));
                break;

            case "CynicalNPC":
                Chat(false, gptResponse, "시니컬한 상담새", Resources.Load<Texture2D>("ETC/Cynicalport"));
                break;

            case "StrengthNPC":
                Chat(false, gptResponse, "장점찾기 상담새", Resources.Load<Texture2D>("ETC/wdepport"));
                break;

            case "CognitiveNPC":
                Chat(false, gptResponse, "인지치료 상담새", Resources.Load<Texture2D>("ETC/Cognitiveport"));
                break;
        }
    }

    public void LayoutVisible(bool b)
    {
        /*AndroidJavaClass kotlin = new AndroidJavaClass("com.unity3d.player.SubActivity");
        kotlin.CallStatic("LayoutVisible", b);*/
    }


    public void Chat(bool isSend, string text, string user, Texture2D picture)
    {
        if (text.Trim() == "") return;

        bool isBottom = scrollBar.value <= 0.00001f;


        //보내는 사람은 노랑, 받는 사람은 흰색영역을 크게 만들고 텍스트 대입
        AreaScript Area = Instantiate(isSend ? YellowArea : WhiteArea).GetComponent<AreaScript>();
        Area.transform.SetParent(ContentRect.transform, false);
        Area.BoxRect.sizeDelta = new Vector2(600, Area.BoxRect.sizeDelta.y);
        Area.TextRect.GetComponent<TMPro.TMP_Text>().text = text;
        Fit(Area.BoxRect);


        // 두 줄 이상이면 크기를 줄여가면서, 한 줄이 아래로 내려가면 바로 전 크기를 대입 
        float X = Area.TextRect.sizeDelta.x + 42;
        float Y = Area.TextRect.sizeDelta.y;
        if (Y > 49)
        {
            for (int i = 0; i < 200; i++)
            {
                Area.BoxRect.sizeDelta = new Vector2(X - i * 2, Area.BoxRect.sizeDelta.y);
                Fit(Area.BoxRect);

                if (Y != Area.TextRect.sizeDelta.y) { Area.BoxRect.sizeDelta = new Vector2(X - (i * 2) + 2, Y); break; }
            }
        }
        else Area.BoxRect.sizeDelta = new Vector2(X, Y);


        // 현재 것에 분까지 나오는 날짜와 유저이름 대입
        DateTime t = DateTime.Now;
        Area.Time = t.ToString("yyyy-MM-dd-HH-mm");
        Area.User = user;


        // 현재 것은 항상 새로운 시간 대입
        int hour = t.Hour;
        if (t.Hour == 0) hour = 12;
        else if (t.Hour > 12) hour -= 12;
        Area.TimeText.text = (t.Hour > 12 ? "오후 " : "오전 ") + hour + ":" + t.Minute.ToString("D2");


        // 이전 것과 같으면 이전 시간, 꼬리 없애기
        bool isSame = LastArea != null && LastArea.Time == Area.Time && LastArea.User == Area.User;
        if (isSame) LastArea.TimeText.text = "";
        Area.Tail.SetActive(!isSame);


        // 타인이 이전 것과 같으면 이미지, 이름 없애기
        if (!isSend)
        {
            Area.UserImage.gameObject.SetActive(!isSame);
            Area.UserText.gameObject.SetActive(!isSame);
            Area.UserText.text = Area.User;
            if (picture != null) Area.UserImage.sprite = Sprite.Create(picture, new Rect(0, 0, picture.width, picture.height), new Vector2(0.5f, 0.5f));
        }


        // 이전 것과 날짜가 다르면 날짜영역 보이기
        if (LastArea != null && LastArea.Time.Substring(0, 10) != Area.Time.Substring(0, 10))
        {
            Transform CurDateArea = Instantiate(DateArea).transform;
            CurDateArea.SetParent(ContentRect.transform, false);
            CurDateArea.SetSiblingIndex(CurDateArea.GetSiblingIndex() - 1);

            string week = "";
            switch (t.DayOfWeek)
            {
                case DayOfWeek.Sunday: week = "일"; break;
                case DayOfWeek.Monday: week = "월"; break;
                case DayOfWeek.Tuesday: week = "화"; break;
                case DayOfWeek.Wednesday: week = "수"; break;
                case DayOfWeek.Thursday: week = "목"; break;
                case DayOfWeek.Friday: week = "금"; break;
                case DayOfWeek.Saturday: week = "토"; break;
            }
            CurDateArea.GetComponent<AreaScript>().DateText.text = t.Year + "년 " + t.Month + "월 " + t.Day + "일 " + week + "요일";
        }


        Fit(Area.BoxRect);
        Fit(Area.AreaRect);
        Fit(ContentRect);
        LastArea = Area;


        // 스크롤바가 위로 올라간 상태에서 메시지를 받으면 맨 아래로 내리지 않음
        if (!isSend && !isBottom) return;
        Invoke("ScrollDelay", 0.03f);
    }


    void Fit(RectTransform Rect) => LayoutRebuilder.ForceRebuildLayoutImmediate(Rect);


    void ScrollDelay() => scrollBar.value = 0;

    #region 기록보관소 버튼
    public void OnClick_reportStorageBtn()
    {
        // 기록보관소 이동 확인창 띄우기
        checkRS.SetActive(true);
    }

    // 네
    public void OnClick_checkreportStorageBtn()
    {
        FindObjectOfType<OpenAIController>().EndSessionAndSaveChat(OnSaveChatComplete2);
    }

    // 아니오
    public void OnClick_checkreportStorageBtn2()
    {
        checkRS.SetActive(false);
    }

    // 기록보관소 버튼용
    private void OnSaveChatComplete2()
    {
        FindObjectOfType<OpenAIController>().OnSumaryResponseReceived -= OnSaveChatComplete; // 구독 해제
        PlayerPrefs.SetString("NPCName", "ReporterNPC"); // 기록보관 씬에서는 채팅기능 실행되지 않게...
        SceneManager.LoadScene("ReportStorage");
    }
    #endregion


    #region 닫기 버튼
    public void OnClick_closeChatBtn()
    {
        // 종료 확인창 띄우기
        checkClose.SetActive(true);
    }

    // 네
    public void OnClick_checkCloseChatBtn()
    {
        FindObjectOfType<OpenAIController>().EndSessionAndSaveChat(OnSaveChatComplete);
    }

    // 아니오
    public void OnClick_checkCloseChatBtn2()
    {
        checkClose.SetActive(false);
    }

    // 닫기 버튼용
    private void OnSaveChatComplete()
    {
        FindObjectOfType<OpenAIController>().OnSumaryResponseReceived -= OnSaveChatComplete; // 구독 해제
        Debug.Log("대화를 종료하고 저장합니다.");
        SceneManager.LoadScene("Main");
    }
    #endregion


}