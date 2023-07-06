using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadImages : MonoBehaviour
{
    public GameObject _prefab, _parent;
    public Text _loadingtext, _countingtext;
    public List<Image> _images;
    private List<string> _ImageURLs = new List<string>();
    private string _ImageURL;
    private bool _inst = false;

    public void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        foreach (Image _image in _images)
        {
            _image.AddComponent<Button>().onClick.AddListener(() => ListenerForImages(_image));
        }
        StartCoroutine(LoadingImages());
    }

    public void ContinueToDownload()
    {
        if(!_inst)
        {
            StartCoroutine(Continue());
            _inst = !_inst;
        }
    }

    public void ListenerForImages(Image _clickedimage)
    {
        PlayerPrefs.SetString("URL", _ImageURLs[_images.IndexOf(_clickedimage)]);
        PlayerPrefs.SetInt("NumberOfScene", 2);
        SceneManager.LoadScene(3);
    }

    public void BackButton()
    {
        PlayerPrefs.SetInt("NumberOfScene", 0);
        SceneManager.LoadScene(3);
    }

    IEnumerator LoadingImages()
    {
        _countingtext.text = _images.Count + "/66";
        int i;
        for (i = 1; i < 7; i++)
        {
            _ImageURL = "http://data.ikppbb.com/test-task-unity-data/pics/" + i + ".jpg";
            _ImageURLs.Add(_ImageURL);
            using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(_ImageURL))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.Success)
                {
                    Texture2D _texture = DownloadHandlerTexture.GetContent(www);
                    Sprite sprite = Sprite.Create(_texture, new Rect(0, 0, _texture.width, _texture.height), Vector2.zero);
                    _images[i - 1].sprite = sprite;
                    yield return null;
                }
            }
        }
    }

    IEnumerator Continue()
    {
        _loadingtext.text = "Загрузка";
        int _count = _images.Count;

        _ImageURL = "http://data.ikppbb.com/test-task-unity-data/pics/" + (_images.Count + 1) + ".jpg";
        _ImageURLs.Add(_ImageURL);
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(_ImageURL))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Texture2D _texture = DownloadHandlerTexture.GetContent(www);
                Sprite _sprite = Sprite.Create(_texture, new Rect(0, 0, _texture.width, _texture.height), Vector2.zero);
                _prefab.GetComponent<Image>().sprite = _sprite;
            }
        }

        if (_images.Count % 2 == 0)
        {
            Instantiate(_prefab, new Vector3(270, _images[_images.Count - 1].transform.position.y - 645), new Quaternion(), _parent.transform);
        }
        else
        {
            Instantiate(_prefab, new Vector3(865, _images[_images.Count - 2].transform.position.y - 645), new Quaternion(), _parent.transform);
        }

        _images.Add(_parent.transform.GetChild(_parent.transform.childCount - 1).GetComponent<Image>());
        foreach (Image _image in _images)
        {
            if (_image.GetComponent<Button>() == false)
            {
                _image.AddComponent<Button>().onClick.AddListener(() => ListenerForImages(_image));
            }
        }


        if (_images.Count > _count & _images.Count < 66)
        {
            _countingtext.text = _images.Count + "/66";
            _loadingtext.text = "";
            _inst = !_inst;
        }
        else if(_images.Count == 66)
        {
            _loadingtext.text = "";
            StopAllCoroutines();
        }
        yield return null;
    }
}
