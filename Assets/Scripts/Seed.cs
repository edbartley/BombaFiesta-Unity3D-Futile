using System;
using UnityEngine;
using System.Collections;

public class Seed : FSprite
{	
	float mSeedSpeed = 20;
	float mXOff = 0;
	float mYOff = 0;


	public Seed (float offsetX, float offsetY) : base("seed.png")
	{
		this.isVisible = false;
		mXOff = offsetX;
		mYOff = offsetY;
	}
	
	public void fireFrom(float xPosition)
	{
		this.x = xPosition + mXOff;
		this.y = mYOff;
		this.isVisible = true;
	}
	
	public void update()
	{
		if(this.isVisible)
		{
			this.y += mSeedSpeed;
			
			if(this.y > Futile.halfHeight + this.height * 4)
			{
				this.isVisible = false;
			}
		}
	}
}