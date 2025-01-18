using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Camera mainCamera; // Main camera
    public AudioClip chewingSound; // Audio for chewing
    private AudioSource audioSource; // Audio source for playing sounds
    public Material highlightMaterial; // Highlight material
    private Material originalMaterial; // Original material
    private Renderer currentRenderer; // Currently highlighted object

    void Start()
    {
        // Initialize the audio source
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
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
            // 检查是否为带有 "Pickup" 标签的物体
            if (hit.CompareTag("Pickup"))
            {
                // 播放咀嚼音效
                if (chewingSound != null)
                {
                    audioSource.PlayOneShot(chewingSound);
                }

                // 销毁该物体
                hit.gameObject.SetActive(false);
                break; // 只处理一个物体，可视需求调整
            }
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

        