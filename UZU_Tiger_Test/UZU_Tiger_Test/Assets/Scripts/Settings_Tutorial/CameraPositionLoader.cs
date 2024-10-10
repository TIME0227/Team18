using UnityEngine;

public class CameraPositionLoader : MonoBehaviour
{
    void Start()
    {
        LoadCameraPosition();
    }

    // ����� ��ġ�� ȸ���� �ҷ��� ī�޶� �����ϴ� �Լ�
    void LoadCameraPosition()
    {
        // ����� ��ġ�� ȸ�� ���� �ҷ�����
        if (PlayerPrefs.HasKey("CameraPosX"))
        {
            float posX = PlayerPrefs.GetFloat("CameraPosX");
            float posY = PlayerPrefs.GetFloat("CameraPosY");
            float posZ = PlayerPrefs.GetFloat("CameraPosZ");
            float rotX = PlayerPrefs.GetFloat("CameraRotX");
            float rotY = PlayerPrefs.GetFloat("CameraRotY");
            float rotZ = PlayerPrefs.GetFloat("CameraRotZ");
            float rotW = PlayerPrefs.GetFloat("CameraRotW");

            // ī�޶� ��ġ�� ȸ���� ����
            Camera.main.transform.position = new Vector3(posX, posY, posZ);
            Camera.main.transform.rotation = new Quaternion(rotX, rotY, rotZ, rotW);
        }
    }
}
