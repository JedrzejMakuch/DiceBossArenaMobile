using DiceBossArena.UI;
using NUnit.Framework;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class ResourceBarViewModelSourceTests
    {
        [Test]
        public void Constructor_ExposesInitialViewModel()
        {
            ResourceBarViewModel initial =
                new(75, 100, "HP");

            ResourceBarViewModelSource source =
                new(initial);

            Assert.That(source.Current.Current, Is.EqualTo(75));
            Assert.That(source.Current.Maximum, Is.EqualTo(100));
            Assert.That(source.Current.Label, Is.EqualTo("HP"));
        }

        [Test]
        public void Set_UpdatesCurrentViewModel()
        {
            ResourceBarViewModelSource source =
                new(new ResourceBarViewModel(100, 100, "HP"));

            source.Set(
                new ResourceBarViewModel(
                    40,
                    100,
                    "HP",
                    ResourceBarVisualState.Warning));

            Assert.That(source.Current.Current, Is.EqualTo(40));

            Assert.That(
                source.Current.VisualState,
                Is.EqualTo(ResourceBarVisualState.Warning));
        }

        [Test]
        public void Set_PublishesUpdatedViewModel()
        {
            ResourceBarViewModelSource source =
                new(new ResourceBarViewModel(100, 100, "HP"));

            ResourceBarViewModel published = default;
            int notificationCount = 0;

            source.Changed += viewModel =>
            {
                published = viewModel;
                notificationCount++;
            };

            source.Set(
                new ResourceBarViewModel(
                    0,
                    100,
                    "HP",
                    ResourceBarVisualState.Depleted));

            Assert.That(notificationCount, Is.EqualTo(1));
            Assert.That(published.Current, Is.Zero);
            Assert.That(published.FillAmount, Is.Zero);

            Assert.That(
                published.VisualState,
                Is.EqualTo(ResourceBarVisualState.Depleted));
        }

        [Test]
        public void RemovedSubscriber_DoesNotReceiveUpdates()
        {
            ResourceBarViewModelSource source =
                new(new ResourceBarViewModel(100, 100, "HP"));

            int notificationCount = 0;

            void HandleChanged(ResourceBarViewModel viewModel)
            {
                notificationCount++;
            }

            source.Changed += HandleChanged;
            source.Changed -= HandleChanged;

            source.Set(
                new ResourceBarViewModel(50, 100, "HP"));

            Assert.That(notificationCount, Is.Zero);
        }
    }
}