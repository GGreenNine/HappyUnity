using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Dropdown))]
public class DropDownNetwork : MonoBehaviour
{
    public Dropdown Dropdown;
    
    private void Awake()
    {
        if (!Dropdown)
            Dropdown = GetComponent<Dropdown>();
    }

    public void CreateDropDown<T>(IEnumerable<T> data, Action<Dropdown.OptionData, T> optionDataOperation, UnityAction<int> OnValueChanged)
    {
        Dropdown.OptionDataList options = new Dropdown.OptionDataList();

        options.options.Add(new Dropdown.OptionData("Empty"));

        foreach (var info in data)
        {
            Dropdown.OptionData optionData = new Dropdown.OptionData();
            optionDataOperation(optionData, info);
            options.options.Add(optionData);
        }
        Dropdown.onValueChanged.AddListener(OnValueChanged);
        Dropdown.options = options.options;
    }
}