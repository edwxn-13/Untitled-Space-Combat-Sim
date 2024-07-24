using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 1 * Time.deltaTime, 0));
    }

    public void StartGame()
    {
        SceneManager.LoadSceneAsync(1);
    }
}
