using UnityEngine;

public class TimeShield : MonoBehaviour
{
    // ��������ײʱ����
    private void OnTriggerEnter(Collider other)
    {
        // �����ײ�������Ƿ���tagΪ"damage2"
        if (other.CompareTag("Damage2"))
        {
            // �����ʾ��Ϣ
            Debug.Log("Damage2���屻ʱ�令�ܻ��У��ݻ����壡");

            // �ݻ���ײ����
            Destroy(other.gameObject);
        }
    }
}


