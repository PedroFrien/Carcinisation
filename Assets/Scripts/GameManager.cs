using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool paused = false;
    public bool menuOpen = false;
    public bool inDialogue = false;


    private FPController player;

    [SerializeField] private GameObject pauseMenu;

    private Animator PMAnimator;
    private bool animActive;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (pauseMenu != null)
        {
            PMAnimator = pauseMenu.GetComponentInParent<Animator>();
            player = FindFirstObjectByType<FPController>();



            SetMouseActive(false);
            SetPause(false);
            pauseMenu.SetActive(false);
        }


    }

    // Update is called once per frame
    void Update()
    {

    }


    public void SetMouseActive(bool active)
    {
        if (active)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void SetPause(bool pause)
    {
        if (pause)
        {
            paused = true;

            SetPlayerMovement(false);


            Time.timeScale = 0;
        }

        else
        {
            paused = false;

            if (!inDialogue) SetPlayerMovement(true);



            Time.timeScale = 1;
        }
    }

    public void SetTimeScale(float scale)
    {
        Time.timeScale = scale;
    }

    public void SetPauseMenu(bool pause)
    {
        if (animActive) return;
        animActive = true;

        StartCoroutine(PauseAnim(pause));
    }

    private IEnumerator PauseAnim(bool active)
    {
        pauseMenu.SetActive(true);
        PMAnimator.SetBool("Active", active);
        SetMouseActive(active);
        SetPause(active);

        float animationLength = .40f; // Get actual animation length
        float elapsedTime = 0f;

        while (elapsedTime < animationLength)
        {
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }


        SetMouseActive(active);
        SetPause(active);
        pauseMenu.SetActive(active);

        animActive = false;

    }

    public void SetPlayerMovement(bool active)
    {
        if (player == null) return;
        if (active)
        {
            player.MovementEnabled = true;
            player.CameraEnabled = true;
            player.LookEnabled = true;
        }
        else
        {
            player.MovementEnabled = false;
            player.CameraEnabled = false;
            player.LookEnabled = false;
        }
    }

    public void SetDialogue(bool start)
    {
        inDialogue = start;
        if (start)
        {
            SetMouseActive(true);
            SetPlayerMovement(false);
        }
        else
        {
            SetMouseActive(false);
            SetPlayerMovement(true);
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
