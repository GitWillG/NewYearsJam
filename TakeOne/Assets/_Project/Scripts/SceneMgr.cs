using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DiceGame
{
    public class SceneMgr : MonoBehaviour
    {
        [SerializeField] private SceneManager sceneManager;
        //[SerializeField] private int sceneBuildIndex;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    
        [ContextMenu("LoadScene")]
        public void LoadNewScene(int sceneToLoad)
        {
            SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single) ;
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
