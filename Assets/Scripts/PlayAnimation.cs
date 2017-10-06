using UnityEngine;
using RGSMS.Animation;

public enum GENERIC_TYPE
{
    IDLE = 0,
}

public class PlayAnimation : MonoBehaviour
{
    [SerializeField]
    private AnimationController m_controller = null;
    
    private void Start ()
    {
        //m_controller.GetAnimationByType(ANIMATION.IDLE).onCompleteAnimation += Final;
        m_controller.PlayByType(ANIMATION.IDLE);
    }

    private bool m_bool = true;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (m_bool)
            {
                Time.timeScale = 0.0f;
               // m_controller.Pause();
            }
            else
            {
                Time.timeScale = 1.0f;
                // m_controller.Resume();
            }

            m_bool = !m_bool;
        }
    }
    
    private void Final ()
    {
        gameObject.SetActive(false);
    }
}
