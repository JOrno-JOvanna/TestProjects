using System.Collections;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonsScript : MonoBehaviour
{
    public void Start()
    {
        if (SceneManager.GetActiveScene().name == "View")
        {
            Screen.orientation = ScreenOrientation.AutoRotation;

            Screen.autorotateToPortraitUpsideDown = false;
            StartCoroutine(LoadImage());
        }
        else
        {
            Screen.orientation = ScreenOrientation.Portrait;
        }
    }

    IEnumerator LoadImage()
    {
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(PlayerPrefs.GetString("URL")))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Texture2D _texture = DownloadHandlerTexture.GetContent(www);
                Sprite _sprite = Sprite.Create(_texture, new Rect(0, 0, _texture.width, _texture.height), Vector2.zero);
                Canvas _canvas = FindObjectOfType<Canvas>();
                _canvas.transform.GetChild(0).GetComponent<Transform>().GetChild(0).GetComponent<Image>().sprite = _sprite;
                PlayerPrefs.DeleteKey("URL");
                yield return null;
            }
        }
    }

    public void LoadButton(int _index)
    {
        PlayerPrefs.SetInt("NumberOfScene", 1);
        SceneManager.LoadScene(_index);
    }

    public void BackButton()
    {
        PlayerPrefs.SetInt("NumberOfScene", 1);
        SceneManager.LoadScene(3);
    }
}
