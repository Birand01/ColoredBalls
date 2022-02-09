using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    [Header("General Variables")]
    private Vector3 startMousePos, startBallPos;
    private bool moveTheBall;
    private Vector2 clampValue = new Vector2(-1.7f, 1.7f);
    private float velocity, cameraVelocity_x, cameraVelocity_y;
    private float pathDirection= 1000.0f;
    [Space]
    [Header("Variables in Range")]
    [Range(0.0f,1.0f)] public float maxSpeed;
    [Range(0.0f, 1.0f)] public float camSpeed;
    [Range(0.0f, 50.0f)] public float pathSpeed;
    [Range(0.0f, 3000.0f)] public float ballRotateSpeed;
    [Space]
    [Header("Private References")]
    private Transform ball;
    private Camera cam; 
    public Transform path;
    private Rigidbody playerRb;
    private Collider collider;
    private Renderer ballRenderer;
    [Space]
    [Header("Particle References(public)")]
    public ParticleSystem colliderParticle;
    public ParticleSystem airEffect;
    public ParticleSystem dustEffect;
    public ParticleSystem ballTrail;
    [Space]
    [Header("Material References")]
    public Material[] ballMats = new Material[2];
    void Start()
    { 
        cam = Camera.main; 
        ball = transform;
        playerRb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        ballRenderer =ball.GetChild(1).GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckTheBall();
        MoveTheBallAlongXAxis();
        MovePath();


    }

    private void CheckTheBall()
    { 
        if (Input.GetMouseButtonDown(0) && MenuManager.menuManagerInstance.gameState)
        {
            moveTheBall = true;
            ballTrail.Play();
            Plane newPlane = new Plane(Vector3.up, 0.0f);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (newPlane.Raycast(ray, out float distance))
            {
                startMousePos = ray.GetPoint(distance);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            moveTheBall = false;
        }   
    }
    private void MoveTheBallAlongXAxis()
    {
        if (moveTheBall)
        { 

            Plane newPlane = new Plane(Vector3.up, 0.0f);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (newPlane.Raycast(ray, out float distance))
            {
                Vector3 mouseNewPos = ray.GetPoint(distance);
                Vector3 MouseNewPos = mouseNewPos - startMousePos;
                Vector3 DesiredBallPos = MouseNewPos + startBallPos;

                // Limit the movement of the Ball on the x-axis
                DesiredBallPos.x = Mathf.Clamp(DesiredBallPos.x, clampValue.x, clampValue.y);
                //Gradually changes a value towards a desired goal over time
                ball.position = new Vector3(Mathf.SmoothDamp(ball.position.x, DesiredBallPos.x, ref velocity, maxSpeed), ball.position.y, ball.position.z);
            }
        }
    }
    private void MovePath() 
    {
        if(MenuManager.menuManagerInstance.gameState)
        {
            var pathNewPos = path.position;
            path.position = new Vector3(pathNewPos.x, pathNewPos.y, Mathf.MoveTowards(pathNewPos.z, -pathDirection, pathSpeed * Time.deltaTime));
            ball.GetChild(1).Rotate(Vector3.right * ballRotateSpeed/2.0f * Time.deltaTime);
        }
       
    }


    
    private void LateUpdate()
    {
        var CameraNewPos = cam.transform.position;
        if(playerRb.isKinematic)
        {
            cam.transform.position = new Vector3(Mathf.SmoothDamp(CameraNewPos.x, ball.transform.position.x, ref cameraVelocity_x, camSpeed)
            , Mathf.SmoothDamp(CameraNewPos.y, ball.transform.position.y+3.0f, ref cameraVelocity_y, camSpeed), CameraNewPos.z);
        }
          
    }

    private void OnTriggerEnter(Collider other)
    {
        switch(other.tag)
        {
            case "Red":
                other.gameObject.SetActive(false);
               // ballRenderer.material = other.GetComponent<Renderer>().material;
                var NewParticle = Instantiate(colliderParticle, transform.position, Quaternion.identity);
                NewParticle.GetComponent<Renderer>().material = other.GetComponent<Renderer>().material;

                var ballTrail1 = ballTrail.trails;
                ballTrail1.colorOverLifetime = other.GetComponent<Renderer>().material.color;

                ballMats[1] = other.GetComponent<Renderer>().material;
                ballRenderer.material = ballMats[1];
                break;
            case "Blue":
                other.gameObject.SetActive(false);
                ballRenderer.material = other.GetComponent<Renderer>().material;
                var NewParticle1 = Instantiate(colliderParticle, transform.position, Quaternion.identity);
                NewParticle1.GetComponent<Renderer>().material = other.GetComponent<Renderer>().material;

                var ballTrail2 = ballTrail.trails;
                ballTrail2.colorOverLifetime = other.GetComponent<Renderer>().material.color;

                ballMats[1] = other.GetComponent<Renderer>().material;
                ballRenderer.material = ballMats[1];
                break;
            case "Green":
                other.gameObject.SetActive(false); 
                other.gameObject.SetActive(false); 
                ballRenderer.material = other.GetComponent<Renderer>().material;
                var NewParticle2 = Instantiate(colliderParticle, transform.position, Quaternion.identity);
                NewParticle2.GetComponent<Renderer>().material = other.GetComponent<Renderer>().material;

                var ballTrail3 = ballTrail.trails;
                ballTrail3.colorOverLifetime = other.GetComponent<Renderer>().material.color;

                ballMats[1] = other.GetComponent<Renderer>().material;
                ballRenderer.material = ballMats[1];
                break;
            case "Yellow":
                other.gameObject.SetActive(false);
                ballRenderer.material = other.GetComponent<Renderer>().material;
                var NewParticle3 = Instantiate(colliderParticle, transform.position, Quaternion.identity);
                NewParticle3.GetComponent<Renderer>().material = other.GetComponent<Renderer>().material;

                var ballTrail4 = ballTrail.trails;
                ballTrail4.colorOverLifetime = other.GetComponent<Renderer>().material.color;

                ballMats[1] = other.GetComponent<Renderer>().material;
                ballRenderer.material = ballMats[1];
                break;
            case "Obstacle":
                this.gameObject.SetActive(false);
                MenuManager.menuManagerInstance.gameState = false;
                MenuManager.menuManagerInstance.menuElement[2].GetComponent<Text>().text = "You Lose";
                break;
        }

        if(other.gameObject.name.Contains("ColorBall"))
        {
            PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score") + 10);
            MenuManager.menuManagerInstance.menuElement[1].GetComponent<Text>().text = PlayerPrefs.GetInt("Score").ToString();
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag=="Path")
        {
            playerRb.isKinematic = false;
            collider.isTrigger = false;
            playerRb.velocity = new Vector3(0.0f, 8.0f, 0.0f);
            pathSpeed = pathSpeed*2;
            var airEffectMain = airEffect.main;
            airEffectMain.simulationSpeed = 10.0f;
            ballTrail.Stop();
            ballRotateSpeed = 3000.0f;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Path"))
        {
            playerRb.isKinematic = collider.isTrigger = true;
            pathSpeed = 30.0f;

            var airEffectMain = airEffect.main;
            airEffectMain.simulationSpeed = 4.0f;
             
            dustEffect.transform.position = collision.contacts[0].point+ new Vector3(0.0f,0.5f,0.0f);
            dustEffect.GetComponent<Renderer>().material = ballRenderer.material;
            dustEffect.Play();
            ballTrail.Play();
        }
    }

}
    
