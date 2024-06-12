using Data;

namespace Game.UI
{
    public class ItemActivityTab : ItemToggle
    {
        protected override void SetContent(int index)
        {
            var table = DataActivity.Get((uint)index);

            var content = table != null ? table.name : parameter.name;

            var components = GetComponentsInChildren<TextBind>(true);

            foreach (var component in components)
            {
                component.SetText(table.name);
            }
        }
    }
}