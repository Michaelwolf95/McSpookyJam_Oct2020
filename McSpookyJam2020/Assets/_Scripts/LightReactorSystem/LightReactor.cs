using UnityEngine;

namespace MichaelWolfGames
{
    public class LightReactor : MonoBehaviour
    {
        [HideInInspector] public bool isInLight = false;
        public virtual void OnEnterLight()
        {
            //Debug.Log("ENTER LIGHT");
            isInLight = true;
        }
        
        public virtual void OnExitLight()
        {
            //Debug.Log("EXIT LIGHT");
            isInLight = false;
        }
    }
}