using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public delegate void OnLoadComplete();
    public static event OnLoadComplete Loaded;
    [SerializeField] private string TitleScene;
    //[SerializeField] private string UIScene;

    [SerializeField] private string[] GameScenes;


    private WaitForEndOfFrame mWaitForEndOfFrame;

    public bool Loading { get; private set; }
    private bool wasLoading = false;

    public static SceneLoader instance = null;

    void Awake()
    {//singleton
        if (instance == null)
            instance = this;
        else if(instance != this)
            Destroy(this);
        
        DontDestroyOnLoad(gameObject);
        mWaitForEndOfFrame = new WaitForEndOfFrame();
        Loading = true;
        StartCoroutine(LoadTitleScenes());
    }
    void Update()
    {
        //if we are not currently loading and we were previously told to load
        //then loading must be complete and we must trigger onLoadComplete() event.
        if (wasLoading == true && Loading == false)
        {
            wasLoading = false;
            EventOnLoadComplete();
        }
    }
    private void EventOnLoadComplete()
    {
        if (Loaded != null)
        {
            Loaded();
        }
    }
    private IEnumerator LoadTitleScenes()
    {
        
        AsyncOperation ao = SceneManager.LoadSceneAsync(TitleScene, LoadSceneMode.Additive);
        if (ao != null)
        {
            while (!ao.isDone)
            {
                yield return mWaitForEndOfFrame;
            }
        }


        yield return mWaitForEndOfFrame;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(TitleScene));
        Loading = false;
    }
    private IEnumerator UnloadScene(string level)
    {
        AsyncOperation ao = SceneManager.UnloadSceneAsync(level);
        if (ao != null)
        {
            while (!ao.isDone)
            {
                yield return mWaitForEndOfFrame;
            }
        }
        yield return mWaitForEndOfFrame;
    }
    private IEnumerator LoadNewScene(string level)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(level, LoadSceneMode.Additive);
        if (ao != null)
        {
            while (!ao.isDone)
            {
                yield return mWaitForEndOfFrame;
            }
        }
        yield return mWaitForEndOfFrame;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(level));//new scene should always be active scene by default, execpt when it isnt
        Loading = false;

    }

    private void LoadLevelFromMenu(string level)
    {
        Loading = true;

        //check if the level requested exists
        foreach (string item in GameScenes)
        {//if it does
            if (level == item)
            {//check if the mainmenu is loaded
                if (SceneManager.GetSceneByName(TitleScene).isLoaded) { 
                    StartCoroutine(UnloadScene(TitleScene));//unload it if it is
                   }
        
                //then load the new level
                StartCoroutine(LoadNewScene(level));
                wasLoading = true;
            }
        }
    }
    private void LoadMainMenu()
    {//unloads current scene then loads mainmenu
        Loading = true;
        
        if (SceneManager.GetActiveScene().isLoaded)
        {
            StartCoroutine(UnloadScene(SceneManager.GetActiveScene().name));//unload it if it is
        }

        //then load the new level
        StartCoroutine(LoadNewScene(TitleScene));
        wasLoading = true;

    }
    private void LoadLevelFromLevel(string levToLoad, string LevUnload)
    {
        Loading = true;
        string leveName = SceneManager.GetActiveScene().name;

        if (SceneManager.GetSceneByName(LevUnload).isLoaded)
        {
            StartCoroutine(UnloadScene(LevUnload));//unload it if it is
        }

        //then load the new level
        StartCoroutine(LoadNewScene(levToLoad));
        wasLoading = true;
    }

    public static void LoadLevel(int sceneIndex)
    {
        SceneLoader sl = SceneLoader.instance;
        if(sceneIndex >= 0 && sceneIndex < sl.GameScenes.Length)
        {
            sl.LoadLevelFromMenu(sl.GameScenes[sceneIndex]);//unloads titlescene then loads a gameScene
            
        }
    }
    public static void LoadTitleScene()
    {
        SceneLoader sl = SceneLoader.instance;
        sl.LoadMainMenu();//unloads currently active scene, then loads titleScene
    }

    public static void RestartCurrentLevel()
    {
        SceneLoader sl = SceneLoader.instance;
        string leveName = SceneManager.GetActiveScene().name;
        sl.LoadLevelFromLevel(leveName, leveName);//unloads then loads itself again
    }

}