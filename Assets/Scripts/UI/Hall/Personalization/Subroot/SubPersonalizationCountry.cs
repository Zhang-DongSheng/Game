using Game.Logic;

namespace Game.UI
{
    public class SubPersonalizationCountry : SubPersonalizationBase
    {
        public override void Refresh()
        {
            personalizatalID = PlayerLogic.Instance.Cache.country;

            OnClickCountry(personalizatalID);
        }

        private void OnClickCountry(uint ID)
        {
            personalizatalID = ID;

            callback?.Invoke(ID);
        }

        public override PersonalizationType Type => PersonalizationType.Country;
    }
}