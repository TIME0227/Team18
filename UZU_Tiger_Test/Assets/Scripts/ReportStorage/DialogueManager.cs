using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    DialogueScript Dialogue;

    public void Awake()
    {
        Dialogue = GetComponent<DialogueScript>();

        // DB에 있는 모든  현재 대화 요약본 데이터 가져와서 출력하기
        createDialogueContent("해당 대화 요약본 텍스트", "NPC이름");

    }

    public void createDialogueContent(string text, string npcName)
    {
        // 아무 대화도 하지 않았다면
        if (text.Trim() == "") return;

        // Dialogue 프리팹에 text 내용 추가하여 Hierarchy에 clone 생성


        // npcName에 따라 NPC 초상화 이미지 변경하여 출력


    }

    public void OnClick_backtoMain()
    {
        SceneManager.LoadScene("Main");
    }
}
