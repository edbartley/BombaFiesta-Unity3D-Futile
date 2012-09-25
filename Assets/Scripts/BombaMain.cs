using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class BombaMain : MonoBehaviour, FSingleTouchableInterface
{
	private int frameCounter = 0;
	public int mTouchPriority = 1;
	private int score = 0;
	
	private FContainer mBombaNode;
	private FContainer mPepperNode;
	
	private List<Bomba> mBombaList = new List<Bomba>();
	private List<Candy> mCandyList = new List<Candy>();

	private Pepper mPepper;
	
	public delegate void FireDelegate();
	
	private Seed mSeed;
	
	private FLabel mScoreLabel;

	
	// Use this for initialization
	void Start ()
	{
		FutileParams fParms = new FutileParams(false,false,true,true);
		fParms.AddResolutionLevel(1024.0f,1.0f,1.0f,"");
		fParms.origin = new Vector2(0.5f, 0.5f);
		Futile.instance.Init(fParms);
		
		Futile.atlasManager.LoadAtlas("Atlases/art");
		Futile.atlasManager.LoadFont("Franchise", "FranchiseFontAtlas.png", "Atlases/FranchiseLarge");
		
		FSprite background = new FSprite("background.png");
		Futile.stage.AddChild(background);
		Futile.stage.AddChild(mBombaNode = new FContainer());
		Futile.stage.AddChild(mPepperNode = new FContainer());

		mPepperNode.AddChild(mPepper = new Pepper(new FireDelegate(fire)));
		
		mSeed = new Seed(18, mPepper.y + mPepper.height/2 - 15);
		Futile.stage.AddChild(mSeed);
		
		mScoreLabel = new FLabel("Franchise", "Score :");
		mScoreLabel.anchorX = 0;
		mScoreLabel.anchorY = 1;
		mScoreLabel.color = new Color(1.0f,0.90f,0.0f);
		mScoreLabel.x = -Futile.halfWidth;
		mScoreLabel.y = Futile.halfHeight;

		Futile.stage.AddChild(mScoreLabel);
		
		Futile.touchManager.AddSingleTouchTarget(this);
		
		FSoundManager.PlayMusic("music", 0.45f);
	}
	
	// Update is called once per frame
	void Update ()
	{
		frameCounter++;
		
		if(frameCounter % 45 == 0)
		{
			createBomba();
		}
		
		for(int index = mBombaList.Count - 1; index >=0; --index)
		{
			Bomba bomba = mBombaList[index];
			bomba.update();
			
			if(bomba.SetGetHasCandy == true)
			{
				bomba.SetGetCandy.update();
			}
			
			if(bomba.y > Futile.halfHeight + bomba.height*2)
			{
			 	if(bomba.SetGetHasCandy == true)
				{
					bomba.SetGetCandy.RemoveFromContainer();
					bomba.SetGetCandy = null;
				}
				
				bomba.RemoveFromContainer();
				mBombaList.Remove(bomba);
			}
			else if(mSeed.isVisible )
			{
				Rect bombaRec = new Rect(bomba.boundsRect);
				bombaRec.x = bomba.x - bomba.width/2;
				bombaRec.y = bomba.y - bomba.height/2;
				bombaRec.width = bomba.width;
				bombaRec.height = bomba.height;
				
				Rect seedRec = new Rect(mSeed.boundsRect);
				seedRec.x = mSeed.x;
				seedRec.y = mSeed.y;
				seedRec.width = mSeed.width;
				seedRec.height = mSeed.height;
				
				if(seedRec.CheckIntersect(bombaRec))
				{
					if(bomba.SetGetHasCandy == true)
					{
						Candy candy = bomba.SetGetCandy;
						
						bomba.SetGetCandy.RemoveFromContainer();
						bomba.SetGetCandy = null;
						
						candy.CandyFalling = true;
						
						Futile.stage.AddChild(candy);
						mCandyList.Add(candy);
					}
					
					FSoundManager.PlaySound("pop", 1.0f);

					mSeed.isVisible = false;
					addPoint();
					bomba.RemoveFromContainer();
					mBombaList.Remove(bomba);
				}
			}
		}
		
		for(int index = mCandyList.Count - 1; index >=0; --index)
		{	
			Candy candy = mCandyList[index];
			
			if(candy.y < -(Futile.halfHeight + candy.mCandySprite.height))
			{
				candy.RemoveFromContainer();
				mCandyList.Remove(candy);
			}
			else
			{
				Rect candyRec = new Rect(candy.mCandySprite.boundsRect);
				candyRec.x = candy.x - candy.mCandySprite.width/2;
				candyRec.y = candy.y - candy.mCandySprite.height/2;
				candyRec.width = candy.mCandySprite.width;
				candyRec.height = candy.mCandySprite.height;
					
				Rect chiliRec = new Rect(mPepper.boundsRect);
				chiliRec.x = mPepperNode.x;
				chiliRec.y = mPepper.y;
				chiliRec.width = mPepper.width;
				chiliRec.height = mPepper.height/2;
								
				if(chiliRec.CheckIntersect(candyRec))
				{
				 	FSoundManager.PlaySound("candy", 1.0f);

					addPoint(candy.pointValue);
					candy.RemoveFromContainer();
					mCandyList.Remove(candy);
				}
				else
				{
					candy.update();
				}
			}
		}
		
		mSeed.update();
		mPepper.update();
	}
	
	void createBomba()
	{
		Bomba bomba = new Bomba();
		
		if(RXRandom.Range(0, 100) > 60)
		{
			Candy candy = new Candy();
			
			bomba.SetGetCandy = candy;
			
			candy.SetSpeed = bomba.GetSpeed;
			
			candy.x = bomba.x;
			candy.y = bomba.y;
			
			mBombaNode.AddChild(candy);
		}
	
		mBombaNode.AddChild(bomba);
		mBombaList.Add(bomba);
	}
	
	public bool HandleSingleTouchBegan(FTouch touch)
	{
		mPepper.fireAnimation();
		return true;
	}
	
	public int rejector = 0;
	public void HandleSingleTouchMoved(FTouch touch)
	{
		rejector++;
		
		if(rejector > 0)
		{
			float xPos = mBombaNode.GlobalToLocal(touch.position).x;
		
			mPepper.setPosition(xPos);
			rejector = 0;
		}
	}

	public void HandleSingleTouchEnded(FTouch touch)
	{
		mPepper.happyAnimation();
	}

	public void HandleSingleTouchCanceled(FTouch touch)
	{
		mPepper.happyAnimation();
	}
	
	public int touchPriority
	{
		get
		{
			return mTouchPriority;
		}
	}
	
	public void fire()
	{
		if(!mSeed.isVisible)
		{
			mSeed.fireFrom(mPepperNode.x);
		 	FSoundManager.PlaySound("fire", 1.0f);
		}
	}
	
	private void addPoint()
	{
		addPoint(1);
	}
	
	private void addPoint(int qty)
	{
		score += qty;
		mScoreLabel.text = "Score :" + score;
	}
}




















