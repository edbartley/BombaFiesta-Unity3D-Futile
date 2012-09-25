using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Candy : FContainer
{
	private static string[] mCandyColors = {"lolipop_bluen.png", "lolipop_green.png", "lolipop_purple.png", "lolipop_red.png", "lolipop_purple.png", "lolipop_yellow.png"};
	private static string mStickName = "lolipop_stick.png";
	
	public FSprite mCandySprite;
	private FSprite mStickSprite;
	public int pointValue = 0;
	
	private float mYSpeed = 0;
	
	private bool isFalling = false;
	
	public bool CandyFalling
	{
		get
		{
			return isFalling;
		}
		
		set
		{
			if(value == true)
			{
				mYSpeed = 0;
			}
			
			isFalling = value;
		}
	}
	
	public float SetSpeed
	{
		set
		{
			mYSpeed = value;
		}
	}
	
	public Candy ()
	{
		mStickSprite = new FSprite(mStickName);
		mStickSprite.anchorY = 1.0f;
		this.AddChild(mStickSprite);
		
		pointValue = (int)RXRandom.Range(0, mCandyColors.Length);
		
		mCandySprite = new FSprite(mCandyColors[pointValue]);
		mCandySprite.anchorY = 2.5f;
		this.AddChild(mCandySprite);
	}
	
	public void update()
	{
		if(!isFalling)
		{
			this.y += mYSpeed;
		}
		else
		{
			mYSpeed += 0.5f;
			this.y -= mYSpeed;
		}
	}
}