using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class Intro : MonoBehaviour
{
    // Title 이미지와 Touch to Start 텍스트의 참조를 가져옵니다.
    public Image titleImage;
    public TMP_Text touchToStartText;

    // 페이드 효과를 조절할 변수들
    public float fadeDuration = 1.0f; // 페이드 인/아웃 시간
    private bool isFadingOut = false; // 현재 페이드아웃 중인지 여부
    private float fadeOutTime = 0f; // 페이드아웃 시작 시간

    // Touch to Start 텍스트의 깜박임을 조절할 변수들
    public float blinkSpeed = 1.0f; // 깜박임 속도
    private bool isTextVisible = true; // 텍스트가 현재 보이는지 여부

    public CameraPositionSaver cameraPositionSaver;

    // 최초 실행 여부를 확인하기 위한 키
    private const string FirstPlayKey = "FirstPlay";

    // 키 초기화
    // PlayerPrefs.DeleteKey("FirstPlay");

    void Start()
    {
        // 초기 설정: 텍스트의 색상 alpha 값을 1로 설정합니다.
        Color textColor = touchToStartText.color;
        textColor.a = 1f;
        touchToStartText.color = textColor;

        // Coroutine을 통해 텍스트 깜박임을 시작합니다.
        StartCoroutine(BlinkText());
    }

    void Update()
    {
        // 화면을 클릭하면 페이드 아웃을 시작합니다.
        if (Input.GetMouseButtonDown(0) && !isFadingOut)
        {
            isFadingOut = true;
            fadeOutTime = Time.time;
            StopCoroutine(BlinkText()); // 깜박임을 중지합니다.
        }

        // 페이드아웃 진행 중일 경우
        if (isFadingOut)
        {
            float elapsed = Time.time - fadeOutTime;
            float fadeProgress = elapsed / fadeDuration;

            // 이미지와 텍스트의 alpha 값을 줄여줍니다.
            Color titleColor = titleImage.color;
            titleColor.a = Mathf.Lerp(1f, 0f, fadeProgress);
            titleImage.color = titleColor;

            Color textColor = touchToStartText.color;
            textColor.a = Mathf.Lerp(1f, 0f, fadeProgress);
            touchToStartText.color = textColor;

            // 페이드아웃이 완료되면 다음 씬으로 전환합니다.
            if (fadeProgress >= 1f)
            {
                titleImage.gameObject.SetActive(false);
                touchToStartText.gameObject.SetActive(false);
                LoadNextScene();
            }
        }
    }

    // 텍스트 깜박임을 위한 Coroutine
    IEnumerator BlinkText()
    {
        while (true)
        {
            // 텍스트의 alpha 값을 조절하여 깜박임 효과를 줍니다.
            Color textColor = touchToStartText.color;
            textColor.a = Mathf.PingPong(Time.time * blinkSpeed, 1f);
            touchToStartText.color = textColor;

            yield return null;
        }
    }

    // 다음 씬을 로드하는 함수
    void LoadNextScene()
    {
        string nextSceneName;

        // 최초 실행 여부 확인
        if (PlayerPrefs.GetInt(FirstPlayKey, 0) == 0)
        {
            // 최초 실행 시 Settings_Tutorial로 이동
            nextSceneName = "Settings_Tutorial";
            PlayerPrefs.SetInt(FirstPlayKey, 1); // 최초 실행 기록 저장
            PlayerPrefs.Save();
        }
        else
        {
            // 이후 실행 시 Main으로 이동
            nextSceneName = "Main";
        }

        cameraPositionSaver.SaveCameraPosition();
        SceneManager.LoadScene(nextSceneName);
    }
}
