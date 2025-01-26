using UnityEngine;

public class TimeShieldManager : MonoBehaviour
{
    public GameObject timeShield; // ʱ�令�ܶ���
    public float shieldDuration = 5f; // ʱ�令�ܳ���ʱ�䣬��λ��
    private float shieldTimer; // ��������ʱ�令�ܴ��ʱ��
    private bool isShieldActive = false; // ʱ�令��״̬

    private void Start()
    {
        if (timeShield != null)
        {
            timeShield.SetActive(false); // Ĭ�Ϲر�ʱ�令��
        }
        else
        {
            Debug.LogWarning("ʱ�令�ܶ���δ���ã�");
        }

        // ��ʼ����ʱ��
        shieldTimer = shieldDuration;
    }

    private void Update()
    {
        // ���ʱ�令���Ǽ���״̬����ʼ����ʱ
        if (isShieldActive)
        {
            shieldTimer -= Time.deltaTime;

            // �����ʱ��Ϊ0��˵��ʱ�令�ܳ�ʱ
            if (shieldTimer <= 0f)
            {
                Debug.Log("ʱ�令�ܳ�ʱ���Զ��رգ�");
                DeactivateTimeShield();  // ����Deactivation�����ر�ʱ�令��
            }
        }
    }

    // ����ʱ�令��
    public void ActivateTimeShield()
    {
        if (isShieldActive || timeShield == null) return;

        isShieldActive = true;
        timeShield.SetActive(true);
        Debug.Log("ʱ�令���Ѽ��");

        // ���ü�ʱ��
        shieldTimer = shieldDuration;
    }

    // ����ʱ�令��
    public void DeactivateTimeShield()
    {
        if (!isShieldActive || timeShield == null) return;

        isShieldActive = false;
        timeShield.SetActive(false);
        Debug.Log("ʱ�令���ѹرգ�");
    }

    // ����ʱ�令�ܣ����磬���¼���ʱ�令��ʱ��
    public void ResetTimeShield()
    {
        shieldTimer = shieldDuration;  // ���ü�ʱ��
    }

    // ���ʱ�令���Ƿ񼤻�
    public bool IsTimeShieldActive()
    {
        return isShieldActive;
    }
}
