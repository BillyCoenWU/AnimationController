using UnityEngine;
using RGSMS.Animation;

public class PlayAnimation : MonoBehaviour
{
    private AnimationController m_animationController = null;

    private void Start ()
    {
        m_animationController = GetComponent<AnimationController>();

        m_animationController.Play(ANIMATION.IDLE);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            m_animationController.Play(ANIMATION.MOVE);
        }
    }
}
