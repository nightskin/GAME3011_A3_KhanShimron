using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Bob : MonoBehaviour
{
    public Image talk;
    public Text objective;
    public GameObject MiniGame;
    bool upset;

    void Start()
    {
        upset = false;
    }

    
    void Update()
    {
        if(upset)
        {
            transform.position += Vector3.right * 5 * Time.deltaTime;
        }
    }

    public void Talk()
    {
        talk.gameObject.SetActive(true);
    }

    public void ShutUp()
    {
        talk.gameObject.SetActive(false);
    }

    public void Match3()
    {
        ShutUp();
        MiniGame.gameObject.SetActive(true);
        MiniGame.transform.Find("GameGrid").GetComponent<Board>().Setup();
    }

    public void Suicide()
    {
        objective.text = "You Hurt Bob's feelings";
        ShutUp();
        upset = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Hazard")
        {
            SceneManager.LoadScene(1);
        }
    }

}
