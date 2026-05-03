using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// =====================================================
// PlayerController.cs
// 역할:
// - 플레이어 컷신 이동
// - 등장 / 퇴장 연출
// - 페이드 인/아웃
// - 플레이어 위치 초기화
//
// 붙이는 곳:
// Player 오브젝트
// =====================================================

public class PlayerController : MonoBehaviour
{
    [Header("플레이어 이미지")]
    public Image playerImage;

    [Header("플레이어 스프라이트")]
    public Sprite idleSprite;
    public Sprite runningSprite;

    [Header("페이드 패널")]
    public Image fadePanel;

    [Header("이동 속도")]
    public float moveSpeed = 1500f;

    [Header("페이드 시간")]
    public float fadeDuration = 1.5f;

    // 이동 중복 방지
    private bool isMoving = false;

    // =====================================================
    // 특정 위치로 이동
    // =====================================================
    public IEnumerator MoveToPosition(
        Vector2 targetPos,
        bool useRunningSprite = true
    )
    {
        isMoving = true;

        RectTransform rt = playerImage.rectTransform;

        // 달리기 이미지 변경
        if (useRunningSprite)
        {
            playerImage.sprite = runningSprite;
        }

        while (Vector2.Distance(rt.anchoredPosition, targetPos) > 5f)
        {
            rt.anchoredPosition =
                Vector2.MoveTowards(
                    rt.anchoredPosition,
                    targetPos,
                    moveSpeed * Time.deltaTime
                );

            yield return null;
        }

        rt.anchoredPosition = targetPos;

        // 기본 이미지 복귀
        if (idleSprite != null)
        {
            playerImage.sprite = idleSprite;
        }

        isMoving = false;
    }

    // =====================================================
    // 페이드 아웃 (검정 화면)
    // =====================================================
    public IEnumerator FadeOut()
    {
        float elapsed = 0f;

        Color color = fadePanel.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;

            color.a =
                Mathf.Clamp01(elapsed / fadeDuration);

            fadePanel.color = color;

            yield return null;
        }

        color.a = 1f;
        fadePanel.color = color;
    }

    // =====================================================
    // 페이드 인 (화면 복귀)
    // =====================================================
    public IEnumerator FadeIn()
    {
        float elapsed = 0f;

        Color color = fadePanel.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;

            color.a =
                1f - Mathf.Clamp01(elapsed / fadeDuration);

            fadePanel.color = color;

            yield return null;
        }

        color.a = 0f;
        fadePanel.color = color;
    }

    // =====================================================
    // 플레이어 위치 즉시 변경
    // =====================================================
    public void SetPosition(Vector2 newPos)
    {
        playerImage.rectTransform.anchoredPosition = newPos;
    }

    // =====================================================
    // 플레이어 이미지 변경
    // =====================================================
    public void SetIdle()
    {
        playerImage.sprite = idleSprite;
    }

    public void SetRunning()
    {
        playerImage.sprite = runningSprite;
    }

    // =====================================================
    // 현재 이동 중인지 확인
    // =====================================================
    public bool IsMoving()
    {
        return isMoving;
    }
}