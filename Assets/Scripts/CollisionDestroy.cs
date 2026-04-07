using UnityEngine;
using System.Collections;

public class CollisionDestroy : MonoBehaviour {

	void OnCollisionEnter () {
		Destroy (this.gameObject); // Destroy Trail
	}
}
