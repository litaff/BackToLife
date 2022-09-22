using UnityEngine;

namespace BackToLife
{
    // should be in every scene
    public class SceneManager : MonoBehaviour
    {
        public static SceneType loadedScene;
        private static SceneType _prevScene;
        
        public SceneType GetLoadedScene()
        {
            return loadedScene;
        }

        public void PreviousScene()
        {
            LoadScene(_prevScene);   
        }
        
        public void LoadScene(int sceneId)
        {
            _prevScene = loadedScene;
            loadedScene = (SceneType) sceneId;
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneId);
        }

        public static void LoadScene(SceneType sceneType)
        {
            _prevScene = loadedScene;
            loadedScene = sceneType;
            UnityEngine.SceneManagement.SceneManager.LoadScene((int) sceneType);
        }

        private void Awake()
        {
            Debug.Log($"Current scene: {loadedScene}");
            if ((int) loadedScene == UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex) return;

            Debug.LogError($"Loaded scene {loadedScene}, but is " +
                           $"{(SceneType) UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex}");
            loadedScene = (SceneType) UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        }

        public enum SceneType
        {
            Menu,
            Gameplay,
            Browser,
            Editor,
            EndLevel,
            SubmitLevel
        }
    }
}