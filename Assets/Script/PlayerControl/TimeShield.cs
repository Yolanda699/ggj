using UnityEngine;

public class TimeShield : MonoBehaviour
{
    // 在物体碰撞时触发
    private void OnTriggerEnter(Collider other)
    {
        // 检查碰撞的物体是否是tag为"damage2"
        if (other.CompareTag("Damage2"))
        {
            // 输出提示信息
            Debug.Log("Damage2物体被时间护盾击中，摧毁物体！");

            // 摧毁碰撞物体
            Destroy(other.gameObject);
        }
    }
}


