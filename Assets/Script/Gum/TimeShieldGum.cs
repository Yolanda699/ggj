using UnityEngine;

public class TimeShieldGum : GumBase
{
    private TimeShieldManager playerTimeShieldManager;  // 引用玩家的时间护盾管理器
    public Enemy[] cannons; // 引用多个大炮的 Enemy 脚本

    private void Start()
    {
        // 获取玩家的时间护盾管理器
        playerTimeShieldManager = FindObjectOfType<TimeShieldManager>();
        if (playerTimeShieldManager == null)
        {
            Debug.LogError("未找到玩家的 TimeShieldManager 脚本，请确保玩家上挂载了该脚本！");
        }

        if (cannons == null || cannons.Length == 0)
        {
            Debug.LogError("未分配任何大炮，请确保在 Inspector 面板中正确设置！");
        }
    }

    public override void ActivateEffect()
    {
        // 激活时间护盾
        if (playerTimeShieldManager != null)
        {
            playerTimeShieldManager.ActivateTimeShield();  // 激活时间护盾
            Debug.Log("时间护盾已激活！");
        }

        foreach (Enemy cannon in cannons)
        {
            if (cannon != null) // 确保大炮引用有效
            {
                cannon.ActivateFire();
            }
        }

        // 播放咀嚼音效
        PlayChewingSound();

        // 隐藏 Gum
        HideGum();
    }
}


