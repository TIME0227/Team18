using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCEmotionController : MonoBehaviour
{
    public Material[] mat = new Material[7]; // ǥ�� material 7���� ����

    public Animator characterAnimator;
    public Renderer characterRenderer;

    /* ǥ�� & ��� list
     * ����������ִ� 
     * �������λ�
     * ������������ִ� 
     * ��������������ִ� 
     * �ܼҸ��ϴ� 
     * �ų������߰Ÿ��� 
     * ���̽����ϴ� 
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

    // ���� Ű���带 �޾Ƽ� ������ ���� ���
    public void UpdateNPCEmotion(string emotion_keyword)
    {
        Debug.Log("NPC ���� Ű���� ����: " + emotion_keyword);

        switch (emotion_keyword)
        {
            case "����������ִ�": PlayListenQuietly(); break;
            case "�������λ�": PlaySmileAndGreet(); break;
            case "������������ִ�": PlayNodAndListen(); break;
            case "��������������ִ�": PlayWorriedNoEnergy(); break;
            case "�ܼҸ��ϴ�": PlayNagging(); break;
            case "�ų������߰Ÿ���": PlayChatterExcited(); break;
            case "���̽����ϴ�": PlaySadTogether(); break;
            default:
                PlayListenQuietly(); // ������ ��� "����������ִ�"�� ����Ʈ�� ����
                break;
        }
    }
}