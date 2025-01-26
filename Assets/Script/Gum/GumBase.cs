using UnityEngine;

public abstract class GumBase : MonoBehaviour
{
    public AudioClip chewingSound; // 咀嚼音效
    private AudioSource audioSource; // 音效播放源

    private void Start()
    {
        // 初始化音效播放源
        audioSource = FindObjectOfType<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("未找到音效播放器，请在场景中添加 AudioSource 组件！");
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
        // 隐藏 Gum 对象
        gameObject.SetActive(false);
        Debug.Log($"{gameObject.name} 已被隐藏！");
    }

    public abstract void ActivateEffect(); // 抽象方法，具体效果由子类实现
}
