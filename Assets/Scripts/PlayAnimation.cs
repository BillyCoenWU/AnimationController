using UnityEngine;
using RGSMS.Animation;

public class PlayAnimation : MonoBehaviour
{
    private void Start ()
    {
        GetComponent<AnimationController>().Play(ANIMATION.IDLE);
    }
}
