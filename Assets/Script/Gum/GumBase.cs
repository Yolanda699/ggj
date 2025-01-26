using UnityEngine;

public abstract class GumBase : MonoBehaviour
{
    public AudioClip chewingSound; // �׽���Ч
    private AudioSource audioSource; // ��Ч����Դ

    private void Start()
    {
        // ��ʼ����Ч����Դ
        audioSource = FindObjectOfType<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("δ�ҵ���Ч�����������ڳ�������� AudioSource �����");
        }
    }

    public void PlayChewingSound()
    {
        if (chewingSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(chewingSound);
        }
    }

    public void HideGum()
    {
        // ���� Gum ����
        gameObject.SetActive(false);
        Debug.Log($"{gameObject.name} �ѱ����أ�");
    }

    public abstract void ActivateEffect(); // ���󷽷�������Ч��������ʵ��
}
