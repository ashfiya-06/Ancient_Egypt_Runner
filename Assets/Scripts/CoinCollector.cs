
using UnityEngine;
using UnityEngine.SceneManagement;

public class CoinCollector : MonoBehaviour
{
    public int coinsCollected = 0;
    public float collectionDistance = 1.3f;
    public float obstacleDistance = 1.2f;

    private bool gameOver = false;
    private GUIStyle counterStyle;
    private GUIStyle gameOverStyle;
    private GUIStyle buttonStyle;

    void Update()
    {
        if (gameOver)
            return;

        GameObject[] objects = FindObjectsByType<GameObject>(
            FindObjectsSortMode.None
        );

        foreach (GameObject obj in objects)
        {
            if (obj == null)
                continue;

            Vector2 playerPosition = new Vector2(
                transform.position.x,
                transform.position.z
            );

            Vector2 objectPosition = new Vector2(
                obj.transform.position.x,
                obj.transform.position.z
            );

            float distance = Vector2.Distance(
                playerPosition,
                objectPosition
            );

            if (obj.name.Contains("Gold Coin"))
            {
                if (distance < collectionDistance)
                {
                    coinsCollected++;
                    Destroy(obj);
                    Debug.Log("Coins collected: " + coinsCollected);
                }
            }
            else if (
                obj.name.Contains("Egyptian Clay Pot") ||
                obj.name.Contains("Egyptian Stone Block") ||
                obj.name.Contains("Low Egyptian Stone Barrier")
            )
            {
                if (distance < obstacleDistance)
                {
                    EndGame();
                    return;
                }
            }
        }
    }

    void EndGame()
    {
        gameOver = true;
        Time.timeScale = 0f;
        Debug.Log("GAME OVER! Final Coins: " + coinsCollected);
    }

    void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void OnGUI()
    {
        if (counterStyle == null)
        {
            counterStyle = new GUIStyle(GUI.skin.label);
            counterStyle.fontSize = 30;
            counterStyle.fontStyle = FontStyle.Bold;
            counterStyle.normal.textColor = Color.yellow;

            gameOverStyle = new GUIStyle(GUI.skin.label);
            gameOverStyle.fontSize = 55;
            gameOverStyle.fontStyle = FontStyle.Bold;
            gameOverStyle.alignment = TextAnchor.MiddleCenter;
            gameOverStyle.normal.textColor = Color.red;

            buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.fontSize = 28;
            buttonStyle.fontStyle = FontStyle.Bold;
        }

        if (!gameOver)
        {
            GUI.Label(
                new Rect(25, 20, 350, 60),
                "COINS: " + coinsCollected,
                counterStyle
            );
        }
        else
        {
            GUI.Box(
                new Rect(0, 0, Screen.width, Screen.height),
                ""
            );

            GUI.Label(
                new Rect(0, Screen.height / 2 - 120,
                    Screen.width, 100),
                "GAME OVER",
                gameOverStyle
            );

            GUI.Label(
                new Rect(0, Screen.height / 2 - 35,
                    Screen.width, 60),
                "Your Coins: " + coinsCollected,
                counterStyle
            );

            if (GUI.Button(
                new Rect(
                    Screen.width / 2 - 110,
                    Screen.height / 2 + 45,
                    220,
                    65
                ),
                "RESTART",
                buttonStyle
            ))
            {
                RestartGame();
            }
        }
    }
}
