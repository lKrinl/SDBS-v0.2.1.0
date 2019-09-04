using System.Collections;

public class AttackInfo : CustomComponentBase {

    public float valueX = 0;
    public string valueString = "test";
    
    //Use this for initialization
    void Start () {
    
    }
    
    //Update is called once per frame
    void Update (){
    
    }
    
    public override void SetData (string[] lines, ref int pointer)
    {
        Debug.Log("AttackInfo setting data!");
        valueX = float.Parse(lines[Pointer++]);
        valueString = lines[pointer++]);
    }
}
