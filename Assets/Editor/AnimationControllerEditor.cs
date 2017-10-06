using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using RGSMS.Animation;

[CustomEditor(typeof(AnimationController))]
public class AnimationControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        GUILayout.Space(10.0f);

        AnimationController animationController = (AnimationController)target;

        if (animationController.componentType == COMPONENT.SPRITE_RENDERER)
        {
            if(animationController.spriteRenderer == null)
            {
                if(GUILayout.Button("Set SpriteRenderer"))
                {
                    animationController.GetSpriteRenderer();
                }
            }

            animationController.spriteRenderer = (SpriteRenderer)EditorGUILayout.ObjectField("Sprite Renderer:", animationController.spriteRenderer, typeof(SpriteRenderer), true);
        }
        else
        {
            if (animationController.image == null)
            {
                if (GUILayout.Button("Set Image"))
                {
                    animationController.GetImage();
                }
            }

            animationController.image = (Image)EditorGUILayout.ObjectField("Image:", animationController.image, typeof(Image), true);
        }

        GUILayout.Space(10.0f);

        animationController.unscaledAnimation = EditorGUILayout.Toggle("Unscaled Time Animation:", animationController.unscaledAnimation);
        animationController.playOnStart = EditorGUILayout.Toggle("Play On Start:", animationController.playOnStart);
    }
}
