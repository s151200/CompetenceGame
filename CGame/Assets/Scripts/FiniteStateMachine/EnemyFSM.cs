using UnityEngine;
using System.Collections;

public class EnemyFSM : CoroutineMachine {
	private Animator anim;
	private NavMeshAgent nav;
	private Player player;
	private bool caught;
	public Transform[] patrolWayPoints;

	public int patrolCount = 0;
	public float chasingRadius;
	public float waitTime = 2.0f;//player took clock
	public float timeAfterCaught = 3.0f; 

	public float slowDownTime = 2.0f;//worker caught the player
	public float slowDownSpeed = 1.0f;

	public float yellAnimationTime = 4f; //yelling animation time
	public float hatOnTime = 3f; //time the player is invisible

	void Awake() {
		// Setting up the references.

		// TODO SJW make animation control a bit more elegant in the code 
		// when walking and talking are working (function calls, enums...etc)
		anim = GetComponent<Animator>();
		anim.SetTrigger("isWalking"); // Default animation of all enemies is walking
		nav = GetComponent<NavMeshAgent>();
		player = GameObject.FindGameObjectWithTag(TagConstants.PLAYER).GetComponent<Player>();
	}

	// StateRoutine is a function pointer of type IEnumerator function()
	protected override StateRoutine InitialState {
		get {
			return StartState;
		}
	}

	/// <summary>
	/// Starting state: the enemy patrols to the first point and
	/// changes to PatrolState. If the player was caught it doesn't
	/// change to patrol state until a few seconds have passed.
	/// </summary>
	/// <returns>The state.</returns>
	IEnumerator StartState() {
		// start patroling 
		nav.destination = patrolWayPoints[patrolCount].position;

		//give some seconds to the player to run away after being caught by a worker
		if ( caught ) {
			yield return new WaitForSeconds(timeAfterCaught);
			caught = false;
		}
			
		yield return new TransitionTo(PatrolState, DefaultTransition);
	}

	/// <summary>
	/// The enemy patrols continuously in its patrolWaiPoints. 
	/// Checks if the player has stopped the time and changes to state StopState
	/// Checks if the player is close enough and changes to state ChaseState
	/// </summary>
	/// <returns>The state.</returns>
	IEnumerator PatrolState() {

		if ( Vector3.Distance(this.transform.position, patrolWayPoints[patrolCount].position) <= nav.stoppingDistance ) {
			patrolCount++;
			if ( patrolCount >= patrolWayPoints.Length ) {
				yield return new TransitionTo(StartState, DefaultTransition);
			}
			nav.destination = patrolWayPoints[patrolCount].position;
		}

		// if player has taken a clock, move to StopState 
		if ( player.clockOn ) {
			yield return new TransitionTo(StopState, DefaultTransition);
		}

		// if the player is close to the enemy, start chasing. But if she has taken a hat they don't see her
		if ( !player.hatOn && Vector3.Distance(this.transform.position, player.transform.position) <= chasingRadius ) {
			yield return new TransitionTo(ChaseState, DefaultTransition);
		}
			
		if ( player.hatOn ) {
			yield return StartCoroutine(HatOn());
		}

		// otherwise keep patroling 
		yield return new TransitionTo(PatrolState, DefaultTransition);
	}

	/// <summary>
	/// The enemy starts chasing the player. If it's caught by a boss
	/// the state changes to end (game over) and if it's caught by a worker
	/// the player is slow down and the worker keeps patroling from the
	/// start, giving the player a few seconds to run
	/// </summary>
	/// <returns>The state.</returns>
	IEnumerator ChaseState() {
		// chase player 
		nav.destination = player.transform.position;

		// if player has taken a clock, move to StopState 
		if ( player.clockOn ) {
			yield return new TransitionTo(StopState, DefaultTransition);
		}

		// if player has taken a hat, ignore her and start patroling again
		if ( player.hatOn ) {
			yield return new TransitionTo(StartState, DefaultTransition);
			yield return StartCoroutine(HatOn());
		}

		// if a boss catches the player, game over
		if ( caught && this.transform.tag == TagConstants.BOSS ) {
			yield return new TransitionTo(EndState, DefaultTransition);
		}
		// if a worker catches the player, slow him down and patrol from the start
		else if ( caught && this.transform.tag == TagConstants.WORKER ) {
			yield return StartCoroutine(SlowDownPlayer());
			yield return new TransitionTo(StartState, DefaultTransition);
		}

		yield return new TransitionTo(ChaseState, DefaultTransition);
	}
	/// <summary>
	/// Stops the enemy for waitTime seconds after the player 
	/// stopped the time by taking a clock. 
	/// </summary>
	/// <returns>The state.</returns>
	IEnumerator StopState() {
		// stop moving for waitTime seconds (time was stopped)
		nav.Stop();
		anim.enabled = false;
		yield return new WaitForSeconds(waitTime);
		player.clockOn = false;
		nav.Resume();
		anim.enabled = true;
		anim.SetTrigger("isWalking");

		// back to patrol state
		yield return new TransitionTo(StartState, DefaultTransition);
	}

	IEnumerator EndState() {
		// to show the yelling animation we wait for a few seconds
		yield return new WaitForSeconds(yellAnimationTime);
		yield return new TransitionTo(null, DefaultTransition);
	}

	/// <summary>
	/// Default transition: things we do while changing state
	/// </summary>
	/// <returns>The transition.</returns>
	/// <param name="from">From.</param>
	/// <param name="to">To.</param>
	IEnumerator DefaultTransition(StateRoutine from, StateRoutine to) {
		// set variable for patrol to 0 only if we move to next state 
		if ( from == PatrolState && to == ChaseState ||
		     from == PatrolState && patrolCount >= patrolWayPoints.Length ) {
			patrolCount = 0;
		}

		// game over
		if ( to == null ) {
			//TODO load end screen
			Application.LoadLevel(Application.loadedLevel);
		}

		//Debug.Log(string.Format("Transitioning from {0} to {1}", from.Method.Name, to == null ? "null" : to.Method.Name));
		yield return null;
	}

	void OnCollisionEnter(Collision other) {
		// player caught
		if ( other.transform.tag == TagConstants.PLAYER ) {
			//Debug.Log("BOSS CATCHES PLAYER");
			nav.Stop(); // We want to stop the boss when he is about to yell at the player
			anim.SetTrigger("isYelling"); // Boss yells at player when caught
			caught = true;
		}
	}

	/// <summary>
	/// Slows down the player (for slowDownTime seconds) after being caught by a worker.
	/// </summary>
	/// <returns>The down player.</returns>
	IEnumerator SlowDownPlayer() {
		player.agent.speed -= slowDownSpeed;
		yield return new WaitForSeconds(slowDownTime);
		player.agent.speed += slowDownSpeed;
	}
		
	IEnumerator HatOn() {
		yield return new WaitForSeconds(hatOnTime);
		player.hatOn = false;
	}
}
