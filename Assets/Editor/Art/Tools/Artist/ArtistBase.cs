using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.Window
{
    public abstract class ArtistBase
    {
        public abstract string Name { get; }

        public abstract void Initialise();

        public abstract void Refresh();
    }
}