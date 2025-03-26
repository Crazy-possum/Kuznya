using System;
using UnityEngine;

[Serializable]
public class ItemSprites
{
    [SerializeField] private Sprite _stage0Sprite;
    [SerializeField] private Sprite _stage1Sprite;
    [SerializeField] private Sprite _stage2Sprite;
    [SerializeField] private Sprite _stage3Sprite;
    [SerializeField] private Sprite _stage4Sprite;

    public Sprite Stage0Sprite => _stage0Sprite;

    public Sprite Stage1Sprite => _stage1Sprite;

    public Sprite Stage2Sprite => _stage2Sprite;

    public Sprite Stage3Sprite => _stage3Sprite;

    public Sprite Stage4Sprite => _stage4Sprite;
}