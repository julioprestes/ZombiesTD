using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacement : MonoBehaviour
{
    [SerializeField] private SpriteRenderer rangeSprite;
    [SerializeField] private CircleCollider2D rangeCollider;
    [SerializeField] private Color gray;
    [SerializeField] private Color red;

    [NonSerialized] public bool isPlacing = true;
    private bool isRestricted = false;

    private Tower tower;

    void Awake()
    {
        tower = GetComponent<Tower>();
        rangeCollider.enabled = false;
    }

    void Update()
    {
        if(isPlacing){
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            transform.position = mousePosition;  
        }
        
        if(Input.GetMouseButtonDown(1) && !isRestricted && tower.cost <= Player.main.money){
            rangeCollider.enabled = true;
            isPlacing = false;
            rangeSprite.enabled = false;
            Player.main.money -= tower.cost;
            GetComponent<TowerPlacement>().enabled = false;
        }

        if(isRestricted){
            rangeSprite.color = red;
        }
        else{
            rangeSprite.color = gray;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Restricted" || collision.gameObject.tag == "Tower" && isPlacing) {
            isRestricted = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Restricted" || collision.gameObject.tag == "Tower" && isPlacing) {
            isRestricted = false;
        }
    }
}
