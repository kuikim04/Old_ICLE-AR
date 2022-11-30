using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class SelectSongScrollview : FancyScrollView<BaseInfo, SelectSongScrollviewContext>
{
    public ScrollPositionController controller;

    new void Awake()
    {
        controller.OnUpdatePosition.AddListener(UpdatePosition);
        controller.OnItemSelected.AddListener(CellSelected);
        SetContext(new SelectSongScrollviewContext { OnPressedCell = OnPressedCell });
        base.Awake();
    }

    public void UpdateData(List<BaseInfo> data)
    {
        cellData = data;
        controller.SetDataCount(cellData.Count);
        UpdateContents();
    }

    void CellSelected(int cell_index)
    {
        context.SelectedIndex = cell_index;
        UpdateContents();
        SelectSongUIScript.singleton.UpdateHintUI();
    }

    void OnPressedCell(SongItemScript cell)
    {
        controller.ScrollTo(cell.DataIndex, 0.4f);
        context.SelectedIndex = cell.DataIndex;
        UpdateContents();
    }
}
