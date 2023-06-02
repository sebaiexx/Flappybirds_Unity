using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;
using System;

public class Level : MonoBehaviour
{
    private const float CAMERA_ORTH_SIZE = 50f;
    private const float PIPE_WIDTH = 7.8f;
    private const float PIPE_HEAD_HEIGHT = .375f;
    private const float PIPE_MOVE_SPEED = 50f;
    private const float PIPE_DESTROY_X_POSITION = -220F;
    private const float PIPE_SPAWN_X_POSITION = 230F;

   /* private const float Ground_DESTROY_X_POSITION = -220F;
    private const float GROUND_SPAWN_X_POSITION = 230F;*/

    private const float BIRD_X_POSITION = 0f;

    private static Level instance;
    public static Level GetInstance()
    {
        return instance;
    }
    private List<Transform> groundList;
    private List <Pipe> pipeList;
    private int pipesPassedCount;
    private float pipeSpawnTimer;
    private float pipeSpawnTimerMax;
    private float gapSize;
    private int pipesSpawned;
    private State state;
    public enum Difficulty
    {
        Easy,
        Medium,
        Hard,
        Impossible,
    }
    private enum State
    {
        WaitingToStart,
        Playing,
        BirdDead,
    }
    private void Awake()
    {
        instance = this;
        groundList = new List<Transform>();
        pipeList = new List<Pipe>();
        pipeSpawnTimerMax = 1f;
        setDifficulty(Difficulty.Easy);
        state = State.WaitingToStart;
    }

    private void Start()
    {
        Bird.GetInstance().OnDied += Bird_OnDied;
        Bird.GetInstance().OnStartedPlaying += Bird_OnStatredPlaying;
       // CreateGapPipes(50f, 20f, 20f);
    }

    private void Bird_OnStatredPlaying(object sender, System.EventArgs e)
    {
        state = State.Playing;
    }

    private void Bird_OnDied(object sender,System.EventArgs e)
    {
        //CMDebug.TextPopupMouse("Dead!");
        state = State.BirdDead;


    }

    private void Update()
    {
        if (state == State.Playing)
        {
            HandlePipeMovement();
            HandlePipeSpawning();
            //HandleGround();
        }
    }

  /* private void SpawnInitialGround()
    {
        Transform groundTransform = Instantiate(GameAssets.Getinstance().pfGround, new Vector3(0, 0, 0), Quaternion.identity);
        groundList.Add(groundTransform);
        Transform groundTransform = Instantiate(GameAssets.Getinstance().pfGround, new Vector3(0, 0, 0), Quaternion.identity);
        groundList.Add(groundTransform);
        Transform groundTransform = Instantiate(GameAssets.Getinstance().pfGround, new Vector3(0, 0, 0), Quaternion.identity);
        groundList.Add(groundTransform);
        
    }
    private void HandleGround()
    {
        foreach(Transform groundtransform in groundList)
        {
            groundtransform.position += new Vector3(-1, 0, 0) * PIPE_MOVE_SPEED * Time.deltaTime;
        
        
        }
    }*/

