using UnityEngine;

public class Interactable : MonoBehaviour
{
    // ��־λ����¼�Ƿ��Ѿ�������
    public bool HasInteracted { get; set; } = false;

    // ����һ�����ص�����
    public GameObject hiddenObject;

    // ��ѡ���ڽ������޸��������ۻ�״̬
    public void OnInteracted()
    {
        // ����ı��������ɫ
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.gray; // �������Ϊ��ɫ
        }
    }
}
