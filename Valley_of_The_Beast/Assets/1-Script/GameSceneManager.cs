using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager instance;

    private void Awake()
    {
        instance = this;
    }

    [SerializeField] ScreenTint screenTint;
    [SerializeField] CameraConfiner cameraConfiner;
    string currentScene;
    AsyncOperation unLoad;
    AsyncOperation load;

    private void Start()
    {
        currentScene = SceneManager.GetActiveScene().name;
    }

    public void InitSwitchScene(string to, Vector3 targetPositon)
    {
        StartCoroutine(Transition(to, targetPositon));
    }

    IEnumerator Transition(string to, Vector3 targetPosition)
    {
        screenTint.Tint();

        yield return new WaitForSeconds(1f / screenTint.speed + 0.1f); //1 segundo dividido pela velocidade de tintura e adição de tempo

        SwitchScene(to, targetPosition);

        while(load != null & unLoad != null)
        {
            if (load.isDone) { load = null; }
            if(unLoad.isDone) { unLoad = null; }
            yield return new WaitForSeconds(0.1f);
        }
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(currentScene));

        cameraConfiner.UpdateBounds();
        screenTint.UnTint();
    }

    public void SwitchScene(string to, Vector3 targetPosition)
    {
        load = SceneManager.LoadSceneAsync(to, LoadSceneMode.Additive);
        unLoad = SceneManager.UnloadSceneAsync(currentScene);
        currentScene = to;
        Transform playerTransform = GameManager.instance.player.transform;

        CinemachineBrain currentCamera = Camera.main.GetComponent<CinemachineBrain>();

        currentCamera.ActiveVirtualCamera.OnTargetObjectWarped(
                playerTransform,
                targetPosition - playerTransform.position
            );

        playerTransform.position = new Vector3(
            targetPosition.x,
            targetPosition.y,
            playerTransform.position.z
            );
    }
}
