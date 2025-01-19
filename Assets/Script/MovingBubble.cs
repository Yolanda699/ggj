using UnityEngine;
using System.Collections;

public class MovingBubble : MonoBehaviour
{
    public Vector3 startPosition;
    public Vector3 endPosition;
    public float moveDuration = 4f; // 移动持续时间（秒）
    public float minInterval = 1f; // 最小间隔时间（秒）
    public float maxInterval = 3f; // 最大间隔时间（秒）

    [Tooltip("AudioSource for bubble sound.")]
    public AudioSource bubbleAudioSource; // 泡泡音效
    public Transform player; // 玩家 Transform
    public float maxDistance = 10f; // 声音完全消失的最大距离

    void Start()
    {
        // 初始化位置
        transform.position = startPosition;

        // 开始循环移动
        StartCoroutine(MoveAndResetRoutine());
    }

    void Update()
    {
        // 根据玩家距离调整音量
        AdjustAudioVolumeBasedOnDistance();
    }

    IEnumerator MoveAndResetRoutine()
    {
        while (true)
        {
            float elapsedTime = 0f;

            // 在泡泡开始移动时播放音效
            if (bubbleAudioSource != null)
            {
                bubbleAudioSource.Play();
            }

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

            // 停止音效（如果需要停止）
            if (bubbleAudioSource != null)
            {
                bubbleAudioSource.Stop();
            }

            // 等待一小段时间，确保移动完成
            yield return new WaitForSeconds(0.1f);

            // 重置到起点
            transform.position = startPosition;

            // 随机生成间隔时间
            float randomInterval = Random.Range(minInterval, maxInterval);

            // 等待随机间隔时间
            yield return new WaitForSeconds(randomInterval);
        }
    }

    private void AdjustAudioVolumeBasedOnDistance()
    {
        if (bubbleAudioSource == null || player == null) return;

        // 计算玩家与泡泡的距离
        float distance = Vector3.Distance(player.position, transform.position);

        // 如果玩家距离小于 maxDistance，则根据距离调整音量；否则音量为 0
        if (distance < maxDistance)
        {
            float volume = Mathf.Lerp(1f, 0f, distance / maxDistance); // 距离越远音量越小
            bubbleAudioSource.volume = volume;
        }
        else
        {
            bubbleAudioSource.volume = 0f;
        }
    }
}
