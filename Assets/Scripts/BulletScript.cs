using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour 
{
	public GameObject explosion_prefab;

	private RaycastHit hit;

	void Start () {
		this.transform.Translate(Vector3.forward * 150f * Time.deltaTime);
		Destroy (this.gameObject, 3f);
	}

	void FixedUpdate () {
		if (Physics.Raycast (transform.position, transform.forward, out hit, 5f)) {
			Instantiate (explosion_prefab, this.transform.position, this.transform.rotation); // Instantiate blast
			Destroy (this.gameObject); // Destroy Bullet
			Destroy (hit.transform.gameObject); // Destroy Trail
		}
	}

	void Update () {
		this.transform.Translate(Vector3.forward * 200f * Time.deltaTime);
	}

	// On Collision
	void OnCollisionEnter (Collision collision) {
		ContactPoint contact = collision.contacts [0]; // Point in collision entered
		Quaternion rotation = Quaternion.FromToRotation(Vector3.up, contact.normal); // Rotation this element

		Instantiate (explosion_prefab, contact.point, rotation); // Instantiate blast
		Destroy (this.gameObject); // Destroy Bullet
	}
}
