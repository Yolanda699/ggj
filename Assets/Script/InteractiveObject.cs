using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Camera mainCamera; // 主摄像机
    public AudioSource chewingAudioSource; // 用于播放咀嚼音效的 AudioSource
    public Material highlightMaterial; // 高亮材质
    private Material originalMaterial; // 原始材质
    private Renderer currentRenderer; // 当前高亮的物体

    void Start()
    {
        // 检查 AudioSource 是否已设置
        if (chewingAudioSource == null)
        {
            Debug.LogError("Chewing AudioSource is not assigned! Please assign an AudioSource in the Inspector.");
        }
    }

    void Update()
    {
        HighlightObject();

        // 按下 E 键尝试拾取物品
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryConsume();
        }
    }

    void TryConsume()
    {
        // 定义检测距离
        float detectionDistance = 5.0f;

        // 定义检测范围的中心和半径
        Vector3 detectionCenter = mainCamera.transform.position + mainCamera.transform.forward * detectionDistance;
        float detectionRadius = 2.0f;

        // 获取球形范围内的所有碰撞体
        Collider[] hits = Physics.OverlapSphere(detectionCenter, detectionRadius);

        foreach (Collider hit in hits)
        {
            // 检查是否为带有 "Pickup" 标签的物体
            if (hit.CompareTag("Pickup"))
            {
                // 播放咀嚼音效
                PlayChewingSound();

                // 销毁该物体
                hit.gameObject.SetActive(false);
                break; // 只处理一个物体
            }
        }
    }

    void PlayChewingSound()
    {
        if (chewingAudioSource != null)
        {
            chewingAudioSource.Play(); // 播放咀嚼音效
        }
        else
        {
            Debug.LogError("Chewing AudioSource is missing!");
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
            if (renderer != null && hit.CompareTag("Pickup"))
            {
                originalMaterial = renderer.material; // 保存原始材质
                renderer.material = highlightMaterial; // 应用高亮材质
                currentRenderer = renderer;
                break; // 只高亮一个物体
            }
        }
    }
}
