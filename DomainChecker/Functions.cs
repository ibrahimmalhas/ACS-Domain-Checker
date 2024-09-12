using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DnsClient;

namespace DomainChecker
{
    public class Functions
    {
        public string[] getTXT(string domain)
        {
            List<string> results = new List<string>();
            try
            {
                // Create a DNS client
                var dnsClient = new LookupClient();

                // Retrieve the TXT records for the specified domain
                var result = dnsClient.Query(domain, QueryType.TXT);

                // Iterate through the TXT records and print them
                foreach (var record in result.Answers.TxtRecords())
                {
                    results.Add(string.Join(", ", record.Text));

                }

            }
            catch (DnsResponseException ex)
            {
                results.Add("ERROR: " + ex.Message);
            }
            return results.ToArray();
        }
        public string[] getSPF(string domain)
        {
            List<string> results = new List<string>();
            try
            {
                // Create a DNS client
                var dnsClient = new LookupClient();

                // Retrieve the DMARC records for the specified domain
                var result = dnsClient.Query(domain, QueryType.TXT);

                // Filter the DMARC records from the TXT records
                var dmarcRecords = result.Answers.TxtRecords()
                    .Where(r => r.Text.Any(t => t.StartsWith("v=spf")));

                // Print out the DMARC records
                foreach (var record in dmarcRecords)
                {
                    foreach (var txtData in record.Text)
                    {
                        results.Add(txtData);
                    }
                }
            }
            catch (DnsResponseException ex)
            {
                results.Add($"An error occurred: {ex.Message}");
            }
            return results.ToArray();
        }
        public string[] getMX(string domain)
        {
            List<string> results = new List<string>();
            try
            {
                // Create a DNS client
                var dnsClient = new LookupClient();

                // Retrieve the MX records for the specified domain
                var mxResult = dnsClient.Query(domain, QueryType.MX);

                // Iterate through the MX records
                foreach (var mxRecord in mxResult.Answers.MxRecords())
                {
                    // Get the host name of the mail exchange server
                    string mxHostname = mxRecord.Exchange.Value;

                    // Resolve the IP addresses for the mail exchange server
                    var ipResult = dnsClient.Query(mxHostname, QueryType.A);

                    // Iterate through the IP addresses and print them
                    foreach (var ipRecord in ipResult.Answers.ARecords())
                    {
                        results.Add($"Hostname: {mxHostname}, IP Address: {ipRecord.Address}");
                    }
                }
            }

            catch (DnsResponseException ex)
            {
                results.Add("ERROR: " + ex.Message);
            }
            return results.ToArray();
        }

        public string[] getDKIM1(string domain)
        {
            List<string> results = new List<string>();
            string selector = "selector1-azurecomm-prod-net"; // Replace with your DKIM selector

            try
            {
                // Create a DNS client
                var dnsClient = new LookupClient();

                // Construct the DKIM lookup domain
                string dkimDomain = $"{selector}._domainkey.{domain}";

                // Retrieve the TXT records for the DKIM domain
                var result = dnsClient.Query(dkimDomain, QueryType.TXT);

                // Print out the TXT records
                foreach (var record in result.Answers.TxtRecords())
                {
                    results.Add(string.Join(", ", record.Text));
                }
            }
            catch (DnsResponseException ex)
            {
                results.Add("ERROR: " + ex.Message);
            }
            return results.ToArray();
        }
        public string[] getDKIM2(string domain)
        {
            List<string> results = new List<string>();
            string selector = "selector2-azurecomm-prod-net"; // Replace with your DKIM selector

            try
            {
                // Create a DNS client
                var dnsClient = new LookupClient();

                // Construct the DKIM lookup domain
                string dkimDomain = $"{selector}._domainkey.{domain}";

                // Retrieve the TXT records for the DKIM domain
                var result = dnsClient.Query(dkimDomain, QueryType.TXT);

                // Print out the TXT records
                foreach (var record in result.Answers.TxtRecords())
                {
                    results.Add(string.Join(", ", record.Text));
                }
            }
            catch (DnsResponseException ex)
            {
                results.Add("ERROR: " + ex.Message);
            }
            return results.ToArray();
        }
        public string[] getCNAME(string domain)
        {
            List<string> results = new List<string>();
            try
            {
                // Create a DNS client
                var dnsClient = new LookupClient();

                // Retrieve the CNAME records for the specified domain
                var result = dnsClient.Query(domain, QueryType.CNAME);

                // Print out the CNAME records
                foreach (var record in result.Answers.CnameRecords())
                {
                    results.Add(record.CanonicalName.ToString().Remove(record.CanonicalName.ToString().Length-1));
                }
            }
            catch (DnsResponseException ex)
            {
                results.Add("ERROR: " + ex.Message);
            }
            return results.ToArray();
        }
        public string[] getDMARC(string domain)
        {
            List<string> results = new List<string>();
            try
            {

                // Create a DNS client
                var dnsClient = new LookupClient();

                // Retrieve the DMARC records for the specified domain
                var result = dnsClient.Query("_dmarc."+domain, QueryType.TXT);

                foreach (var record in result.Answers.TxtRecords())
                {
                    results.Add(string.Join(", ", record.Text));
                }
            }
            catch (DnsResponseException ex)
            {
                results.Add($"An error occurred: {ex.Message}");
            }
            return results.ToArray();
        }
    }
}
