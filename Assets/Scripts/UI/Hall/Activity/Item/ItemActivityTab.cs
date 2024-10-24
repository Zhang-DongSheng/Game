using Game.Data;

namespace Game.UI
{
    public class ItemActivityTab : ItemToggle
    {
        protected override void SetContent(int index)
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