using UnityEngine;

public class Shield : MonoBehaviour
{
    private ShieldManager shieldManager;  // ����ShieldManager
    private int remainingHits;  // ʣ����Ե����Ĺ�������
    public int maxHits = 3;  // ���ɵ����������������ڽű�������

    private void Start()
    {
        // ��ȡShieldManager���
        shieldManager = GetComponentInParent<ShieldManager>();

        if (shieldManager == null)
        {
            Debug.LogWarning("ShieldManagerδ�ҵ�����ȷ��������ShieldManager�������");
        }

        // ��ʼ��ʣ���������
        remainingHits = maxHits;
    }

    // ��������ײʱ����
    private void OnTriggerEnter(Collider other)
    {
        // �����ײ�������Ƿ���Damage����
        if (other.CompareTag("Damage1"))
        {
            // �����ʾ��Ϣ
            Debug.Log("Damage object hit by shield!");

            // ������ײ����
            other.gameObject.SetActive(false);

            // ����ʣ���������
            remainingHits--;

            // �������û��ʣ���������������Manager�����ػ���
            if (remainingHits <= 0)
            {
                Debug.Log("�����Ѵݻ٣�");
                if (shieldManager != null)
                {
                    shieldManager.DeactivateShield();  // ����ShieldManager�����ػ��ܷ���
                }
            }
        }
    }

    // ���û��ܣ����磬���¼����ʱ���ÿɵ����Ĵ�����
    public void ResetShield()
    {
        remainingHits = maxHits;
    }
}



