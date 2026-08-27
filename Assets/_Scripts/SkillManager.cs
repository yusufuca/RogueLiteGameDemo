using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    public List<Skills> mySkills = new List<Skills>();
    public List<Skills> castedSkills = new List<Skills>();
    Skills lastCastSkill;
    Animator animator;
    Attack myAttackScript;
    float skillValue;
    public GameObject coolDownIcon;
    public GameObject durationBar;
    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        myAttackScript = GetComponent<Attack>();
        
    }
    public void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.Q)) 
        {
            CastSkill(mySkills[0]);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            CastSkill(mySkills[1]);
        }
    }
    public void CastSkill(Skills skill)
    {

        if (!castedSkills.Contains(skill))
        {
            Debug.Log("Skill Casting");

            float duration = skill.duration;
            string skillString = skill.animTriggerString;
            //Animate Skill
            if (!skill.isContinous)
            {
                animator.SetTrigger(skillString);
            }
            else
            {
                StartCoroutine(ContinousSkillRoutine(skill));
            }

            //Calculate Value
            CalculateValue(skill);
            //Effect

            // cooldown
            StartCoroutine(CoolDownRoutine(skill));
        }
        else
        {
            Debug.Log("Wait Cooldown");
        }

    }
    private IEnumerator ContinousSkillRoutine(Skills skill)
    {
        animator.SetBool(skill.animTriggerString, true);
        float timer = skill.duration;
        while(timer > 0)
        {
            timer -= Time.deltaTime;
            durationBar.GetComponent<Image>().fillAmount = Mathf.Clamp01(timer/skill.duration);
            yield return null;
        }
        durationBar.GetComponent<Image>().fillAmount = 0;
        animator.SetBool(skill.animTriggerString, false);
    }
    private IEnumerator CoolDownRoutine(Skills skill)
    {
        castedSkills.Add(skill);
        float timer = skill.coolDown;
        while(timer > 0) 
        {
            timer -= Time.deltaTime;
            coolDownIcon.GetComponent<Image>().fillAmount = Mathf.Clamp01(timer / skill.coolDown);
            yield return null;
        }
        coolDownIcon.GetComponent<Image>().fillAmount = 0;
       
        RemoveTheSkillBuff(skill);
        castedSkills.Remove(skill);
    }
    private IEnumerator EffectDurationRoutine(Skills skill, Attack script)
    {
        script.isStunned= true;
        Debug.Log(("Enemy stunned"));
        yield return new WaitForSeconds(skill.value);
        Debug.Log("Enemy stun false");
        script.isStunned = false;
    }
    private void CalculateValue(Skills skill)
    {
        switch (skill.skillTpye)
        {
            case SkillTypes.Attack:
                Debug.Log("Casting Attack Skill");
                
                myAttackScript.currentDamage *= skill.value;
                break;
            case SkillTypes.Stun:
                Debug.Log("Casting Stun Skill");
                StunSkill(skill);
                break;
        }
    }
    private void RemoveTheSkillBuff(Skills skill)
    {
        switch (skill.skillTpye)
        {
            case SkillTypes.Attack:
                Debug.Log("Removed Attack Buff");

                myAttackScript.currentDamage /= skill.value;
                break;
            case SkillTypes.Stun:
                //Debug.Log("Casted Stun Skill");
                break;
        }
    }
    public void StunSkill(Skills skill)
    {
        Collider closestEnemy = null;
        Collider[] hittedEnemies = Physics.OverlapSphere(transform.position, 2f, myAttackScript.mask);
        float closestDistance = Mathf.Infinity;
        if (hittedEnemies.Length > 0)
        {
            for (int i = 0; i < hittedEnemies.Length; i++)
            {
                float distance = Vector3.Distance(transform.position, hittedEnemies[i].transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = hittedEnemies[i];
                }
            }
        }
        if (closestEnemy != null) 
        {
            Attack enemyAttackScript = closestEnemy.gameObject.GetComponent<Attack>();
            StartCoroutine(EffectDurationRoutine(skill, enemyAttackScript));
         
            
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 2f);
    }
}
