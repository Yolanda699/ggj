using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathHandle : MonoBehaviour
{
    public List<GameObject> objectsToReactivate; // Objects to reactivate on respawn
    public Vector3 respawnPosition;  // Respawn position
    public GameObject deathPopupUI;

    private Renderer characterRenderer;
    private CharacterController cc;
    private bool isDead = false;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        characterRenderer = GetComponent<Renderer>();

        if (deathPopupUI != null)
        {
            deathPopupUI.SetActive(false);
        }

        respawnPosition = transform.position;
    }

    public void HandleDeath()
    {
        if (isDead) return; // 如果已经死亡，直接返回，避免重复触发复活逻辑

        Debug.Log("Player died!");

        if (deathPopupUI != null)
        {
            deathPopupUI.SetActive(true);
        }

        if (characterRenderer != null)
        {
            characterRenderer.enabled = false;
        }

        if (cc != null)
        {
            cc.enabled = false;
        }

        isDead = true;

        // 禁用玩家输入或其他控制
        DisablePlayerInputs();

        // 启动死亡延迟
        StartCoroutine(RespawnAfterDelay());
    }

    private void DisablePlayerInputs()
    {
        // 禁用输入逻辑，例如玩家的移动、攻击等
    }

    private IEnumerator RespawnAfterDelay()
    {
        yield return new WaitForSeconds(3f); // 死亡延迟

        // 复活时隐藏死亡UI
        if (deathPopupUI != null)
        {
            deathPopupUI.SetActive(false);
        }

        // Reactivate necessary objects after respawn
        foreach (GameObject obj in objectsToReactivate)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }

        // 复活玩家
        Respawn();
    }

    private void Respawn()
    {
        if (!isDead) return; // 确保只在死亡状态时复活

        transform.position = respawnPosition;

        // 恢复玩家控制
        if (characterRenderer != null)
        {
            characterRenderer.enabled = true;
        }

        if (cc != null)
        {
            cc.enabled = true;
        }

        isDead = false;

        // 恢复玩家输入
        EnablePlayerInputs();

        Debug.Log("Player respawned!");

        // 重置或重新激活拾取物品等
        ReactivatePickups();
    }

    private void EnablePlayerInputs()
    {
        // 恢复玩家输入控制
    }

    private void ReactivatePickups()
    {
        foreach (GameObject pickup in GameObject.FindGameObjectsWithTag("Pickup"))
        {
            pickup.SetActive(true); // 重置拾取物品状态
        }
    }
}
