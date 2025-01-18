using UnityEngine;

public class Interactable : MonoBehaviour
{
    // 标志位：记录是否已经被交互
    public bool HasInteracted { get; set; } = false;

    // 引用一个隐藏的物体
    public GameObject hiddenObject;

    // 可选：在交互后修改物体的外观或状态
    public void OnInteracted()
    {
        // 例如改变物体的颜色
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.gray; // 交互后变为灰色
        }
    }
}
