using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

// [CustomEditor(typeof(ChatManager))]
public class ChatEditor : MonoBehaviour
{
    ChatManager chatManager;
    string text;
    public TMP_InputField InputField;
    public Button sendBtn;


    void OnEnable()
    {
        chatManager = GetComponent<ChatManager>();
    }


    /*public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        *//*EditorGUILayout.BeginHorizontal();
        text = EditorGUILayout.TextArea(text);*//*

        text = InputField.text;

        // 사용자가 입력한 메시지를 보내기 버튼을 통해 처리
        if (GUILayout.Button("보내기", GUILayout.Width(60)) && text.Trim() != "")
        {
            // 사용자의 메시지를 보내고 화면에 표시
            chatManager.SendUserMessage(text);
            text = "";
            GUI.FocusControl(null);
        }

        *//*EditorGUILayout.EndHorizontal();*//*
    }*/

    public void OnClick_sendBtn()
    {
        text = InputField.text;
        // 사용자의 메시지를 보내고 화면에 표시
        chatManager.SendUserMessage(text);
        text = "";
        // GUI.FocusControl(null);
    }
    

}