using UnityEngine;

public class JumpGum : GumBase
{
    private ThirdPersonController playerController; // 引用玩家控制器脚本

    private void Start()
    {
        // 获取玩家的控制器
        playerController = FindObjectOfType<ThirdPersonController>();
        if (playerController == null)
        {
            Debug.LogError("未找到玩家的 ThirdPersonController 脚本，请确保玩家上挂载了该脚本！");
        }
    }

    public override void ActivateEffect()
    {
        // 激活跳跃
        if (playerController != null)
        {
            playerController.ActivateJump(); // 激活跳跃方法
            Debug.Log("跳跃已激活！");
        }

        // 播放咀嚼音效
        PlayChewingSound();

        // 隐藏 Gum
        HideGum();
    }
}
