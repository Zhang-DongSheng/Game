﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.UI.Guidance
{
    public class ItemGuidanceDrag : ItemGuidanceBase
    {
        public void Close()
        {
            UIManager.Instance.Close((int)UIPanel.Guidance);
        }
    }
}