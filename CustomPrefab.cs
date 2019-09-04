using UnityEngine;
using System.Collections;

public class CustomPrefab {

    string name;
    string[] dataLines;
    int dataPointer = 0;
    
    public CustomPrefab(string name, string[] scriptLines) {
        this.name = name;
        this.dataLines = scriptLines;
    }
    
    //Create game object, then parse data file to add components and data.
    public GameObject Instantiate() {
        //Todo Custom Meshes go here.
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Mesh);
        go.name = name;
        while(dataPointer < dataLines.Length) {
            if(dataLines[dataPointer].Length < 1) {
                dataPointer++; //Skip any whitespace
                continue;
            }
            
            //Check for any keyword tokens
            if(dataLines[dataPointer].StartsWith("customcomponent:")) {
                //Separate the component name from the header
                string componentName = dataLines[dataPointer].Substring(dataLines[dataPointer].IndexOf(":") + 1)
                dataPointer++;
                //Unity uses C# reflection and allows us to simply pass the string name of the script we want to
                CustomComponentBase c = go.AddComponent(componentName) as CustomComponentBase;
