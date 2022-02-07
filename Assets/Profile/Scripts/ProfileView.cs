using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using YondaimeFramework;

namespace Tag
{
    public class ProfileView : CustomBehaviour
    {
        public Text Result1;
        public Text Result2;

        private IProfileComponent profileComponent;

        public void Awake()
        {
            profileComponent = GetComponentFromMyGameObject<IProfileComponent>();
        }

        public void OnTestClass() 
        {
            profileComponent.TestClass();
            Result1.text = profileComponent.GetClassResult();
        }

        public void OnTestInterface() 
        {
            profileComponent.TestInterface();
            Result2.text = profileComponent.GetInterfaceResult();
        }

    }

    public interface IProfileComponent 
    {
        public void SetIterations(int interations);
        public string GetClassResult();
        public string GetInterfaceResult();
        public void TestClass();
        public void TestInterface();

    }
}
