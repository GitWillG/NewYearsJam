using UnityEngine;
using UnityEngine.SceneManagement;

namespace DiceGame
{
    public class SceneManager : MonoBehaviour
    {
        [SerializeField] private UnityEngine.SceneManagement.SceneManager sceneManager;
        //[SerializeField] private int sceneBuildIndex;
        
        [ContextMenu("LoadScene")]
        public void LoadNewScene(int sceneToLoad)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single) ;
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
