using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCEmotionController : MonoBehaviour
{
    public Material[] mat = new Material[7]; // 표정 material 7개를 저장

    public Animator characterAnimator;
    public Renderer characterRenderer;

    /* 표정 & 모션 list
     * 가만히들어주는 
     * 웃으며인사
     * 끄덕끄덕들어주는 
     * 기운없어보여걱정해주는 
     * 잔소리하는 
     * 신나서재잘거리는 
     * 같이슬퍼하는 
     */

    void PlayListenQuietly()
    {
        characterAnimator.SetTrigger("PlayIdleB");
        characterRenderer.material = mat[0];
    }

    void PlaySmileAndGreet()
    {
        characterAnimator.SetTrigger("PlayHello");
        characterRenderer.material = mat[1];
    }

    void PlayNodAndListen()
    {
        characterAnimator.SetTrigger("PlayYes");
        characterRenderer.material = mat[2];
    }

    void PlayWorriedNoEnergy()
    {
        characterAnimator.SetTrigger("PlaySick");
        characterRenderer.material = mat[3];
    }

    void PlayNagging()
    {
        characterAnimator.SetTrigger("PlayAttack");
        characterRenderer.material = mat[4];
    }

    void PlayChatterExcited()
    {
        characterAnimator.SetTrigger("PlayJump");
        characterRenderer.material = mat[5];
    }

    void PlaySadTogether()
    {
        characterAnimator.SetTrigger("PlayIdleA");
        characterRenderer.material = mat[6];
    }

    // 감정 키워드를 받아서 적절한 동작 출력
    public void UpdateNPCEmotion(string emotion_keyword)
    {
        Debug.Log("NPC 감정 키워드 받음: " + emotion_keyword);

        switch (emotion_keyword)
        {
            case "가만히들어주는": PlayListenQuietly(); break;
            case "웃으며인사": PlaySmileAndGreet(); break;
            case "끄덕끄덕들어주는": PlayNodAndListen(); break;
            case "기운없어보여걱정해주는": PlayWorriedNoEnergy(); break;
            case "잔소리하는": PlayNagging(); break;
            case "신나서재잘거리는": PlayChatterExcited(); break;
            case "같이슬퍼하는": PlaySadTogether(); break;
            default:
                PlayListenQuietly(); // 예외일 경우 "가만히들어주는"을 디폴트로 실행
                break;
        }
    }
}