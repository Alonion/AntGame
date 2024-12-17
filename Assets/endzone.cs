using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endzone : MonoBehaviour
{
    public GameObject ant;
    public GameObject food;
    bool foodCollide = false;
    bool antCollide = false;
    // Start is called before the first frame update
    void Start()
    {
        if(ant=null)
        {
            ant = GameObject.FindWithTag("Ant");
        }
        if(food = null)
        {
            food = GameObject.FindWithTag("Food");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (food != null)
        {
            if (collision.CompareTag("Food"))
            {
                Debug.Log("food");
                foodCollide = true;
            }
        }
        if (ant != null)
        {
            if (collision.gameObject == ant)
            {
                Debug.Log("THE ANT IS IN THE BASKET");
                antCollide = true;
               


            }
            
        }
        if (antCollide && foodCollide)
        {
            Debug.Log("GAME OVER");
            antCollide = false;
            foodCollide = false;
        }
        else

        {
            Debug.Log("what?!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
