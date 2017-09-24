using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class ImageSelector : MonoBehaviour
{
    public SpriteAtlas CurAtlas { set { _atlas = value; } get { return _atlas; } }
    public SpriteAtlas PreAtlas { set; get; }
    public Sprite[] SpriteArray { set; get; }
    public string[] SpriteNameArray { set; get; }

    public int CurIndex { set; get; }
    public int PreIndex { set; get; }
    public int SpriteCount { set; get; }

    [SerializeField]
    private SpriteAtlas _atlas = null;

}
