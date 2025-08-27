using Moq;
using ReportBuilder.Interfaces;
using ReportBuilder.ReportCreators;
using ReportBuilder.Types;
using FluentAssert;

namespace ReportBuilder.Tests
{
    public class OperatorStatesCounterReportCreatorTests
    {
        [Fact]
        public void Add_FamilyWithTwoSessionInReadyState_ShouldBeEqualTwo()
        {
            // ARRANGE
            var name = "Иванов Иван";
            ISessionReportCreator sut =
                new OperatorStatesCounterReportCreator(Mock.Of<IReportPrinter>());
            var session1 = new Session(
                DateTime.Now,
                DateTime.Now,
                "",
                name,
                "Готов",
                1);
            var session2 = new Session(
                DateTime.Now,
                DateTime.Now,
                "",
                name,
                "Готов",
                1);

            // ACT
            sut.Add(session1);
            sut.Add(session2);
            var data = sut.GetAll().ToList();

            // ASSERT
            var dateItems1 = data[0].Split("\t");
            dateItems1[0].ShouldBeEqualTo(name);
            int.Parse(dateItems1[2])
                .ShouldBeEqualTo(2);
        }

        [Fact]
        public void Add_FamilyWithTwoSessionInReadyAndPauseState_ShouldBeOneOnBothState()
        {
            // ARRANGE
            var name = "Иванов Иван";
            ISessionReportCreator sut =
                new OperatorStatesCounterReportCreator(Mock.Of<IReportPrinter>());
            var session1 = new Session(
                DateTime.Now,
                DateTime.Now,
                "",
                name,
                "Готов",
                1);
            var session2 = new Session(
                DateTime.Now,
                DateTime.Now,
                "",
                name,
                "Пауза",
                1);

            // ACT
            sut.Add(session1);
            sut.Add(session2);
            var data = sut.GetAll().ToList();

            // ASSERT
            var dateItems1 = data[0].Split("\t");
            dateItems1[0].ShouldBeEqualTo(name);
            int.Parse(dateItems1[1])
                .ShouldBeEqualTo(1);
            int.Parse(dateItems1[2])
                .ShouldBeEqualTo(1);
        }

        [Fact]
        public void Add_TwoSessionWithDifferentFamilyInReadyState_ReadyStateCountShouldOne()
        {
            // ARRANGE
            ISessionReportCreator sut =
                new OperatorStatesCounterReportCreator(Mock.Of<IReportPrinter>());
            var session1 = new Session(
                DateTime.Now,
                DateTime.Now,
                "",
                "Иванов Иван",
                "Готов",
                1);
            var session2 = new Session(
                DateTime.Now,
                DateTime.Now,
                "",
                "Неиванов Неиван",
                "Готов",
                1);

            // ACT
            sut.Add(session1);
            sut.Add(session2);
            var data = sut.GetAll().ToList();

            // ASSERT
            var dateItems1 = data[0].Split("\t");
            dateItems1[0].ShouldBeEqualTo(session1.Operator);
            int.Parse(dateItems1[2])
                .ShouldBeEqualTo(1);

            var dateItems2 = data[1].Split("\t");
            dateItems2[0].ShouldBeEqualTo(session2.Operator);
            int.Parse(dateItems2[2])
                .ShouldBeEqualTo(1);
        }
    }
}
