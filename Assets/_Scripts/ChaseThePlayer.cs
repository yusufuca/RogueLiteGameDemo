using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ChaseThePlayer : MonoBehaviour
{
    public Transform playerTransform;
    [SerializeField] private GameObject player;
    AnimEvents playerAnimEvents;
    StatManager statManager;
    [SerializeField] private float maxLookAheadX = 3;
    [SerializeField] private float maxLookAheadY = 3;
    [SerializeField] private float chaseSpeed = 1f;
    [SerializeField] private float minCenterDistance = 0.5f;
    [SerializeField] private float defaultX = 0;
    [SerializeField] private float defaultZ = -15;
    [SerializeField] private float defaultY = 15;
    [SerializeField] private float shakeMultiplier;
    [SerializeField] private float shakeSpeed = 0.1f;
    private Vector3 currentVelocity;
    private float shakeVelocity;
    private Vector3 originalEuler;
    private int lastSign = 1;
    private float playerWeight;
    [SerializeField] private Vector2 mousePos;

    private float lookAheadX;
    private float lookAheadZ;

    private Vector3 lookAheadVector;
    private void Awake()
    {
        playerAnimEvents = player.GetComponentInChildren<AnimEvents>();
        statManager = player.GetComponent<StatManager>();
        playerWeight = statManager.myData.movementStats.weight;
        originalEuler = transform.eulerAngles;
    }
    private void OnEnable()
    {
        playerAnimEvents.OnStepLanded += ScreenShake;
    }
    private void Update()
    {

        CalculateLookAhead();
    }
    void LateUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, playerTransform.position + lookAheadVector,ref currentVelocity , chaseSpeed);
    }
    private void CalculateLookAhead()
    {
        
        mousePos = new Vector2((Input.mousePosition.x / Screen.width) * 2 -1, (Input.mousePosition.y / Screen.height) *2 -1);
        float distance = mousePos.magnitude;
        if(distance < minCenterDistance)
        {
            lookAheadX = defaultX;
            lookAheadZ = defaultZ;
        }
        else
        {
            lookAheadX = mousePos.x * maxLookAheadX;
            lookAheadZ = (mousePos.y * maxLookAheadY) + defaultZ;
        }



        lookAheadVector = new Vector3(lookAheadX, defaultY, lookAheadZ);




    }
    private void ScreenShake()
    {
        StopCoroutine(ScreenShakeRoutine());
        StartCoroutine(ScreenShakeRoutine());
    }
    private IEnumerator ScreenShakeRoutine()
    {
        lastSign *= -1;
        float shake = UnityEngine.Random.Range(1f, 1.5f) * shakeMultiplier * (playerWeight / 1000);
        float targetEuler = originalEuler.z + shake * lastSign;
        float timer = 0;
        while (timer < shakeSpeed)
        {
            float newZ = Mathf.SmoothDampAngle(transform.eulerAngles.z, targetEuler,ref shakeVelocity, shakeSpeed);
            transform.eulerAngles = new Vector3(originalEuler.x,originalEuler.y,newZ);
            timer += Time.deltaTime;
            yield return null;
        }
        timer = 0;
        while (timer < shakeSpeed)
        {
            float newZ = Mathf.SmoothDampAngle(transform.eulerAngles.z, originalEuler.z, ref shakeVelocity, shakeSpeed);
            transform.eulerAngles = new Vector3(originalEuler.x, originalEuler.y, newZ);
            timer += Time.deltaTime;
            yield return null;
        }
        transform.eulerAngles = originalEuler;
        shakeVelocity = 0;
    }
    
}
