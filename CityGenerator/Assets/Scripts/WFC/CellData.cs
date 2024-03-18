using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[System.Serializable]
public class CellData
{
    public Vector2 coords; // Координаты ячейки
    public bool isCollapsed; // Флаг, указывающий на то, произошло ли свертывание ячейки
    // Другие параметры ячейки...

    public CellData(Vector2 coords, bool isCollapsed)
    {
        this.coords = coords;
        this.isCollapsed = isCollapsed;
        // Инициализация других параметров ячейки...
    }
}
