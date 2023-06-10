using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainBuildTouch : MonoBehaviour
{
    [SerializeField] SpriteRenderer _background;
    [SerializeField] List<BuildTouch> _buildTouches = new List<BuildTouch>();
    public BuildTouch mainBuildTouch;
    public bool isReady;

    public bool CheckBuilds()
    {
        BuildTouch buildTouch;
        isReady = true;

        for (int i = 0; i < _buildTouches.Count; i++)
        {
            buildTouch = _buildTouches[i];

            if (!buildTouch.isFree) isReady = false;
        }
        return isReady;
    }
    public void DrawGreen()
    {
        BuildManager.Instance.SetMaterialGreen(_background);
    }
    public void DrawBackground(bool tempCheck)
    {
        BuildManager buildManager = BuildManager.Instance;

        if (tempCheck)
            buildManager.SetMaterialGreen(_background);
        else
            buildManager.SetMaterialRed(_background);
    }
    public void SaveGridID()
    {
        GridChecked();
        GridSave();
        GameManager.Instance.GridPlacementWrite(GridSystem.Instance.mainGrid);
    }
    public BuildTouch GetBuildTouch()
    {
        return mainBuildTouch;
    }
    public bool CheckGrid()
    {
        bool isChecked = true;
        for (int i1 = 0; i1 < _buildTouches.Count; i1++)
            for (int i2 = 0; i2 < _buildTouches.Count; i2++)
                if (_buildTouches[i1] == _buildTouches[i2] && i1 != i2) isChecked = false;

        return isChecked;
    }

    private void GridChecked()
    {
        foreach (BuildTouch buildTouch in _buildTouches)
        {
            GridSystem.Instance.mainGrid.horizontalGrids[buildTouch.verticalCount].gridBool[buildTouch.horizontalCount] = true;
            GridSystem.Instance.mainGrid.horizontalGrids[buildTouch.verticalCount].gridGameObject[buildTouch.horizontalCount] = gameObject;
        }
    }
    private void GridSave()
    {
        GridSystem.Instance.mainGrid.buildTypes.Add(GetComponent<InGameSelectedSystem>().mainBuildStat);
        GridSystem.Instance.mainGrid.builds.Add(gameObject);
        GridSystem.Instance.mainGrid.buildLevel.Add(1);

        GridSystem.BuildID buildID = new GridSystem.BuildID();

        buildID.buildIntHorizontal = mainBuildTouch.horizontalCount;
        buildID.buildIntVertical = mainBuildTouch.verticalCount;

        GridSystem.Instance.mainGrid.buildID.Add(buildID);
    }
}