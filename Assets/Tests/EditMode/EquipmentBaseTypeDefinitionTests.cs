using DiceBossArena.Game;
using NUnit.Framework;
using UnityEngine;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class EquipmentBaseTypeDefinitionTests
    {
        [Test]
        public void InitializeForTests_AssignsDefinitionData()
        {
            EquipmentBaseTypeDefinition definition =
                ScriptableObject.CreateInstance<
                    EquipmentBaseTypeDefinition>();

            try
            {
                definition.InitializeForTests(
                    "  iron_sword  ",
                    EquipmentSlotType.MainHand,
                    EquipmentBaseTypeCategory.Sword);

                Assert.That(
                    definition.BaseTypeId.Value,
                    Is.EqualTo("iron_sword"));

                Assert.That(
                    definition.SlotType,
                    Is.EqualTo(
                        EquipmentSlotType.MainHand));

                Assert.That(
                    definition.Category,
                    Is.EqualTo(
                        EquipmentBaseTypeCategory.Sword));
            }
            finally
            {
                Object.DestroyImmediate(definition);
            }
        }
    }
}