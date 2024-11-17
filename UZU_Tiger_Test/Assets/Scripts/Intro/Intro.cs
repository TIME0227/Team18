using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class Intro : MonoBehaviour
{
    // Title �̹����� Touch to Start �ؽ�Ʈ�� ������ �����ɴϴ�.
    public Image titleImage;
    public TMP_Text touchToStartText;

    // ���̵� ȿ���� ������ ������
    public float fadeDuration = 1.0f; // ���̵� ��/�ƿ� �ð�
    private bool isFadingOut = false; // ���� ���̵�ƿ� ������ ����
    private float fadeOutTime = 0f; // ���̵�ƿ� ���� �ð�

    // Touch to Start �ؽ�Ʈ�� �������� ������ ������
    public float blinkSpeed = 1.0f; // ������ �ӵ�
    private bool isTextVisible = true; // �ؽ�Ʈ�� ���� ���̴��� ����

    public CameraPositionSaver cameraPositionSaver;

    // ���� ���� ���θ� Ȯ���ϱ� ���� Ű
    private const string FirstPlayKey = "FirstPlay";

    // Ű �ʱ�ȭ
    // PlayerPrefs.DeleteKey("FirstPlay");

    void Start()
    {
        // �ʱ� ����: �ؽ�Ʈ�� ���� alpha ���� 1�� �����մϴ�.
        Color textColor = touchToStartText.color;
        textColor.a = 1f;
        touchToStartText.color = textColor;

        // Coroutine�� ���� �ؽ�Ʈ �������� �����մϴ�.
        StartCoroutine(BlinkText());
    }

    void Update()
    {
        // ȭ���� Ŭ���ϸ� ���̵� �ƿ��� �����մϴ�.
        if (Input.GetMouseButtonDown(0) && !isFadingOut)
        {
            isFadingOut = true;
            fadeOutTime = Time.time;
            StopCoroutine(BlinkText()); // �������� �����մϴ�.
        }

        // ���̵�ƿ� ���� ���� ���
        if (isFadingOut)
        {
            float elapsed = Time.time - fadeOutTime;
            float fadeProgress = elapsed / fadeDuration;

            // �̹����� �ؽ�Ʈ�� alpha ���� �ٿ��ݴϴ�.
            Color titleColor = titleImage.color;
            titleColor.a = Mathf.Lerp(1f, 0f, fadeProgress);
            titleImage.color = titleColor;

            Color textColor = touchToStartText.color;
            textColor.a = Mathf.Lerp(1f, 0f, fadeProgress);
            touchToStartText.color = textColor;

            // ���̵�ƿ��� �Ϸ�Ǹ� ���� ������ ��ȯ�մϴ�.
            if (fadeProgress >= 1f)
            {
                titleImage.gameObject.SetActive(false);
                touchToStartText.gameObject.SetActive(false);
                LoadNextScene();
            }
        }
    }

    // �ؽ�Ʈ �������� ���� Coroutine
    IEnumerator BlinkText()
    {
        while (true)
        {
            // �ؽ�Ʈ�� alpha ���� �����Ͽ� ������ ȿ���� �ݴϴ�.
            Color textColor = touchToStartText.color;
            textColor.a = Mathf.PingPong(Time.time * blinkSpeed, 1f);
            touchToStartText.color = textColor;

            yield return null;
        }
    }

    // ���� ���� �ε��ϴ� �Լ�
    void LoadNextScene()
    {
        string nextSceneName;

        // ���� ���� ���� Ȯ��
        if (PlayerPrefs.GetInt(FirstPlayKey, 0) == 0)
        {
            // ���� ���� �� Settings_Tutorial�� �̵�
            nextSceneName = "Settings_Tutorial";
            PlayerPrefs.SetInt(FirstPlayKey, 1); // ���� ���� ��� ����
            PlayerPrefs.Save();
        }
        else
        {
            // ���� ���� �� Main���� �̵�
            nextSceneName = "Main";
        }

        cameraPositionSaver.SaveCameraPosition();
        SceneManager.LoadScene(nextSceneName);
    }
}
