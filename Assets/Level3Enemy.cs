using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3Enemy : EnemyInfo
{
    // Start is called before the first frame update
    void FixedUpdate()
    {
        m_Grounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i] != groundCollider)
            {//exempt from the collider on the monster
                m_Grounded = true;
            }
        }

        if (eatingAnimator.GetCurrentAnimatorStateInfo(0).IsName("L3-Eat"))
        {

            // print(eatingAnimator.GetCurrentAnimatorStateInfo(0).length);
            IsOpenMouth = false;
        }
        else
        {
            OpenMouthCheck();
        }

    }
}
