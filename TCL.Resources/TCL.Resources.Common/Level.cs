using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace TCL.Resources.Common
{
    public enum LevelType
    {
       [Description("一级组织机构")]
        LevelOne=1,
       [Description("二级组织机构")]
        LevelTwo=2,
       [Description("三级组织机构")]
        LevelThree=3,
       [Description("三级组织机构查询")]
        LevelThreeQuery=4
    }
    public class Level
    {
    }
}