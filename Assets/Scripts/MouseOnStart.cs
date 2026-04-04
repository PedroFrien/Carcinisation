using UnityEngine;

public class MouseOnStart : MonoBehaviour
{
    private GameManager gameManager;

    [SerializeField] private bool mouseOnStart = true;

    public bool Win;
    public bool Lose;

    private void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        gameManager.SetPause(false);

        if (mouseOnStart)
        { 
            gameManager.SetMouseActive(true);
        }
        else
        {
            gameManager.SetMouseActive(false);
        }
        
        if (Win)
        {
            FindFirstObjectByType<AudioManager>().PlaySound("Win", transform.position, gameObject);
        }
        else if (Lose)
        {
            FindFirstObjectByType<AudioManager>().PlaySound("Lose", transform.position, gameObject);
        }

    }
}
