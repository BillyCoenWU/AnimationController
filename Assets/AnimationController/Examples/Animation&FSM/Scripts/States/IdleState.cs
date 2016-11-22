using UnityEngine;

namespace CharacterState
{
	public class Idle : ICharacterState
	{
		private Character m_character = null;

		private int m_currentTimes = 0;

		public void EnterState (Character character)
		{
			m_character = character;

			m_character.animationController.PlayByType(ANIMATION_TYPE.IDLE);
		}

		public void Update ()
		{
			if(m_character.lifeManager.life <= 0)
			{
				m_character.SetState(new Death());
				return;
			}

			if(Input.GetKeyDown(KeyCode.Space))
			{
				m_character.SetState(new Attack());
				return;
			}

			if(Input.GetKeyDown(KeyCode.LeftControl))
			{
				m_character.fromIdle = true;
				m_character.SetState(new Gesture());
				return;
			}

			if(!Mathf.Approximately(0.0f, Input.GetAxis("Horizontal")))
			{
				m_character.SetState(new Move());
				return;
			}

			if(m_character.animationController.isDone)
			{
				m_currentTimes++;

				if(m_currentTimes >= 10)
				{
					m_character.fromIdle = true;
					m_character.SetState(new Gesture());
				}
			}
		}

		public void FixedUpdate () {}
	}
}
