using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleProjectile : MonoBehaviour
{
    [SerializeField] private float lifespan = 1.5f;
    [SerializeField] private float lifespanTimer = 0f;
    [SerializeField] private float speed = 10f;
    [SerializeField] public PlayerCombatController player;
    [SerializeField] public float power = 0.1f;
    [SerializeField] private GameObject particles;
    [SerializeField] AudioSource soundEffects;
    [SerializeField] private AudioClip gunHitSound;


    // Start is called before the first frame update
    void Start()
    {
      lifespanTimer = 0f;
      soundEffects = GameObject.FindGameObjectWithTag("sound").GetComponentInChildren<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        lifespanTimer += Time.deltaTime;
        if (lifespanTimer > lifespan) {
          Destroy(gameObject);
        }
        transform.position += this.transform.forward * Time.deltaTime * speed;
    }

    void OnTriggerEnter(Collider other)
    {
      if (player != null) {
        if (other.gameObject.CompareTag("target1")) {
          player.score += power * 5 * 1.75f;
          Instantiate(particles, this.transform.position, Quaternion.Euler(0, 0, 0) );
          if (power > 0.25f) {
            Instantiate(particles, this.transform.position, Quaternion.Euler(0, 0, 0) );
          }

          float soundlevel = power + 0.225f;
          
          float pitchMod = Random.Range(-0.25f, 0.25f);
          soundEffects.pitch = 1 + pitchMod;
          soundEffects.PlayOneShot(gunHitSound, 0.5F * soundlevel);

        } else if (other.gameObject.CompareTag("target2")) {
          player.score += power * 5 * 5.25f;
          Instantiate(particles, this.transform.position, Quaternion.Euler(0, 0, 0) );
          Instantiate(particles, this.transform.position, Quaternion.Euler(0, 0, 0) );
          if (power > 0.25f) {
            Instantiate(particles, this.transform.position, Quaternion.Euler(0, 0, 0) );
            Instantiate(particles, this.transform.position, Quaternion.Euler(0, 0, 0) );
          }

          float soundlevel = power + 0.5f;
          
          float pitchMod = Random.Range(-0.25f, 0.25f);
          soundEffects.pitch = 1 + pitchMod;
          soundEffects.PlayOneShot(gunHitSound, 0.6F * soundlevel);

        }
      }
      /*if (other.gameObject.CompareTag("Switch")) {
            if (other.gameObject.GetComponent<Switch>() != null) {
              other.gameObject.GetComponent<Switch>().Toggle();
            }
            Destroy(gameObject);
        } else if (other.gameObject.CompareTag("AllowsBubble") || other.gameObject.CompareTag("Player")) {
            //
        } else {
          Destroy(gameObject);
      }*/

    }
    void OnColliderEnter(Collider other)
    {
        /*if (other.gameObject.CompareTag("Switch")) {
            other.gameObject.GetComponent<Switch>().Toggle();
        } else if (other.gameObject.CompareTag("AllowsBubble") || other.gameObject.CompareTag("Player")) {
            //
        } else {
          Destroy(gameObject);
        }*/

    }
}
