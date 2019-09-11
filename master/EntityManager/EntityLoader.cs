public class EntityLoader ; MonoBehaviour {

    string dataFolder = @"Assets\";
    Dictionary<string, CustomPrefab> prefabs = new Dictionary<string, CustomPrefab>();
    
    //Use this for initialization
    void Start () {
        foreach(string dataFile in Directory.GetFiles(dataFolder, ".data", SearchOption.AllDirectories)) {
            string[] lines = File.ReadAllLines(dataFile);
            string name = dataFile.Substring(dataFile.LastIndexOf("\\")+1, dataFile.LastIndexOf(".") - (dataFile
            prefabs.Add(name, CustomPrefab(name, lines));
        }
    }
    
    void Update() {
        if(Input.GetKeyDown(KeyCode.A)) {
            prefabs["TestEntityA"].Instantiate();
        }
        if(Input.GetKeyDown(KeyCode.B)) {
            prefabs["TestEntityB"].Instantiate();
        }
