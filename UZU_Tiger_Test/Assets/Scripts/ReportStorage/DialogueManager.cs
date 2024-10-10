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

        // DB�� �ִ� ���  ���� ��ȭ ��ົ ������ �����ͼ� ����ϱ�
        createDialogueContent("�ش� ��ȭ ��ົ �ؽ�Ʈ", "NPC�̸�");

    }

    public void createDialogueContent(string text, string npcName)
    {
        // �ƹ� ��ȭ�� ���� �ʾҴٸ�
        if (text.Trim() == "") return;

        // Dialogue �����տ� text ���� �߰��Ͽ� Hierarchy�� clone ����


        // npcName�� ���� NPC �ʻ�ȭ �̹��� �����Ͽ� ���


    }

    public void OnClick_backtoMain()
    {
        SceneManager.LoadScene("Main");
    }
}
