using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float levelReloadDelay = 1f;
    [SerializeField] AudioClip playerCrash;
    [SerializeField] AudioClip playerLevelFinish;
    [SerializeField] ParticleSystem playerCrashParticle;
    [SerializeField] ParticleSystem playerLevelFinishParticle;

    AudioSource audioSource;

    bool isTransitioning = false;
    bool DisableCollisions = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        //DebuggerCheat();
    }
    
    void OnCollisionEnter(Collision other) 
    {
        if (isTransitioning || DisableCollisions){ return; }

        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("Start of the level");
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartCrashSequence();
                break;   
        }
    }

    void DebuggerCheat()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadLevelCheat();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            DisableCollisions = !DisableCollisions;
        }
    }
    void StartSuccessSequence()
        {
            isTransitioning = true;
            playerLevelFinishParticle.Play();
            audioSource.Stop();
            audioSource.PlayOneShot(playerLevelFinish);
            GetComponent<Movement>().enabled = false;
            Invoke ("LoadNextLevel", levelReloadDelay);
            
        }
        
    void StartCrashSequence()
        {
            isTransitioning = true;
            playerCrashParticle.Play();
            audioSource.Stop();
            audioSource.PlayOneShot(playerCrash);
            GetComponent<Movement>().enabled = false;
            Invoke ("ReloadLevel", levelReloadDelay);

        }
    void LoadLevelCheat()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }                
    void ReloadLevel()
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex);
        }

    void LoadNextLevel()
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            int nextSceneIndex = currentSceneIndex + 1;
            if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
            {
                nextSceneIndex = 0;
            }
            SceneManager.LoadScene(nextSceneIndex);           
        }
}
