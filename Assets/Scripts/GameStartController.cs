using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameStartController : MonoBehaviour
{
    public PlayerExitController playerExitController;

    public TMP_Text dialogueText;
    public TMP_Text characterText;
    public GameObject dialogueBox;

    public Image playerImage;
    public Image enemyImage;
    public Image centerImage;

    public Sprite playerMotion1;
    public Sprite playerMotion2;
    public Sprite enemyMotionA;
    public Sprite enemyMotionB;

    public Sprite ruReady;
    public Sprite count3;
    public Sprite count2;
    public Sprite count1;
    public Sprite countStart;

    bool started = false;

    void Start()
    {
        dialogueBox.SetActive(false);
        characterText.text = "";
        centerImage.gameObject.SetActive(false);
        playerImage.gameObject.SetActive(false);
        enemyImage.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!started && Input.GetKeyDown(KeyCode.Space))
        {
            started = true;
            StartCoroutine(StartSequence());
        }
    }

    IEnumerator StartSequence()
    {
        // 1. 플레이어 등장
        playerImage.sprite = playerMotion1;
        playerImage.gameObject.SetActive(true);
        yield return StartCoroutine(SlideIn(
            playerImage.rectTransform,
            -1500,
            playerImage.rectTransform.anchoredPosition.x,
            0.5f));


        yield return new WaitForSeconds(0.3f);

        // 2. 적 등장
        enemyImage.sprite = enemyMotionA;
        enemyImage.gameObject.SetActive(true);
        yield return StartCoroutine(SlideIn(
            enemyImage.rectTransform,
            1500,
            enemyImage.rectTransform.anchoredPosition.x,
            0.5f));

        yield return new WaitForSeconds(0.3f);

        // 3. 적 모션 변경 + 또잉
        enemyImage.sprite = enemyMotionB;
        yield return StartCoroutine(BounceEffect(enemyImage.rectTransform));

        dialogueBox.SetActive(true);
        characterText.text = "character text!";

        yield return new WaitForSeconds(1f);

        // 4. 플레이어 모션 변경 + 또잉
        playerImage.sprite = playerMotion2;
        yield return StartCoroutine(BounceEffect(playerImage.rectTransform));

        yield return new WaitForSeconds(1f);

        // 5. 대사창 숨김
        dialogueBox.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        // 6. Ready 이미지
        centerImage.gameObject.SetActive(true);
        centerImage.sprite = ruReady;
        yield return new WaitForSeconds(1.3f);
        centerImage.gameObject.SetActive(false);

        yield return new WaitForSeconds(0f);

        // 7. 카운트다운
        yield return StartCoroutine(Countdown());

        Debug.Log("게임시작!");
    }

    IEnumerator SlideIn(RectTransform target, float startX, float endX, float duration)
    {
        float elapsed = 0f;
        Vector2 pos = target.anchoredPosition;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(elapsed / duration);
            t = Mathf.SmoothStep(0, 1, t);

            pos.x = Mathf.Lerp(startX, endX, t);
            target.anchoredPosition = pos;

            yield return null;
        }

        pos.x = endX;
        target.anchoredPosition = pos;
    }

    IEnumerator Countdown()
    {
        centerImage.gameObject.SetActive(true);

        centerImage.sprite = count3;
        yield return new WaitForSeconds(0.4f);

        centerImage.sprite = count2;
        yield return new WaitForSeconds(0.4f);

        centerImage.sprite = count1;
        yield return new WaitForSeconds(0.4f);

        centerImage.sprite = countStart;
        yield return new WaitForSeconds(0.8f);

        centerImage.gameObject.SetActive(false);
    }

    // ★ 추가된 또잉 애니메이션 함수
    IEnumerator BounceEffect(RectTransform target)
    {
        Vector3 originalScale = target.localScale;
        Vector3 bigScale = originalScale * 1.15f;

        float time = 0f;

        // 커지기
        while (time < 0.1f)
        {
            time += Time.deltaTime;
            target.localScale = Vector3.Lerp(originalScale, bigScale, time / 0.1f);
            yield return null;
        }

        time = 0f;

        // 다시 줄어들기
        while (time < 0.1f)
        {
            time += Time.deltaTime;
            target.localScale = Vector3.Lerp(bigScale, originalScale, time / 0.1f);
            yield return null;
        }

        target.localScale = originalScale;
    }
}