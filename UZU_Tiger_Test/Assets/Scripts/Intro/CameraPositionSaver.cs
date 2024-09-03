using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraPositionSaver : MonoBehaviour
{
    // 스크립트가 적용된 오브젝트의 위치와 회전을 저장하는 함수
    public void SaveCameraPosition()
    {
        Vector3 position = Camera.main.transform.position;
        Quaternion rotation = Camera.main.transform.rotation;

        // PlayerPrefs에 위치와 회전을 저장
        PlayerPrefs.SetFloat("CameraPosX", position.x);
        PlayerPrefs.SetFloat("CameraPosY", position.y);
        PlayerPrefs.SetFloat("CameraPosZ", position.z);
        PlayerPrefs.SetFloat("CameraRotX", rotation.x);
        PlayerPrefs.SetFloat("CameraRotY", rotation.y);
        PlayerPrefs.SetFloat("CameraRotZ", rotation.z);
        PlayerPrefs.SetFloat("CameraRotW", rotation.w);

        PlayerPrefs.Save();
    }
}
