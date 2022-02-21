using UnityEngine;


namespace YondaimeFramework
{
    [CreateAssetMenu(fileName = "RuntimeIdContainer", menuName = "YondaimeFramework/RuntimeIdContainer")]
    public class RuntimeIdContainer : ScriptableObject
    {
        
        [SerializeField][HideInInspector] private ComponentId[] _ids;



        public ComponentId[] GetIds() 
        {
            if (_ids.Length == 0) 
            {
                throw new System.Exception("No Runtime Id Generated,Please Generate RuntimeIds via CentralIdDataSo.");
            }
            return _ids;
        }

        public void SetIds(ComponentId[] ids) 
        { 
            _ids = ids; 
        }
        


    }
}
