using UnityEngine;

public class ShieldManager : MonoBehaviour
{
    public GameObject shield; // 护盾对象
    private bool isShieldActive = false; // 护盾状态

    private void Start()
    {
        if (shield != null)
        {
            shield.SetActive(false); // 默认关闭护盾
        }
        else
        {
            Debug.LogWarning("护盾对象未设置！");
        }
    }

    public void ActivateShield()
    {
        if (isShieldActive || shield == null) return;

        isShieldActive = true;
        shield.SetActive(true);
        Debug.Log("护盾已激活！");
    }

    public void DeactivateShield()
    {
        if (!isShieldActive || shield == null) return;

        isShieldActive = false;
        shield.SetActive(false);
        Debug.Log("护盾已关闭！");
    }

    public bool IsShieldActive()
    {
        return isShieldActive;
    }
}

