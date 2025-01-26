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
        if (isDead) return; // ����Ѿ�������ֱ�ӷ��أ������ظ����������߼�

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

        // ��������������������
        DisablePlayerInputs();

        // ���������ӳ�
        StartCoroutine(RespawnAfterDelay());
    }

    private void DisablePlayerInputs()
    {
        // ���������߼���������ҵ��ƶ���������
    }

    private IEnumerator RespawnAfterDelay()
    {
        yield return new WaitForSeconds(3f); // �����ӳ�

        // ����ʱ��������UI
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

        // �������
        Respawn();
    }

    private void Respawn()
    {
        if (!isDead) return; // ȷ��ֻ������״̬ʱ����

        transform.position = respawnPosition;

        // �ָ���ҿ���
        if (characterRenderer != null)
        {
            characterRenderer.enabled = true;
        }

        if (cc != null)
        {
            cc.enabled = true;
        }

        isDead = false;

        // �ָ��������
        EnablePlayerInputs();

        Debug.Log("Player respawned!");

        // ���û����¼���ʰȡ��Ʒ��
        ReactivatePickups();
    }

    private void EnablePlayerInputs()
    {
        // �ָ�����������
    }

    private void ReactivatePickups()
    {
        foreach (GameObject pickup in GameObject.FindGameObjectsWithTag("Pickup"))
        {
            pickup.SetActive(true); // ����ʰȡ��Ʒ״̬
        }
    }
}
