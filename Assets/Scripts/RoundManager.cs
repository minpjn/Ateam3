using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// =============================================
// RoundManager.cs
// 역할: 게임 전체 라운드를 관리하는 스크립트
//       - NPC 처치 감지
//       - 라운드 전환 (배경/NPC 이미지 교체)
//       - HP 초기화
// 붙이는 곳: Hierarchy의 [RoundManager] 빈 오브젝트
// =============================================

public class RoundManager : MonoBehaviour
{
    [Header("라운드 설정")]
    public int totalRounds = 3;  // 총 라운드 수
    public int currentRound = 1; // 현재 라운드 (1부터 시작)

    [Header("라운드별 배경 이미지 - 순서대로 연결!")]
    // Size를 3으로 설정 후 Element 0,1,2에 각 라운드 배경 연결
    public Sprite[] backgroundSprites;

    [Header("라운드별 NPC 이미지 - 순서대로 연결!")]
    // Size를 3으로 설정 후 Element 0,1,2에 각 라운드 NPC 이미지 연결
    public Sprite[] npcSprites;

    [Header("연결할 오브젝트들")]
    public PlayerExitController playerExit;  // PlayerImage 오브젝트 연결
    public Image backgroundImage;            // Background 오브젝트 연결
    public Image npcImage;                   // EnemyImage 오브젝트 연결
    public Image playerImage;                // PlayerImage 오브젝트 연결
    public NPCHealth npcHealth;              // EnemyImage 오브젝트 연결

    [Header("플레이어 가만히 이미지")]
    // playerMotion2 이미지를 여기에 연결
    public Sprite playerIdleSprite;

    // -----------------------------------------------
    // OnNPCDefeated() : NPCHealth에서 HP가 0이 되면 호출
    // -----------------------------------------------
    public void OnNPCDefeated()
    {
        // PlayerExitController에게 퇴장 시작 명령
        // 퇴장이 끝나면 OnExitComplete() 자동 호출
        playerExit.StartExit(OnExitComplete);
    }

    // 퇴장 연출이 완전히 끝난 후 자동으로 호출되는 함수
    void OnExitComplete()
    {
        currentRound++; // 라운드 증가

        if (currentRound > totalRounds)
        {
            // 모든 라운드 클리어!
            Debug.Log("게임 클리어!");
            // TODO: 엔딩 처리 여기에 추가
        }
        else
        {
            // 다음 라운드 시작
            StartCoroutine(StartNextRound());
        }
    }

    // 다음 라운드를 준비하는 함수
    IEnumerator StartNextRound()
    {
        Debug.Log($"=== {currentRound}라운드 준비 중 ===");

        // 1. 배경 이미지 교체
        if (backgroundSprites.Length >= currentRound)
        {
            backgroundImage.sprite = backgroundSprites[currentRound - 1];
            Debug.Log("배경 교체 완료");
        }

        // 2. NPC 이미지 교체 + 보이게 설정
        if (npcSprites.Length >= currentRound)
        {
            npcImage.sprite = npcSprites[currentRound - 1];
            npcImage.gameObject.SetActive(true);

            // 혹시 알파값이 0이 됐을 경우 강제로 1로 설정
            Color nc = npcImage.color;
            nc.a = 1f;
            npcImage.color = nc;
        }

        // 3. NPC HP 초기화
        npcHealth.currentHP = npcHealth.maxHP;

        // 4. 플레이어 시작 위치로 복귀 + 가만히 이미지로 교체
        playerExit.ResetPlayer(playerIdleSprite);
        playerImage.gameObject.SetActive(true);

        // 혹시 알파값이 0이 됐을 경우 강제로 1로 설정
        Color pc = playerImage.color;
        pc.a = 1f;
        playerImage.color = pc;

        // 5. 페이드 인 (검은 화면 → 서서히 밝아짐)
        yield return StartCoroutine(playerExit.FadeIn());

        Debug.Log($"=== {currentRound}라운드 시작! ===");
        // TODO: 여기서 다음 라운드 대화 시작 신호 추가
    }
}