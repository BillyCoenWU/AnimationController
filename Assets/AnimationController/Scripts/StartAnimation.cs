using UnityEngine;

public enum START_MODE
{
	AWAKE = 0,
	ON_ENABLE = 1,
	START = 2
}

[RequireComponent (typeof (AnimationController))]
public class StartAnimation : MonoBehaviour
{
	protected AnimationController m_animationController = null;

	[SerializeField]
	protected START_MODE m_mode = START_MODE.AWAKE;

	[SerializeField]
	protected float m_delay = 0.0f;

	protected virtual void Awake ()
	{
		m_animationController = GetComponent<AnimationController>();
	}
}
