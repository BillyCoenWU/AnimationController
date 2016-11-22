using UnityEngine;
using CharacterState;

public class Character : MonoBehaviour
{
	private ICharacterState m_state = null;

	private AnimationController m_animationController = null;
	public AnimationController animationController
	{
		get
		{
			return m_animationController;
		}
	}

	[SerializeField]
	private LifeManager m_lifeManager = null;
	public LifeManager lifeManager
	{
		get
		{
			return m_lifeManager;
		}
	}

	private Transform m_transform = null;
	public new Transform transform
	{
		get
		{
			if(m_transform == null)
			{
				m_transform = base.transform;
			}

			return m_transform;
		}
	}

	private bool m_fromIdle = true;
	public bool fromIdle
	{
		set
		{
			m_fromIdle = value;
		}
	}

	private void Awake ()
	{
		m_animationController = GetComponent<AnimationController>();

		m_animationController.GetAnimationByType(ANIMATION_TYPE.ATTACK).onCompleteAnimation += EndGestureOrAttack;
		m_animationController.GetAnimationByType(ANIMATION_TYPE.GESTURE).onCompleteAnimation += EndGestureOrAttack;

		SetState(new Idle());
	}

	private void Update ()
	{
		if(m_state != null)
		{
			m_state.Update();
		}
	}

	private void FixedUpdate ()
	{
		if(m_state != null)
		{
			m_state.FixedUpdate();
		}
	}

	public void SetState (ICharacterState newState)
	{
		m_state = newState;

		if(m_state != null)
		{
			m_state.EnterState(this);
		}
	}

	private void EndGestureOrAttack ()
	{
		if(m_fromIdle)
		{
			SetState(new Idle());
		}
		else SetState(new Move());
	}
}
