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
                CustomComponentBase c = go.AddComponent(componentName) as CustomComponentBase;
                
                while(dataLines[dataPointer].Length < 1) {
                    dataPointer++;//Clear any white space after the component token
                }
                
                if(c !=null){
                    //Pass the dataPointer as a (ref) to continue with any modifications
                    c.SetData(dataLines, ref dataPointer);
                } else {
                    Debug.Log("Error adding" + componentName + "! Ensure the name is typed correctly.");
                }
            } else if (dataLines[dataPointer].StartsWith("position:")) {
                string vec3Position = dataLines[dataPointer].Substring(dataLines[dataPointer].IndexOf(":") + 1);
                string[] posComponents = vec3Position.Split(',');
                go.transform.position = new Vector3(float.Parse(posComponents[0]), float.Parse(posComponents[1]);
                dataPointer++;
                continue;
            //else if (other keywords?) {
                //other processing
            //}
            } else {
                Debug.Log("Line: `" + dataLines[dataPointer] + "` not recognized as a valid token.");
                dataPointer++;
                continue;
            }
    }
    dataPointer = 0;
    return go;
}
                                                 
                                                    
