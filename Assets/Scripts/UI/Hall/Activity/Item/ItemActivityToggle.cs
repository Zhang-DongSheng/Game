using Game.Data;

namespace Game.UI
{
    public class ItemActivityToggle : ItemToggle
    {
        protected override void SetContent(string name)
        {
            var table = DataActivity.Get((uint)index);

            parameter.name = table != null ? table.name : parameter.name;

            var components = GetComponentsInChildren<TextBind>(true);

            foreach (var component in components)
            {
                component.SetText(parameter.name);
            }
        }
    }
}