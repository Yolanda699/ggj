using UnityEngine;

public class TimeShieldManager : MonoBehaviour
{
    public GameObject timeShield; // 时间护盾对象
    public float shieldDuration = 5f; // 时间护盾持续时间，单位秒
    private float shieldTimer; // 用来跟踪时间护盾存活时间
    private bool isShieldActive = false; // 时间护盾状态

    private void Start()
    {
        if (timeShield != null)
        {
            timeShield.SetActive(false); // 默认关闭时间护盾
        }
        else
        {
            Debug.LogWarning("时间护盾对象未设置！");
        }

        // 初始化计时器
        shieldTimer = shieldDuration;
    }

    private void Update()
    {
        // 如果时间护盾是激活状态，开始倒计时
        if (isShieldActive)
        {
            shieldTimer -= Time.deltaTime;

            // 如果计时器为0，说明时间护盾超时
            if (shieldTimer <= 0f)
            {
                Debug.Log("时间护盾超时，自动关闭！");
                DeactivateTimeShield();  // 调用Deactivation方法关闭时间护盾
            }
        }
    }

    // 激活时间护盾
    public void ActivateTimeShield()
    {
        if (isShieldActive || timeShield == null) return;

        isShieldActive = true;
        timeShield.SetActive(true);
        Debug.Log("时间护盾已激活！");

        // 重置计时器
        shieldTimer = shieldDuration;
    }

    // 隐藏时间护盾
    public void DeactivateTimeShield()
    {
        if (!isShieldActive || timeShield == null) return;

        isShieldActive = false;
        timeShield.SetActive(false);
        Debug.Log("时间护盾已关闭！");
    }

    // 重置时间护盾（例如，重新激活时间护盾时）
    public void ResetTimeShield()
    {
        shieldTimer = shieldDuration;  // 重置计时器
    }

    // 检查时间护盾是否激活
    public bool IsTimeShieldActive()
    {
        return isShieldActive;
    }
}
