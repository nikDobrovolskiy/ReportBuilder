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
        public void Add_AddTwoSessionOverlappedByOneDay_ShouldBeOneForAllAndTwoForOverlappedDays()
        {
            // ARRANGE
            var dates = new List<DateTime>
            {
                DateTime.Now, DateTime.Now.AddDays(1), DateTime.Now.AddDays(2)
            };
            ISessionReportCreator sut =
                new DateMaxCounterReportCreator(Mock.Of<IReportPrinter>());
            var session1 = new Session(
                dates[0],
                dates[1],
                "",
                "",
                "",
                0);
            var session2 = new Session(
                dates[1],
                dates[2],
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
                .ShouldBeEqualTo(dates[0].Date);
            int.Parse(dateItems1[1])
                .ShouldBeEqualTo(1);

            var dateItems2 = data[1].Split("\t");
            ParseDateTime(dateItems2[0])
                .ShouldBeEqualTo(dates[1].Date);
            int.Parse(dateItems2[1])
                .ShouldBeEqualTo(2);

            var dateItems3 = data[2].Split("\t");
            ParseDateTime(dateItems3[0])
                .ShouldBeEqualTo(dates[2].Date);
            int.Parse(dateItems3[1])
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
