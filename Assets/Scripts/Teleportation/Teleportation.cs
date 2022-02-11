using UnityEngine;

public class Teleportation : MonoBehaviour {
	private Camera _camera;
	[SerializeField] private GameObject playerCharacter;
	[SerializeField] private GameObject teleportationSphere;
	private GameObject _sphere;

	[SerializeField] private MovementActions mv;
	private bool teleportationEnabled = false;
	private Vector3 direction = Vector3.zero;
	
	void Start() {
		_camera = Camera.main;
	}

	void Awake() {
		mv = new MovementActions();
	}

	void OnEnable() {
		mv.Enable();
	}

	private bool keyPressed = false;
	void Update() {
		if (!keyPressed && mv.Player.teleport.ReadValue<float>() > 0) {
			keyPressed = true;
			HandleTeleportationCircle();
		} else if (keyPressed && mv.Player.teleport.ReadValue<float>() <= 0) {
			keyPressed = false;
		}

		if (teleportationEnabled) {
			var point = new Vector3(_camera.pixelWidth / 2.0f, _camera.pixelHeight / 2.0f, 0 );
			Ray ray = _camera.ScreenPointToRay(point);
			if (Physics.Raycast(ray, out var hit)) {
				if (hit.transform.CompareTag("Floor")) {
					direction = hit.point - ray.origin;
					_sphere.transform.position = hit.point;
				}
			}

			if (mv.Player.click.ReadValue<float>() > 0 ) {
				playerCharacter.GetComponent<CharacterController>().Move(direction);
				teleportationEnabled = false;
				Destroy(_sphere);
				direction = Vector3.zero;
			}
		}			
	}

	private void HandleTeleportationCircle() {
		if (!teleportationEnabled) {
			teleportationEnabled = keyPressed =  true;
			_sphere = Instantiate(teleportationSphere) as GameObject;
		} else {
			teleportationEnabled = false;
			Destroy(_sphere);
			direction = Vector3.zero;
		} 

	}
}