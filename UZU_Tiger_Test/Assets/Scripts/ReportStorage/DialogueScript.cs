using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using Unity.VisualScripting;

public class DialogueScript : MonoBehaviour
{
    public Image Bg;
    public Image NPC_portrait;
    public TMP_Text Dialogue_Text;
    public TMP_Text Dialogue_Date;
    public Button deleteDialogueBtn;

/*    DataService ds;

    private void Awake()
    {
        ds = new DataService("database.db");
    }*/

    public void OnClick_deleteDialogue()
    {
        // DB������ �����ǵ��� �ؾ���


        Debug.Log("��ȭ ����� �����մϴ�.");
        Destroy(gameObject);
    }
}
