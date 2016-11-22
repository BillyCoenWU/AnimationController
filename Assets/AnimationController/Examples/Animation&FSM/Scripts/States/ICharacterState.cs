namespace CharacterState
{
	public interface ICharacterState
	{
		void EnterState (Character character);
		void FixedUpdate ();
		void Update ();
	}
}
