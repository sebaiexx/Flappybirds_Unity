using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    private static GameAssets instance;
    public static GameAssets Getinstance()
    {
        return instance;
    }
    private void Awake()
    {
        instance = this;
    } 

    public Sprite pipeHeadSprite;
    public Transform pfPipeBody;
    public Transform pfPipeHead;
    public Transform pfGround;
    
    public AudioClip birdJump;

}
