using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using YondaimeFramework;
using UnityEngine;

 public class ProfileView : CustomBehaviour
    {
        [SerializeField] Image overlayImage;
        public Text Result1;
        public Text Result2;

        IProfileComponent profileComponent;
        
        public void Start()
        {
            profileComponent = GetComponentFromMyGameObject<IProfileComponent>();
        }

        public void OnTestClass() 
        {
            overlayImage.gameObject.SetActive(true);
        
            profileComponent.TestClass();
            Result1.text = profileComponent.GetClassResult();

            overlayImage.gameObject.SetActive(false);
        }  

        public void OnTestInterface() 
        {
            overlayImage.gameObject.SetActive(true);
        
        profileComponent.TestInterface();
        
            Result2.text = profileComponent.GetInterfaceResult();
            overlayImage.gameObject.SetActive(false);
        }

        public void ResetResults() 
        {
          Result1.text = "Result:";
          Result2.text = "Result:";
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

