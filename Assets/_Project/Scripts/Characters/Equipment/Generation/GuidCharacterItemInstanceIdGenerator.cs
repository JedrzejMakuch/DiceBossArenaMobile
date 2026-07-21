using System;

namespace DiceBossArena.Game
{
    public sealed class
        GuidCharacterItemInstanceIdGenerator :
        ICharacterItemInstanceIdGenerator
    {
        public CharacterItemInstanceId Generate()
        {
            return new CharacterItemInstanceId(
                Guid.NewGuid().ToString("N"));
        }
    }
}