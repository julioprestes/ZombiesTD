using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [Header("Tower Stats")]
    public float range = 8f;
    public int damage = 25;
    public float fireRate = 1f;
    public int cost = 50;

    [Header("Targeting Mode")]
    public bool first = true;
    public bool last = false;
    public bool strong = false;

    [Header("Effects")]
    [SerializeField] GameObject fireEffect;
    [SerializeField] AudioClip fireSound; 

    private AudioSource audioSource; 

    [NonSerialized]
    public GameObject target;
    private float cooldown = 0f;

    void Start()
    {
        fireEffect.SetActive(false);
        audioSource = GetComponent<AudioSource>(); 
    }

    void Update()
    {
        if (target) {
            if (cooldown >= fireRate) {
                transform.right = target.transform.position - transform.position;

                target.GetComponent<Enemy>().damage(damage);
                cooldown = 0f; 
                StartCoroutine(FireEffect());
            }
            else {
                cooldown += 1 * Time.deltaTime;
            }
        }
    }

    IEnumerator FireEffect() {
        fireEffect.SetActive(true);

        if (fireSound != null && audioSource != null) {
            audioSource.PlayOneShot(fireSound);
        }

        yield return new WaitForSeconds(0.5f);
        fireEffect.SetActive(false);
    }
}