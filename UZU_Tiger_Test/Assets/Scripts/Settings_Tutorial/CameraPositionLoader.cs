using UnityEngine;

public class CameraPositionLoader : MonoBehaviour
{
    void Start()
    {
        LoadCameraPosition();
    }

    // 저장된 위치와 회전을 불러와 카메라에 적용하는 함수
    void LoadCameraPosition()
    {
        // 저장된 위치와 회전 값을 불러오기
        if (PlayerPrefs.HasKey("CameraPosX"))
        {
            float posX = PlayerPrefs.GetFloat("CameraPosX");
            float posY = PlayerPrefs.GetFloat("CameraPosY");
            float posZ = PlayerPrefs.GetFloat("CameraPosZ");
            float rotX = PlayerPrefs.GetFloat("CameraRotX");
            float rotY = PlayerPrefs.GetFloat("CameraRotY");
            float rotZ = PlayerPrefs.GetFloat("CameraRotZ");
            float rotW = PlayerPrefs.GetFloat("CameraRotW");

            // 카메라에 위치와 회전을 적용
            Camera.main.transform.position = new Vector3(posX, posY, posZ);
            Camera.main.transform.rotation = new Quaternion(rotX, rotY, rotZ, rotW);
        }
    }
}
