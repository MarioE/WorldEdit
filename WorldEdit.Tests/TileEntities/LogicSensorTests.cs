using NUnit.Framework;
using WorldEdit.TileEntities;

namespace WorldEdit.Tests.TileEntities
{
    [TestFixture]
    public class LogicSensorTests
    {
        [TestCase(5)]
        public void GetData(int data)
        {
            var sensor = new LogicSensor(Vector.Zero, LogicSensorType.Day, false, data);

            Assert.That(sensor.Data, Is.EqualTo(data));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void GetIsEnabled(bool isEnabled)
        {
            var sensor = new LogicSensor(Vector.Zero, LogicSensorType.Day, isEnabled, 0);

            Assert.That(sensor.IsEnabled, Is.EqualTo(isEnabled));
        }

        [TestCase(1, 1)]
        public void GetPosition(int x, int y)
        {
            var sensor = new LogicSensor(new Vector(x, y), LogicSensorType.Day, false, 0);

            Assert.That(sensor.Position, Is.EqualTo(new Vector(x, y)));
        }

        [TestCase(LogicSensorType.Liquid)]
        public void GetType(LogicSensorType type)
        {
            var sensor = new LogicSensor(Vector.Zero, type, false, 0);

            Assert.That(sensor.Type, Is.EqualTo(type));
        }

        [TestCase(1, 1)]
        public void WithPosition(int x, int y)
        {
            ITileEntity sensor = new TrainingDummy(Vector.Zero, -1);

            sensor = sensor.WithPosition(new Vector(x, y));

            Assert.That(sensor.Position, Is.EqualTo(new Vector(x, y)));
        }
    }
}
