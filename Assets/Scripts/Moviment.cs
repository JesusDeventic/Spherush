using UnityEngine;
using System.Collections;

public class Moviment : MonoBehaviour {

	public GameObject trail_out;
	public GameObject trail;
	public GameObject bullet_out;
	public GameObject bullet1;
	public GameObject explosion_prefab;
	    
	public float lowSpeed;
	public float hightSpeed;

	GameObject bullet_temp; // Temporary bullet
    private float speed;
	private float altura = 0f;
	private float deltaRotation = 90f; // Angle for rotation of Spacecraft
	private float scaleRastro = 0f;
	private RaycastHit hit;

	// Se ejecuta aunque no se use el objeto. Antes del Start()
	void Awake() 
	{
		GameObject main_camera = GameObject.FindGameObjectWithTag("MainCamera");
		Debug.Log (main_camera.transform.position);
		speed = lowSpeed;
	}

	// Use this for initialization
	void Start ()
    {		
		trail = Instantiate (trail, trail_out.transform.position, trail_out.transform.rotation) as GameObject; 
	}

	void FixedUpdate () {
		if (Physics.Raycast (transform.position, transform.forward, out hit, 2f)) {
			Instantiate (explosion_prefab, this.transform.position, this.transform.rotation); // Instantiate blast
			Destroy (this.gameObject); // Destroy Spacecraft
			Destroy (hit.transform.gameObject); // Destroy Trail
		}
	}
	
	// Update is called once per frame
	void Update ()
	{				
		Movimiento ();
		Shoot();
		Avance ();
		crearEstela ();
	}   

	// MOVIMIENTOS DE LA NAVE
	private void Movimiento ()
	{
		// Control del giro
		if (Input.GetKeyDown(KeyCode.D)) {
			scaleRastro = 0f;
			this.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + 90, transform.rotation.eulerAngles.z);
			trail = Instantiate (trail, trail_out.transform.position, trail_out.transform.rotation) as GameObject;
		} else if (Input.GetKeyDown(KeyCode.A)) {
			scaleRastro = 0f;
			this.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y - 90, transform.rotation.eulerAngles.z);
			trail = Instantiate (trail, trail_out.transform.position, trail_out.transform.rotation) as GameObject;
		}  

		// Control del salto
		if (Input.GetKey (KeyCode.Space)) {
			if (altura < 200f) {
				altura = 200f;
				this.transform.Translate (new Vector3 (0f, altura, 0f) * Time.deltaTime);
			}
		} else {
			if (altura > 0f) {				
				this.transform.Translate (new Vector3 (0f, -altura, 0f) * Time.deltaTime);
				altura = 0f;
			}
		}
	}

	// DISPAROS DE LA NAVE
	private void Shoot () 
	{
		//if (bullet_temp == null) {
			// Disparo principal
			if (Input.GetKeyDown (KeyCode.Mouse0)) {
				print ("Nave: " + this.transform.position + ", Bullet: " + bullet_out.transform.position);

				bullet_temp = Instantiate (bullet1, bullet_out.transform.position, bullet_out.transform.rotation) as GameObject;
			}
		//} 
	}		

	// AVANCE DE LA NAVE
	private void Avance () {
		// Control de velocidad
		if (Input.GetKey(KeyCode.LeftShift)) {
			speed = hightSpeed * Time.deltaTime;
		}
		else {
			speed = lowSpeed * Time.deltaTime;
		}

		// Avance automatico
		this.transform.Translate(Vector3.forward * speed);
	}

	// REESCALADO Y SEGUIMIENTO DE LA ESTELA
	private void crearEstela () 
	{				
		// Vamos moviendo la estela hacia atras porque al crecer lo hace de ambos lados
		trail.transform.Translate (Vector3.down * speed / 2);
		// La incrementacion de la escala en cada frame debe ser la mitad que la velocidad,
		//ya que al escalar se escala de los dos extremos, es decir, incrementacion * 2 (implicito)
		scaleRastro += speed / 2;
		// Reescalamos la estela
		trail.transform.localScale = new Vector3(trail.transform.localScale.x, scaleRastro, trail.transform.localScale.z);
	}

	// COLISION DE LA NAVE
	void OnCollisionEnter (Collision colision)
	{
		ContactPoint contacto = colision.contacts [0]; // Point in collision entered
		Quaternion rotacion = Quaternion.FromToRotation(Vector3.up, contacto.normal); // Rotation this element

		Instantiate (explosion_prefab, contacto.point, rotacion); // Instantiate blast
		Destroy (this.gameObject); // Destroy Spacecraft
	}		
}
