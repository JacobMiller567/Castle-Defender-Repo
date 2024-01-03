using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WispBlessing : MonoBehaviour
{
    [SerializeField] private LayerMask towerMask;
    public TowerManager manager;
    public float abilityCooldown;
    public float blessingRadius; 
    private bool canUseAbility;
    public List<Transform> blessedTowers;
    public float rangeBlessing;
    public float speedBlessing;

    void Start()
    {
        canUseAbility = true;
        manager.UpdateTowerStats(blessingRadius, abilityCooldown);
    }
    void Update()
    {
        if (canUseAbility == true && manager.towerIsPurchased == true)
        {
            CastBlessing();   
        }
    }

    public float GetBlessingRadius()
    {
        return blessingRadius;
    }


    IEnumerator RefreshAbility()
    {
        yield return new WaitForSeconds(abilityCooldown);
        canUseAbility = true;  
    }


    public void CastBlessing()
    {
        if (canUseAbility)
        {
            RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, manager.TowerRange, (Vector2)transform.position, 0f, towerMask);
            if (hits.Length > 0)
            {
                for (int i = 0; i < hits.Length; i++)
                {
                    RaycastHit2D hit = hits[i];
                    if (hit.transform == null) return;
                    
                    if (!blessedTowers.Contains(hit.transform))
                    {
                        TowerManager tower = hit.transform.GetComponent<TowerManager>();
                        if (tower == null) return;

                        if (tower.towerIsPurchased == true)
                        {
                            if (tower.isFlameTower == true)
                            {
                                TowerFlame flameTower = tower.GetComponentInChildren<TowerFlame>();
                                if (!blessedTowers.Contains(flameTower.transform) && flameTower != null) 
                                {
                                    blessedTowers.Add(flameTower.transform);
                                    flameTower.BoostTower(rangeBlessing, speedBlessing);
                                }
                            } 
                            else if (tower.isSlowTower == true)
                            {
                                TowerSlowMotion slomoTower = tower.GetComponentInChildren<TowerSlowMotion>();
                                if (!blessedTowers.Contains(slomoTower.transform) && slomoTower != null) 
                                {
                                    blessedTowers.Add(slomoTower.transform);
                                    slomoTower.BoostTower(rangeBlessing, speedBlessing);
                                }
                            }
                            else
                            {
                                TowerDetection moreTowers = tower.GetComponentInChildren<TowerDetection>();
                                if (!blessedTowers.Contains(moreTowers.transform) && moreTowers != null)
                                {
                                    blessedTowers.Add(moreTowers.transform);
                                    moreTowers.BoostTower(rangeBlessing, speedBlessing);
                                }
                            }
                        }         
                    }
                }
                canUseAbility = false;
                StartCoroutine(RefreshAbility());
            }
        }
    }


    public void ResetAllBlessings()
    {
        foreach (Transform tower in blessedTowers)
        {
            TowerManager towerManager = tower.GetComponent<TowerManager>();
            if (towerManager != null && towerManager.towerIsPurchased)
            {
                if (towerManager.isFlameTower == true)
                {
                    TowerFlame flameTower = tower.GetComponentInChildren<TowerFlame>();
                    if (flameTower != null)
                    {
                        flameTower.ResetTower();
                    }    
                } 
                else if (towerManager.isSlowTower == true)
                {
                    TowerSlowMotion slomoTower = tower.GetComponentInChildren<TowerSlowMotion>();
                    if (slomoTower != null)
                    {
                       slomoTower.ResetTower(); 
                    }
                }
                else
                {
                    TowerDetection moreTowers = tower.GetComponentInChildren<TowerDetection>();
                    if (moreTowers != null)
                    {
                        moreTowers.ResetTower();
                    }
                }
            }
        }
    }

}
