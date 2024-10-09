using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ChatManager))]
public class ChatEditor : Editor
{
    ChatManager chatManager;
    string text;


    void OnEnable()
    {
        chatManager = target as ChatManager;
    }


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.BeginHorizontal();
        text = EditorGUILayout.TextArea(text);

        // 사용자가 입력한 메시지를 보내기 버튼을 통해 처리
        if (GUILayout.Button("보내기", GUILayout.Width(60)) && text.Trim() != "")
        {
            // 사용자의 메시지를 보내고 화면에 표시
            chatManager.SendUserMessage(text);
            text = "";
            GUI.FocusControl(null);
        }

        EditorGUILayout.EndHorizontal();
    }

}