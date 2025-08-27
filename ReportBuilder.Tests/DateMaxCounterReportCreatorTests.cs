using Moq;
using ReportBuilder.Interfaces;
using ReportBuilder.ReportCreators;
using ReportBuilder.Types;
using System.Globalization;
using FluentAssert;

namespace ReportBuilder.Tests
{
    public class DateMaxCounterReportCreatorTests
    {
        [Fact]
        public void Add_OneSessionStartAndEndOnOneDay_ShouldBeOneForDay()
        {
            // ARRANGE
            ISessionReportCreator sut =
                new DateMaxCounterReportCreator(Mock.Of<IReportPrinter>());
            var session1 = new Session(
                new DateTime(2025, 8, 26, 9, 15, 0),
                new DateTime(2025, 8, 26, 19, 15, 0),
                "",
                "",
                "",
                0);

            // ACT
            sut.Add(session1);
            var data = sut.GetAll().ToList();

            // ASSERT
            var dateItems1 = data[0].Split("\t");
            ParseDateTime(dateItems1[0])
                .ShouldBeEqualTo(session1.DateStart.Date);
            int.Parse(dateItems1[1])
                .ShouldBeEqualTo(1);
        }

        [Fact]
        public void Add_OneSessionStartAndEndOnDifferentDays_ShouldBeOneForAllDays()
        {
            // ARRANGE
            ISessionReportCreator sut =
                new DateMaxCounterReportCreator(Mock.Of<IReportPrinter>());
            var session1 = new Session(
                new DateTime(2025, 8, 26, 9, 15, 0),
                new DateTime(2025, 8, 27, 19, 15, 0),
                "",
                "",
                "",
                0);

            // ACT
            sut.Add(session1);
            var data = sut.GetAll().ToList();

            // ASSERT
            var dateItems1 = data[0].Split("\t");
            ParseDateTime(dateItems1[0])
                .ShouldBeEqualTo(session1.DateStart.Date);
            int.Parse(dateItems1[1])
                .ShouldBeEqualTo(1);
            var dateItems2 = data[1].Split("\t");
            ParseDateTime(dateItems2[0])
                .ShouldBeEqualTo(session1.DateEnd.Date);
            int.Parse(dateItems2[1])
                .ShouldBeEqualTo(1);
        }

        [Fact]
        public void Add_TwoSessionOverlappedByOneDay_ShouldBeOneForSecondAndTwoForOverlappedDay()
        {
            // ARRANGE
            ISessionReportCreator sut =
                new DateMaxCounterReportCreator(Mock.Of<IReportPrinter>());
            var session1 = new Session(
                new DateTime(2025, 8, 26, 9, 15, 0),
                new DateTime(2025, 8, 26, 19, 15, 0),
                "",
                "",
                "",
                0);
            var session2 = new Session(
                new DateTime(2025, 8, 26, 11, 15, 0),
                new DateTime(2025, 8, 27, 19, 15, 0),
                "",
                "",
                "",
                0);

            // ACT
            sut.Add(session1);
            sut.Add(session2);
            var data = sut.GetAll().ToList();

            // ASSERT
            var dateItems1 = data[0].Split("\t");
            ParseDateTime(dateItems1[0])
                .ShouldBeEqualTo(session1.DateStart.Date);
            int.Parse(dateItems1[1])
                .ShouldBeEqualTo(2);

            var dateItems2 = data[1].Split("\t");
            ParseDateTime(dateItems2[0])
                .ShouldBeEqualTo(session2.DateEnd.Date);
            int.Parse(dateItems2[1])
                .ShouldBeEqualTo(1);
        }

        [Fact]
        public void Add_TwoSessionOneOneDayNotOverlappedByTime_ShouldBeOneForAll()
        {
            // ARRANGE
            ISessionReportCreator sut =
                new DateMaxCounterReportCreator(Mock.Of<IReportPrinter>());
            var session1 = new Session(
                new DateTime(2025, 8, 26, 9, 15, 0),
                new DateTime(2025, 8, 26, 19, 15, 0),
                "",
                "",
                "",
                0);
            var session2 = new Session(
                new DateTime(2025, 8, 26, 20, 15, 0),
                new DateTime(2025, 8, 27, 19, 15, 0),
                "",
                "",
                "",
                0);

            // ACT
            sut.Add(session1);
            sut.Add(session2);
            var data = sut.GetAll().ToList();

            // ASSERT
            var dateItems1 = data[0].Split("\t");
            ParseDateTime(dateItems1[0])
                .ShouldBeEqualTo(session1.DateStart.Date);
            int.Parse(dateItems1[1])
                .ShouldBeEqualTo(1);

            var dateItems2 = data[1].Split("\t");
            ParseDateTime(dateItems2[0])
                .ShouldBeEqualTo(session2.DateEnd.Date);
            int.Parse(dateItems2[1])
                .ShouldBeEqualTo(1);
        }

        private static DateTime ParseDateTime(string date)
        {
            return DateTime.ParseExact(
                date,
                "dd.MM.yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.AdjustToUniversal);
        }
    }
}
