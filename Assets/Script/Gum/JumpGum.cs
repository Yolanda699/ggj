using UnityEngine;

public class JumpGum : GumBase
{
    private ThirdPersonController playerController; // ������ҿ������ű�

    private void Start()
    {
        // ��ȡ��ҵĿ�����
        playerController = FindObjectOfType<ThirdPersonController>();
        if (playerController == null)
        {
            Debug.LogError("δ�ҵ���ҵ� ThirdPersonController �ű�����ȷ������Ϲ����˸ýű���");
        }
    }

    public override void ActivateEffect()
    {
        // ������Ծ
        if (playerController != null)
        {
            playerController.ActivateJump(); // ������Ծ����
            Debug.Log("��Ծ�Ѽ��");
        }

        // ���ž׽���Ч
        PlayChewingSound();

        // ���� Gum
        HideGum();
    }
}
