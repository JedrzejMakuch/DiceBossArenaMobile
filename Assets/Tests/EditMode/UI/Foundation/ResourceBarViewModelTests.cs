using DiceBossArena.UI;
using NUnit.Framework;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class ResourceBarViewModelTests
    {
        [Test]
        public void Constructor_PreservesValidValues()
        {
            ResourceBarViewModel viewModel =
                new(
                    75,
                    100,
                    "HP",
                    ResourceBarVisualState.Warning);

            Assert.That(viewModel.Current, Is.EqualTo(75));
            Assert.That(viewModel.Maximum, Is.EqualTo(100));
            Assert.That(viewModel.Label, Is.EqualTo("HP"));

            Assert.That(
                viewModel.VisualState,
                Is.EqualTo(ResourceBarVisualState.Warning));

            Assert.That(viewModel.FillAmount, Is.EqualTo(0.75f));
            Assert.That(viewModel.ValueText, Is.EqualTo("75 / 100"));
        }

        [Test]
        public void Constructor_ClampsCurrentBelowZero()
        {
            ResourceBarViewModel viewModel =
                new(-20, 100);

            Assert.That(viewModel.Current, Is.Zero);
            Assert.That(viewModel.FillAmount, Is.Zero);
        }

        [Test]
        public void Constructor_ClampsCurrentAboveMaximum()
        {
            ResourceBarViewModel viewModel =
                new(150, 100);

            Assert.That(viewModel.Current, Is.EqualTo(100));
            Assert.That(viewModel.FillAmount, Is.EqualTo(1f));
        }

        [Test]
        public void Constructor_ClampsNegativeMaximumToZero()
        {
            ResourceBarViewModel viewModel =
                new(50, -100);

            Assert.That(viewModel.Current, Is.Zero);
            Assert.That(viewModel.Maximum, Is.Zero);
            Assert.That(viewModel.FillAmount, Is.Zero);
            Assert.That(viewModel.ValueText, Is.EqualTo("0 / 0"));
        }

        [Test]
        public void Constructor_HandlesZeroMaximum()
        {
            ResourceBarViewModel viewModel =
                new(0, 0);

            Assert.That(viewModel.FillAmount, Is.Zero);
        }

        [Test]
        public void Constructor_ReplacesNullLabelWithEmptyString()
        {
            ResourceBarViewModel viewModel =
                new(10, 20, null);

            Assert.That(viewModel.Label, Is.Empty);
        }

        [Test]
        public void Constructor_PreservesDepletedStateForDeath()
        {
            ResourceBarViewModel viewModel =
                new(
                    0,
                    100,
                    "HP",
                    ResourceBarVisualState.Depleted);

            Assert.That(viewModel.Current, Is.Zero);
            Assert.That(viewModel.FillAmount, Is.Zero);

            Assert.That(
                viewModel.VisualState,
                Is.EqualTo(ResourceBarVisualState.Depleted));
        }
    }
}