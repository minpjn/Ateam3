using UnityEngine;

public class TestManager : MonoBehaviour
{
    public NPCHealth npcHealth;

    void Update()
    {
        // 스페이스바 누르면 NPC 즉시 사망
        if (Input.GetKeyDown(KeyCode.A))
        {
            npcHealth.TakeDamage(9999);
            Debug.Log("테스트: NPC 즉사!");
        }
    }
}