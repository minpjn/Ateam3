using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class DialogueManager : MonoBehaviour
{

    public PlayerController playerController;


    [Header("대사")]
    public TextMeshProUGUI dialogueText;

    public string[] dialogues;

    private int currentIndex = 0;

    [Header("씬 이동")]
    public SceneLoader sceneLoader;

    [Header("플레이어")]
    public Image playerImage;

    public Sprite runningSprite;

    [Header("대사창")]
    public GameObject dialogueBox;

    [Header("페이드")]
    public Image fadePanel;

    [Header("발소리")]
    public AudioSource footstepAudio;

    [Header("이동 설정")]
    public float moveDistance = 2000f;

    public float moveDuration = 2f;

    private bool isEnding = false;

    void Start()
    {
        ShowDialogue();

        Color color = fadePanel.color;
        color.a = 0f;
        fadePanel.color = color;
    }

    void Update()
    {
        if (isEnding)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            NextDialogue();
        }
    }

    void ShowDialogue()
    {
        dialogueText.text = dialogues[currentIndex];
    }

    void NextDialogue()
    {
        currentIndex++;

        if (currentIndex < dialogues.Length)
        {
            ShowDialogue();
        }
        else
        {
            StartCoroutine(EndingSequence());
        }
    }

    IEnumerator EndingSequence()
    {
        // 대사창 숨기기
        dialogueBox.SetActive(false);

        // 플레이어 달리기 이미지
        playerController.SetRunning();

        // 페이드 아웃 동시에 시작
        StartCoroutine(
            playerController.FadeOut()
        );

        // 오른쪽 화면 밖으로 이동
        yield return StartCoroutine(
            playerController.MoveToPosition(
                new Vector2(1800, -200)
            )
        );

        // GameScene 이동
        sceneLoader.LoadGameScene();
    }
}