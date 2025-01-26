using UnityEngine;

public class GumInteraction : MonoBehaviour
{
    public Camera mainCamera; // Main camera
    public AudioClip chewingSound; // ���õľ׽���Ч�������� Inspector �����ã�
    public Material highlightMaterial; // Highlight material
    private Material originalMaterial; // Original material
    private Renderer currentRenderer; // Currently highlighted object

    private ShieldManager playerController; // ������ҵĿ��ƽű�
    private AudioSource chewingAudioSource; // ���ڲ��ž׽���Ч�� AudioSource

    void Start()
    {
        // ��� AudioClip �Ƿ�������
        if (chewingSound == null)
        {
            Debug.LogError("Chewing sound is not assigned! Please assign an AudioClip in the Inspector.");
        }

        // ��ȡ����� AudioSource ���
        chewingAudioSource = GetComponent<AudioSource>();
        if (chewingAudioSource == null)
        {
            chewingAudioSource = gameObject.AddComponent<AudioSource>(); // ���û�� AudioSource ������Զ����һ��
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
        // ���������
        float detectionDistance = 5.0f; // �ɸ�������������ֵ

        // �����ⷶΧ�����ĺͰ뾶
        Vector3 detectionCenter = mainCamera.transform.position + mainCamera.transform.forward * detectionDistance;
        float detectionRadius = 2.0f; // �������뾶

        // ��ȡ���η�Χ�ڵ�������ײ��
        Collider[] hits = Physics.OverlapSphere(detectionCenter, detectionRadius);

        foreach (Collider hit in hits)
        {
            // ����Ƿ�Ϊ���� "Gum" ��ǩ������
            if (hit.CompareTag("Gum"))
            {
                PlayChewingSound();

                // ��ȡ GumBase ����ű������� ActivateEffect()
                GumBase gum = hit.GetComponent<GumBase>();
                if (gum != null)
                {
                    gum.ActivateEffect();
                }
                else
                {
                    Debug.LogWarning($"{hit.name} û�й��� GumBase �ű���");
                }

                break; // ֻ����һ�� Gum���ɸ�����Ҫ�Ƴ�
            }
        }
    }

    // �������õľ׽���Ч
    void PlayChewingSound()
    {
        if (chewingSound != null && chewingAudioSource != null)
        {
            chewingAudioSource.PlayOneShot(chewingSound); // ���ž׽���Ч
        }
        else
        {
            Debug.LogError("Chewing AudioSource ����Чδ���ã�");
        }
    }

    void HighlightObject()
    {
        // �����һ�θ���������
        if (currentRenderer != null)
        {
            currentRenderer.material = originalMaterial; // �ָ�ԭʼ����
            currentRenderer = null;
        }

        // �������μ�ⷶΧ
        float detectionDistance = 5.0f; // ������
        float detectionRadius = 2.0f;   // ���뾶
        Vector3 detectionCenter = mainCamera.transform.position + mainCamera.transform.forward * detectionDistance;

        // ��ȡ��ⷶΧ�ڵ�������ײ��
        Collider[] hits = Physics.OverlapSphere(detectionCenter, detectionRadius);

        // ������⵽����ײ��
        foreach (Collider hit in hits)
        {
            Renderer renderer = hit.GetComponent<Renderer>();
            if (renderer != null && hit.CompareTag("Gum"))
            {
                originalMaterial = renderer.material; // ����ԭʼ����
                renderer.material = highlightMaterial; // Ӧ�ø�������
                currentRenderer = renderer;
                break; // ֻ����һ������
            }
        }
    }
}


