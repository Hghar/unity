using System.Collections.Generic;
using System.Threading.Tasks;
using Battle;
using CompositeDirectorWithGeneratingComposites.CompositeDirector;
using Fight;
using Fight.Fractions;
using Model.Commands.Creation;
using Model.Commands.Helpers;
using Model.Commands.Parts;
using Moq;
using NUnit.Framework;
using Parameters;
using Plugins.CompositeDirectorPlugin;
using Units;

namespace Model.Commands.Tests
{
    public class CommandBuilderTests
    {
        [Test]
        public void IncreaseHealth_MaxHealthBecomeChanges()
        {
            // Arrange.
            var pool = new Mock<IMinionPool>();
            var minion = new Mock<IMinion>();
            var parameters = new Mock<IUnitParameters>();
            var armor = new Armor(100);
            parameters.Setup((unitParameters => unitParameters.Armor)).Returns(armor);
            minion.Setup((minion1 => minion1.Fraction)).Returns(Fraction.Minions);
            var affectableMinion = minion.As<IAffectable>();
            affectableMinion.Setup((affectable => affectable.IncreaseFixedEnergy(It.IsAny<int>())))
                .Callback((() => parameters.Object.Armor.Increase(100)));
            affectableMinion.Setup((affectable => affectable.DecreaseFixedEnergy(It.IsAny<int>())))
                .Callback((() => parameters.Object.Armor.Decrease(100)));
            minion.Setup((minion1 => minion1.Parameters)).Returns(parameters.Object);
            pool.Setup((minionPool => minionPool.Minions)).Returns(new List<IMinion>{minion.Object});
            var finishPublisher = new Mock<IBattleFinishPublisher>();
            var worker = new CommandWorker();
            var director = new CompositeDirector();
            var compositePool = director.SetupComposite<IAffectable>();
            compositePool.Add(affectableMinion.Object);
            var builder = new CommandBuilder(compositePool as IAffectable, worker, null);
            builder.Initialize(pool.Object, null, null);
            
            // Act.
            var builderParameters = new StringCommandParameters(
                CommandType.Active, 
                "IncreaseFixedEnergy", 
                new object[]{100},
                TargetType.AllAllies,
                new object[]{},
                0,
                null);
            var command = builder.BuildWithParameters(builderParameters);
            command.Perform();
            CompositeHelper.Perform();

            // Assert.
            Assert.AreEqual(200, parameters.Object.Armor.Value);
        }

        [Test]
        public void IncreaseHealth_UndoChangesAfterBattle()
        {
            // Arrange.
            var pool = new Mock<IMinionPool>();
            var minion = new Mock<IMinion>();
            var parameters = new Mock<IUnitParameters>();
            var armor = new Armor(100);
            parameters.Setup((unitParameters => unitParameters.Armor)).Returns(armor);
            minion.Setup((minion1 => minion1.Fraction)).Returns(Fraction.Minions);
            var affectableMinion = minion.As<IAffectable>();
            affectableMinion.Setup((affectable => affectable.Silence()))
                .Callback((() => parameters.Object.Health.IncreaseMax(100)));
            affectableMinion.Setup((affectable => affectable.Silence()))
                .Callback((() => parameters.Object.Health.DecreaseMax(100)));
            minion.Setup((minion1 => minion1.Parameters)).Returns(parameters.Object);
            pool.Setup((minionPool => minionPool.Minions)).Returns(new List<IMinion>{minion.Object});
            var finishPublisher = new Mock<IBattleFinishPublisher>();
            var worker = new CommandWorker();
            var director = new CompositeDirector();
            var compositePool = director.SetupComposite<IAffectable>();
            compositePool.Add(affectableMinion.Object);
            var builder = new CommandBuilder(compositePool as IAffectable, worker, null);
            builder.Initialize(pool.Object, null, null);

            // Act.
            var builderParameters = new StringCommandParameters(
                CommandType.Active, 
                "IncreaseHealth", 
                new object[]{100},
                TargetType.AllAllies,
                new object[]{},
                0,
                null);
            var command = builder.BuildWithParameters(builderParameters);
            command.Perform();
            CompositeHelper.Perform();
            command.Undo();
            CompositeHelper.Perform();
            
            // Assert.
            Assert.AreEqual(100, parameters.Object.Armor.Value);
        }

        // [Test]
        // public async Task IncreaseHealth_Passive_MaxHealthBecomeChanges()
        // {
        //     // Arrange.
        //     var pool = new Mock<IMinionPool>();
        //     var minion = new Mock<IMinion>();
        //     var parameters = new Mock<IUnitParameters>();
        //     var health = new Health(100);
        //     parameters.Setup((unitParameters => unitParameters.Health)).Returns(health);
        //     minion.Setup((minion1 => minion1.Fraction)).Returns(Fraction.Minions);
        //     var affectableMinion = minion.As<IAffectable>();
        //     affectableMinion.Setup((affectable => affectable.IncreaseHealth(It.IsAny<int>())))
        //         .Callback((() => parameters.Object.Health.IncreaseMax(100)));
        //     minion.Setup((minion1 => minion1.Parameters)).Returns(parameters.Object);
        //     pool.Setup((minionPool => minionPool.Minions)).Returns(new[] {minion.Object});
        //     var finishPublisher = new Mock<IBattleFinishPublisher>();
        //     var worker = new CommandWorker(finishPublisher.Object);
        //     var director = new CompositeDirector();
        //     var compositePool = director.SetupComposite<IAffectable>();
        //     compositePool.Add(affectableMinion.Object);
        //     var builder = new CommandBuilder(compositePool as IAffectable, pool.Object, worker);
        //     
        //     // Act.
        //     builder.SetType(CommandType.Passive);
        //     builder.SetDuration(1);
        //     builder.SetFrequency(0.01f);
        //     builder.SetAction("IncreaseHealth", 100);
        //     builder.SetTarget(TargetType.AllAllies);
        //     var command = builder.Build();
        //     command.Perform();
        //     for (int i = 0; i < 10; i++)
        //     {
        //         await Task.Yield();
        //         CompositeHelper.Perform();
        //     }
        //     finishPublisher.Raise((publisher => publisher.BattleFinished += null));
        //
        //     // Assert.
        //     Assert.AreEqual(1000, parameters.Object.Health.MaxValue);
        // }
    }
}