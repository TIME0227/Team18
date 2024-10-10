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

        // ����ڰ� �Է��� �޽����� ������ ��ư�� ���� ó��
        if (GUILayout.Button("������", GUILayout.Width(60)) && text.Trim() != "")
        {
            // ������� �޽����� ������ ȭ�鿡 ǥ��
            chatManager.SendUserMessage(text);
            text = "";
            GUI.FocusControl(null);
        }

        *//*EditorGUILayout.EndHorizontal();*//*
    }*/

    public void OnClick_sendBtn()
    {
        text = InputField.text;
        // ������� �޽����� ������ ȭ�鿡 ǥ��
        chatManager.SendUserMessage(text);
        text = "";
        // GUI.FocusControl(null);
    }
    

}