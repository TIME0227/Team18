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
        // ������� �޽����� ������ ȭ�鿡 ǥ��
        SendUserMessage(text);
        text = "";
        GUI.FocusControl(null);
        InputField.text = "";
    }


    /*public void ReceiveMessage(string text)
    {
        if (MineToggle.isOn) Chat(true, text, "��", null);
        else Chat(false, text, "Ÿ��", null);
    }*/

    /*public void ReceiveMessage(string text)
    {
        Chat(false, text, "�丣", Resources.Load<Texture2D>("ETC/�ڸ���"));  // GPT�� ������ �Ͼ������
    }*/

    public void SendMessage(string text)
    {
        Chat(true, text, "��", null);  // ���� ���� �޽����� ���������
    }

    public void SendUserMessage(string userMessage)
    {
        // ����ڰ� ���� �޽����� ����� ��ǳ������ ǥ��
        Chat(true, userMessage, "��", null);

        // OpenAIController�� ���� GPT ���� ��û
        FindObjectOfType<OpenAIController>().SendMessageToAI(userMessage);
    }

    public void ReceiveGPTResponse(string gptResponse)
    {
        // GPT�� ������ ��� ��ǳ������ ǥ��
        // Chat(false, gptResponse, "����", null);

        switch (PlayerPrefs.GetString("NPCName"))
        {
            case "KindNPC":
                Chat(false, gptResponse, "����� ����", Resources.Load<Texture2D>("ETC/Kindport"));
                break;

            case "CynicalNPC":
                Chat(false, gptResponse, "�ô����� ����", Resources.Load<Texture2D>("ETC/Cynicalport"));
                break;

            case "StrengthNPC":
                Chat(false, gptResponse, "����ã�� ����", Resources.Load<Texture2D>("ETC/wdepport"));
                break;

            case "CognitiveNPC":
                Chat(false, gptResponse, "����ġ�� ����", Resources.Load<Texture2D>("ETC/Cognitiveport"));
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


        //������ ����� ���, �޴� ����� ��������� ũ�� ����� �ؽ�Ʈ ����
        AreaScript Area = Instantiate(isSend ? YellowArea : WhiteArea).GetComponent<AreaScript>();
        Area.transform.SetParent(ContentRect.transform, false);
        Area.BoxRect.sizeDelta = new Vector2(600, Area.BoxRect.sizeDelta.y);
        Area.TextRect.GetComponent<TMPro.TMP_Text>().text = text;
        Fit(Area.BoxRect);


        // �� �� �̻��̸� ũ�⸦ �ٿ����鼭, �� ���� �Ʒ��� �������� �ٷ� �� ũ�⸦ ���� 
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


        // ���� �Ϳ� �б��� ������ ��¥�� �����̸� ����
        DateTime t = DateTime.Now;
        Area.Time = t.ToString("yyyy-MM-dd-HH-mm");
        Area.User = user;


        // ���� ���� �׻� ���ο� �ð� ����
        int hour = t.Hour;
        if (t.Hour == 0) hour = 12;
        else if (t.Hour > 12) hour -= 12;
        Area.TimeText.text = (t.Hour > 12 ? "���� " : "���� ") + hour + ":" + t.Minute.ToString("D2");


        // ���� �Ͱ� ������ ���� �ð�, ���� ���ֱ�
        bool isSame = LastArea != null && LastArea.Time == Area.Time && LastArea.User == Area.User;
        if (isSame) LastArea.TimeText.text = "";
        Area.Tail.SetActive(!isSame);


        // Ÿ���� ���� �Ͱ� ������ �̹���, �̸� ���ֱ�
        if (!isSend)
        {
            Area.UserImage.gameObject.SetActive(!isSame);
            Area.UserText.gameObject.SetActive(!isSame);
            Area.UserText.text = Area.User;
            if (picture != null) Area.UserImage.sprite = Sprite.Create(picture, new Rect(0, 0, picture.width, picture.height), new Vector2(0.5f, 0.5f));
        }


        // ���� �Ͱ� ��¥�� �ٸ��� ��¥���� ���̱�
        if (LastArea != null && LastArea.Time.Substring(0, 10) != Area.Time.Substring(0, 10))
        {
            Transform CurDateArea = Instantiate(DateArea).transform;
            CurDateArea.SetParent(ContentRect.transform, false);
            CurDateArea.SetSiblingIndex(CurDateArea.GetSiblingIndex() - 1);

            string week = "";
            switch (t.DayOfWeek)
            {
                case DayOfWeek.Sunday: week = "��"; break;
                case DayOfWeek.Monday: week = "��"; break;
                case DayOfWeek.Tuesday: week = "ȭ"; break;
                case DayOfWeek.Wednesday: week = "��"; break;
                case DayOfWeek.Thursday: week = "��"; break;
                case DayOfWeek.Friday: week = "��"; break;
                case DayOfWeek.Saturday: week = "��"; break;
            }
            CurDateArea.GetComponent<AreaScript>().DateText.text = t.Year + "�� " + t.Month + "�� " + t.Day + "�� " + week + "����";
        }


        Fit(Area.BoxRect);
        Fit(Area.AreaRect);
        Fit(ContentRect);
        LastArea = Area;


        // ��ũ�ѹٰ� ���� �ö� ���¿��� �޽����� ������ �� �Ʒ��� ������ ����
        if (!isSend && !isBottom) return;
        Invoke("ScrollDelay", 0.03f);
    }


    void Fit(RectTransform Rect) => LayoutRebuilder.ForceRebuildLayoutImmediate(Rect);


    void ScrollDelay() => scrollBar.value = 0;

    #region ��Ϻ����� ��ư
    public void OnClick_reportStorageBtn()
    {
        // ��Ϻ����� �̵� Ȯ��â ����
        checkRS.SetActive(true);
    }

    // ��
    public void OnClick_checkreportStorageBtn()
    {
        FindObjectOfType<OpenAIController>().EndSessionAndSaveChat(OnSaveChatComplete2);
    }

    // �ƴϿ�
    public void OnClick_checkreportStorageBtn2()
    {
        checkRS.SetActive(false);
    }

    // ��Ϻ����� ��ư��
    private void OnSaveChatComplete2()
    {
        FindObjectOfType<OpenAIController>().OnSumaryResponseReceived -= OnSaveChatComplete; // ���� ����
        PlayerPrefs.SetString("NPCName", "ReporterNPC"); // ��Ϻ��� �������� ä�ñ�� ������� �ʰ�...
        SceneManager.LoadScene("ReportStorage");
    }
    #endregion


    #region �ݱ� ��ư
    public void OnClick_closeChatBtn()
    {
        // ���� Ȯ��â ����
        checkClose.SetActive(true);
    }

    // ��
    public void OnClick_checkCloseChatBtn()
    {
        FindObjectOfType<OpenAIController>().EndSessionAndSaveChat(OnSaveChatComplete);
    }

    // �ƴϿ�
    public void OnClick_checkCloseChatBtn2()
    {
        checkClose.SetActive(false);
    }

    // �ݱ� ��ư��
    private void OnSaveChatComplete()
    {
        FindObjectOfType<OpenAIController>().OnSumaryResponseReceived -= OnSaveChatComplete; // ���� ����
        Debug.Log("��ȭ�� �����ϰ� �����մϴ�.");
        SceneManager.LoadScene("Main");
    }
    #endregion


}