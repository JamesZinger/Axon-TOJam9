using UnityEngine;
using System.Collections;

public class PointBurst : MonoBehaviour 
{
    private Rect imgRect;
    private Rect groupRect;
    private Rect costRect;

    Texture2D img;
    string text = "";
    GUISkin skin;
    float cost;
    int keys;

    float moveY;

    float elapsedTime;
    const float MAX_TIME = 3f;

	// Use this for initialization
	void Start () 
    {
        skin = Game.Instance.Skin;
        img = (Texture2D)Resources.Load("Sprites/GUI/UI/allanKeyNotification", typeof(Texture2D));
        imgRect = new Rect(0,0, 100, 50);
        groupRect = new Rect(200, 100, 100, 100);
        costRect = new Rect(0, 50, 100, 50);

        moveY = -10;
	}

    public void SetUp(Vector2 pos, float cost, int keys)
    {
        groupRect.Set(pos.x, pos.y, 100, 100);
        this.cost = cost;
        this.keys = keys;
    }
	
	// Update is called once per frame
	void Update () 
    {
        if (elapsedTime < (MAX_TIME + 1))
        {
            elapsedTime += Time.fixedDeltaTime; 
            groupRect.y += moveY * Time.fixedDeltaTime * 10;
        }

        if (elapsedTime >= MAX_TIME)
            Destroy(this.gameObject);
        
	}

    void OnGUI()
    {
        GUI.skin = skin;
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1.0f * Screen.width / 856, 1.0f * Screen.height / 642, 1.0f));

        GUI.BeginGroup(groupRect);
        {
            if (keys > 0) GUI.Box(imgRect, "+" + keys, skin.GetStyle("Allen Key"));
            GUI.Label(costRect, "-$" + cost, skin.GetStyle("Cost"));
        }
        GUI.EndGroup();
    }
}
