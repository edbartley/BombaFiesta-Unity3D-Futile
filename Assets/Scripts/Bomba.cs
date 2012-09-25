using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Bomba : FSprite
{
	private static string[] mBombaColors = {"bomba_black.png", "bomba_blue.png", "bomba_green.png", "bomba_orange.png", "bomba_purple.png", "bomba_red.png", "bomba_yellow.png"};
	
	private float mYSpeed = 0;
	public float GetSpeed
	{
		get
		{
			return mYSpeed;
		}
	}
	
	private bool mHasCandy = false;
	private Candy mCandy = null;
	
	public Candy SetGetCandy
	{
		set
		{
			if(value != null)
			{
				mHasCandy = true;
			}
			else
			{
				mHasCandy = false;
			}
			
			mCandy = value;
		}
		
		get
		{
			return mCandy;
		}
	}
	
	public bool SetGetHasCandy
	{
		get
		{
			return mHasCandy;
		}
		set
		{
			mHasCandy = value;
		}
	}
	
	public Bomba () : base(mBombaColors[(int)RXRandom.Range(0, mBombaColors.Length)])
	{
		this.x = RXRandom.Range(-Futile.halfWidth + this.width/2, Futile.halfWidth - this.width/2);
		
		this.y = -Futile.halfHeight	- this.height/2;
		
		mYSpeed = RXRandom.Range(5, 10);
	}
	
	public void update()
	{
		this.y += mYSpeed;
	}
}

