using UnityEngine;

public class StartAnimationByIndex : StartAnimation
{
	[SerializeField]
	private int m_index = 0;

	protected override void Awake ()
	{
		base.Awake ();

		if(m_mode == START_MODE.AWAKE)
		{
			if(Mathf.Approximately(m_delay, 0.0f))
			{
				m_animationController.PlayByIndex(m_index);
			}
			else m_animationController.PlayByIndexWithDelay(m_index, m_delay);
		}
	}

	private void OnEnable ()
	{
		if(m_mode == START_MODE.ON_ENABLE)
		{
			if(Mathf.Approximately(m_delay, 0.0f))
			{
				m_animationController.PlayByIndex(m_index);
			}
			else m_animationController.PlayByIndexWithDelay(m_index, m_delay);
		}
	}

	private void Start ()
	{
		if(m_mode == START_MODE.START)
		{
			if(Mathf.Approximately(m_delay, 0.0f))
			{
				m_animationController.PlayByIndex(m_index);
			}
			else m_animationController.PlayByIndexWithDelay(m_index, m_delay);
		}
	}
}
