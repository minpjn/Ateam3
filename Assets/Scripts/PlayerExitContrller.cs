using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// =============================================
// PlayerExitController.cs
// 역할: NPC 처치 후 플레이어 퇴장 연출 담당
//       - 달리기 이미지로 교체
//       - 오른쪽으로 이동
//       - 화면 페이드 아웃/인
// 붙이는 곳: Hierarchy의 [PlayerImage] 오브젝트
// =============================================

public class PlayerExitController : MonoBehaviour
{
    [Header("플레이어 이미지 설정")]
    public Image playerImage;       // PlayerImage 오브젝트 연결
    public Sprite runningSprite;    // 달리기 이미지 (playerMotion1) 연결

    [Header("이동 설정")]
    public float exitSpeed = 1500f;   // 오른쪽으로 나가는 속도
    public float exitTargetX = 1500f; // 화면 밖 도착 지점 X좌표

    [Header("플레이어 시작 위치 - 직접 입력!")]
    // ★ Play 후 슬라이드 인 완료된 순간 일시정지해서
    //   PlayerImage의 Pos X, Pos Y 값을 여기에 직접 입력하세요!
    public Vector2 playerStartPos;

    [Header("페이드 설정")]
    public Image fadePanel;        // FadePanel 오브젝트 연결
    public float fadeDuration = 1.5f; // 페이드 걸리는 시간(초)

    private bool isExiting = false;        // 퇴장 중복 실행 방지용
    private Coroutine moveCoroutine;       // MoveRight 코루틴 추적용

    // -----------------------------------------------
    // StartExit() : RoundManager가 호출하는 함수
    // NPC가 죽으면 이 함수가 실행되어 퇴장 연출 시작
    // -----------------------------------------------
    public void StartExit(System.Action onComplete)
    {
        // 이미 퇴장 중이면 중복 실행 방지
        if (!isExiting)
            StartCoroutine(ExitCoroutine(onComplete));
    }

    IEnumerator ExitCoroutine(System.Action onComplete)
    {
        isExiting = true;

        // 1. 달리기 이미지로 교체
        playerImage.sprite = runningSprite;

        // 2. 페이드 아웃 + 오른쪽 이동 동시 시작
        StartCoroutine(FadeOut());
        moveCoroutine = StartCoroutine(MoveRight());

        // 3. 페이드 시간만큼 대기
        yield return new WaitForSeconds(fadeDuration);

        // 4. 이동 코루틴 강제 종료 (아직 실행 중일 경우 대비)
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        // 5. RoundManager에게 "퇴장 끝났다!" 알림
        onComplete?.Invoke();
    }

    // 플레이어를 오른쪽으로 이동시키는 함수
    IEnumerator MoveRight()
    {
        RectTransform rt = playerImage.rectTransform;
        Vector2 pos = rt.anchoredPosition;

        // exitTargetX에 도달할 때까지 매 프레임 이동
        while (pos.x < exitTargetX)
        {
            pos.x += exitSpeed * Time.deltaTime;
            rt.anchoredPosition = pos;
            yield return null; // 다음 프레임까지 대기
        }
    }

    // 화면을 점점 검게 만드는 함수 (알파 0 → 1)
    public IEnumerator FadeOut()
    {
        float elapsed = 0f;
        Color color = fadePanel.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsed / fadeDuration);
            fadePanel.color = color;
            yield return null;
        }

        // 완전히 검은 화면으로 고정
        color.a = 1f;
        fadePanel.color = color;
    }

    // 검은 화면을 점점 투명하게 만드는 함수 (알파 1 → 0)
    public IEnumerator FadeIn()
    {
        float elapsed = 0f;
        Color color = fadePanel.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            color.a = 1f - Mathf.Clamp01(elapsed / fadeDuration);
            fadePanel.color = color;
            yield return null;
        }

        // 완전히 투명하게 고정
        color.a = 0f;
        fadePanel.color = color;
    }

    // -----------------------------------------------
    // ResetPlayer() : 다음 라운드 시작 시 플레이어 초기화
    // RoundManager의 StartNextRound()에서 호출
    // -----------------------------------------------
    public void ResetPlayer(Sprite idleSprite)
    {
        // Inspector에서 입력한 시작 위치로 강제 복귀
        playerImage.rectTransform.anchoredPosition = playerStartPos;

        // 가만히 있는 이미지로 교체 (playerMotion2)
        playerImage.sprite = idleSprite;

        // 퇴장 플래그 초기화 (다음 퇴장 연출 가능하게)
        isExiting = false;
    }
}