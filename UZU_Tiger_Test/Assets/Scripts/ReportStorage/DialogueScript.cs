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
    public int Dialogue_id;

    DataService ds;

    private void Awake()
    {
        ds = new DataService("database.db");
    }

    public void OnClick_deleteDialogue()
    {
        // DB에서도 삭제되도록 해야함
        ds.DeleteOneSessionLog(Dialogue_id);

        Debug.Log("대화 기록을 삭제합니다.");
        Destroy(gameObject);
    }
}
