using UnityEngine;

namespace CharacterState
{
	public class Attack : ICharacterState
	{
		private Character m_character = null;

		public void EnterState (Character character)
		{
			m_character = character;

			m_character.animationController.PlayByType(ANIMATION_TYPE.ATTACK);
		}

		public void Update ()
		{
			if(m_character.lifeManager.life <= 0)
			{
				m_character.SetState(new Death());
				return;
			}
		}

		public void FixedUpdate () {}
	}
}
