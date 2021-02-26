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
            if (enemy.name == "Fluke Mother")
            {
                CensorObject(enemy);
            }

            // We've actually made a wrapper around the function the game uses
            // to check whether an object is dead. We need to not mess it up
            // by passing through the actual value unchanged.
            return isAlreadyDead;
        }

        // Note that this won't follow the target around. It's designed to
        // censor fluke marm, who spends most of her time vibing.
        private void CensorObject(GameObject target)
        {
            GameObject canvasObject = new GameObject("Black Bar Canvas");
            Canvas canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;

            GameObject blackBox = new GameObject("Black Box Censor");
            blackBox.AddComponent<CanvasRenderer>();

            // Prepare a 1x1 black sprite
            Texture2D blackTexture = new Texture2D(1, 1);
            blackTexture.SetPixel(0, 0, Color.black);
            blackTexture.Apply();
            Sprite blackSprite = Sprite.Create(blackTexture, new Rect(0, 0, 1, 1), Vector2.zero);

            Image image = blackBox.AddComponent<Image>();
            image.sprite = blackSprite;

            blackBox.transform.SetParent(canvasObject.transform);
            blackBox.transform.position = target.transform.position;
            blackBox.transform.localScale = new Vector3(0.05f, 0.08f);
        }
    }
}
