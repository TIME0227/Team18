using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraPositionSaver : MonoBehaviour
{
    // ��ũ��Ʈ�� ����� ������Ʈ�� ��ġ�� ȸ���� �����ϴ� �Լ�
    public void SaveCameraPosition()
    {
        Vector3 position = Camera.main.transform.position;
        Quaternion rotation = Camera.main.transform.rotation;

        // PlayerPrefs�� ��ġ�� ȸ���� ����
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
