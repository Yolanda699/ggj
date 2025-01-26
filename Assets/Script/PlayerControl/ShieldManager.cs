using UnityEngine;

public class ShieldManager : MonoBehaviour
{
    public GameObject shield; // ���ܶ���
    private bool isShieldActive = false; // ����״̬

    private void Start()
    {
        if (shield != null)
        {
            shield.SetActive(false); // Ĭ�Ϲرջ���
        }
        else
        {
            Debug.LogWarning("���ܶ���δ���ã�");
        }
    }

    public void ActivateShield()
    {
        if (isShieldActive || shield == null) return;

        isShieldActive = true;
        shield.SetActive(true);
        Debug.Log("�����Ѽ��");
    }

    public void DeactivateShield()
    {
        if (!isShieldActive || shield == null) return;

        isShieldActive = false;
        shield.SetActive(false);
        Debug.Log("�����ѹرգ�");
    }

    public bool IsShieldActive()
    {
        return isShieldActive;
    }
}

