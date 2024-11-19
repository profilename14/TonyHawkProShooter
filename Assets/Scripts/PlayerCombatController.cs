using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCombatController : MonoBehaviour
{
    RotationController rotationController;
    [SerializeField] AudioSource soundEffects;
    [SerializeField] private AudioClip gunShotSound;
    [SerializeField] private GameObject waveSpellPrefab;
    [SerializeField] private float waveSpellCooldown = 0.12f;
    [SerializeField] private Rigidbody playerRigidbody;
    [SerializeField] private GameObject hiddenMessage;
    private float curCharge = 0.2f;
    private float maxCharge = 0.8f;
    private bool isCharging;
    

    public Slider healthBar;
    public Slider PHBar;
    
    private float timeLeft = 120f;
    public float score = 0f;
    private float castTimer = 0f;
    private float maxScore = 50f;

    public bool isFacingMouse = false;

    
    bool gameWon = false;

    // Start is called before the first frame update
    void Start()
    {
        rotationController = transform.parent.gameObject.GetComponent<RotationController>();

        //Get all weapons that are children of the weapon container.
        //int i = 0;
        /*foreach(Transform child in weaponContainer.transform)
        {
            weaponObjects[i] = child.gameObject;
            i++;
        }*/

        healthBar = GameObject.FindWithTag("Health Bar").GetComponent<Slider>();
        PHBar = GameObject.FindWithTag("PH Bar").GetComponent<Slider>();
        healthBar.maxValue= maxScore;
        PHBar.maxValue = 120;
    }

    // Update is called once per frame
    void Update()
    {
        if (score >= maxScore) {
            gameWon = false;
            score = 0;
            timeLeft = 120;
            maxScore += 50;
            healthBar.maxValue = maxScore;
        }

        if (maxScore >= 249) { // 50, 100, 150 stages, after which the game enters freeplay (500 point cycles with no time limit)
            gameWon = true;
            maxScore = 500;
        }

        if (maxScore > 149) {
            hiddenMessage.SetActive(true);
        }

        if (gameWon == false) {
            timeLeft -= Time.deltaTime;
        }
        
        if (timeLeft <= 0 && gameWon == false) {
            Destroy(gameObject);
        }
        PHBar.value = timeLeft;
        healthBar.value = score;

        if (castTimer > 0)
        {
          castTimer -= Time.deltaTime;
        }




        if (true)
        {

            //Time.timeScale = 1f;

            if (Input.GetMouseButtonDown(1))
            {
                isCharging = true;
                curCharge = 0.05f;
                
            }
            else if (Input.GetMouseButton(1))
            {
                if (curCharge < maxCharge) {
                    curCharge += Time.deltaTime;
                    if (curCharge > maxCharge) {
                        curCharge = maxCharge + 0.05f;
                    }
                }
                
            }
            if (Input.GetMouseButtonUp(1) && isCharging)
            {
                rotationController.snapToCurrentMouseAngle();
                FireBlast(curCharge);
                isCharging = false;
                
            }
            
        }
    }


    private void addPushForward(float amount)
    {
        //rotationController.snapToCurrentMouseAngle();
        Vector3 rotForward = rotationController.GetRotationDirection();
        applyKnockback(rotForward, amount);
    }

    private void FireBlast(float charge) {
        castTimer = waveSpellCooldown;


        Vector3 waveSpellAnchor = transform.position + rotationController.GetRotationDirection();
        Vector3 curRotation = rotationController.GetRotationDirection();
        float angle = -Mathf.Atan2(curRotation.z, curRotation.x) * Mathf.Rad2Deg + 90;

        rotationController.snapToCurrentMouseAngle();

        GameObject BulletProj = Instantiate(waveSpellPrefab, waveSpellAnchor, Quaternion.Euler(0, angle, 0) );

        BulletProj.GetComponentInChildren<BubbleProjectile>().power = charge;
        BulletProj.GetComponentInChildren<BubbleProjectile>().player = this;

        float soundlevel = charge + 0.325f;
        
        float pitchMod = Random.Range(-0.25f, 0.25f);
        soundEffects.pitch = 1 + pitchMod;
        soundEffects.PlayOneShot(gunShotSound, 0.3F * soundlevel);

        addPushForward(-7 * charge * 5);


    }


    public void applyKnockback(Vector3 velocity, float power) {

        playerRigidbody.AddForce(velocity * power, ForceMode.Impulse);
      
    }

}
