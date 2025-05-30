using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public abstract class BaseTerminal : MonoBehaviour
{
    [SerializeField] private TextAsset luaScriptFile;
    [SerializeField] private TerminalAssignedObjects assignedObjects;
    [SerializeField] private TerminalCanvas canvas;
    [SerializeField] protected List<VariablePlatform> allLevelPlatforms = new();

    protected string luaScript;
    protected Script script;
    protected string functionName;


    protected virtual void Start()
    {
        if (LoadScriptContents())
        {
            SetupBaseVariables();
            PrepareTerminal();
            AssignObjects();
            OnScriptReady();
        }
    }

    protected virtual void OnScriptReady()
    {
        if (allLevelPlatforms.Any(platform => script.Globals[platform.variableName] == null))
        {
            ShowCodeIsWorking(false);
            return;
        }

        ShowCodeIsWorking(true);
    }

    protected void PrepareTerminal()
    {
        canvas.SetCodeText(luaScript);
        //canvas.SetVariableText(script.Globals, allLevelPlatforms);
    }

    protected void AssignObjects()
    {
        foreach (ObjectOnLevel onLevel in assignedObjects.objectsOnLevels)
        {
            if (onLevel.gameObject == null)
            {
                Debug.LogError($"[Terminal] GameObject '{onLevel.name}' is null!");
            }
            else
            {
                script.Globals[onLevel.name] = UserData.Create(onLevel.gameObject);
                Debug.Log($"[Terminal] Injected '{onLevel.name}' into Lua.");
            }
        }
    }

    protected virtual bool LoadScriptContents()
    {
        if (luaScriptFile != null)
        {
            luaScript = luaScriptFile.text;
            var match = Regex.Match(luaScript, @"function\s+([a-zA-Z_][\w]*)");
            if (match.Success)
            {
                functionName = match.Groups[1].Value;
                Debug.Log("Found function: " + functionName);
            }
            return true;
        }

        Debug.LogError("Lua script not assigned.");
        return false;
    }

    protected virtual void SetupBaseVariables()
    {
        script = ScriptRunner.Instance.GetScript();

        foreach (VariablePlatform platform in allLevelPlatforms)
        {
            script.Globals[platform.variableName] = DynValue.Nil;
            platform.variableAdded.AddListener(UpdateVariable);
            platform.variableRemoved.AddListener(RemoveVariable);
        }
    }

    protected void UpdateVariable(string variableName, object value)
    {
        switch (value)
        {
            case bool b: script.Globals[variableName] = DynValue.NewBoolean(b); break;
            case int i: script.Globals[variableName] = DynValue.NewNumber(i); break;
            default: script.Globals[variableName] = DynValue.Nil; break;
        }

        //canvas.SetVariableText(script.Globals, allLevelPlatforms);
        OnVariableChanged();
    }

    protected void RemoveVariable(string variableName)
    {
        script.Globals[variableName] = DynValue.Nil;
        //canvas.SetVariableText(script.Globals, allLevelPlatforms);
        OnVariableChanged();
    }

    protected void ResetAssignedElements()
    {
        assignedObjects.ResetObjects();
    }

    protected void ShowCodeIsWorking(bool isWorking)
    {
        canvas.SetImageColor(isWorking);
    }
    protected virtual void OnVariableChanged()
    {

    }
}
