using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Win : MonoBehaviour
{
    float HP;
    void Update()
    {
        HP = gameObject.GetComponent<EnemyAI>().ReturnHP();

        if (HP <= 0)
            GameManager.instance.ShowMenu(GameManager.MenuType.Win, true);
    }
}
