using UnityEngine;
using System.Collections;

public class MovingBubble : MonoBehaviour
{
    public Vector3 startPosition; 
    public Vector3 endPosition;   
    public float moveDuration = 4f; // 移动持续时间（秒）
    public float minInterval = 1f; // 最小间隔时间（秒）
    public float maxInterval = 3f; // 最大间隔时间（秒）

    void Start()
    {
        // 初始化位置
        transform.position = startPosition;

        // 开始循环移动
        StartCoroutine(MoveAndResetRoutine());
    }

    IEnumerator MoveAndResetRoutine()
    {
        while (true)
        {
            float elapsedTime = 0f;

            // 从起点移动到终点
            while (elapsedTime < moveDuration)
            {
                float t = Mathf.SmoothStep(0, 1, elapsedTime / moveDuration);
                transform.position = Vector3.Lerp(startPosition, endPosition, t);
                elapsedTime += Time.deltaTime;
                yield return null; // 等待下一帧
            }

            // 确保位置到达终点
            transform.position = endPosition;

            // 等待一小段时间，确保移动完成
            yield return new WaitForSeconds(0.1f);

            // 重置回起点
            transform.position = startPosition;

            // 随机生成间隔时间
            float randomInterval = Random.Range(minInterval, maxInterval);

            // 等待随机间隔时间
            yield return new WaitForSeconds(randomInterval);
        }
    }
}
