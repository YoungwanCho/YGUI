using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

[CustomEditor(typeof(ImageSelector))]
[CanEditMultipleObjects]
public class ImageSelectorEditor : Editor
{
    private Sprite[] _emptySprite = new Sprite[0];
    private string[] _emptyString = new string[0];

    private ImageSelector _imageSelector;
    private Image _image;

    public void OnEnable()
    {
        //Debug.Log("ImageSelectorEditor OnEnable");
        GameObject go = Selection.activeGameObject;
        Debug.Log(go.name);
        _imageSelector = go.GetComponent<ImageSelector>();
        _image = go.GetComponent<Image>();
        if (_image == null)
        {
            _image = go.AddComponent<Image>();
        }

        _imageSelector.CurIndex = GetSpriteSelectIndex();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (_imageSelector.CurAtlas == null) // 아틀라스가 없는경우
        {
            //Debug.Log("Atlas is None");
            _imageSelector.SpriteArray = _emptySprite;
            _imageSelector.SpriteNameArray = _emptyString;
            _imageSelector.SpriteCount = 0;
            _imageSelector.PreIndex = -1;
            _imageSelector.CurIndex = 0;
            _imageSelector.PreAtlas = null;
            _imageSelector.CurAtlas = null;
            _image = null;
        }
        else
        {
            //Debug.LogFormat("Atals Name : {0}, Sprite Count : {1}", _imageSelector.CurAtlas.name, _imageSelector.CurAtlas.spriteCount);
            _imageSelector.SpriteCount = _imageSelector.CurAtlas.spriteCount;
            if (_imageSelector.SpriteCount > 0)
            {
                if (_imageSelector.PreAtlas != null)
                {
                    //Debug.Log("There is a previously selected atlas");
                    if (_imageSelector.PreAtlas == _imageSelector.CurAtlas) // 아틀라스가 바뀌지 않은 경우
                    {
                        //Debug.Log("Atlas does not change");
                        _imageSelector.PreIndex = _imageSelector.CurIndex;
                        _imageSelector.CurIndex = EditorGUILayout.Popup("Sprite", _imageSelector.CurIndex, _imageSelector.SpriteNameArray);

                        if (_imageSelector.PreIndex == _imageSelector.CurIndex) // 스프라이트가 바뀌지 않은 경우
                        {

                        }
                        else //스프라이트가 바뀐경우
                        {
                            //Debug.Log("Atlas does not change, only sprite changes");
                            _image.sprite = _imageSelector.SpriteArray[_imageSelector.CurIndex];
                            _image.SetNativeSize();
                            _imageSelector.PreIndex = _imageSelector.CurIndex;
                        }
                    }
                    else // 아틀라스가 바뀐경우
                    {
                        //Debug.Log("Atlas changed");
                        _imageSelector.PreAtlas = _imageSelector.CurAtlas;
                        _imageSelector.PreIndex = 0;
                        _imageSelector.CurIndex = 0;
                        _imageSelector.SpriteArray = new Sprite[_imageSelector.SpriteCount];
                        _imageSelector.SpriteNameArray = new string[_imageSelector.SpriteCount];

                        _imageSelector.CurAtlas.GetSprites(_imageSelector.SpriteArray);

                        for (int i = 0; i < _imageSelector.SpriteCount; i++)
                        {
                            _imageSelector.SpriteNameArray[i] = _imageSelector.SpriteArray[i].name;
                            //Debug.LogFormat("Sprite Name : {0}", _imageSelector.SpriteArray[i].name);
                        }
                        _imageSelector.PreIndex = _imageSelector.CurIndex;
                        EditorGUILayout.Popup("Sprite", _imageSelector.CurIndex, _imageSelector.SpriteNameArray);
                        _image.sprite = _imageSelector.SpriteArray[_imageSelector.CurIndex];
                        _image.SetNativeSize();
                    }
                }
                else
                {
                    //Debug.Log("You have no previously selected atlas.");
                    _imageSelector.PreAtlas = _imageSelector.CurAtlas;
                    _imageSelector.PreIndex = _imageSelector.CurIndex;

                    _imageSelector.SpriteArray = new Sprite[_imageSelector.SpriteCount];
                    _imageSelector.SpriteNameArray = new string[_imageSelector.SpriteCount];
                    _imageSelector.CurAtlas.GetSprites(_imageSelector.SpriteArray);

                    for (int i = 0; i < _imageSelector.SpriteCount; i++)
                    {
                        _imageSelector.SpriteNameArray[i] = _imageSelector.SpriteArray[i].name;
                        //Debug.LogFormat("Sprite Name : {0}", _imageSelector.SpriteArray[i].name);
                    }
                    _imageSelector.CurIndex = EditorGUILayout.Popup("Sprite", _imageSelector.CurIndex, _imageSelector.SpriteNameArray);
                    _image.sprite = _imageSelector.SpriteArray[_imageSelector.CurIndex];
                    _image.SetNativeSize();
                }
            }
            else // 아틀라스는 있지만 아틀라스안에 스프라이트가 없는 경우
            {
                //Debug.LogWarning("There are no sprites in the Atlas.");
                _imageSelector.SpriteArray = _emptySprite;
                _imageSelector.SpriteNameArray = _emptyString;
                _imageSelector.SpriteCount = 0;
                _imageSelector.PreIndex = -1;
                _imageSelector.CurIndex = 0;
                _imageSelector.PreAtlas = null;
                _imageSelector.CurAtlas = null;
                _image.sprite = null;
            }
        }
    }

    public int GetSpriteSelectIndex()
    {
        int result = 0;
        if (_imageSelector.CurAtlas != null && _image != null && _image.sprite != null)
        {
            if(_image != null)
            {
                Sprite[] spArr = new Sprite[_imageSelector.CurAtlas.spriteCount];
                _imageSelector.CurAtlas.GetSprites(spArr);
                for (int i = 0; i < spArr.Length; i++)
                {
                    if(spArr[i].name == _image.sprite.name)
                    {
                        result = i;
                        break;
                    }
                }
            }
        }
        return result;
    }

}
