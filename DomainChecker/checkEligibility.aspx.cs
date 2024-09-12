using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DnsClient;
using DnsClient.Protocol;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Sockets;
using System.Net;
//using System.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace DomainChecker
{
   
    public partial class checkEligibility : System.Web.UI.Page
    {
        Functions func = new Functions();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["domain"] != null)
                {
                    DomainTextBox.Text = Request.QueryString["domain"];
                    drawTable(Request.QueryString["domain"].ToString());
                    Label2.Text= CheckDomainAgainstDNSBLs(Request.QueryString["domain"].ToString());
                    Page.Title = "ACS Quota Checker - " + Request.QueryString["domain"].ToString();
                }
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string url = Request.Url.AbsolutePath;
            Response.Redirect(url + "?domain=" + DomainTextBox.Text);

        }
        public async Task check_eligibility(string domain)
        {
            
            string[] dnsbls = {
            "dbl.spamhaus.org",
            "bl.spamcop.net",
            "dnsbl.sorbs.net",
            "black.dnsbl.brukalai.lt",
            "dnsbl.spfbl.net",
            "rhsbl-h.rbl.polspam.pl"
        };
            var lookup = new LookupClient(new LookupClientOptions(
                new[]
                {
                    new IPEndPoint(IPAddress.Parse("1.1.1.1"),53), // Google DNS
                    new IPEndPoint(IPAddress.Parse("1.0.0.1"),53)  // Google DNS
                })
            {
                UseCache = false,
                UseRandomNameServer = true
            });


            foreach (var dnsbl in dnsbls)
            {
                var query = $"{domain}.{dnsbls}";
                var result = await lookup.QueryAsync(query, QueryType.A);

                if (result.Answers.Count > 0) {
                    foreach(var answer in result.Answers)
                    {
                        if(answer is ARecord aRecord)
                        {
                            var ipAddress = aRecord.Address.ToString();
                           // if(ipAddress.StartsWith("127.0.0") )//&& ipAddress != "92.242.129.221")
                            Label2.Text += ($"{domain} is listed in {dnsbl} (IP: {ipAddress})") + "<br/>";
                           // else
                               // Label2.Text += ($"{domain} is not listed in {dnsbl} (IP: {ipAddress})") + "<br/>";
                        }

                    }
               // Label2.Text += ($"{domain} is listed in {dnsbl}") + "<br/>";
                    
                }
            else
                {
                    Label2.Text += ($"{domain} is not listed in {dnsbl}") + "<br/>";
                }
            }
        }
        public void drawTable(string domain)
        {
            string content = "";
            content += "<table><tr><th> TXT Record </th><th> SPF Record </th><th> DKIM1 </th><th> DKIM2 </th>" +
                "<th> MX Record </th><th> DMARC </th></tr>";

            // Getting TXT
            string[] record = func.getTXT(DomainTextBox.Text);
            int counter = 0;
            if (record.Count() == 0)
                content += "<tr><td style='color: red;'> No Record Available</td>";
            else
            {
                foreach (string result in record)
                {
                    if (result.ToLower().StartsWith("ms-domain-verification=") && counter == 0)
                    {
                        content += "<td style='color: green;'> Record Available</td>";
                        counter++;
                        break;
                    }
                    else if (result == record[record.Length - 1])
                        content += "<tr><td style='color: red;'> No Records Available</td>";
                }
                
            }

            // Getting SPF
             record = func.getSPF(DomainTextBox.Text);
            counter = 0;
            if (record.Count() == 0)
                content += "<td style='color: red;'> No Record Available</td>";
            else
            {
                foreach (string result in record)
                {
                    if (result.ToLower().Contains("spf.protection.outlook.com") && counter == 0)
                    {
                        content += "<td style='color: green;'> Record Available</td>";
                        counter++;
                        break;
                    }
                    else if (result == record[record.Length - 1])
                        content += "<td style='color: red;'> No Record Available</td>";

                }
            }

            // Getting DKIM1
            record = func.getDKIM1(DomainTextBox.Text);
            if (record.Count() == 0)
                content += "<td style='color: red;'> No Record Available</td>";
          else
                content += "<td style='color: green;'> Record Available</td>";

            // Getting DKIM2
            record = func.getDKIM2(DomainTextBox.Text);
            if (record.Count() == 0)
                content += "<td style='color: red;'> No Record Available</td>";
            else
                content += "<td style='color: green;'> Record Available</td>";

            // Getting MX
            record = func.getMX(DomainTextBox.Text);
            if (record.Count() == 0)
                content += "<td style='color: red;'> No Record Available</td>";
            else
                content += "<td style='color: green;'> Record Available</td>";

            // Getting DMARC
            record = func.getDMARC(DomainTextBox.Text);
            if (record.Count() == 0)
                content += "<td style='color: red;'> No Record Available</td></tr>";
            else
                content += "<td style='color: green;'> Record Available</td></tr>";

            content += "</table> <b>* For more information about the records, please  <a href='https://aka.ms/acsdomainchecker?domain=" + DomainTextBox.Text + "' target='_blank'> click here</a>";
            RecordsDiv.InnerHtml = content;
        }
        private string CheckDomainAgainstDNSBLs(string domain)
        {
            int counter = 0;
            string result = $"<br/><b>DNSBL status for: {domain}</b><br/><br/>";

            var builder = new ConfigurationBuilder()
           .SetBasePath(Server.MapPath("~"))
           .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            var configuration = builder.Build();
            var dnsbls = configuration.GetSection("DNSBLs").Get<List<List<string>>>();

            for(int i=0;i<dnsbls.Count; i++)
            {
                string query = $"{domain}.{dnsbls[i][0]}";
                try
                {
                    // Attempt to resolve the domain + DNSBL combination
                    IPHostEntry dnsblEntry = Dns.GetHostEntry(query);
                    string listedIp = dnsblEntry.AddressList[0].ToString();
                    if (listedIp.StartsWith("127.0.0.2"))
                    {
                        result += $"<b style='color:red'>Listed in:</b> {dnsbls[i][2]}, URL: <a href='{dnsbls[i][1]}' target='_blank'>{dnsbls[i][1]}</a><br/>";
                        counter++;
                    }
                }
                catch (Exception ex)
                {
                   // result += $"Error: {ex.Message}<br/>";
                }
            }
            if (counter >0)
                return result;
            else
                return "<p style='color: green;'> The domain is not listed in any of the DNSBLs</p>";
        }
    }
}