using System.Collections;
using UnityEngine;

public class MyPlayer : MonoBehaviour
{
	//Scriptable object which holds all the player's movement parameters. If you don't want to use it
	//just paste in all the parameters, though you will need to manuly change all references in this script
	public PlayerProp Data;

	#region COMPONENTS
	public Rigidbody2D RB { get; private set; }
	public Animator anim { get; private set; }
	public Shoot shootComp;
	public Drop dropComp;
	public GameObject playerGraphics;
	#endregion

	#region STATE PARAMETERS
	//Variables control the various actions the player can perform at any time.
	//These are fields which can are public allowing for other sctipts to read them
	//but can only be privately written to.
	public float targetSpeed;
	public bool IsFacingRight { get; private set; }
	public bool IsFrozen;
	public bool IsJumping { get; private set; }
	public bool CollisionWithWall;

	//Timers (also all fields, could be private and a method returning a bool could be used)
	public float LastOnGroundTime { get; private set; }

	//Jump
	private bool _isJumpCut;
	private bool _isJumpFalling;

	#endregion

	#region INPUT PARAMETERS
	public Vector2 _moveInput;

	public float LastPressedJumpTime { get; private set; }
	#endregion

	#region CHECK PARAMETERS
	//Set all of these up in the inspector
	[Header("Checks")]
	[SerializeField] private Transform _groundCheckPoint;
	//Size of groundCheck depends on the size of your character generally you want them slightly small than width (for ground) and height (for the wall check)
	[SerializeField] private Vector2 _groundCheckSize = new Vector2(0.49f, 0.03f);
	#endregion

	#region LAYERS & TAGS
	[Header("Layers & Tags")]
	[SerializeField] private LayerMask _groundLayer;
	#endregion

	private static MyPlayer instance;
	public static MyPlayer Instance
	{
		get
		{
			if (instance == null) instance = GameObject.FindObjectOfType<MyPlayer>();
			return instance;
		}
	}

	private void Awake()
	{
		RB = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		shootComp = GetComponentInChildren<Shoot>();
		dropComp = GetComponentInChildren<Drop>();
	}

	private void Start()
	{
		CollisionWithWall = false;
		SetGravityScale(Data.gravityScale);
		IsFacingRight = true;
	}

