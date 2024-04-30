using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loadScene : MonoBehaviour
{
    public void SingleStart()
    {
        SceneManager.LoadScene("Tetris 1P");
    }

    public void TouStart()
    {
        SceneManager.LoadScene("Tetris 1P Tou");
    }

    public void TwoStart()
    {
        SceneManager.LoadScene("Tetris 2P");
    }

    public void SingleFinishGame()
    {
        SceneManager.LoadScene("Single End");
    }

    public void FinishGame2()
    {
        SceneManager.LoadScene("P2 End");
    }
    public void RestartGame()
    {
        SceneManager.LoadScene("Menu");
    }
    public void ExitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
