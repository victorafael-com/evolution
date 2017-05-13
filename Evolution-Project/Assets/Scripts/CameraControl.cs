using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class CameraControl : MonoBehaviour {
	[Range(0,1)]
	public float watchedPercent;
	int watchAmmount;
	private OrganismManager manager;

	private Camera cam;
	private Vector3 startPos;
	private float startSize;
	public float yOffset = 0.2f;
	public float padding = 0.4f;
	public float easingSpeed = 5;

	[Header("Canvas Positioning")]
	public RawImage imgRenderer;
	private Vector2 desiredSize;

	// Use this for initialization
	void Start () {
		manager = FindObjectOfType<OrganismManager> ();
		if (manager == null) {
			enabled = false;
			return;
		}
		cam = GetComponent<Camera> ();

		startSize = cam.orthographicSize;
		startPos = cam.transform.position;

		watchAmmount = (int)(manager.ammount * watchedPercent);

		desiredSize = imgRenderer.rectTransform.sizeDelta;

		RenderTexture texture = new RenderTexture ((int)desiredSize.x, (int)desiredSize.y, 16);

		cam.targetTexture = texture;
		imgRenderer.texture = texture;
	}
	
	// Update is called once per frame
	void Update () {
		//Not worried with the performance. This is not the method I want to use with the camera at the end.
		//Will keep that until I find a better method to move the camera;
		Organism[] organisms = manager.Organisms.OrderByDescending (o => o.MaxDistance).Take(watchAmmount).ToArray ();

		float maxX = organisms.Max (o => o.MaxX) + padding;
		float minX = organisms.Min (o => o.MinX) - padding;
		float maxY = (organisms.Max (o => o.MaxY) + padding) * 0.5f; //Ortographic size behaves like "radius" :P

		float ratio = (float)desiredSize.x / desiredSize.y;

		float Width = maxX - minX;
		float Height = Mathf.Max( Width / (ratio * 2), maxY);

		float lerp = easingSpeed * Time.deltaTime;
		cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, Height, lerp);

		Vector3 targetPos = new Vector3 (Mathf.Lerp (minX, maxX, 0.5f), Height - yOffset * Height, -10);;

		cam.transform.position = Vector3.Lerp (cam.transform.position, targetPos, lerp);
	}
}
