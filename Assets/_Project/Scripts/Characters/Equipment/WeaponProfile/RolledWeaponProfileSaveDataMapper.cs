using System;

namespace DiceBossArena.Game
{
    public sealed class RolledWeaponProfileSaveDataMapper
    {
        public RolledWeaponProfileSaveData ToSaveData(
            RolledWeaponProfile profile)
        {
            if (profile == null)
            {
                throw new ArgumentNullException(
                    nameof(profile));
            }

            RolledWeaponAttackLineSaveData[] lines =
                new RolledWeaponAttackLineSaveData[
                    profile.Lines.Count];

            for (int index = 0;
                 index < profile.Lines.Count;
                 index++)
            {
                RolledWeaponAttackLine line =
                    profile.Lines[index];

                lines[index] =
                    new RolledWeaponAttackLineSaveData(
                        line.LineId.Value,
                        line.Element,
                        line.MinDamage,
                        line.MaxDamage);
            }

            return new RolledWeaponProfileSaveData(
                lines);
        }

        public RolledWeaponProfile FromSaveData(
            RolledWeaponProfileSaveData data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(
                    nameof(data));
            }

            if (data.Lines == null)
            {
                throw new ArgumentException(
                    "Saved weapon profile lines cannot be null.",
                    nameof(data));
            }

            RolledWeaponAttackLine[] lines =
                new RolledWeaponAttackLine[
                    data.Lines.Length];

            for (int index = 0;
                 index < data.Lines.Length;
                 index++)
            {
                RolledWeaponAttackLineSaveData lineData =
                    data.Lines[index];

                if (lineData == null)
                {
                    throw new ArgumentException(
                        "Saved weapon profile cannot contain null lines.",
                        nameof(data));
                }

                lines[index] =
                    new RolledWeaponAttackLine(
                        new WeaponAttackLineId(
                            lineData.LineId),
                        lineData.Element,
                        lineData.MinDamage,
                        lineData.MaxDamage);
            }

            return new RolledWeaponProfile(
                lines);
        }
    }
}