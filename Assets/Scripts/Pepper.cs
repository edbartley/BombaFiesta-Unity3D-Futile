using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pepper : FSprite
{
	enum FRAMES
	{
		HAPPY = 0,
		LOADING = 1,
		FIRE = 2,
		POW = 3
	};
	
	FRAMES mState = FRAMES.HAPPY;
	
	private static string[] mPepperFrames = {"chili_happy.png", "chili_loading.png", "chili_fire.png", "pow.png"};
	
	FSprite mPowSprite;
	
	BombaMain.FireDelegate mFireDelegate;

		
	public Pepper (BombaMain.FireDelegate fireDelegate) : base(mPepperFrames[(int)FRAMES.HAPPY])
	{
		mFireDelegate = fireDelegate;
		
		this.y = -Futile.halfHeight + this.height/2;
		
		mPowSprite = new FSprite(mPepperFrames[(int)FRAMES.POW]);
		mPowSprite.x = 18;
		mPowSprite.y = this.y + this.height/2 - 15;
	}
	
	public void setPosition(float xPos)
	{
		if(xPos < -Futile.halfWidth + this.width/4)
		{
			xPos = -Futile.halfWidth + this.width/4;
		}
		else if(xPos > Futile.halfWidth - this.width/4)
		{
			xPos = Futile.halfWidth - this.width/4;
		}
		
		this.container.x = xPos;
	}
	
	public void fireAnimation()
	{
		mState = FRAMES.LOADING;
	}
	
	public void happyAnimation()
	{
		mState = FRAMES.HAPPY;
	}

	
	private int frameCount = 0;

	public void update()
	{	
		switch(mState)
		{
		case FRAMES.LOADING:
			if(frameCount == 1)
			{
				this.SetElementByName(mPepperFrames[(int)FRAMES.LOADING]);
				this.container.RemoveChild(mPowSprite);
			}
			else if(frameCount >= 30)
			{
				frameCount = 0;
				mState = FRAMES.FIRE;
			}
			break;
		case FRAMES.FIRE:
			if(frameCount == 1)
			{
				this.SetElementByName(mPepperFrames[(int)FRAMES.FIRE]);
				this.container.AddChild(mPowSprite);
				mFireDelegate();
			}
			else if(frameCount >= 30)
			{
				frameCount = 0;
				mState = FRAMES.LOADING;
			}
		
			break;
			
		case FRAMES.HAPPY:
			this.SetElementByName(mPepperFrames[(int)FRAMES.HAPPY]);
			this.container.RemoveChild(mPowSprite);

			break;
		}
	
		frameCount++;
	}
}