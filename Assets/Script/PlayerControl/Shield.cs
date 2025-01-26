using UnityEngine;

public class Shield : MonoBehaviour
{
    private ShieldManager shieldManager;  // 引用ShieldManager
    private int remainingHits;  // 剩余可以抵御的攻击次数
    public int maxHits = 3;  // 最大可抵御攻击次数，可在脚本中设置

    private void Start()
    {
        // 获取ShieldManager组件
        shieldManager = GetComponentInParent<ShieldManager>();

        if (shieldManager == null)
        {
            Debug.LogWarning("ShieldManager未找到！请确保护盾是ShieldManager的子物件");
        }

        // 初始化剩余抵御次数
        remainingHits = maxHits;
    }

    // 在物体碰撞时触发
    private void OnTriggerEnter(Collider other)
    {
        // 检查碰撞的物体是否是Damage物体
        if (other.CompareTag("Damage1"))
        {
            // 输出提示信息
            Debug.Log("Damage object hit by shield!");

            // 隐藏碰撞物体
            other.gameObject.SetActive(false);

            // 减少剩余抵御次数
            remainingHits--;

            // 如果护盾没有剩余抵御次数，调用Manager来隐藏护盾
            if (remainingHits <= 0)
            {
                Debug.Log("护盾已摧毁！");
                if (shieldManager != null)
                {
                    shieldManager.DeactivateShield();  // 调用ShieldManager的隐藏护盾方法
                }
            }
        }
    }

    // 重置护盾（例如，重新激活护盾时重置可抵御的次数）
    public void ResetShield()
    {
        remainingHits = maxHits;
    }
}



