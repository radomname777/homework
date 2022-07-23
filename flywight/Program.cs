namespace flywight
{
    class Program
    {
        static void Main(string[] args)
        {
            CompanyInformation company = new CompanyInformation();
            company.CompanyName = "Baku Company";
            company.Address = "Baku";

            var reportA = ReportFactory.GetReport("B");
            reportA.SetCompanyInformation(company);

            var reportA1 = ReportFactory.GetReport("B");
            reportA1.SetCompanyInformation(company);

            company.CompanyName = "Russia Company";
            company.Address = "Moskov";
            var reportB = ReportFactory.GetReport("R");
            reportB.SetCompanyInformation(company);
        }
    }

    public interface IReport
    {
        void SetCompanyInformation(CompanyInformation company);
    }

    public class ReportA : IReport
    {
        private CompanyInformation? company;

        public string? ReportName { get; set; }

        public CompanyInformation CompanyInformation{get => this.company;}

        public void SetCompanyInformation(CompanyInformation company)
        {
            this.company = company;
            Console.WriteLine($"{company.CompanyName} - {company.Address}");
        }
    }

    public class ReportB : IReport
    {
        private CompanyInformation? company;

        public string? ReportName { get; set; }

        public CompanyInformation CompanyInformation
        {
            get
            {
                return this.company;
            }
        }

        public void SetCompanyInformation(CompanyInformation company)
        {
            this.company = company;
            Console.WriteLine($"{company.CompanyName} - {company.Address}");
        }
    }

    public class CompanyInformation
    {
        public string? CompanyName { get; set; }
        public string? Address { get; set; }
    }

    public class ReportFactory
    {
        static Dictionary<string, IReport> reports = new Dictionary<string, IReport>();

        public static IReport GetReport(string key)
        {
            if (reports.Keys.Contains(key)) return reports[key];
            
            switch (key)
            {
                case "B":
                    IReport reportA = new ReportA();
                    reports.Add(key, reportA);
                    return reportA;
                case "R":
                    IReport reportB = new ReportB();
                    reports.Add(key, reportB);
                    return reportB;

            }
            return null;
        }
    }
}