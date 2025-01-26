using UnityEngine;

public class GumInteraction : MonoBehaviour
{
    public Camera mainCamera; // Main camera
    public AudioClip chewingSound; // 设置的咀嚼音效（可以在 Inspector 中设置）
    public Material highlightMaterial; // Highlight material
    private Material originalMaterial; // Original material
    private Renderer currentRenderer; // Currently highlighted object

    private ShieldManager playerController; // 引用玩家的控制脚本
    private AudioSource chewingAudioSource; // 用于播放咀嚼音效的 AudioSource

    void Start()
    {
        // 检查 AudioClip 是否已设置
        if (chewingSound == null)
        {
            Debug.LogError("Chewing sound is not assigned! Please assign an AudioClip in the Inspector.");
        }

        // 获取并检查 AudioSource 组件
        chewingAudioSource = GetComponent<AudioSource>();
        if (chewingAudioSource == null)
        {
            chewingAudioSource = gameObject.AddComponent<AudioSource>(); // 如果没有 AudioSource 组件，自动添加一个
        }
    }

    void Update()
    {
        HighlightObject();

        // Consume item with E
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryConsume();
        }
    }

    void TryConsume()
    {
        // 定义检测距离
        float detectionDistance = 5.0f; // 可根据需求调整这个值

        // 定义检测范围的中心和半径
        Vector3 detectionCenter = mainCamera.transform.position + mainCamera.transform.forward * detectionDistance;
        float detectionRadius = 2.0f; // 检测区域半径

        // 获取球形范围内的所有碰撞体
        Collider[] hits = Physics.OverlapSphere(detectionCenter, detectionRadius);

        foreach (Collider hit in hits)
        {
            // 检查是否为带有 "Gum" 标签的物体
            if (hit.CompareTag("Gum"))
            {
                PlayChewingSound();

                // 获取 GumBase 子类脚本并调用 ActivateEffect()
                GumBase gum = hit.GetComponent<GumBase>();
                if (gum != null)
                {
                    gum.ActivateEffect();
                }
                else
                {
                    Debug.LogWarning($"{hit.name} 没有挂载 GumBase 脚本！");
                }

                break; // 只处理一个 Gum，可根据需要移除
            }
        }
    }

    // 播放设置的咀嚼音效
    void PlayChewingSound()
    {
        if (chewingSound != null && chewingAudioSource != null)
        {
            chewingAudioSource.PlayOneShot(chewingSound); // 播放咀嚼音效
        }
        else
        {
            Debug.LogError("Chewing AudioSource 或音效未设置！");
        }
    }

    void HighlightObject()
    {
        // 清除上一次高亮的物体
        if (currentRenderer != null)
        {
            currentRenderer.material = originalMaterial; // 恢复原始材质
            currentRenderer = null;
        }

        // 定义球形检测范围
        float detectionDistance = 5.0f; // 检测距离
        float detectionRadius = 2.0f;   // 检测半径
        Vector3 detectionCenter = mainCamera.transform.position + mainCamera.transform.forward * detectionDistance;

        // 获取检测范围内的所有碰撞体
        Collider[] hits = Physics.OverlapSphere(detectionCenter, detectionRadius);

        // 遍历检测到的碰撞体
        foreach (Collider hit in hits)
        {
            Renderer renderer = hit.GetComponent<Renderer>();
            if (renderer != null && hit.CompareTag("Gum"))
            {
                originalMaterial = renderer.material; // 保存原始材质
                renderer.material = highlightMaterial; // 应用高亮材质
                currentRenderer = renderer;
                break; // 只高亮一个物体
            }
        }
    }
}