    private void HandlePipeSpawning()
    {
        pipeSpawnTimer -= Time.deltaTime;
        if (pipeSpawnTimer < 0)
        {
            pipeSpawnTimer += pipeSpawnTimerMax;
            float heightEdgeLimit = 10f;
            float minHeight = gapSize * 0.5f+ heightEdgeLimit;
            float totalHeight = CAMERA_ORTH_SIZE * 2f;
            float maxHeight = totalHeight - gapSize * 0.5f - heightEdgeLimit;
            float height = UnityEngine.Random.Range(minHeight, maxHeight);
            CreateGapPipes(height, gapSize, PIPE_SPAWN_X_POSITION);

        }
    }
    private void HandlePipeSpawning(float heightEdgeLimit)
    {
        pipeSpawnTimer -= Time.deltaTime;
        if (pipeSpawnTimer < 0)
        {
            pipeSpawnTimer += pipeSpawnTimerMax;
            float minHeight = gapSize * 0.5f+ heightEdgeLimit;
            float totalHeight = CAMERA_ORTH_SIZE * 2f;
            float maxHeight = totalHeight - gapSize * 0.5f - heightEdgeLimit;
            float height = UnityEngine.Random.Range(minHeight, maxHeight);
            CreateGapPipes(height, gapSize, PIPE_SPAWN_X_POSITION);
        }
    }
    private void HandlePipeMovement()
    {
       for(int i = 0;i<pipeList.Count;i++)
        {
            Pipe pipe = pipeList[i];
            bool isToTheRightOfBird = pipe.GetXPosition() > BIRD_X_POSITION;
            pipe.Move();
            if(isToTheRightOfBird && pipe.GetXPosition() <= BIRD_X_POSITION && pipe.IsBottom())
            {
                pipesPassedCount++;
            }
            if(pipe.GetXPosition() < PIPE_DESTROY_X_POSITION)
            {
                pipe.DestroySelf();
                pipeList.Remove(pipe);
                i--;
            }
        }
    }
    private void setDifficulty(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                gapSize = 50f;
                pipeSpawnTimerMax = 1.2f;
                break;
            case Difficulty.Medium:
                gapSize = 40f;
                pipeSpawnTimerMax = 1.1f;
                break;
            case Difficulty.Hard:
                gapSize = 30f;
                pipeSpawnTimerMax = 1.0f;
                break;
            case Difficulty.Impossible:
                gapSize = 20f;
                pipeSpawnTimerMax = .9f; 
                break;

        }
    }
    private Difficulty GetDifficulty()
    {
        if (pipesSpawned >= 30) return Difficulty.Impossible;
        if (pipesSpawned >= 20) return Difficulty.Medium;
        if (pipesSpawned >= 10) return Difficulty.Hard;
        return Difficulty.Easy;

    }
    private void CreateGapPipes(float gapY, float gapSize,float xPosition)
    {
        CreatePipe(gapY - gapSize * 0.5f, xPosition, true);
        CreatePipe(CAMERA_ORTH_SIZE *2f - gapY - gapSize * 0.5f, xPosition, false);
        pipesSpawned++;
        setDifficulty(GetDifficulty());
    }
    private void CreatePipe(float height, float xPosition,bool createBottom)
    {
        Transform pipeHead = Instantiate(GameAssets.Getinstance().pfPipeHead);
        float pipeHeadYPosition;
        if (createBottom){
            pipeHeadYPosition = -CAMERA_ORTH_SIZE + height - PIPE_HEAD_HEIGHT * .5f;
        }
        else
        {
            pipeHeadYPosition = +CAMERA_ORTH_SIZE - height + PIPE_HEAD_HEIGHT * .5f;
        }
        pipeHead.position = new Vector3(xPosition, pipeHeadYPosition);
       

        Transform pipeBody = Instantiate(GameAssets.Getinstance().pfPipeBody);
        float pipeBodyYPosition;
        if (createBottom)
        {
            pipeBodyYPosition = -CAMERA_ORTH_SIZE;
        }
        else
        {
            pipeBodyYPosition = +CAMERA_ORTH_SIZE;
            pipeBody.localScale = new Vector3(1, -1, 1);
        }

        pipeBody.position = new Vector3(xPosition, pipeBodyYPosition);
        


        SpriteRenderer pipeBodySpriteRenderer = pipeBody.GetComponent<SpriteRenderer>();
        pipeBodySpriteRenderer.size = new Vector2(PIPE_WIDTH, height);

        BoxCollider2D pipeBodyCollider = pipeBody.GetComponent<BoxCollider2D>();
        pipeBodyCollider.size = new Vector2(PIPE_WIDTH, height);
        pipeBodyCollider.offset = new Vector2(0f, height * 0.5f);
        Pipe pipe = new Pipe(pipeHead, pipeBody, createBottom);
        pipeList.Add(pipe);
    }

    public int GetPipesSpawned()
    {
        return pipesSpawned; 
    }
    public int GetPipesPassedCount()
    {
        return pipesPassedCount;
    }
    /*
     * Represents a single pipe
     * */
    private class Pipe
    {
        private Transform pipeHeadTransform;
        private Transform pipeBodyTransform;
        private bool isBottom;
        public Pipe(Transform pipeHeadTransform,Transform pipeBodyTransform,bool isBottom)
        {
            this.pipeHeadTransform = pipeHeadTransform;
            this.pipeBodyTransform = pipeBodyTransform;
            this.isBottom = isBottom;
        }
        public void Move()
        {
            pipeHeadTransform.position += new Vector3(-1, 0, 0) * PIPE_MOVE_SPEED * Time.deltaTime;
            pipeBodyTransform.position += new Vector3(-1, 0, 0) * PIPE_MOVE_SPEED * Time.deltaTime;
        }
        public float GetXPosition()
        {
            return pipeHeadTransform.position.x;
        }
        public bool IsBottom()
        {
            return isBottom;
        }
        public void DestroySelf()
        {
            Destroy(pipeBodyTransform.gameObject);
            Destroy(pipeHeadTransform.gameObject);
        }
    }
}
