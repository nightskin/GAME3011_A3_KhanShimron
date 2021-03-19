using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class TankControls : MonoBehaviour
{
    public float speed = 3;
    public float turnSpeed = 10;
    public bool canTalk;
    public GameObject bob;
    
    void Start()
    {
        canTalk = false;
        bob = GameObject.Find("Bob");
    }
    
    void Update()
    {
        if(Input.GetKey(KeyCode.W))
        {
            transform.position += transform.forward * speed * Time.deltaTime;
        }
        else if(Input.GetKey(KeyCode.S))
        {
            transform.position -= transform.forward * speed * Time.deltaTime;
        }

        if(Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.up, -turnSpeed * Time.deltaTime);
        }
        else if(Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime);
        }

        if(canTalk && Input.GetKeyDown(KeyCode.Space))
        {
            bob.GetComponent<Bob>().Talk();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Hazard")
        {
            SceneManager.LoadScene(2);
        }
        if(other.gameObject.tag == "NPC")
        {
            canTalk = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "NPC")
        {
            canTalk = false;
            bob.GetComponent<Bob>().ShutUp();
        }
    }
}
