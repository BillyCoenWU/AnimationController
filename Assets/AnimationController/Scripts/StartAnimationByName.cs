using UnityEngine;

public class StartAnimationByName : StartAnimation
{
	[SerializeField]
	private string m_name = "";

	protected override void Awake ()
	{
		base.Awake ();

		if(m_mode == START_MODE.AWAKE)
		{
			if(Mathf.Approximately(m_delay, 0.0f))
			{
				m_animationController.PlayByName(m_name);
			}
			else m_animationController.PlayByNameWithDelay(m_name, m_delay);
		}
	}

	private void OnEnable ()
	{
		if(m_mode == START_MODE.ON_ENABLE)
		{
			if(Mathf.Approximately(m_delay, 0.0f))
			{
				m_animationController.PlayByName(m_name);
			}
			else m_animationController.PlayByNameWithDelay(m_name, m_delay);
		}
	}

	private void Start ()
	{
		if(m_mode == START_MODE.START)
		{
			if(Mathf.Approximately(m_delay, 0.0f))
			{
				m_animationController.PlayByName(m_name);
			}
			else m_animationController.PlayByNameWithDelay(m_name, m_delay);
		}
	}
}
