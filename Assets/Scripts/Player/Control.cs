using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour
{

    [Header("Controle")]
    [SerializeField] float puissanceAcceleration=1f;
    [SerializeField] float torque=0.2f;
    [Space]
    [SerializeField] float fraction;
    [SerializeField] float airFractionLacet;
    [SerializeField] float torqueTournant;
    [SerializeField] float rappelForce;

    [Header("Effets")]
    [SerializeField] ParticleSystem particuleVitesse;
    [SerializeField] GameObject acclerationEffet;


    [SerializeField] WheelCollider Roue;
    //[SerializeField] Transform t;
    

    Vector3 initPosition;
    Quaternion initRot;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState= CursorLockMode.Locked;
        initPosition=transform.position;
        initRot=transform.rotation;
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void respawn(){
        transform.position = initPosition;
        transform.rotation = initRot;
    }

    private void FixedUpdate() {

        // Gestion de l'acceleration 
        if(Input.GetButton("Fire1")){ 
            rb.AddForce( transform.forward*puissanceAcceleration,ForceMode.Acceleration);
            acclerationEffet.SetActive(true);
        }else{
            acclerationEffet.SetActive(false);
        }

        if(Roue.isGrounded){
            // Gestion de la rotation avec la souris et controller xBOX
            transform.Rotate( ((- transform.forward ) * torque * Input.GetAxis("Horizontal")+ (transform.right) * torque/2 * Input.GetAxis("Vertical"))*Time.fixedDeltaTime,Space.World);

            if(rb.velocity.magnitude - rb.velocity.z > 20f)
                particuleVitesse.emissionRate=400f;
            
            // Proto Drift (on tourne juste fraction fois plus vite.)
            if(Input.GetButton("Fire2")){
                // L'inclinaison du pod fait tourner en lacet
                transform.Rotate(Vector3.up * Vector3.Dot(-transform.right,Vector3.up)*Time.fixedDeltaTime * torqueTournant * fraction,Space.World);
            }else{
                // L'inclinaison du pod fait tourner en lacet
                transform.Rotate(Vector3.up * Vector3.Dot(-transform.right,Vector3.up)*Time.fixedDeltaTime *torqueTournant ,Space.World);
                // Petit moment d'inertie pour remettre le pod droit (pas grand chose mais sympa)
                transform.Rotate(new Vector3(Vector3.Dot(transform.forward,Vector3.up)*rappelForce,0f,Vector3.Dot(-transform.right,Vector3.up)*rappelForce)*Time.fixedDeltaTime);
            }
        }else{
            particuleVitesse.emissionRate=0f;
            // Gestion de la rotation avec la souris et controller xBOX
            transform.Rotate(Vector3.up * Vector3.Dot(-transform.right,Vector3.up)*Time.fixedDeltaTime * torqueTournant * airFractionLacet,Space.World);
            transform.Rotate( (( - transform.forward ) *torque * Input.GetAxis("Horizontal") + (transform.right) * torque * Input.GetAxis("Vertical"))*Time.fixedDeltaTime ,Space.World);
        }

        
    }
}
