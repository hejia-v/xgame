using UnityEngine;

namespace Invector
{
    public  class vMonoBehaviour : MonoBehaviour
    {
        [SerializeField] [HideInInspector]       
        public bool openCloseEvents = false;
        [SerializeField][HideInInspector]
        public bool openCloseWindow = false;
    }  
}
