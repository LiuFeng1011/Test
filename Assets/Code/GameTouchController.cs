using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameDir
{
    Up = 0,
    Right = 1,
    Down = 2,
    Left = 3
}
/// <summary>
/// 玩家操作控制器
/// 
/// </summary>
public class GameTouchController{
    public delegate void OnTouchDown(Vector3 pos);
    public delegate void OnTouchUp(Vector3 pos);
    public delegate void OnTouchMove(Vector3 pos, Vector3 movedis);

    public OnTouchDown onTouchDown;
    public OnTouchUp onTouchUp;
    public OnTouchMove onTouchMove;

	float slideDis = 20;

	public Vector3 m_InputStartVec;
	public Vector3 m_InputFinalVec;
    private float m_Inputtime;

	//确保一次操作对应一次动作
	public bool isStartMouse = false;


	public void Init(){

	}
	// Update is called once per frame
	public void Update () {
		Control() ;
		KeyControl() ;
	}

    private void KeyControl(){
		if (Input.GetKeyDown (KeyCode.W))
		{
			ChangeDirection(GameDir.Up);
		}
		if (Input.GetKeyDown (KeyCode.S))
		{
			ChangeDirection(GameDir.Down);
		}
		if (Input.GetKeyDown (KeyCode.A))
		{
			ChangeDirection(GameDir.Left);
		}
		if (Input.GetKeyDown (KeyCode.D))
		{
			ChangeDirection(GameDir.Right);
		}
	}

	//捕捉玩家控制动作
	private void Control() 
	{
		if (Input.GetMouseButtonDown(0))
		{
			m_InputStartVec = Input.mousePosition;
			isStartMouse = true;
            TouchDown(m_InputStartVec);
		}
		else if (Input.GetMouseButtonUp(0))
        {
            TouchUp(Input.mousePosition);
		}
		if (isStartMouse && Input.GetMouseButton(0))
		{
            TouchMove(Input.mousePosition,Input.mousePosition-m_InputFinalVec);

            m_InputFinalVec = Input.mousePosition;

			float vy = m_InputFinalVec.y - m_InputStartVec.y;
			float vx = m_InputFinalVec.x - m_InputStartVec.x;

            if (Mathf.Abs(vy) > Mathf.Abs(vx))
            {
                if (vy >= slideDis)
                {
                    ChangeDirection(GameDir.Up);
                }
                else if (vy < -slideDis)
                {
                    ChangeDirection(GameDir.Down);
                }
            }
            else
            {
                if (vx >= slideDis)
                {
                    ChangeDirection(GameDir.Right);
                }
                else if (vx < -slideDis)
                {
                    ChangeDirection(GameDir.Left);
                }
            }

            if (m_Inputtime > 0.2f && (Mathf.Abs(vy) >= slideDis * 0.2f || Mathf.Abs(vx) >= slideDis * 0.2f))
            {
                if (Mathf.Abs(vy) >= Mathf.Abs(vx))
                {
                    if (vy >= slideDis * 0.2f)
                    {
                        ChangeDirection(GameDir.Up);
                    }
                    else if (vy < -slideDis * 0.2f)
                    {
                        ChangeDirection(GameDir.Down);
                    }
                }
                else 
                {
                    if (vx >= slideDis * 0.2f)
                    {
                        ChangeDirection(GameDir.Right);
                    }
                    else if (vx < -slideDis * 0.2f)
                    {
                        ChangeDirection(GameDir.Left);
                    }
                }

            }
            else 
            {
                m_Inputtime += Time.deltaTime;
            }
		}


	}

	//捕捉到动作
	public void ChangeDirection(GameDir dir){
        //Debug.Log("ChangeDirection : " + dir);
        //send touch dir event
	}

    public void TouchDown(Vector3 p){
        if (onTouchDown != null) onTouchDown(p);
    }
    public void TouchUp(Vector3 p)
    {

        if (onTouchUp != null) onTouchUp(p);
    }
    public void TouchMove(Vector3 p, Vector3 dis)
    {

        if (onTouchMove != null) onTouchMove(p, dis);
    }
}
