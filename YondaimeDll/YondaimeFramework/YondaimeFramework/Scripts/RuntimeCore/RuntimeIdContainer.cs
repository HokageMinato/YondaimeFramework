using UnityEngine;


namespace YondaimeFramework
{
    [CreateAssetMenu(fileName = "RuntimeIdContainer", menuName = "YondaimeFramework/RuntimeIdContainer")]
    public class RuntimeIdContainer : ScriptableObject
    {
        [SerializeField] private ComponentId[] _ids;

        public ComponentId[] GetIds() 
        {
            return _ids;
        }

        public void SetIds(ComponentId[] ids) 
        { 
            _ids = ids; 
        }
        

    }
}
