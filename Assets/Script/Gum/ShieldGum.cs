using UnityEngine;

public class ShieldGum : GumBase
{
    private ShieldManager playerShieldManager;

    private void Start()
    {
        // 获取玩家的护盾管理器
        playerShieldManager = FindObjectOfType<ShieldManager>();
        if (playerShieldManager == null)
        {
            Debug.LogError("未找到玩家的 ShieldManager 脚本，请确保玩家上挂载了该脚本！");
        }
    }

    public override void ActivateEffect()
    {
        // 激活护盾
        if (playerShieldManager != null)
        {
            playerShieldManager.ActivateShield();
            Debug.Log("护盾已激活！");
        }

        // 播放咀嚼音效
        PlayChewingSound();

        // 隐藏 Gum
        HideGum();
    }
}
