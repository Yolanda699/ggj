using UnityEngine;

public class TimeShieldGum : GumBase
{
    private TimeShieldManager playerTimeShieldManager;  // ������ҵ�ʱ�令�ܹ�����
    public Enemy[] cannons; // ���ö�����ڵ� Enemy �ű�

    private void Start()
    {
        // ��ȡ��ҵ�ʱ�令�ܹ�����
        playerTimeShieldManager = FindObjectOfType<TimeShieldManager>();
        if (playerTimeShieldManager == null)
        {
            Debug.LogError("δ�ҵ���ҵ� TimeShieldManager �ű�����ȷ������Ϲ����˸ýű���");
        }

        if (cannons == null || cannons.Length == 0)
        {
            Debug.LogError("δ�����κδ��ڣ���ȷ���� Inspector �������ȷ���ã�");
        }
    }

    public override void ActivateEffect()
    {
        // ����ʱ�令��
        if (playerTimeShieldManager != null)
        {
            playerTimeShieldManager.ActivateTimeShield();  // ����ʱ�令��
            Debug.Log("ʱ�令���Ѽ��");
        }

        foreach (Enemy cannon in cannons)
        {
            if (cannon != null) // ȷ������������Ч
            {
                cannon.ActivateFire();
            }
        }

        // ���ž׽���Ч
        PlayChewingSound();

        // ���� Gum
        HideGum();
    }
}


