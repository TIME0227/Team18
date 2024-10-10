using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UserDataManager : MonoBehaviour
{
    // InputField 또는 TMP_InputField 오브젝트들
    public TMP_InputField nicknameInput;
    public TMP_InputField sexInput;
    public TMP_InputField ageInput;
    public TMP_InputField jobInput;

    // 버튼 오브젝트들
    public Button saveBtn;
    public Button skipBtn;

    // 활성화/비활성화할 오브젝트들
    public GameObject tutorialObject;
    public GameObject settingsObject;

    void Start()
    {
        tutorialObject.SetActive(false);
        settingsObject.SetActive(true);

        // 버튼 클릭 이벤트 리스너 설정
        saveBtn.onClick.AddListener(SaveData);
        skipBtn.onClick.AddListener(SkipData);
    }

    // 입력된 데이터를 저장하는 함수
    void SaveData()
    {
        // 각각의 입력 필드에서 값을 가져와 저장합니다.
        PlayerPrefs.SetString("Nickname", nicknameInput.text);
        PlayerPrefs.SetString("Sex", sexInput.text);
        PlayerPrefs.SetString("Age", ageInput.text);
        PlayerPrefs.SetString("Job", jobInput.text);
        Debug.Log("입력한 데이터를 저장합니다.");

        // 저장된 데이터를 즉시 저장합니다.
        PlayerPrefs.Save();

        // 오브젝트 활성화/비활성화 처리
        tutorialObject.SetActive(true);
        settingsObject.SetActive(false);
    }

    // null 값을 저장하는 함수
    void SkipData()
    {
        // 각각에 null 값을 저장합니다.
        PlayerPrefs.SetString("Nickname", null);
        PlayerPrefs.SetString("Sex", null);
        PlayerPrefs.SetString("Age", null);
        PlayerPrefs.SetString("Job", null);
        Debug.Log("건너뛰기");

        // 저장된 데이터를 즉시 저장합니다.
        PlayerPrefs.Save();

        // 오브젝트 활성화/비활성화 처리
        tutorialObject.SetActive(true);
        settingsObject.SetActive(false);
    }

    // 다른 씬에서 데이터를 불러오는 함수
    public static string GetUserData(string key)
    {
        // 해당 키에 저장된 값을 불러옵니다.
        return PlayerPrefs.GetString(key, null); // 기본값을 null로 설정
    }

    // 다른 씬에서 UserData를 사용하고 싶을 때 아래 코드 사용
    // string nickname = UserDataManager.GetUserData("Nickname");
    // string sex = UserDataManager.GetUserData("Sex");
    // string age = UserDataManager.GetUserData("Age");
    // string job = UserDataManager.GetUserData("Job");

}
