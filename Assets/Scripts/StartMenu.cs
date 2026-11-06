using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    void OnGUI()
    {
        GUIStyle titleStyle = new GUIStyle();
        titleStyle.fontSize = 55;
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.alignment = TextAnchor.MiddleCenter;
        titleStyle.normal.textColor = Color.yellow;

        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.fontSize = 35;
        buttonStyle.fontStyle = FontStyle.Bold;

        GUIStyle subtitleStyle = new GUIStyle();
        subtitleStyle.fontSize = 22;
        subtitleStyle.alignment = TextAnchor.MiddleCenter;
        subtitleStyle.normal.textColor = Color.white;

        GUI.Label(
            new Rect(0, 100, Screen.width, 100),
            "ANCIENT EGYPT RUNNER",
            titleStyle
        );

        GUI.Label(
            new Rect(0, 210, Screen.width, 50),
            "Travel through the land of Pharaohs!",
            subtitleStyle
        );

        if (GUI.Button(
            new Rect(Screen.width / 2 - 120, 320, 240, 75),
            "PLAY",
            buttonStyle
        ))
        {
            SceneManager.LoadScene(1);
        }
    }
}