using UnityEngine;
using System.Collections;

public class StartAnimationByType : StartAnimation
{
	[SerializeField]
	private ANIMATION_TYPE m_type = ANIMATION_TYPE.EMPTY;

	protected override void Awake ()
	{
		base.Awake ();

		if(m_mode == START_MODE.AWAKE)
		{
			if(Mathf.Approximately(m_delay, 0.0f))
			{
				m_animationController.PlayByType(m_type);
			}
			else m_animationController.PlayByTypeWithDelay(m_type, m_delay);
		}
	}

	private void OnEnable ()
	{
		if(m_mode == START_MODE.ON_ENABLE)
		{
			if(Mathf.Approximately(m_delay, 0.0f))
			{
				m_animationController.PlayByType(m_type);
			}
			else m_animationController.PlayByTypeWithDelay(m_type, m_delay);
		}
	}

	private void Start ()
	{
		if(m_mode == START_MODE.START)
		{
			if(Mathf.Approximately(m_delay, 0.0f))
			{
				m_animationController.PlayByType(m_type);
			}
			else m_animationController.PlayByTypeWithDelay(m_type, m_delay);
		}
	}
}
