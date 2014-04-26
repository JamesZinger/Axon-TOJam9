using UnityEngine;
using System.Collections;

public class ObjectGenerator : MonoBehaviour {
	float startY = 0;
	public Rigidbody2D background;
	float maxHeight;
	public float jumpForce;

	public GameObject prefab;
	public BoxCollider2D currentPrefab;


	// Use this for initialization
	void Start () {
		//Add GetInitialVelocity to event handler
		currentPrefab = Instantiate(prefab, Vector3(Screen.width/2, 0, 0), Quaternion.identity);
		Game.Instance
	}
	
	// Update is called once per frame
	void Update () {
		Rect checkRect = new Rect(currentPrefab.center.x - currentPrefab.size.x, currentPrefab.center.y - currentPrefab.size.y, currentPrefab.size.x, currentPrefab.size.y);
		if(currentPrefab.transform.position.x < Screen.width / 2){
			currentPrefab = Instantiate(prefab, Vector3(GetNextDist(checkRect), 0, 0), Quaternion.identity);
		}
	}

	void GetInitialVelocity(EventArgs e){
		StartCoroutine(calcMaxHeight);
	}

	IEnumerable calcMaxHeight(){
		float initialVelocity;
		//This will trigger on the first pass
		return new WaitForFixedUpdate();
		//This will trigger after a frame, at the end of a fixed update.
		//return new WaitForEndOfFrame();
		initialVelocity = Game.Instance.player.velocity;
		maxHeight = (initialVelocity * initialVelocity) / (2.0f*Physics2D.gravity.y);
	}
	float GetNextDist(Rect obstacle){
		//For now, return farDist so successfully jumping or sliding an object always results in safety
		float closeDist, farDist;
		//Check if we can jump the oncoming obstacle
		if(obstacle.y >= maxHeight){
			//Object is too tall to jump, therefore slide
			closeDist = obstacle.xMax;
			farDist = obstacle.xMax + slideDist;
		}else{
			//Object is short enough to slide
			closeDist = CalcEarliestJump(obstacle);
			farDist = CalcLatestJump(obstacle);
		}
		return farDist;
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
		peak.x = collision.x - background.velocity*timeToCollide;
		timeToLand = Mathf.Sqrt( (2.0f*maxHeight)/(-Physics2D.gravity.y) );
		start.x = peak.x + background.velocity*timeToLand;
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
		peak.x = collision.x + background.velocity*timeToCollide;
		timeToLand = Mathf.Sqrt( (2.0f*maxHeight)/(-Physics2D.gravity.y) );
		start.x = peak.x + background.velocity*timeToLand;
		start.y = 0;

		//Should be renamed end
		return start;
	}
}
