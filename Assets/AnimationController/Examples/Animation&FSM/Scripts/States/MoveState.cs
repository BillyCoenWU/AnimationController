using UnityEngine;

namespace CharacterState
{
	public class Move : ICharacterState
	{
		private Vector3 m_latePosition = Vector3.zero;

		private Character m_character = null;

		private float speed = 2.0f;

		private float LIMIT_X = 8.0f;

		private static string HORIZONTAL = "Horizontal";

		public void EnterState (Character character)
		{
			m_character = character;

			m_character.animationController.PlayByType(ANIMATION_TYPE.WALK);
		}

		public void FixedUpdate ()
		{
			m_character.transform.position += m_character.transform.right * (!m_character.animationController.spriteRenderer.flipX? speed : -speed) * Time.fixedDeltaTime;

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

			if(Mathf.Approximately(0.0f, Input.GetAxis(HORIZONTAL)))
			{
				m_character.SetState(new Idle());
				return;
			}

			if((Input.GetAxis(HORIZONTAL) > 0.0f && m_character.animationController.spriteRenderer.flipX) ||
			   (Input.GetAxis(HORIZONTAL) < 0.0f && !m_character.animationController.spriteRenderer.flipX))
			{
				m_character.animationController.spriteRenderer.flipX = !m_character.animationController.spriteRenderer.flipX;
			}
		}
	}
}