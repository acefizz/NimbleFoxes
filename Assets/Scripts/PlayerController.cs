using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


//TODO: When the player picks up / changes abilities, SetAbilitiesIcon should be used, there is another TODO in Collectables where the game object for the ability needs to be read.


public class PlayerController : MonoBehaviour
{
    [Header("___| Components |___")]
    [SerializeField] CharacterController controller;
    [SerializeField] int pushbackTime;
    [SerializeField] Renderer modelBody;
    [SerializeField] Renderer modelHead;

    [Header("___| Player Settings |___")]
    [SerializeField] int HP;
    [Range(1,3)] [SerializeField] int lives;
    int livesRemaining;
    [Range(3, 8)][SerializeField] int playerSpeed;
    [Range(10, 15)][SerializeField] int jumpHeight;
    [Range(15, 50)][SerializeField] int gravity;
    [Range(1, 3)][SerializeField] int maxJumps;
    [SerializeField] GameObject checkpointToSpawnAt;
    Vector3 startCheckpoint;

    [Header("___| Collectables |___")]
    public int coins;

    [Header("---| Gun Stats |---")]
    [SerializeField] List<GunSetup> gunList = new List<GunSetup>();
    [SerializeField] GameObject gunModel;
    [SerializeField] float shotDamage;
    [SerializeField] float shotRate;
    [SerializeField] int shotDist;
    [SerializeField] GameObject hitEffect;
    public string gunName;
    public string abilityName;

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
        if(gunList.Count > 0)
            changeGun();
        startCheckpoint = checkpointToSpawnAt.transform.position;
        livesRemaining = lives;
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
                Instantiate(hitEffect, hit.point, hitEffect.transform.rotation);
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
            GameManager.instance.ShowMenu(GameManager.MenuType.Lose, true);
            if (livesRemaining > 0)
            {

                GameManager.instance.respawnButton.interactable = true;
                GameManager.instance.SetRespawnText($"All of your light has been lost, you have {livesRemaining} balls of light remaining to revive");
                livesRemaining--;
                ResetHP();
            }
            else
            {
                GameManager.instance.checkpoint = startCheckpoint;
                GameManager.instance.respawnButton.interactable = false;
                GameManager.instance.SetRespawnText("You have no light left to revive, you can return to when you came to this world.");
                isDead = true;
            }
        }
    }

    public int ReturnHP()
    {
        return HP;
    }
    public void SetPlayerPos()
    {
        controller.enabled = false;
        transform.position = GameManager.instance.playerSpawnLocation;
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
    public int Lives(int life = 0)
    {
        lives += life;
        return lives;
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
        gunName = gun.gunName;

        gunModel.GetComponent<MeshFilter>().sharedMesh = gun.GunModel.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gun.GunModel.GetComponent<MeshRenderer>().sharedMaterial;

        gunList.Add(gun);
        selectedGun = gunList.Count - 1;

        SetWeaponIcon();
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
        gunName = gunList[selectedGun].gunName;

        gunModel.GetComponent<MeshFilter>().sharedMesh = gunList[selectedGun].GunModel.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunList[selectedGun].GunModel.GetComponent<MeshRenderer>().sharedMaterial;

        SetWeaponIcon();
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
    public void SetWeaponIcon()
    {
        for (int i = 0; i < GameManager.instance.gunNames.Length; i++)
        {
            if (GameManager.instance.gunNames[i] == gunName)
            {
                switch (i)
                {
                    case 0:
                        GameManager.instance.weapon1.SetActive(true);
                        GameManager.instance.weapon2.SetActive(false);
                        GameManager.instance.weapon3.SetActive(false);
                        GameManager.instance.weapon4.SetActive(false);
                        GameManager.instance.weapon5.SetActive(false);
                        GameManager.instance.weapon6.SetActive(false);
                        break;
                    case 1:
                        GameManager.instance.weapon1.SetActive(false);
                        GameManager.instance.weapon2.SetActive(true);
                        GameManager.instance.weapon3.SetActive(false);
                        GameManager.instance.weapon4.SetActive(false);
                        GameManager.instance.weapon5.SetActive(false);
                        GameManager.instance.weapon6.SetActive(false);
                        break;
                    case 2:
                        GameManager.instance.weapon1.SetActive(false);
                        GameManager.instance.weapon2.SetActive(false);
                        GameManager.instance.weapon3.SetActive(true);
                        GameManager.instance.weapon4.SetActive(false);
                        GameManager.instance.weapon5.SetActive(false);
                        GameManager.instance.weapon6.SetActive(false);
                        break;
                    case 3:
                        GameManager.instance.weapon1.SetActive(false);
                        GameManager.instance.weapon2.SetActive(false);
                        GameManager.instance.weapon3.SetActive(false);
                        GameManager.instance.weapon4.SetActive(true);
                        GameManager.instance.weapon5.SetActive(false);
                        GameManager.instance.weapon6.SetActive(false);
                        break;
                    case 4:
                        GameManager.instance.weapon1.SetActive(false);
                        GameManager.instance.weapon2.SetActive(false);
                        GameManager.instance.weapon3.SetActive(false);
                        GameManager.instance.weapon4.SetActive(false);
                        GameManager.instance.weapon5.SetActive(true);
                        GameManager.instance.weapon6.SetActive(false);
                        break;
                    case 5:
                        GameManager.instance.weapon1.SetActive(false);
                        GameManager.instance.weapon2.SetActive(false);
                        GameManager.instance.weapon3.SetActive(false);
                        GameManager.instance.weapon4.SetActive(false);
                        GameManager.instance.weapon5.SetActive(false);
                        GameManager.instance.weapon6.SetActive(true);
                        break;
                    default:
                        GameManager.instance.weapon1.SetActive(false);
                        GameManager.instance.weapon2.SetActive(false);
                        GameManager.instance.weapon3.SetActive(false);
                        GameManager.instance.weapon4.SetActive(false);
                        GameManager.instance.weapon5.SetActive(false);
                        GameManager.instance.weapon6.SetActive(false);
                        break;
                }
                break;
            }
        }
    }
    public void SetAbilityIcon()
    {
        for (int i = 0; i < GameManager.instance.abilityNames.Length; i++)
        {
            if (GameManager.instance.abilityNames[i] == gunName)
            {
                switch (i)
                {
                    case 0:
                        GameManager.instance.ability1.SetActive(true);
                        GameManager.instance.ability2.SetActive(false);
                        GameManager.instance.ability3.SetActive(false);
                        GameManager.instance.ability4.SetActive(false);
                        GameManager.instance.ability5.SetActive(false);
                        GameManager.instance.ability6.SetActive(false);
                        break;
                    case 1:
                        GameManager.instance.ability1.SetActive(false);
                        GameManager.instance.ability2.SetActive(true);
                        GameManager.instance.ability3.SetActive(false);
                        GameManager.instance.ability4.SetActive(false);
                        GameManager.instance.ability5.SetActive(false);
                        GameManager.instance.ability6.SetActive(false);
                        break;
                    case 2:
                        GameManager.instance.ability1.SetActive(false);
                        GameManager.instance.ability2.SetActive(false);
                        GameManager.instance.ability3.SetActive(true);
                        GameManager.instance.ability4.SetActive(false);
                        GameManager.instance.ability5.SetActive(false);
                        GameManager.instance.ability6.SetActive(false);
                        break;
                    case 3:
                        GameManager.instance.ability1.SetActive(false);
                        GameManager.instance.ability2.SetActive(false);
                        GameManager.instance.ability3.SetActive(false);
                        GameManager.instance.ability4.SetActive(true);
                        GameManager.instance.ability5.SetActive(false);
                        GameManager.instance.ability6.SetActive(false);
                        break;
                    case 4:
                        GameManager.instance.ability1.SetActive(false);
                        GameManager.instance.ability2.SetActive(false);
                        GameManager.instance.ability3.SetActive(false);
                        GameManager.instance.ability4.SetActive(false);
                        GameManager.instance.ability5.SetActive(true);
                        GameManager.instance.ability6.SetActive(false);
                        break;
                    case 5:
                        GameManager.instance.ability1.SetActive(false);
                        GameManager.instance.ability2.SetActive(false);
                        GameManager.instance.ability3.SetActive(false);
                        GameManager.instance.ability4.SetActive(false);
                        GameManager.instance.ability5.SetActive(false);
                        GameManager.instance.ability6.SetActive(true);
                        break;
                    default:
                        GameManager.instance.ability1.SetActive(false);
                        GameManager.instance.ability2.SetActive(false);
                        GameManager.instance.ability3.SetActive(false);
                        GameManager.instance.ability4.SetActive(false);
                        GameManager.instance.ability5.SetActive(false);
                        GameManager.instance.ability6.SetActive(false);
                        break;
                }
                break;
            }
        }
    }
    public List<GunSetup> ReturnGunList()
    {
        return gunList;
    }
    public int ReturnSelectedGun()
    {
        return selectedGun;
    }
    public Vector3 ReturnStartCheckpoint()
    {
        return checkpointToSpawnAt.transform.position;
    }
    public CharacterController ReturnController()
    {
        return controller;
    }
}