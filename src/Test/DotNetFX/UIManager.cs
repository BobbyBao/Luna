using SharpLuna;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test;

public class UIManager
{
    [LuaHide]
    public static AlertBox alertBox;
    [LuaHide]
    public static ConfirmBox confirmBox;
    [LuaHide]
    static TableTree tableTree;
    public static void Init(TableTree tt)
    {
        tableTree = tt;
        alertBox = new AlertBox();
        confirmBox = new ConfirmBox();
    }

    [LuaAsync]
    public static void ShowAlertBox(string message, string title, Action onFinished = null)
    {
        alertBox.Show(message, title, onFinished);
    }

    [LuaAsync]
    public static void ShowConfirmBox(string message, string title, Action<bool> onFinished = null)
    {
        confirmBox.Show(message, title, onFinished);
    }

    public static void AddListener(Action action)
    {
        tableTree.onRecharge = action;
    }
}
