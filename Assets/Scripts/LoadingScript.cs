using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScript : MonoBehaviour
{
    [SerializeField] private Slider _loadingBar;
    [SerializeField] private Text _loadingText;

    public void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        StartCoroutine(Loading(PlayerPrefs.GetInt("NumberOfScene")));;
    }

    IEnumerator Loading(int _index)
    {
        PlayerPrefs.DeleteKey("NumberOfScene");
        float _progress = 0;
        while (_progress <= 1)
        {
            yield return new WaitForSeconds(0.1f);
            _progress += 0.1f;
            _loadingBar.value = _progress;
            _loadingText.text = "Загрузка " + string.Format("{0:0}%", _progress * 100f);
            if (_progress >= 1)
            {
                SceneManager.LoadScene(_index);
            }
            yield return null;
        }
    }
}
