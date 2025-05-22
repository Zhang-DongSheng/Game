using UnityEngine.UI;

namespace Game.UI
{
    public class ItemConsoleToggle : ItemToggle
    {
        protected override void SetContent(string name)
        {
            var components = GetComponentsInChildren<Text>(true);

            foreach (var component in components)
            {
                component.SetText(name);
            }
        }
    }
}