using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DiceBossArena.Game
{
    public sealed class CharacterActionSet
    {
        private readonly
            ReadOnlyCollection<CharacterActionContent>
                contents;

        public IReadOnlyList<CharacterActionContent>
            Contents =>
                contents;

        public int Count =>
            contents.Count;

        public CharacterActionContent this[
            CharacterActionSlot slot]
        {
            get
            {
                int index =
                    (int)slot;

                if (index < 0 ||
                    index >= contents.Count)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(slot));
                }

                return contents[index];
            }
        }

        public CharacterActionSet(
            IReadOnlyList<CharacterActionContent>
                contents)
        {
            this.contents =
                CopyAndValidate(contents)
                    .AsReadOnly();
        }

        private static List<CharacterActionContent>
            CopyAndValidate(
                IReadOnlyList<CharacterActionContent>
                    source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(
                    nameof(source));
            }

            if (source.Count !=
                CharacterActionSlots.Count)
            {
                throw new ArgumentException(
                    "Action set must contain exactly " +
                    $"{CharacterActionSlots.Count} actions.",
                    nameof(source));
            }

            List<CharacterActionContent> result =
                new(source.Count);

            for (int i = 0; i < source.Count; i++)
            {
                CharacterActionContent content =
                    source[i];

                if (!content.IsValid)
                {
                    throw new ArgumentException(
                        $"Action at index {i} is invalid.",
                        nameof(source));
                }

                CharacterActionSlot expectedSlot =
                    CharacterActionSlots.All[i];

                if (content.Slot != expectedSlot)
                {
                    throw new ArgumentException(
                        $"Action at index {i} must use " +
                        $"{expectedSlot} slot.",
                        nameof(source));
                }

                result.Add(content);
            }

            return result;
        }
    }
}