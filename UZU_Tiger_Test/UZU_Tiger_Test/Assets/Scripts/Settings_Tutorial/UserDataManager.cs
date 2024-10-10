using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UserDataManager : MonoBehaviour
{
    // InputField �Ǵ� TMP_InputField ������Ʈ��
    public TMP_InputField nicknameInput;
    public TMP_InputField sexInput;
    public TMP_InputField ageInput;
    public TMP_InputField jobInput;

    // ��ư ������Ʈ��
    public Button saveBtn;
    public Button skipBtn;

    // Ȱ��ȭ/��Ȱ��ȭ�� ������Ʈ��
    public GameObject tutorialObject;
    public GameObject settingsObject;

    void Start()
    {
        tutorialObject.SetActive(false);
        settingsObject.SetActive(true);

        // ��ư Ŭ�� �̺�Ʈ ������ ����
        saveBtn.onClick.AddListener(SaveData);
        skipBtn.onClick.AddListener(SkipData);
    }

    // �Էµ� �����͸� �����ϴ� �Լ�
    void SaveData()
    {
        // ������ �Է� �ʵ忡�� ���� ������ �����մϴ�.
        PlayerPrefs.SetString("Nickname", nicknameInput.text);
        PlayerPrefs.SetString("Sex", sexInput.text);
        PlayerPrefs.SetString("Age", ageInput.text);
        PlayerPrefs.SetString("Job", jobInput.text);
        Debug.Log("�Է��� �����͸� �����մϴ�.");

        // ����� �����͸� ��� �����մϴ�.
        PlayerPrefs.Save();

        // ������Ʈ Ȱ��ȭ/��Ȱ��ȭ ó��
        tutorialObject.SetActive(true);
        settingsObject.SetActive(false);
    }

    // null ���� �����ϴ� �Լ�
    void SkipData()
    {
        // ������ null ���� �����մϴ�.
        PlayerPrefs.SetString("Nickname", null);
        PlayerPrefs.SetString("Sex", null);
        PlayerPrefs.SetString("Age", null);
        PlayerPrefs.SetString("Job", null);
        Debug.Log("�ǳʶٱ�");

        // ����� �����͸� ��� �����մϴ�.
        PlayerPrefs.Save();

        // ������Ʈ Ȱ��ȭ/��Ȱ��ȭ ó��
        tutorialObject.SetActive(true);
        settingsObject.SetActive(false);
    }

    // �ٸ� ������ �����͸� �ҷ����� �Լ�
    public static string GetUserData(string key)
    {
        // �ش� Ű�� ����� ���� �ҷ��ɴϴ�.
        return PlayerPrefs.GetString(key, null); // �⺻���� null�� ����
    }

    // �ٸ� ������ UserData�� ����ϰ� ���� �� �Ʒ� �ڵ� ���
    // string nickname = UserDataManager.GetUserData("Nickname");
    // string sex = UserDataManager.GetUserData("Sex");
    // string age = UserDataManager.GetUserData("Age");
    // string job = UserDataManager.GetUserData("Job");

}
