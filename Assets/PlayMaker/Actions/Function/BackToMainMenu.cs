using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{

    [ActionCategory("Function")]
    public class BackToMainMenu : FsmStateAction
    {
        /// <summary>
        /// 1 disable every screen
        /// 2 destroy current song
        /// 3 activate login screen
        /// </summary>
        // Code that runs on entering the state.
        public override void OnEnter()
        {
            // 1
            var screens = MyScreen.singleton.screens;
            foreach (var item in screens)
                item.gameObject.SetActive(false);
            
            // 2
            if (MotionInfoScript.singleton != null)
            {
                GameObject.Destroy(MotionInfoScript.singleton.gameObject);
            }

            // 3
            var login = MyScreen.singleton.GetScreen("LOGIN");
            if (login)
                login.gameObject.SetActive(true);

            Finish();
        }
    }
}
