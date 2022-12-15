using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("___| Components |___")]
    [SerializeField] CharacterController controller;
    [SerializeField] int pushbackTime;
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
    [SerializeField] List<GunSetup> gunList = new List<GunSetup>();
    [SerializeField] GameObject gunModel;
    [SerializeField] float shotDamage;
    [SerializeField] float shotRate;
    [SerializeField] int shotDist;

    [Header("---| Audio |---")]
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip gunShot;
    [Range(0, 1)][SerializeField] float gunShotVol;
    [SerializeField] AudioClip[] playerHurt;
    [Range(0, 1)][SerializeField] float playerHurtVol;
    [SerializeField] AudioClip[] playerJump;
    [Range(0, 1)][SerializeField] float playerJumpVol;
    [SerializeField] AudioClip[] playerSteps;
    [Range(0, 1)][SerializeField] float playerStepsVol;

    Color retOrigColor;
    int timesJumped;
    int HPOrig;
    int selectedGun;
    private Vector3 playerVelocity;
    Vector3 move;
    bool isShooting;
    bool isSprinting;
    bool stepPlaying;

    int extraDmg;

    Vector3 pushback;


    public bool isDead;

    void Start()
    {
        HPOrig = HP;
        SetPlayerPos();
        ResetHP();
    }

    void Update()
    {
        if (!GameManager.instance.isPaused)
        {
            pushback = Vector3.Lerp(pushback, Vector3.zero, Time.deltaTime * pushbackTime);   
            Movement();
            if (!stepPlaying && move.magnitude > 0.5f && controller.isGrounded)
            {
                StartCoroutine(playSteps());
            }

            if (gunList.Count > 0)
            {
                gunSelect();
                StartCoroutine(Shoot());
            }
        }
        GameManager.instance.reticle.GetComponent<Image>().color = AimonEnemy() ? Color.green : Color.red;
    }
    void Movement()
    {
        if (controller.isGrounded && playerVelocity.y < 0)
        {
            timesJumped = 0;
            playerVelocity.y = 0f;
        }

        isSprinting = Input.GetKey(KeyCode.LeftShift);
        move = (transform.right * Input.GetAxis("Horizontal")) + (transform.forward * Input.GetAxis("Vertical"));
        controller.Move(move * Time.deltaTime * (isSprinting ? playerSpeed * 2 : playerSpeed));

        if (Input.GetButtonDown("Jump") && timesJumped < maxJumps)
        {
            timesJumped++;
            playerVelocity.y = jumpHeight;
            aud.PlayOneShot(playerJump[UnityEngine.Random.Range(0, playerJump.Length)], playerJumpVol);
        }

        playerVelocity.y -= gravity * Time.deltaTime;
        controller.Move((playerVelocity + pushback) * Time.deltaTime);
    }
    IEnumerator Shoot()
    {
        if (!isShooting && Input.GetButton("Shoot"))
        {
            isShooting = true;

            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shotDist))
            {
                if (hit.collider.GetComponent<IDamage>() != null)
                {
                    hit.collider.GetComponent<IDamage>().takeDamage((shotDamage + extraDmg));
                }
            }
            aud.PlayOneShot(gunList[selectedGun].gunShot, gunShotVol);

            yield return new WaitForSeconds(shotRate);
            isShooting = false;
        }
    }
        IEnumerator PlayerDamageFlash()
    {
        GameManager.instance.ShowMenu(GameManager.MenuType.PlayerDamageFlash, true);
        yield return new WaitForSeconds(0.1f);
        GameManager.instance.ShowMenu(GameManager.MenuType.PlayerDamageFlash, false);
    }
    public void takeDamage(int dmg)
    {
        HP -= dmg;
        aud.PlayOneShot(playerHurt[UnityEngine.Random.Range(0, playerHurt.Length)], playerHurtVol);
        UpdatePlayerHPBar();
        StartCoroutine(PlayerDamageFlash());
        if (HP <= 0)
        {
            isDead = true;
            GameManager.instance.ShowMenu(GameManager.MenuType.Lose, true);
        }
    }
    public void SetPlayerPos()
    {
        controller.enabled = false;
        transform.position = GameManager.instance.playerSpawnPos.transform.position;
        controller.enabled = true;
    }

    public void ResetHP()
    {
        HP = HPOrig;
        UpdatePlayerHPBar();
    }
    public void AddHp(int hp)
    {
        HP += hp;
        UpdatePlayerHPBar();
    }

    bool AimonEnemy()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shotDist))
        {
            if (hit.collider.GetComponent<IDamage>() != null)
            {
                return true;
            }
        }
        return false;
    }
    public void UpdatePlayerHPBar()
    {
        GameManager.instance.playerHpBar.fillAmount = (float)HP / (float)HPOrig;
        if (HP > HPOrig / 2)
            GameManager.instance.playerHpBar.color = Color.green;
        else if (HP <= HPOrig / 2)
            GameManager.instance.playerHpBar.color = Color.red;
    }
    public void GunPickup(GunSetup gun)
    {
        shotDamage = gun.shotDamage;
        shotRate = gun.shotRate;
        shotDist = gun.shotDist;

        gunModel.GetComponent<MeshFilter>().sharedMesh = gun.GunModel.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gun.GunModel.GetComponent<MeshRenderer>().sharedMaterial;

        gunList.Add(gun);
        selectedGun = gunList.Count - 1;
    }
    public void PushbackInput(Vector3 direction)
    {
        pushback = direction;
    }
    void gunSelect()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectedGun < gunList.Count - 1)
        {
            selectedGun++;
            changeGun();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && selectedGun > 0)
        {
            selectedGun--;
            changeGun();
        }
    }
    void changeGun()
    {
        shotDamage = gunList[selectedGun].shotDamage;
        shotRate = gunList[selectedGun].shotRate;
        shotDist = gunList[selectedGun].shotDist;

        gunModel.GetComponent<MeshFilter>().sharedMesh = gunList[selectedGun].GunModel.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunList[selectedGun].GunModel.GetComponent<MeshRenderer>().sharedMaterial;
    }
    IEnumerator playSteps()
    {
        stepPlaying = true;
        aud.PlayOneShot(playerSteps[UnityEngine.Random.Range(0, playerSteps.Length)], playerStepsVol);
        yield return new WaitForSeconds(0.5f);
        stepPlaying = false;
    }
    public void AddCoins(int amount)
    {
        coins += amount;
    }

    public void AddJumps(int amount)
    {
        maxJumps += amount;
    }

    public void AddSpeed(int amount)
    {
        playerSpeed += amount;
    }

    public void AddDamage(int amount)
    {
        extraDmg += amount;
    }

    public int GetMaxJumps()
    {
        return maxJumps;
    }

    public int GetSpeed()
    {
        return playerSpeed;
    }

    public float GetDamage()
    {
        return (shotDamage + extraDmg);
    }
}