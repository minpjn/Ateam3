using UnityEngine;
using UnityEngine.UI;

// =============================================
// NPCHealth.cs
// 역할: NPC의 HP를 관리하는 스크립트
// 붙이는 곳: Hierarchy의 [EnemyImage] 오브젝트
// =============================================

public class NPCHealth : MonoBehaviour
{
    [Header("NPC HP 설정")]
    public int maxHP = 100;    // NPC 최대 HP (Inspector에서 변경 가능)
    public int currentHP;      // 현재 HP (게임 중에 실시간으로 변함)

    [Header("연결할 오브젝트")]
    public RoundManager roundManager; // RoundManager 오브젝트를 Inspector에서 연결

    void Start()
    {
        // 게임 시작 시 현재 HP를 최대 HP로 초기화
        currentHP = maxHP;
    }

    // -----------------------------------------------
    // TakeDamage() : 공격 성공 시 외부에서 호출하는 함수
    // 사용법: npcHealth.TakeDamage(30); → 30 데미지
    // -----------------------------------------------
    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        // HP가 0 밑으로 내려가지 않게 고정
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        Debug.Log($"NPC HP: {currentHP}/{maxHP}");

        // HP가 0이 되면 RoundManager에게 "NPC 죽었다!" 알림
        if (currentHP <= 0)
        {
            Debug.Log("NPC 쓰러짐! 라운드 전환 시작");
            roundManager.OnNPCDefeated();
        }
    }
}