using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("___| Components |___")]
    [SerializeField] CharacterController controller;
    [SerializeField] Renderer modelBody;
    [SerializeField] Renderer modelHead;

    [Header("___| Player Settings |___")]
    [SerializeField] int HP;
    [Range(3, 8)][SerializeField] int playerSpeed;
    [Range(10, 15)][SerializeField] int jumpHeight;
    [Range(15, 50)][SerializeField] int gravity;
    [Range(1, 3)][SerializeField] int maxJumps;

    [Header("___| Collectables |___")]
    public int coins;



    [Header("---| Gun Stats |---")]
    [SerializeField] int shotDamage;
    [SerializeField] float shotRate;
    [SerializeField] int shotDist;
    //////[SerializeField] GameObject recticle;

    //////Color rectOrigColor;
    int timesJumped;
    private Vector3 playerVelocity;
    Vector3 move;
    bool isShooting;

    void Start()
    {
        //////rectOrigColor = recticle.GetComponent<Image>().color;
    }

    void Update()
    {
        movement(); //REMOVE THIS ONCE CODE IS UNCOMMENTED
        StartCoroutine(shoot()); //REMOVE THIS ONCE CODE IS UNCOMMENTED

        //if (!gameManager.instance.isPaused)
        //{
        //    movement();
        //    StartCoroutine(shoot());
        //}
        //////if (rectOnEnemy())
        //////    recticle.GetComponent<Image>().color = Color.red;
        //////else
        //////    recticle.GetComponent<Image>().color = rectOrigColor;
    }
    void movement()
    {
        if (controller.isGrounded && playerVelocity.y < 0)
        {
            timesJumped = 0;
            playerVelocity.y = 0f;
        }

        move = (transform.right * Input.GetAxis("Horizontal")) + (transform.forward * Input.GetAxis("Vertical"));
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (Input.GetButtonDown("Jump") && timesJumped < maxJumps)
        {
            timesJumped++;
            playerVelocity.y = jumpHeight;
        }

        playerVelocity.y -= gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
    IEnumerator shoot()
    {
        if (!isShooting && Input.GetButton("Shoot"))
        {
            isShooting = true;

            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shotDist))
            {
                if (hit.collider.GetComponent<IDamage>() != null)
                {
                    hit.collider.GetComponent<IDamage>().takeDamage(shotDamage);
                }
            }
            Debug.Log("I Shoot");
            yield return new WaitForSeconds(shotRate);
            isShooting = false;
        }
    }
    //////bool rectOnEnemy()
    //////{
    //////    RaycastHit hit;
    //////    if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shotDist))
    //////    {
    //////        if (hit.collider.GetComponent<IDamage>() != null)
    //////        {
    //////            return true;
    //////        }
    //////    }
    //////    return false;
    //////}
    public void takeDamage(int dmg)
    {
        HP -= dmg;
    }
    public void addJumps(int amount)
    {
        maxJumps += amount;
        //coins -= gameManager.instance.jumpCost;
    }
    public void addCoins(int amount)
    {
        coins += amount;
    }
}