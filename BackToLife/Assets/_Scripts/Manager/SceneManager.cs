using System;
using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Events;

namespace BackToLife
{
    public class SceneManager : MonoBehaviour
    {
        public static SceneType loadedScene;

        private void Awake()
        {
            Debug.Log($"Current scene: {loadedScene}");
        }

        public void LoadScene(int sceneId)
        {
            loadedScene = (SceneType) sceneId;
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneId);
        }

        public void LoadScene(SceneType sceneType)
        {
            LoadScene((int) sceneType);
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