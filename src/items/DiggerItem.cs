using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;

namespace hammertime.items
{
    public class DiggerItem : Item
    {
        
        public override bool OnBlockBrokenWith(IWorldAccessor world, Entity byEntity, ItemSlot itemslot,
            BlockSelection blockSel, float dropQuantityMultiplier = 1f)
        {
            if (base.OnBlockBrokenWith(world, byEntity, itemslot, blockSel, dropQuantityMultiplier))
            {
                if (byEntity is EntityPlayer)
                {
                    var player = world.PlayerByUid((byEntity as EntityPlayer).PlayerUID);
                    switch (blockSel.Face.Axis)
                    {
                        case EnumAxis.X:
                            destroyBlocks(world, blockSel.Position.AddCopy(0, -1, -1),
                                blockSel.Position.AddCopy(0, 1, 1), player, blockSel.Position);
                            break;
                        case EnumAxis.Y:
                            destroyBlocks(world, blockSel.Position.AddCopy(-1, 0, -1),
                                blockSel.Position.AddCopy(1, 0, 1), player, blockSel.Position);
                            break;
                        case EnumAxis.Z:
                            destroyBlocks(world, blockSel.Position.AddCopy(-1, -1, 0),
                                blockSel.Position.AddCopy(1, 1, 0), player, blockSel.Position);
                            break;
                    }
                }

                return true;
            }

            return false;
        }

        public void destroyBlocks(IWorldAccessor world, BlockPos min, BlockPos max, IPlayer player, BlockPos centerBlockPos)
        {
            var centerBlock = world.BlockAccessor.GetBlock(centerBlockPos);
            var itemStack = new ItemStack(this);
            Block tempBlock;
            var miningTimeMainBlock = GetMiningSpeed(itemStack, centerBlock, player);
            float miningTime;
            var tempPos = new BlockPos();
            for (int x = min.X; x <= max.X; x++)
            {
                for (int y = min.Y; y <= max.Y; y++)
                {
                    for (int z = min.Z; z <= max.Z; z++)
                    {
                        tempPos.Set(x, y, z);
                        tempBlock = world.BlockAccessor.GetBlock(tempPos);
                        if (player.WorldData.CurrentGameMode == EnumGameMode.Creative)
                            world.BlockAccessor.SetBlock(0, tempPos);
                        else
                        {
                            //Check if we can mine this block
                            miningTime = tempBlock.GetMiningSpeed(itemStack, tempBlock, player);
                            if (this.ToolTier >= tempBlock.RequiredMiningTier //Нужный уровень инструмента
                                && miningTimeMainBlock * 1.5f >= miningTime //Время добычи не сильно отличается
                                && MiningSpeed.ContainsKey(tempBlock.BlockMaterial)) //И материал  металл или камень
                            {
                                world.BlockAccessor.BreakBlock(tempPos, player);
                            }
                            
                        }
                    }
                }
            }
        }
    }
}