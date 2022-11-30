using UnityEngine;
using UnityEngine.UI;

namespace HutongGames.PlayMaker.Actions
{

    [ActionCategory(ActionCategory.GUI)]
    public class AnimateUIColor : ComponentAction<Image>
    {
        [RequiredField]
        [CheckForComponent(typeof(Image))]
        public FsmOwnerDefault gameObject;

        public FsmColor color1, color2;
        public FsmAnimationCurve curve;

        private Image script;

        public override void OnEnter()
        {
            var go = Fsm.GetOwnerDefaultTarget(gameObject);
            if (UpdateCache(go))
                script = cachedComponent;
        }

        public override void OnUpdate()
        {
            var value = curve.curve.Evaluate(Time.time);
            var target_color = Color.Lerp(color1.Value, color2.Value, value);
            script.color = Color.Lerp(script.color, target_color, 8f * Time.deltaTime);
        }
    }
}
