using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NavMeshPlus.Components;

public class SoldierMoveSystem : MonoSingleton<SoldierMoveSystem>
{
    public enum SoldierAnimType
    {
        run = 0,
        idle = 1,
        dead = 2,
        attack = 3
    }


    [SerializeField] NavMeshSurface _surface2D;

    GameObject _mainSoldier;
    NavMeshAgent _navMeshAgent;
    Vector3 _target;


    public void RebuildNavMesh()
    {
        _surface2D.BuildNavMeshAsync();
    }
    public void SetInitialNavMeshSettingsForSoldier(GameObject soldier)
    {
        NavMeshAgent navMeshAgent = soldier.GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
    }
    public void CancelSoldierFocus()
    {
        _mainSoldier = null;
    }
    public void SetSoldierFocus(GameObject soldier)
    {
        _mainSoldier = soldier;
        _navMeshAgent = soldier.GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (SelectSystem.Instance.GetSelectEnumStat() == SelectSystem.SelectEnumStat.soldierMove && _mainSoldier != null)
            if (Input.GetMouseButton(1))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

                if (hit.collider != null)
                {
                    if (hit.collider.gameObject.CompareTag("Rival"))
                    {
                        SoldierID soldierID = _navMeshAgent.gameObject.GetComponent<SoldierID>();
                        soldierID.FollowRival(hit.collider.gameObject);
                        soldierID.SetSoldierAnim(SoldierAnimType.run);
                    }
                    else if (hit.collider.gameObject.CompareTag("Build"))
                    {
                        GameObject build = hit.collider.gameObject;
                        InGameSelectedSystem inGameSelectedSystem = build.GetComponent<InGameSelectedSystem>();

                        if (inGameSelectedSystem.mainBuildStat == InfoPanel.InfoPanelStat.hospital)
                            _mainSoldier.GetComponent<SoldierID>().SoldierHeal();
                    }
                    else
                    {
                        SelectSoldierDestination();
                        SetSoldierDestination();
                        _navMeshAgent.gameObject.GetComponent<SoldierID>().SetSoldierAnim(SoldierAnimType.run);
                    }
                }
                else
                {
                    SelectSoldierDestination();
                    SetSoldierDestination();
                    _navMeshAgent.gameObject.GetComponent<SoldierID>().SetSoldierAnim(SoldierAnimType.run);
                }
            }
    }
    private void SetSoldierDestination()
    {
        _navMeshAgent.SetDestination(_target);
    }
    private void SelectSoldierDestination()
    {
        _target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _target.z = 0;
    }
}
