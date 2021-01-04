using Vintagestory.API.Common;

namespace hammertime
{
    public class HammerTime : ModSystem
    {
        public override void Start(ICoreAPI api)
        {
            base.Start(api);
            
            api.RegisterItemClass("ItemDigger", typeof(items.DiggerItem));
        }
    }
}