
namespace YondaimeFramework
{
    [System.Serializable]
    public struct CustomerType 
    {
        #region PUBLIC_VARIABLES
        public string type;
        #endregion

        #region PUBLIC_METHODS
        public bool Equals(CustomerType otherId)
        {
            return type == otherId.type;
        }
        #endregion
    }
}
