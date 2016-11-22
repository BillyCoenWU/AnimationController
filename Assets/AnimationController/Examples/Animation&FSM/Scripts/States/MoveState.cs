using UnityEngine;

namespace CharacterState
{
	public class Move : ICharacterState
	{
		private Vector3 m_latePosition = Vector3.zero;

		private Character m_character = null;

		private bool m_lookingToRight = true;

		private float speed = 2.0f;

		private float LIMIT_X = 8.0f;

		public void EnterState (Character character)
		{
			m_character = character;

			m_character.animationController.PlayByType(ANIMATION_TYPE.WALK);

			m_lookingToRight = (m_character.transform.localScale.x > 0.0f);
		}

		public void FixedUpdate ()
		{
			m_character.transform.position += m_character.transform.right * (m_lookingToRight ? speed : -speed) * Time.fixedDeltaTime;

			m_latePosition = m_character.transform.position;
			m_latePosition.x = Mathf.Clamp(m_latePosition.x, -LIMIT_X, LIMIT_X);

			m_character.transform.position = m_latePosition;
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
				m_character.fromIdle = false;
				m_character.SetState(new Gesture());
				return;
			}

			if(Mathf.Approximately(0.0f, Input.GetAxis("Horizontal")))
			{
				m_character.SetState(new Idle());
				return;
			}

			if((Input.GetAxis("Horizontal") > 0.0f && !m_lookingToRight) ||
			   (Input.GetAxis("Horizontal") < 0.0f && m_lookingToRight))
			{
				m_lookingToRight = !m_lookingToRight;
				m_character.transform.localScale = new Vector3(m_character.transform.localScale.x * -1.0f,
															   m_character.transform.localScale.y,
															   m_character.transform.localScale.z);
			}
		}
	}
}