	private void Update()
	{
		if(IsFrozen)
		{
			RB.velocity = new Vector2(0, RB.velocity.y);
			_moveInput.x = 0;
			_moveInput.y = 0;
			targetSpeed = 0;
			anim.SetBool("isRunning", false);
			anim.SetFloat("speed", 0f);

		}
		if(!IsFrozen)
		{

			#region TIMERS
			LastOnGroundTime -= Time.deltaTime;
			#endregion

			#region INPUT HANDLER
			_moveInput.x = Input.GetAxisRaw("Horizontal");
			_moveInput.y = Input.GetAxisRaw("Vertical");

			if(_moveInput.x == 0)
				anim.SetBool("isRunning", false);
			else
            {
				CheckDirectionToFace(_moveInput.x > 0);
				anim.SetBool("isRunning", true);
			}

			if (Input.GetKeyDown(KeyCode.Space))
			{
				OnJumpInput();
			}

			if (Input.GetKeyUp(KeyCode.Space))
			{
				OnJumpUpInput();
			}
			#endregion

			#region COLLISION CHECKS
			if (!IsJumping)
			{
				//Ground Check
				if (Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _groundLayer) && !IsJumping) //checks if set box overlaps with ground
				{
					LastOnGroundTime = Data.coyoteTime; //if so sets the lastGrounded to coyoteTime
				}
			}
			#endregion

			#region JUMP CHECKS
			if (IsJumping && RB.velocity.y < 0)
			{
				IsJumping = false;
			}

			if (LastOnGroundTime > 0 && !IsJumping)
			{
				_isJumpCut = false;

				if (!IsJumping)
					_isJumpFalling = false;
			}

			if(IsJumping)
				anim.SetBool("isJumping", true);
			else
				anim.SetBool("isJumping", false);

			//Jump
			if (CanJump() && LastPressedJumpTime > 0)
			{
				anim.SetTrigger("takeOf");
				IsJumping = true;
				_isJumpCut = false;
				_isJumpFalling = false;
				Jump();
			}
			#endregion

			#region GRAVITY
			//Higher gravity if we've released the jump input or are falling
			else if (RB.velocity.y < 0 && _moveInput.y < 0)
			{
				//Much higher gravity if holding down
				SetGravityScale(Data.gravityScale * Data.fastFallGravityMult);
				//Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds
				RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -Data.maxFastFallSpeed));
			}
			else if (_isJumpCut)
			{
				//Higher gravity if jump button released
				SetGravityScale(Data.gravityScale * Data.jumpCutGravityMult);
				RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -Data.maxFallSpeed));
			}
			else if ((IsJumping || _isJumpFalling) && Mathf.Abs(RB.velocity.y) < Data.jumpHangTimeThreshold)
			{
				SetGravityScale(Data.gravityScale * Data.jumpHangGravityMult);
			}
			else if (RB.velocity.y < 0)
			{
				//Higher gravity if falling
				SetGravityScale(Data.gravityScale * Data.fallGravityMult);
				//Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds
				RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -Data.maxFallSpeed));
			}
			else
			{
				//Default gravity if standing on a platform or moving upwards
				SetGravityScale(Data.gravityScale);
			}
			#endregion
		}
	}

	private void FixedUpdate()
	{
		if(!IsFrozen)
			Run(1);
	}

	#region INPUT CALLBACKS
	//Methods which whandle input detected in Update()
	public void OnJumpInput()
	{
		LastPressedJumpTime = Data.jumpInputBufferTime;
	}

	public void OnJumpUpInput()
	{
		if (CanJumpCut())
			_isJumpCut = true;
	}
	#endregion

	#region GENERAL METHODS
	public void SetGravityScale(float scale)
	{
		RB.gravityScale = scale;
	}

	private void Sleep(float duration)
	{
		//Method used so we don't need to call StartCoroutine everywhere
		//nameof() notation means we don't need to input a string directly.
		//Removes chance of spelling mistakes and will improve error messages if any
		StartCoroutine(nameof(PerformSleep), duration);
	}

	private IEnumerator PerformSleep(float duration)
	{
		Time.timeScale = 0;
		yield return new WaitForSecondsRealtime(duration); //Must be Realtime since timeScale with be 0 
		Time.timeScale = 1;
	}
	#endregion

	//MOVEMENT METHODS
	#region RUN METHODS
	private void Run(float lerpAmount)
	{

        #region Speed Levels
        float speedCheckerL1 = CoinCounter.Instance.maxCoins / 2;     //Level 1 out of 2
		float speedCheckerL2 = CoinCounter.Instance.maxCoins; //Level 2 out of 2

		//Check Level 1 out of 2
		if(CoinCounter.Instance.Coins <= speedCheckerL1)
			targetSpeed = Mathf.Lerp(RB.velocity.x, _moveInput.x * Data.runMaxSpeed, lerpAmount);
		//Check Level 2 out of 2
		else if(CoinCounter.Instance.Coins <= speedCheckerL2)
			targetSpeed = Mathf.Lerp(RB.velocity.x, _moveInput.x * Data.runMaxSpeed, lerpAmount) * 2.5f / 4;
        #endregion

        if(IsFrozen)
			targetSpeed = 0;

		#region Animation Checks
		if(targetSpeed == 0 || CollisionWithWall)
		{
			anim.SetFloat("speed", 0f);
			anim.SetTrigger("run");
		}
		else if(targetSpeed != 0 || !CollisionWithWall)
		{
			anim.SetFloat("speed", Mathf.Lerp(0, 1, Data.runLerpAmount));
			anim.SetTrigger("run");
		}
        #endregion

        #region Calculate AccelRate
        float accelRate;

		//Gets an acceleration value based on if we are accelerating (includes turning) 
		//or trying to decelerate (stop). As well as applying a multiplier if we're air borne.
		if (LastOnGroundTime > 0)
			accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount : Data.runDeccelAmount;
		else
			accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount * Data.accelInAir : Data.runDeccelAmount * Data.deccelInAir;
		#endregion

		#region Add Bonus Jump Apex Acceleration
		//Increase are acceleration and maxSpeed when at the apex of their jump, makes the jump feel a bit more bouncy, responsive and natural
		if ((IsJumping || _isJumpFalling) && Mathf.Abs(RB.velocity.y) < Data.jumpHangTimeThreshold)
		{
			accelRate *= Data.jumpHangAccelerationMult;
			targetSpeed *= Data.jumpHangMaxSpeedMult;
		}
		#endregion

		#region Conserve Momentum
		//We won't slow the player down if they are moving in their desired direction but at a greater speed than their maxSpeed
		if (Data.doConserveMomentum && Mathf.Abs(RB.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(RB.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && LastOnGroundTime < 0)
		{
			//Prevent any deceleration from happening, or in other words conserve are current momentum
			//You could experiment with allowing for the player to slightly increae their speed whilst in this "state"
			accelRate = 0;
		}
		#endregion

		//Calculate difference between current velocity and desired velocity
		float speedDif = targetSpeed - RB.velocity.x;
		//Calculate force along x-axis to apply to thr player

		float movement = speedDif * accelRate;

		//Convert this to a vector and apply to rigidbody
		RB.AddForce(movement * Vector2.right, ForceMode2D.Force);

		/*
		 * For those interested here is what AddForce() will do
		 * RB.velocity = new Vector2(RB.velocity.x + (Time.fixedDeltaTime  * speedDif * accelRate) / RB.mass, RB.velocity.y);
		 * Time.fixedDeltaTime is by default in Unity 0.02 seconds equal to 50 FixedUpdate() calls per second
		*/
	}

	private void Turn()
	{
		//stores scale and flips the player along the x axis, 
		Vector3 scale = playerGraphics.transform.localScale;
		scale.x *= -1;
		playerGraphics.transform.localScale = scale;

		IsFacingRight = !IsFacingRight;
	}
	#endregion

	#region JUMP METHODS
	private void Jump()
	{
		//Ensures we can't call Jump multiple times from one press
		LastPressedJumpTime = 0;
		LastOnGroundTime = 0;

		#region Perform Jump
		//We increase the force applied if we are falling
		//This means we'll always feel like we jump the same amount 
		//(setting the player's Y velocity to 0 beforehand will likely work the same, but I find this more elegant :D)
		float force = Data.jumpForce;
		if (RB.velocity.y < 0)
			force -= RB.velocity.y;

		RB.AddForce(Vector2.up * force, ForceMode2D.Impulse);
		#endregion
	}
	#endregion


	#region CHECK METHODS
	public void CheckDirectionToFace(bool isMovingRight)
	{
		if (isMovingRight != IsFacingRight)
			Turn();
	}

	private bool CanJump()
	{
		return LastOnGroundTime > 0 && !IsJumping;
	}

	private bool CanJumpCut()
	{
		return IsJumping && RB.velocity.y > 0;
	}

	public void SetThrowing()
    {
		anim.SetBool("isThrowing", false);
		anim.SetTrigger("throw");
		anim.SetBool("isThrowing", true);
		shootComp.shooting = true;
	}

	public void InstantiateObject()
    {
		if(!shootComp.canShoot)
		{
			anim.SetBool("throwAction", true);
			shootComp.InstantiateBag();
		}
    }

	public void SetNotThrowing()
    {
		anim.SetBool("isThrowing", false);
		anim.SetBool("throwAction", false);
		shootComp.canShoot = false;
		shootComp.shooting = false;
		shootComp.bagsInstantiating = 0;
	}

	#endregion


	#region EDITOR METHODS
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(_groundCheckPoint.position, _groundCheckSize);
		Gizmos.color = Color.blue;
	}
	#endregion
}