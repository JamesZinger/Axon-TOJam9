using UnityEngine;
using System.Collections;

public class ObjectGenerator : MonoBehaviour {
	float startY = 0;
	public Rigidbody2D background;
	float maxHeight;
	float _defaultBuffer = 50.0f;
	public float jumpForce;
	float _ratio = 4.0f/3.0f;
	float _size;
	public GameObject prefab;
	public GameObject currentPrefab;


	// Use this for initialization
	void Start () {
		_size = Camera.main.orthographicSize;
		currentPrefab = (GameObject)Instantiate(prefab, new Vector3((_size * _ratio) * 2, 2.35f, 0), Quaternion.identity);
		Game.Instance.Player.Jump += OnJump;
	}

	// Update is called once per frame
	void Update () {
		currentPrefab.rigidbody2D.velocity = new Vector2(-5.0f,0);
		Rect checkRect = new Rect(currentPrefab.transform.position.x, currentPrefab.transform.position.y, currentPrefab.transform.localScale.x, currentPrefab.transform.localScale.y);
		Debug.DrawLine(new Vector2(checkRect.x, checkRect.y), new Vector2(checkRect.x, checkRect.y - checkRect.height));
		Debug.DrawLine(new Vector2(checkRect.x, checkRect.y), new Vector2(checkRect.x + checkRect.width, checkRect.y));
		if(currentPrefab.transform.position.x < (_size * _ratio)){
			currentPrefab = (GameObject)Instantiate(prefab, new Vector3(GetNextDist(checkRect), currentPrefab.transform.position.y, 0), Quaternion.identity);
			Debug.Log(checkRect);
		}
	}

	void OnJump(){
		StartCoroutine(calcMaxHeight());
	}

	IEnumerator calcMaxHeight(){
		float initialVelocity;
		//This will trigger on the first pass
		yield return new WaitForFixedUpdate();
		//This will trigger after a frame, at the end of a fixed update.
		//return new WaitForEndOfFrame();
		initialVelocity = Game.Instance.Player.rigidbody2D.velocity.y;
		maxHeight = -(initialVelocity * initialVelocity) / (2.0f*Physics2D.gravity.y);
		Debug.Log(maxHeight);
	}
	float GetNextDist(Rect obstacle){
		//For now, return farDist so successfully jumping or sliding an object always results in safety
		Vector2 closeDist, farDist;
		//Check if we can jump the oncoming obstacle
		if(obstacle.y >= maxHeight){
			//Object is too tall to jump, therefore slide
			closeDist = new Vector2(obstacle.xMax, 0);
			farDist = new Vector2(obstacle.xMax + _defaultBuffer, 0);
			Debug.Log("Tall");
		}else{
			//Object is short enough to slide
			closeDist = CalcEarliestJump(obstacle);
			farDist = CalcLatestJump(obstacle);
			Debug.Log("Short");
		}
		Debug.Log(farDist);
		return farDist.x;
	}
	//Calculate the earliest take-off point and return the landing. Use the bottom-left corner of the player hitbox
	Vector2 CalcEarliestJump(Rect obstacle){
		Vector2 start;//Where the jump has to start
		Vector2 peak;//The peak of the first jump
		Vector2 collision;//Where the player hits the obstacle
		float timeToCollide;//Time between jump peak and collision
		float timeToLand;//Time from peak to ground (this shouldn't ever change

		collision = new Vector2(obstacle.xMax, obstacle.yMin);
		timeToCollide = Mathf.Sqrt(2.0f*(collision.y - maxHeight) / -Physics2D.gravity.y);
		peak.y = maxHeight;
		peak.x = collision.x - background.velocity.x*timeToCollide;
		timeToLand = Mathf.Sqrt( (2.0f*maxHeight)/(-Physics2D.gravity.y) );
		start.x = peak.x + background.velocity.x*timeToLand;
		start.y = 0;

		//Should be renamed end
		return start;
	}
	//Latest take-off point. Use the bottom-right corner of the player
	Vector2 CalcLatestJump(Rect obstacle){
		//All the same as above
		Vector2 start;
		Vector2 peak;
		Vector2 collision;
		float timeToCollide;
		float timeToLand;

		//The collision here is with the top left, not the top right, of the obstacle
		collision = new Vector2(obstacle.xMin, obstacle.yMin);
		timeToCollide = Mathf.Sqrt(2.0f*(collision.y - maxHeight) / -Physics2D.gravity.y);
		peak.y = maxHeight;
		peak.x = collision.x + background.velocity.x*timeToCollide;
		timeToLand = Mathf.Sqrt( (2.0f*maxHeight)/(-Physics2D.gravity.y) );
		start.x = peak.x + background.velocity.x*timeToLand;
		start.y = 0;

		//Should be renamed end
		return start;
	}
}
