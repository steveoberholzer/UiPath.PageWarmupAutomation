using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using K2Warmup.Workflows.Models;

namespace K2Warmup.Workflows.Notifications
{
    public class WarmupNotification
    {
        public string To { get; private set; }
        public string Subject { get; private set; }
        public string HtmlBody { get; private set; }

        public WarmupNotification(string notificationEmailAddresses, List<WarmupResult> warmupResults)
        {
            To = ParseEmailAddresses(notificationEmailAddresses);
            Subject = BuildSubject(warmupResults);
            HtmlBody = BuildHtmlBody(warmupResults);
        }

        private string ParseEmailAddresses(string emailAddresses)
        {
            var addresses = emailAddresses
                .Split(new char[] { ',', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(e => e.Trim())
                .Where(e => !string.IsNullOrEmpty(e));

            return string.Join(";", addresses);
        }

        private string BuildSubject(List<WarmupResult> warmupResults)
        {
            int total = 0;
            int failed = 0;
            try
            {
                total = warmupResults.Count;
            }
            catch { }
            try
            {
                failed = warmupResults.Where(r => !r.Success).Count();
            }
            catch { }

            string status = failed == 0 ? "All Pages Warmed Successfully" : $"{failed} of {total} Pages Failed";

            return $"Web Application Warmup Report – {DateTime.Now:yyyy-MM-dd} – {status}";
        }

        private string BuildHtmlBody(List<WarmupResult> warmupResults)
        {
            int total = warmupResults.Count;
            int succeeded = warmupResults.Where(r => r != null).Count(r => r.Success);
            int failed = total - succeeded;

            var sb = new StringBuilder();

            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html><head><style>");
            sb.AppendLine("  body { font-family: Arial, sans-serif; font-size: 14px; color: #333; }");
            sb.AppendLine("  h2 { color: #2c3e50; }");
            sb.AppendLine("  .summary { margin-bottom: 20px; }");
            sb.AppendLine("  .summary span { margin-right: 20px; font-weight: bold; }");
            sb.AppendLine("  .success { color: #27ae60; }");
            sb.AppendLine("  .failure { color: #e74c3c; }");
            sb.AppendLine("  table { border-collapse: collapse; width: 100%; }");
            sb.AppendLine("  th { background-color: #2c3e50; color: #fff; text-align: left; padding: 8px 12px; }");
            sb.AppendLine("  td { padding: 7px 12px; border-bottom: 1px solid #ddd; vertical-align: top; }");
            sb.AppendLine("  tr:nth-child(even) { background-color: #f9f9f9; }");
            sb.AppendLine("  .error-detail { font-size: 12px; color: #e74c3c; margin-top: 4px; }");
            sb.AppendLine("</style></head><body>");

            sb.AppendLine($"<h2>Web Application Warmup Report</h2>");
            sb.AppendLine($"<p>Run on {DateTime.Now:dddd, MMMM d, yyyy} at {DateTime.Now:HH:mm:ss}</p>");

            sb.AppendLine("<div class=\"summary\">");
            sb.AppendLine($"  <span>Total: {total}</span>");
            sb.AppendLine($"  <span class=\"success\">Succeeded: {succeeded}</span>");
            if (failed > 0)
                sb.AppendLine($"  <span class=\"failure\">Failed: {failed}</span>");
            sb.AppendLine("</div>");

            sb.AppendLine("<table>");
            sb.AppendLine("  <thead><tr>");
            sb.AppendLine("    <th>URL</th>");
            sb.AppendLine("    <th>Status</th>");
            sb.AppendLine("    <th>Start Time</th>");
            sb.AppendLine("    <th>End Time</th>");
            sb.AppendLine("    <th>Duration</th>");
            sb.AppendLine("  </tr></thead>");
            sb.AppendLine("  <tbody>");

            foreach (var result in warmupResults)
            {
                if (result != null)
                {
                    string statusLabel = result.Success
                        ? "<span class=\"success\">Success</span>"
                        : "<span class=\"failure\">Failed</span>";

                    string errorDetail = result.Error != null
                        ? $"<div class=\"error-detail\">{System.Net.WebUtility.HtmlEncode(result.Error.Message)}</div>"
                        : string.Empty;

                    string duration = result.EndTime == default
                        ? "–"
                        : $"{result.Duration.TotalSeconds:F1}s";

                    string endTime = result.EndTime == default
                        ? "–"
                        : result.EndTime.ToString("HH:mm:ss");

                    sb.AppendLine("  <tr>");
                    sb.AppendLine($"    <td>{System.Net.WebUtility.HtmlEncode(result.PageConfig.Url)}</td>");
                    sb.AppendLine($"    <td>{statusLabel}{errorDetail}</td>");
                    sb.AppendLine($"    <td>{result.StartTime:HH:mm:ss}</td>");
                    sb.AppendLine($"    <td>{endTime}</td>");
                    sb.AppendLine($"    <td>{duration}</td>");
                    sb.AppendLine("  </tr>");
                }
                else
                {
                    sb.AppendLine("  <tr>");
                    sb.AppendLine($"    <td>-</td>");
                    sb.AppendLine($"    <td>-</td>");
                    sb.AppendLine($"    <td>-</td>");
                    sb.AppendLine($"    <td>-</td>");
                    sb.AppendLine($"    <td>-</td>");
                    sb.AppendLine("  </tr>");
                }
            }

            sb.AppendLine("  </tbody>");
            sb.AppendLine("</table>");
            sb.AppendLine("</body></html>");

            return sb.ToString();
        }
    }
}
