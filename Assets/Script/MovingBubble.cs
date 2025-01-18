using UnityEngine;
using System.Collections;

public class MovingBubble : MonoBehaviour
{
    public Vector3 startPosition; 
    public Vector3 endPosition;   
    public float moveDuration = 4f; // �ƶ�����ʱ�䣨�룩
    public float minInterval = 1f; // ��С���ʱ�䣨�룩
    public float maxInterval = 3f; // �����ʱ�䣨�룩

    void Start()
    {
        // ��ʼ��λ��
        transform.position = startPosition;

        // ��ʼѭ���ƶ�
        StartCoroutine(MoveAndResetRoutine());
    }

    IEnumerator MoveAndResetRoutine()
    {
        while (true)
        {
            float elapsedTime = 0f;

            // ������ƶ����յ�
            while (elapsedTime < moveDuration)
            {
                float t = Mathf.SmoothStep(0, 1, elapsedTime / moveDuration);
                transform.position = Vector3.Lerp(startPosition, endPosition, t);
                elapsedTime += Time.deltaTime;
                yield return null; // �ȴ���һ֡
            }

            // ȷ��λ�õ����յ�
            transform.position = endPosition;

            // �ȴ�һС��ʱ�䣬ȷ���ƶ����
            yield return new WaitForSeconds(0.1f);

            // ���û����
            transform.position = startPosition;

            // ������ɼ��ʱ��
            float randomInterval = Random.Range(minInterval, maxInterval);

            // �ȴ�������ʱ��
            yield return new WaitForSeconds(randomInterval);
        }
    }
}
