using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // 메인 메뉴 → 스토리씬
    // 엔딩씬 → 스토리씬 [다시하기 버튼]
    public void LoadStoryScene()
    {
        SceneManager.LoadScene("StoryScene");
    }

    // 스토리씬 → 게임씬
    public void LoadGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    // 엔딩씬 → 메인씬 [나가기 버튼]
    public void LoadMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }
}