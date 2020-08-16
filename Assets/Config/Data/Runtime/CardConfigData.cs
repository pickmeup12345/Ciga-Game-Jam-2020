using UnityEngine;
using System.Collections;

///
/// !!! Machine generated code !!!
/// !!! DO NOT CHANGE Tabs to Spaces !!!
/// 
[System.Serializable]
public class CardConfigData
{
  [SerializeField]
  int id;
  public int Id { get {return id; } set { id = value;} }
  
  [SerializeField]
  string destext;
  public string Destext { get {return destext; } set { destext = value;} }
  
  [SerializeField]
  string yestext;
  public string Yestext { get {return yestext; } set { yestext = value;} }
  
  [SerializeField]
  string notext;
  public string Notext { get {return notext; } set { notext = value;} }
  
  [SerializeField]
  string iconname;
  public string Iconname { get {return iconname; } set { iconname = value;} }
  
  [SerializeField]
  int yestrust;
  public int Yestrust { get {return yestrust; } set { yestrust = value;} }
  
  [SerializeField]
  int yesmood;
  public int Yesmood { get {return yesmood; } set { yesmood = value;} }
  
  [SerializeField]
  int yesmoney;
  public int Yesmoney { get {return yesmoney; } set { yesmoney = value;} }
  
  [SerializeField]
  int yessatiety;
  public int Yessatiety { get {return yessatiety; } set { yessatiety = value;} }
  
  [SerializeField]
  int yesenddays;
  public int Yesenddays { get {return yesenddays; } set { yesenddays = value;} }
  
  [SerializeField]
  int notrust;
  public int Notrust { get {return notrust; } set { notrust = value;} }
  
  [SerializeField]
  int nomood;
  public int Nomood { get {return nomood; } set { nomood = value;} }
  
  [SerializeField]
  int nomoney;
  public int Nomoney { get {return nomoney; } set { nomoney = value;} }
  
  [SerializeField]
  int nosatiety;
  public int Nosatiety { get {return nosatiety; } set { nosatiety = value;} }
  
  [SerializeField]
  int noenddays;
  public int Noenddays { get {return noenddays; } set { noenddays = value;} }
  
  [SerializeField]
  int[] weight = new int[0];
  public int[] Weight { get {return weight; } set { weight = value;} }
  
  [SerializeField]
  int[] stage = new int[0];
  public int[] Stage { get {return stage; } set { stage = value;} }
  
}