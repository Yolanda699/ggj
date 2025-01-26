using UnityEngine;

public class ShieldGum : GumBase
{
    private ShieldManager playerShieldManager;

    private void Start()
    {
        // ��ȡ��ҵĻ��ܹ�����
        playerShieldManager = FindObjectOfType<ShieldManager>();
        if (playerShieldManager == null)
        {
            Debug.LogError("δ�ҵ���ҵ� ShieldManager �ű�����ȷ������Ϲ����˸ýű���");
        }
    }

    public override void ActivateEffect()
    {
        // �����
        if (playerShieldManager != null)
        {
            playerShieldManager.ActivateShield();
            Debug.Log("�����Ѽ��");
        }

        // ���ž׽���Ч
        PlayChewingSound();

        // ���� Gum
        HideGum();
    }
}
