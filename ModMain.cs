using System;
using UnityEngine;
using UnityEngine.UI;

namespace HollowKnightCensored
{
    public class ModMain : Modding.Mod
    {
        public override string GetVersion() => "1.0.0";

        public ModMain() : base("Hollow Knight Censored")
        {
            //
        }

        public override void Initialize()
        {
            base.Initialize();

            Modding.ModHooks.Instance.OnEnableEnemyHook += HandleEnableEnemy;
        }

        private bool HandleEnableEnemy(GameObject enemy, bool isAlreadyDead)
        {
            CensorObject(enemy);

            // We've actually made a wrapper around the function the game uses
            // to check whether an object is dead. We need to not mess it up
            // by passing through the actual value unchanged.
            return isAlreadyDead;
        }

        // Censor the target with a black box. Will create a black box the same
        // size as the target currently is, and will anchor it to the center of
        // the target. If the target changes the size of its sprite/mesh/other-
        // material then the censor may get out of place.
        private void CensorObject(GameObject target)
        {
            // We need the game object to have a renderer so we can figure out
            // how big the object is (to make an effective censor for it)
            Renderer targetRenderer = target.GetComponent<Renderer>();
            if (targetRenderer == null) {
                return;
            }

            GameObject censor = new GameObject(
                "Censor",
                typeof(SpriteRenderer));

            // Create a 1x1 black sprite
            Texture2D blackTexture = new Texture2D(1, 1);
            blackTexture.SetPixel(0, 0, Color.black);
            blackTexture.Apply();
            Sprite blackSprite = Sprite.Create(
                blackTexture,
                new Rect(0, 0, 1, 1),
                // This makes the pivot point the center of sprite
                new Vector2(0.5f, 0.5f),
                // And this makes the sprite 1 world unit by 1 world unit. The
                // default value makes it 1/100 world unit by 1/100...
                1);

            SpriteRenderer renderer = censor.GetComponent<SpriteRenderer>();
            renderer.sprite = blackSprite;
            renderer.sortingLayerName = targetRenderer.sortingLayerName;
            renderer.sortingOrder = targetRenderer.sortingOrder + 1;

            // Size our censor to be the same size as the object, and keep it
            // anchored to the center of the object
            censor.transform.parent = target.transform;
            censor.transform.localPosition =
                target.transform.InverseTransformPoint(
                    targetRenderer.bounds.center);
            censor.transform.localScale =
                target.transform.InverseTransformVector(
                    targetRenderer.bounds.size);
        }
    }
}
