using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YondaimeFramework;

public class TestIterationCountHolder : CustomBehaviour
{
	[SerializeField] InputField iterationField;
	public int iterationCount = 0;

	public void OnIterationCountChanged()
    {
        ParseIterationCountValue();
        ResetResults();
        SetIterationCount();
    }

    private void ParseIterationCountValue()
    {
        string value = iterationField.text;

        if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
            return;

        iterationCount = int.Parse(value);
    }

    private void ResetResults() 
    {
        List<ProfileView> profileTests = GetComponentsFromLibrary<ProfileView>();

        for (int i = 0; i < profileTests.Count; i++)
        {
            profileTests[i].ResetResults();
        }
    }

    private void SetIterationCount() 
    {
        List<IProfileComponent> profileTests = GetComponentsFromLibrary<IProfileComponent>();

        Debug.Log(profileTests.Count);

        for (int i = 0; i < profileTests.Count; i++)
        {
            profileTests[i].SetIterations(iterationCount);
        }
    }

}