using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    public List<Skills> mySkills = new List<Skills>();
    public List<Skills> castedSkills = new List<Skills>();
    [SerializeField] private InputReader inputReader;
    [SerializeField] private string myTag;
    private string playerTag = "Player";
    Animator animator;
    StatManager myStatManagerScript;
    public GameObject coolDownIcon;
    public GameObject durationBar;
    public UI_Manager UI_Manager;
    private Attack attackScript;
    private void Awake()
    {
        attackScript = GetComponent<Attack>();
        myTag = gameObject.tag;
    }

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        myStatManagerScript = GetComponent<StatManager>();
        UI_Manager = FindAnyObjectByType<UI_Manager>();
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
            StartCoroutine(CoolDownDeny(skill));
        }

    }
    private IEnumerator ContinousSkillRoutine(Skills skill)
    {
        animator.SetBool(skill.animTriggerString, true);
        if(skill == mySkills[1]) attackScript.cast2Request = true;
        float timer = skill.duration;
        int skillIndex = 0;
        skillIndex = mySkills.IndexOf(skill);
        Image skillImage = UI_Manager.skillContainer[skillIndex].transform.Find("Duration").GetComponent<Image>();
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            if(myTag == playerTag) skillImage.fillAmount = Mathf.Clamp01(timer/skill.duration);
            attackScript.cast2Request = true;
            yield return null;
        }
        if (myTag == playerTag) skillImage.fillAmount = 0;
        animator.SetBool(skill.animTriggerString, false);
        attackScript.cast2Request = false;
    }
    private IEnumerator CoolDownRoutine(Skills skill)
    {
        castedSkills.Add(skill);
        float timer = skill.coolDown;
        int skillIndex = 0;
        skillIndex = mySkills.IndexOf(skill);
        Image skillImage = UI_Manager.skillContainer[skillIndex].transform.Find("CoolDown").GetComponent<Image>();
        while (timer > 0) 
        {
            
            timer -= Time.deltaTime;

            if (myTag == playerTag) skillImage.fillAmount = Mathf.Clamp01(timer / skill.coolDown);
            yield return null;
        }

        if (myTag == playerTag) skillImage.fillAmount = 0;
       
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
    private IEnumerator CoolDownDeny(Skills skill)
    {
        int skillIndex = 0;
        skillIndex = mySkills.IndexOf(skill);
        Image denyImage = UI_Manager.skillContainer[skillIndex].transform.Find("Deny").GetComponent<Image>();

        float timer = 0.5f;

        while (timer > 0)
        {
            if (!castedSkills.Contains(skill)) break;
            if(skill == mySkills[1]) attackScript.cast2Request = false;
            timer -= Time.deltaTime;
            if (myTag == playerTag) denyImage.enabled = true;
            yield return null;
        }
        if (myTag == playerTag) denyImage.enabled = false;

    }
    private void CalculateValue(Skills skill)
    {
        switch (skill.skillTpye)
        {
            case SkillTypes.Attack:
                Debug.Log("Casting Attack Skill");
                
                myStatManagerScript.currentDamage *= skill.value;
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

                myStatManagerScript.currentDamage /= skill.value;
                break;
            case SkillTypes.Stun:
                //Debug.Log("Casted Stun Skill");
                break;
        }
    }
    public void StunSkill(Skills skill)
    {
        Collider closestEnemy = null;
        Collider[] hittedEnemies = Physics.OverlapSphere(transform.position, 2f, myStatManagerScript.mask);
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
