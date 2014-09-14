using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Sapper
{
    //интерфейс всего поля, по сути ядра игры(можно наверное так назвать)
    public interface ISapper
    {
        Statuses Status
        {
            get;
        }


        ICell[][] Field
        {
            get;
        }

        ICell[][] recalculateField(Point p, bool mouseButton);
    }

    //интерфейс объекта ячейки
    public interface ICell
    {
        bool Equals(ICell cell);
        ICell Copy();

        //участвует ли данная ячейка в игре вообще
        Activity Active
        {
            get;
            set;
        }

        //открыта или закрыта ячейка в данный момент
        Visibility Opened
        {
            get;
            set;
        }

        //заминирована ли ячейка
        PlaceHolders Mined
        {
            get;
        }

        //помечена ли ячейка игроком
        Marks Marked
        {
            get;
            set;
        }

        int Count
        {
            get;
            set;
        }
    }


    public enum Statuses
    {
        CONTINUE,
        FAIL,
        WIN
    }
    public enum Activity 
    {
        ENABLED,
        DISABLED
    }
    
    public enum Visibility
    {
        OPENED,
        CLOSED
    }
   
    public enum PlaceHolders
    {
        MINED,
        NOT_MINED
    }

    public enum Marks
    {
        MARKED,
        NOT_MARKED
    }
}
