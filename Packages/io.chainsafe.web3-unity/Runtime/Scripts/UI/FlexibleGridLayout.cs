using UnityEngine;
using UnityEngine.UI;

// Lifted from https://github.com/rob1997/Controller/blob/main/Packages/com.ekaka.ui/Runtime/Common/FlexibleGridLayout.cs
public class FlexibleGridLayout : LayoutGroup
{
    public enum FitType
    {
        FixedRows,
        FixedColumns,
    }

    public new RectOffset padding = new RectOffset();

    public FitType fitType;

    public int rows;
    public int columns;

    public Vector2 cellSize;
    public Vector2 spacing;

    public bool fitX;
    public bool fitY;

    public bool centerX;
    public bool centerY;

    [Tooltip(
        "instead of using pixels for size, fit cell to anchor (percentage of screen size) - this makes layout more responsive")]
    public bool anchorFitX;

    [Tooltip("cell width will be calculated as a percentage of this Rect's width, if null we instead use Screen.width")]
    public RectTransform anchorRectX;

    [Tooltip(
        "instead of using pixels for size, fit cell to anchor (percentage of screen size) - this makes layout more responsive")]
    public bool anchorFitY;

    [Tooltip(
        "cell height will be calculated as a percentage of this Rect's height, if null we instead use Screen.height")]
    public RectTransform anchorRectY;

    [Tooltip("value is normalized (0 - 1), cell size as a percentage of screen size")]
    public Vector2 anchorCellSize;

    private int _childCount;

    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();

        switch (fitType)
        {
            case FitType.FixedRows:
                columns = Mathf.CeilToInt(transform.childCount / (float)rows);
                break;
            case FitType.FixedColumns:
                rows = Mathf.CeilToInt(transform.childCount / (float)columns);
                break;
        }

        Rect parentRect = rectTransform.rect;

        float parentWidth = parentRect.width;
        float parentHeight = parentRect.height;

        float cellWidth = (parentWidth - (spacing.x * (columns - 1)) - padding.left - padding.right) / (float)columns;

        float cellHeight = (parentHeight - (spacing.y * (rows - 1)) - padding.top - padding.bottom) / (float)rows;

        if (anchorRectX != null && anchorRectX.rect.width <= 0)
        {
            //can't ForceRebuildLayoutImmediate in case it's in another layout causing a circular dependency causing a stack overflow
            LayoutRebuilder.MarkLayoutForRebuild(rectTransform);

            return;
        }

        float rectWidth = (float)Screen.width;

        if (fitX)
            cellSize.x = cellWidth;

        else
        {
            if (anchorFitX)
            {
                if (anchorRectX != null)
                {
                    if (anchorRectX.rect.width <= 0)
                    {
                        //can't ForceRebuildLayoutImmediate in case it's in another layout causing a circular dependency causing a stack overflow
                        LayoutRebuilder.MarkLayoutForRebuild(rectTransform);

                        return;
                    }

                    rectWidth = anchorRectX.rect.width;
                }

                cellSize.x = rectWidth * anchorCellSize.x;
            }
        }

        anchorCellSize.x = cellSize.x / rectWidth;

        float rectHeight = (float)Screen.height;

        if (fitY)
            cellSize.y = cellHeight;

        else
        {
            if (anchorFitY)
            {
                if (anchorRectY != null)
                {
                    if (anchorRectY.rect.height <= 0)
                    {
                        //can't ForceRebuildLayoutImmediate in case it's in another layout causing a circular dependency causing a stack overflow
                        LayoutRebuilder.MarkLayoutForRebuild(rectTransform);

                        return;
                    }

                    rectHeight = anchorRectY.rect.height;
                }

                cellSize.y = rectHeight * anchorCellSize.y;
            }
        }

        anchorCellSize.y = cellSize.y / rectHeight;

        int childrenCount = rectChildren.Count;

        for (int i = 0; i < childrenCount; i++)
        {
            int rowCount = i / columns;
            int columnCount = i % columns;

            RectTransform item = rectChildren[i];

            var xPos = (cellSize.x * columnCount) + (spacing.x * columnCount) + padding.left;

            if (centerX)
            {
                int itemsPerRow = rowCount + 1 == rows && childrenCount % columns != 0
                    ? childrenCount % columns
                    : columns;

                float spaceLeftOnRow = parentWidth - (cellSize.x * itemsPerRow) - (spacing.x * (itemsPerRow - 1)) -
                                       padding.left - padding.right;
                xPos += (float)spaceLeftOnRow / 2f;
            }

            var yPos = (cellSize.y * rowCount) + (spacing.y * rowCount) + padding.top;

            if (centerY)
            {
                float spaceLeftOnColumn = parentHeight - (cellSize.y * rows) - (spacing.y * (rows - 1)) - padding.top -
                                          padding.bottom;
                yPos += (float)spaceLeftOnColumn / 2f;
            }

            SetChildAlongAxis(item, 0, xPos, cellSize.x);
            SetChildAlongAxis(item, 1, yPos, cellSize.y);
        }

        //Set Preferred sizes
        SetLayoutInputForAxis(0, (cellSize.x * columns) + (spacing.x * (columns - 1)) + padding.left + padding.right, 0,
            0);
        SetLayoutInputForAxis(0, (cellSize.y * rows) + (spacing.y * (rows - 1)) + padding.top + padding.bottom, 0, 1);
    }

    public override void CalculateLayoutInputVertical()
    {
    }

    public override void SetLayoutHorizontal()
    {
    }

    public override void SetLayoutVertical()
    {
    }

    private void Update()
    {
        //Whenever new element is added
        if (_childCount != transform.childCount)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
            _childCount = transform.childCount;
        }
    }
}