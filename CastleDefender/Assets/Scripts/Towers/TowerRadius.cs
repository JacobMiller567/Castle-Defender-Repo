using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerRadius : MonoBehaviour
{
    [SerializeField] private GameObject parent;
    [SerializeField] private UpgradeTower tower;
    public float range;
    
    void Update() 
    {
        range = tower.GiveTowerRadius(); 
        // Used to set scale of radius without parent object effecting scale size 
        Vector3 inverseScale = new Vector3(1 / parent.transform.localScale.x, 1 / parent.transform.localScale.y, 1 / parent.transform.localScale.z);   
        gameObject.transform.localScale = Vector3.Scale(gameObject.transform.localScale, inverseScale);
        gameObject.transform.localScale = new Vector3(range/ parent.transform.localScale.x, range/ parent.transform.localScale.y, range/ parent.transform.localScale.z);
    }
    
    public void DisplayRadius()
    {      
        Vector3 inverseScale = new Vector3(1 / parent.transform.localScale.x, 1 / parent.transform.localScale.y, 1 / parent.transform.localScale.z);   
        gameObject.transform.localScale = Vector3.Scale(gameObject.transform.localScale, inverseScale);
        gameObject.transform.localScale = new Vector3(range/ parent.transform.localScale.x, range/ parent.transform.localScale.y, range/ parent.transform.localScale.z);
    } 
}
