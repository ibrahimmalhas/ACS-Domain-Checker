using DnsClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DomainChecker
{

    public partial class WebForm1 : System.Web.UI.Page
    {
        Functions func = new Functions();
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if(!IsPostBack)
            { 
            if (Request.QueryString["domain"] != null)
            {
                DomainTextBox.Text = Request.QueryString["domain"];
                drawTable(Request.QueryString["domain"].ToString());
                    Page.Title = "ACS Domain Checker - " + Request.QueryString["domain"].ToString();
            }
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string url = Request.Url.AbsolutePath;
            Response.Redirect(url + "?domain=" + DomainTextBox.Text);
        }
        protected void drawTable(string domain)
        {
            copyButton.Visible = true;
            string content = "";
            content += "<table><tr><td><b> TXT </b></td></tr>";

            // Printing TXT
            if (func.getTXT(DomainTextBox.Text).Count() == 0)
                content += "<tr><td style='color: red;'> No Records Available.</td></tr>";
            foreach (string result in func.getTXT(DomainTextBox.Text))
            {
                content += "<tr><td>" + result + "</td></tr>";
            }

            // Printing SPF
            content += "<tr><td>&nbsp;</td></tr><tr><td><b> SPF </b></td></tr>";
            if (func.getSPF(DomainTextBox.Text).Count() == 0)
                content += "<tr><td style='color: red;'> No Records Available.</td></tr>";
            foreach (string result in func.getSPF(DomainTextBox.Text))
            {
                content += "<tr><td>" + result + "</td></tr>";
            }

            // Printing MX
            content += "<tr><td>&nbsp;</td></tr><tr><td><b> MX </b></td></tr>";
            if (func.getMX(DomainTextBox.Text).Count() == 0)
                content += "<tr><td style='color: red;'> No Records Available.</td></tr>";
            foreach (string result in func.getMX(DomainTextBox.Text))
            {
                content += "<tr><td>" + result + "</td></tr>";
            }



            //Printing DKIM1
            content += "<tr><td>&nbsp;</td></tr><tr><td><b> DKIM1 (selector1-azurecomm-prod-net._domainkey." + DomainTextBox.Text + ") </b></td></tr>";
            if (func.getDKIM1(DomainTextBox.Text).Count() == 0)
                content += "<tr><td style='color: red;'> No Records Available.</td></tr>";
            foreach (string result in func.getDKIM1(DomainTextBox.Text))
            {
                content += "<tr><td style='word-wrap: break-word;'>" + result + "</td></tr>";
            }

            //Printing DKIM2
            content += "<tr><td>&nbsp;</td></tr><tr><td><b> DKIM2 (selector2-azurecomm-prod-net._domainkey." + DomainTextBox.Text + ") </b></td></tr>";
            if (func.getDKIM2(DomainTextBox.Text).Count() == 0)
                content += "<tr><td style='color: red;'> No Records Available.</td></tr>";
            foreach (string result in func.getDKIM2(DomainTextBox.Text))
            {
                content += "<tr><td style='word-wrap: break-word;'>" + result + "</td></tr>";
            }

            //Printing DMARC
            content += "<tr><td>&nbsp;</td></tr><tr><td><b> DMARC </b></td></tr>";
            if (func.getDMARC(DomainTextBox.Text).Count() == 0)
                content += "<tr><td style='color: red;'> No Records Available.</td></tr>";
            foreach (string result in func.getDMARC(DomainTextBox.Text))
            {
                content += "<tr><td>" + result + "</td></tr>";
            }

            //Printing CNAME
            content += "<tr><td>&nbsp;</td></tr><tr><td><b> CNAME </b></td></tr>";
            if (func.getCNAME("www." + DomainTextBox.Text).Count() == 0)
                content += "<tr><td style='color: red;'> No Records Available.</td></tr>";
            foreach (string result in func.getCNAME("www." + DomainTextBox.Text))
            {
                content += "<tr><td>" + result + "</td></tr>";
            }

            content += "</table>";
            RecordsDiv.InnerHtml = content;
        }
    }
}