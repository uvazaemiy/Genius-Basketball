using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private Transform UpBorder;
    [SerializeField] private Transform DownBorder;
    [SerializeField] private Transform[] SpawnPositions;
    public Animation ShieldAnim;
    [Header("Obstacles")]
    [SerializeField] Transform ObstaclesParent;
    public Transform CannonBallPosition;
    public Transform CannonDirectionPosition;
    public Animation CannonBombAnimation;
    [Space]
    [Header("Logic")]
    [SerializeField] ParticleSystem particles;
    public bool allowDrag;
    public bool isGame;
    public bool finish;
    public int ObjectCount = 0;
    public Ball[] balls;

    private IEnumerator PredStart()
    {
        yield return new WaitForEndOfFrame();
        float cameraOffset = Camera.main.orthographicSize / 5;
        UpBorder.position = new Vector3(UpBorder.position.x, UpBorder.position.y * cameraOffset, UpBorder.position.z);
        DownBorder.position = new Vector3(DownBorder.position.x, DownBorder.position.y * cameraOffset, DownBorder.position.z);
    }
    
    private void Start()
    {
        instance = this;

        StartCoroutine(PredStart());
    }

    public void StartGame()
    {
        allowDrag = false;
        if (CameraMoving.instance != null)
            CameraMoving.instance.allowDrag = false;
        isGame = true;

        if (TutorialController.instance != null)
            StartCoroutine(TutorialController.instance.DisableObjects());

        foreach (Ball ball in balls)
        {
            ball.animator.SetBool("isPlaying", false);
            ball.rb.isKinematic = false;
            ball.rb.AddRelativeForce(ball.direction.position, ForceMode2D.Impulse);
        }
    }

    public void SpawnObstacle(InventoryItem prefabTemplate, Vector3 spawnPosition, InventoryCell cell)
    {
        InventoryItem newObject = Instantiate(prefabTemplate, Vector3.zero, Quaternion.identity, ObstaclesParent) as InventoryItem;
        if (newObject.isShield)
            newObject.transform.Rotate(0, 0, -90);
        newObject.transform.position = spawnPosition;
        newObject.cell = cell;
        if (!newObject.isCannon)
            foreach (SpriteRenderer sprite in newObject.images)
                sprite.sortingOrder = ObjectCount + 2;   
        ObjectCount++;
    }

    public IEnumerator EndGame()
    {
        if (isGame)
        {
            if (balls[0].inNat)
            {
                if (balls.Length > 1)
                    if (!balls[1].inNat)
                        yield break;
                
                isGame = false;
                yield return new WaitForSeconds(1);
                finish = true;
                particles.Play();
                StartCoroutine(UIController.instance.ShowWinLosePanel());

                int currentScene = SceneManager.GetActiveScene().buildIndex;

                currentScene = currentScene == 10 ? 2 : currentScene;
                
                SoundController.instance.PlayWinSound();
                SaveController.instance.SaveScene(currentScene + 1);
            }
        }
    }

    public void RestartGame()
    {
        if (finish)
            return;
        
        if (TutorialController.instance != null)
            TutorialController.instance.EnableObjects();

        if (CameraMoving.instance != null)
            CameraMoving.instance.allowDrag = true;

        foreach (Ball ball in balls)
            StartCoroutine(ball.SwitchTrail());
            
        allowDrag = true;
        isGame = false;
        for (int i = 0; i < balls.Length; i ++)
            StartCoroutine(balls[i].RespawnBall(SpawnPositions[i].position, true));
    }

    public void ChangeScene()
    {
        StartCoroutine(ChangeSceneRoutine());
    }

    private IEnumerator ChangeSceneRoutine()
    {
        yield return StartCoroutine(UIController.instance.WhiteFadeRoutine(1, true));
        
        UIController.instance.DeleteAllTween();
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(PlayerPrefs.GetInt("savedLevel"));
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
            Application.Quit();
    }
}
