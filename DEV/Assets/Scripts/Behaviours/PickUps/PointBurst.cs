using UnityEngine;
using System.Collections;

public class PointBurst : MonoBehaviour 
{
    private Rect imgRect;
    private Rect groupRect;
    private Rect costRect;

    GUISkin skin;
    float cost;
    GUIStyle style;
    int keys;

    float moveY;

    float elapsedTime;
    const float MAX_TIME = 3f;

	// Use this for initialization
	void Start () 
    {
        style = Game.Instance.Skin.GetStyle("Cost");
        skin = Game.Instance.Skin;
        imgRect = new Rect(0,0, 100, 50);
        groupRect = new Rect(200, 100, 100, 100);
        costRect = new Rect(0, 50, 100, 50);

        moveY = -10;
	}

    public void SetUpForFurniture(Vector2 pos, float cost, int keys)
    {
        style = Game.Instance.Skin.GetStyle("Cost");
        groupRect.Set(pos.x, pos.y, 100, 100);
        this.cost = cost;
        this.keys = keys;
    }
    public void SetUpForCash(Vector2 pos, float cost)
    {
        style = Game.Instance.Skin.GetStyle("Charge");
        groupRect.Set(pos.x, pos.y, 100, 100);
        this.cost = cost;
        keys = 0;
        
    }
	
	// Update is called once per frame
	void Update () 
    {
		if ( Game.Instance.IsPaused )
			return;

        if (elapsedTime < (MAX_TIME + 1))
        {
            elapsedTime += Time.fixedDeltaTime; 
            groupRect.y += moveY * Time.fixedDeltaTime * 10;
        }

		if ( elapsedTime >= MAX_TIME )
		{
			Game.Instance.PointBursts.Dequeue();
			Destroy( this.gameObject );
		}
        
	}

    void OnGUI()
    {
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1.0f * Screen.width / 856, 1.0f * Screen.height / 642, 1.0f));

        GUI.BeginGroup(groupRect);
        {
            if (keys > 0) GUI.Box(imgRect, "+" + keys, skin.GetStyle("Allen Key"));
            GUI.Label(costRect, "$" + cost.ToString("0.00"), style);
        }
        GUI.EndGroup();
    }
}
